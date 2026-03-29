using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NotificationWorker.Application.Contracts;
using NotificationWorker.Application.Services;
using NotificationWorker.Domain.Models.Emails;
using Xunit;

namespace NotificationWorker.Tests.Application.Services;

public class EmailDispatcherTests
{
    private readonly Mock<ILogger<EmailDispatcher>> _logger = new();
    private readonly Mock<IEmailQueuePublisher> _publisher = new();

    private EmailDispatcher CreateSut() =>
        new(_logger.Object, _publisher.Object);

    [Fact]
    public async Task SendAsync_WhenPublishSucceeds_ShouldPublishOnce()
    {
        // Arrange
        var email = BuildEmail();

        _publisher
            .Setup(p => p.PublishAsync(It.IsAny<EmailToBeSend>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var sut = CreateSut();

        // Act
        await sut.SendAsync(email);

        // Assert
        _publisher.Verify(
            p => p.PublishAsync(It.IsAny<EmailToBeSend>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task SendAsync_WhenPublishThrowsTransientError_ShouldRetry()
    {
        // Arrange
        var email = BuildEmail();
        var callCount = 0;

        _publisher
            .Setup(p => p.PublishAsync(It.IsAny<EmailToBeSend>(), It.IsAny<CancellationToken>()))
            .Returns(() =>
            {
                callCount++;
                // falha nas duas primeiras tentativas, sucesso na terceira
                return callCount < 3
                    ? Task.FromException(new HttpRequestException("connection refused"))
                    : Task.CompletedTask;
            });

        var sut = CreateSut();

        // Act
        await sut.SendAsync(email);

        // Assert
        _publisher.Verify(
            p => p.PublishAsync(It.IsAny<EmailToBeSend>(), It.IsAny<CancellationToken>()),
            Times.Exactly(3));
    }

    [Fact]
    public async Task SendAsync_WhenPublishThrowsNonRetriableError_ShouldNotRetry()
    {
        // Arrange
        var email = BuildEmail();

        _publisher
            .Setup(p => p.PublishAsync(It.IsAny<EmailToBeSend>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("invalid state"));

        var sut = CreateSut();

        // Act
        var act = async () => await sut.SendAsync(email);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();

        _publisher.Verify(
            p => p.PublishAsync(It.IsAny<EmailToBeSend>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    private static EmailToBeSend BuildEmail() => new()
    {
        To = ["test@test.com"],
        Subject = "Test subject",
        Body = "<p>Hello</p>"
    };
}