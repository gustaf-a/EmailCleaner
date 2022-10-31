using MailProviderService.Configuration;
using MailProviderService.MessageQueue;
using RabbitMQ.Client;

namespace MailProviderServiceTests.Mocks.MessageQueue;

internal class RabbitMqChannelFactoryMock : IChannelFactory
{
    private static IModel _fakeModel;

    public static void SetFakeModel(IModel fakeModel)
        => _fakeModel = fakeModel;

    public RabbitMqChannelFactoryMock()
    {
        if (_fakeModel is null)
            throw new Exception($"FakeModel must be set before creating instances of {nameof(RabbitMqChannelFactoryMock)}");
    }

    public IModel Create(MessageQueueOptions messageQueueOptions)
    {
        return _fakeModel;
    }
}
