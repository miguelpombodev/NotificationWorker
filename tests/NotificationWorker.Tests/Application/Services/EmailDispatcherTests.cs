using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Microsoft.Extensions.Logging;
using NotificationWorker.Application.Contracts;
using NotificationWorker.Application.Services;
using NotificationWorker.Domain.Enums;
using NotificationWorker.Domain.Models;
using NotificationWorker.Domain.Models.Providers;
using Xunit;

namespace NotificationWorker.Tests.Application.Services;

public class EmailDispatcherTests
{
    private readonly Mock<ITemplateRenderer> _templateRendererMock = new();
    private readonly Mock<ILogger<EmailDispatcher>> _loggerMock = new();
    private readonly Mock<IOptions<RabbitMqOptions>> _rabbitMqOptions = new();

    private EmailDispatcher CreateSut() =>
        new(_templateRendererMock.Object, _loggerMock.Object, _rabbitMqOptions.Object);

    [Fact]
    public async Task SendAsync_WhenTemplateNotFound_ShouldNotRetry()
    {
        // Arrange
        NotificationChannel emailChannel = NotificationChannel.Email;
        var notification = new NotificationRequested
        {
            Channel = emailChannel,
            Project = "invalid-project",
            Template = "welcome",
            Recipient = "test@test.com",
            Data = new Dictionary<string, object>()
            {
                { "subject", "welcome test" },
                { "name", "user" },
                { "loginUrl", "https://test.com" },
                { "role", "Developer" }
            }
        };

        _templateRendererMock
            .Setup(r => r.RenderAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object>()))
            .ThrowsAsync(new DirectoryNotFoundException(
                "Project Folder invalid_project in Templates folder does not exist! Please check it "));

        var sut = CreateSut();

        // Act
        var act = async () => await sut.SendAsync(notification);

        // Assert
        await act.Should().ThrowAsync<DirectoryNotFoundException>();


        // check if method was called only once
        _templateRendererMock.Verify(
            r => r.RenderAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object>()),
            Times.Once);
    }

    [Fact]
    public async Task SendAsync_WhenTemplateIsUnknown_ShouldThrowInvalidOperation()
    {
        // Arrange
        NotificationChannel emailChannel = NotificationChannel.Email;
        var notification = new NotificationRequested
        {
            Channel = emailChannel,
            Project = "notification-worker",
            Template = "unknow-template",
            Recipient = "test@test.com",
            Data = new Dictionary<string, object>() 
            {
                { "subject", "welcome test" },
                { "name", "user" },
                { "loginUrl", "https://test.com" },
                { "role", "Developer" }
            }
        };

        var sut = CreateSut();

        // Act
        var act = async () => await sut.SendAsync(notification);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("Invalid template");
    }
}