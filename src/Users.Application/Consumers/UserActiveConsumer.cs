using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Users.Application.Dtos.Requests;
using Users.Application.Rabbit;
using Users.Application.Services.Interfaces;

namespace Users.Api.Consumers
{
    public class UserActiveConsumer : BackgroundService
    {
        private readonly ILogger<UserActiveConsumer> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly RabbitMqSetup _setup;
        private readonly IModel _channel;
        private const int MaxRetries = 3;

        public UserActiveConsumer(ILogger<UserActiveConsumer> logger, IServiceScopeFactory serviceScopeFactory, RabbitMqSetup setup)
        {
            _logger = logger;
            _setup = setup;
            _channel = _setup.CreateChannel("user_active");
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (ch, ea) =>
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var userServices = scope.ServiceProvider.GetRequiredService<IUserServices>();

                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                try
                {
                    var userEvent = JsonSerializer.Deserialize<UserActiveEvent>(message);

                    _logger.LogInformation($"[GamesApi] Novo usuário detectado: {userEvent?.Name} - {userEvent?.Email}");

                    await userServices.BlockUserAsync(
                        new BlockUserRequest()
                        {
                            Id = userEvent.Id,
                            EnableBlocking = false
                        });
                }
                catch (Exception ex)
                {
                    int retryCount = GetRetryCount(ea.BasicProperties);

                    if (retryCount >= MaxRetries)
                    {
                        _logger.LogInformation($"Enviando para DLQ após {retryCount} tentativas");
                        SendToDlq(ea.Body.ToArray());
                        _channel.BasicAck(ea.DeliveryTag, false);
                    }
                    else
                    {
                        _logger.LogInformation($"Retry {retryCount + 1} de {MaxRetries}");
                        SendToRetryQueue(ea.Body.ToArray(), retryCount + 1);
                        _channel.BasicAck(ea.DeliveryTag, false);
                    }
                }
            };

            _channel.BasicConsume("user_active_queue", false, consumer);
            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel.Close();
            base.Dispose();
        }

        private int GetRetryCount(IBasicProperties props)
        {
            if (props.Headers != null && props.Headers.TryGetValue("x-retry-count", out var value))
            {
                return int.Parse(Encoding.UTF8.GetString((byte[])value));
            }
            return 0;
        }

        private void SendToRetryQueue(byte[] body, int retryCount)
        {
            var props = _channel.CreateBasicProperties();
            props.Persistent = true;
            props.Headers = new Dictionary<string, object>
            {
                { "x-retry-count", Encoding.UTF8.GetBytes(retryCount.ToString()) }
            };

            _channel.BasicPublish(
                exchange: "",
                routingKey: "user_create_queue",
                basicProperties: props,
                body: body);
        }

        private void SendToDlq(byte[] body)
        {
            var props = _channel.CreateBasicProperties();
            props.Persistent = true;

            _channel.BasicPublish(
                exchange: "",
                routingKey: "user_create_dlq",
                basicProperties: props,
                body: body);
        }
    }

    public record UserActiveEvent(Guid Id, string Name, string Email, DateTime CreatedAt);
}

