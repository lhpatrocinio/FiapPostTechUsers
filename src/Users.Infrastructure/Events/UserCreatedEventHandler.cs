using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using Users.Application.Events;
using Users.Domain.Events;

namespace Users.Infrastructure.Events
{
    public class UserCreatedEventHandler: IUserCreatedEventHandler
    {
        public void PublishUserCreatedEvent(UserCreatedEvent user)
        {
            var factory = new ConnectionFactory() { HostName = "rabbitmq" }; // ou nome do container no docker-compose
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(
                queue: "user-created-queue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var message = JsonSerializer.Serialize(new
            {
                UserId = user.Id,
                Name = user.Name,
                Email = user.Email,
                CreatedAt = DateTime.UtcNow
            });

            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(
                exchange: "",
                routingKey: "user-created-queue",
                basicProperties: null,
                body: body);
        }

    
    }
}
