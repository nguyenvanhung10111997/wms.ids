namespace wms.infrastructure.RabbitMQ
{
    public interface IRabbitMQProducer : IDisposable
    {
        public void SendMessage<T>(T message, string topic);
    }
}
