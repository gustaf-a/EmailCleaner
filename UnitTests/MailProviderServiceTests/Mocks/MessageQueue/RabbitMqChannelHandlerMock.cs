using MailProviderService.Configuration;
using MailProviderService.MessageQueue;
using RabbitMQ.Client;

namespace MailProviderServiceTests.Mocks.MessageQueue;

public class RabbitMqChannelHandlerMock : IChannelHandler
{
    private static IModel _fakeModel = null;

    public static void SetFakeModel(IModel fakeModel)
        => _fakeModel = fakeModel;

    public RabbitMqChannelHandlerMock()
    {
    }

    public IModel Create(MessageQueueOptions messageQueueOptions)
    {
        return _fakeModel;
    }

    public IModel GetChannel()
        => _fakeModel;

    public void StartCollecting(IBasicConsumer consumer)
    {
        _fakeModel.BasicConsume("", false, consumer);
    }

    public void StopCollecting()
    {
    }
}
