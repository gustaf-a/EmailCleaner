﻿using MailProviderService.Configuration;
using MailProviderService.MessageQueue;
using Moq;
using RabbitMQ.Client;

namespace MailProviderServiceTests.Mocks.MessageQueue;

internal class RabbitMqChannelBuilderMock : IChannelBuilder
{
    private readonly Mock<IModel> _mock;

    public RabbitMqChannelBuilderMock(Mock<IModel> mock)
    {
        mock.Setup(m =>
                m.BasicConsume(It.IsAny<string>(),
                It.IsAny<bool>(),
                It.IsAny<string>(),
                It.IsAny<bool>(),
                It.IsAny<bool>(),
                It.IsAny<IDictionary<string, object>>(),
                It.IsAny<IBasicConsumer>()))
            .Returns("test_tag");

        _mock = mock;
    }

    public IModel BuildChannel(MessageQueueOptions messageQueueOptions)
    {
        return _mock.Object;
    }
}
