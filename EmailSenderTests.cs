using System.Threading.Tasks;
using Moq;
using Microsoft.Extensions.Configuration;
using BulkyBook.Utility;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net;
using NUnit.Framework;

public class EmailSenderTests
{
    [Test]
    public async Task SendEmailAsync_ValidEmail_SendsSuccessfully()
    {
        // Arrange: Create a mock configuration with a fake SendGrid API key
        var mockConfig = new Mock<IConfiguration>();
        mockConfig.Setup(c => c.GetValue<string>("SendGrid:SecretKey")).Returns("fake-api-key");

        // Create an instance of EmailSender using the mock configuration
        var emailSender = new EmailSender(mockConfig.Object);

        // Mock SendGridClient to prevent real API calls
        var mockSendGridClient = new Mock<SendGridClient>("fake-api-key");
        mockSendGridClient
            .Setup(client => client.SendEmailAsync(It.IsAny<SendGridMessage>(), default))
            .ReturnsAsync(new Response(HttpStatusCode.OK, null, null));

        // Act: Call SendEmailAsync
        var result = emailSender.SendEmailAsync("test@example.com", "Test Subject", "Test Message");

        // Assert: Ensure the method does not return null
        Assert.That(result, Is.Not.Null);
    }
}
