namespace shipman.Server.Infrastructure.Mail;
using shipman.Server.Application.Interfaces;

public class MailSenderLoggingDecorator : IMailSender
{
    private readonly ILogger<MailSenderLoggingDecorator> _logger;
    private readonly IMailSender _inner;

    public MailSenderLoggingDecorator(
        ILogger<MailSenderLoggingDecorator> logger,
        IMailSender inner)
    {
        _logger = logger;
        _inner = inner;
    }

    public async Task SendAsync(string to, string subject, string body)
    {
        _logger.LogInformation("Sending email → To: {To}, Subject: {Subject}", to, subject);

        try
        {
            await _inner.SendAsync(to, subject, body);
            _logger.LogInformation("Email successfully sent to {To}", to);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {To}", to);
            throw;
        }
    }
}
