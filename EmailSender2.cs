using BulkyBook.Utility;
using Moq;
using SendGrid;
using NUnit.Framework;
using Moq;
using Microsoft.Extensions.Configuration;
using BulkyBook.Utility;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace BulkyBook.Tests
{
    public class EmailSenderTests
    {
        [Test] // This is an NUnit attribute to mark the method as a test
        public async Task SendEmailAsync_InvalidEmail_ThrowsException()
        {
            // Arrange: Mock the configuration to return a fake SendGrid API key
            var mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(c => c.GetValue<string>("SendGrid:SecretKey")).Returns("fake-api-key");

            // Create an instance of EmailSender using the mock configuration
            var emailSender = new EmailSender(mockConfig.Object);

            // Mock SendGridClient to simulate a failed response when an invalid email is passed
            var mockSendGridClient = new Mock<SendGridClient>("fake-api-key");

            // Setup: Make the SendGridClient simulate a failure for an invalid email (e.g., throwing an exception)
            mockSendGridClient
                .Setup(client => client.SendEmailAsync(It.IsAny<SendGridMessage>(), default))
                .ThrowsAsync(new InvalidOperationException("Invalid email address"));

            // Act & Assert: Call SendEmailAsync with an invalid email and expect an exception
            var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await emailSender.SendEmailAsync("invalid-email", "Test Subject", "Test Message")
            );

            // Assert: Verify the exception message
            Assert.That(ex.Message, Is.EqualTo("Invalid email address"));

        }
    }
}
