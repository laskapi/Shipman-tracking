using shipman.Server.Application.Interfaces;

public class FakeMailSender : IMailSender
{
    public Task SendAsync(string to, string subject, string body)
    {
        // Simulate sending email (no logging here — decorator handles it)
        return Task.CompletedTask;
    }
}
