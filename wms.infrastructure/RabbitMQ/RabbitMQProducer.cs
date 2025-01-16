using wms.infrastructure.Configurations;
using wms.infrastructure.PollyRetry;
using Newtonsoft.Json;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace wms.infrastructure.RabbitMQ
{
    internal class RabbitMQProducer : IRabbitMQProducer
    {
        private readonly IPollyRetryHelper _pollyRetryHelper;

        public RabbitMQProducer(IPollyRetryHelper pollyRetryHelper)
        {
            _pollyRetryHelper = pollyRetryHelper;
        }

        public void SendMessage<T>(T message, string topic)
        {
            var factory = new ConnectionFactory
            {
                HostName = AppCoreConfig.Providers.RabbitMQ.Host,
                UserName = AppCoreConfig.Providers.RabbitMQ.Username,
                Password = AppCoreConfig.Providers.RabbitMQ.Password
            };

            var connection = factory.CreateConnection();

            using (var channel = connection.CreateModel())
            {
                //declare the queue after mentioning name and a few property related to that
                channel.ExchangeDeclare(exchange: $"{topic}_processing_exchange", type: ExchangeType.Fanout);
                channel.QueueDeclare($"{topic}_queue", exclusive: false);
                channel.QueueBind($"{topic}_queue", $"{topic}_processing_exchange", routingKey: topic);

                var json = JsonConvert.SerializeObject(message);
                var body = Encoding.UTF8.GetBytes(json);
                Console.WriteLine(json);
                channel.BasicPublish(exchange: $"{topic}_processing_exchange", routingKey: topic, body: body);
            }
        }

        #region Destructor
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~RabbitMQProducer() { Dispose(false); }
        #endregion
    }
}
