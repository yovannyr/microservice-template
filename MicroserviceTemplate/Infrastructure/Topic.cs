
using System;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Infrastructure
{
    public class Topic
    {
        private readonly string exchange;
        private readonly string topic;
        private readonly MessageSerializer messageSerializer = new MessageSerializer();


        ConnectionFactory connectionFactory = new ConnectionFactory() { HostName = System.Configuration.ConfigurationManager.AppSettings["RabbitMq-host"] };

        public Topic(string exchange, string topic)
        {
            this.exchange = exchange;
            this.topic = topic;
        }


        public void Subscribe(Action<string> action)
        {
            Subscribe(
                (msg) =>
                {
                    var msgStr = messageSerializer.Deserialize(msg.Body);
                    action(msgStr);
                });
        }

        public void Subscribe<T>(Action<T> action)
        {
            Subscribe(
                (msg) =>
                {
                    var msgStr = messageSerializer.Deserialize<T>(msg.Body);
                    action(msgStr);
                });
        }

        private void Subscribe(Action<BasicDeliverEventArgs> onDelivery)
        {
            var task = new Task(
                () =>
                {
                    using (var connection = this.connectionFactory.CreateConnection())
                    {
                        using (var channel = connection.CreateModel())
                        {
                            channel.ExchangeDeclare(exchange, "fanout");

                            var queueName = channel.QueueDeclare().QueueName;

                            channel.QueueBind(queueName, topic, "");
                            var consumer = new QueueingBasicConsumer(channel);
                            channel.BasicConsume(queueName, true, consumer);

                            while (true)
                            {
                                var ea = consumer.Queue.Dequeue();
                                onDelivery(ea);
                            }
                        }
                    }
                });

            task.Start();
        }

        public void Publish(string message)
        {
            Publish(messageSerializer.Serialize(message));
        }


        public void Publish<T>(T message)
        {
            Publish(messageSerializer.Serialize<T>(message));
        }

        private void Publish(byte[] message)
        {
            using (var connection = connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange, "fanout");
                channel.BasicPublish(topic, "", null, message);
            }
        }


    }
}
