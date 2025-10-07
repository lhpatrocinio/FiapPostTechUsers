using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Users.Application.Dtos.Requests;
using Users.Application.Services.Interfaces;

namespace Users.Api.Consumers
{
    public class UserActiveConsumer : BackgroundService
    {
        private readonly ILogger<UserActiveConsumer> _logger;
        private IConnection _connection;
        private IModel _channel;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public UserActiveConsumer(ILogger<UserActiveConsumer> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;

            var factory = new ConnectionFactory() { HostName = "rabbitmq" }; // ou nome do container no docker-compose
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(
                queue: "active-queue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
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

                var userEvent = JsonSerializer.Deserialize<UserActiveEvent>(message);

                _logger.LogInformation($"[GamesApi] Novo usuário detectado: {userEvent?.Name} - {userEvent?.Email}");

                await userServices.BlockUserAsync(
                    new BlockUserRequest()
                    {
                        Id = userEvent.UserId,
                        EnableBlocking = false
                    });

            };

            _channel.BasicConsume("active-queue", false, consumer);

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }

    public record UserActiveEvent(Guid UserId, string Name, string Email, DateTime CreatedAt);
}

