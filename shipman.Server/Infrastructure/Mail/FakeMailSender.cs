using shipman.Server.Application.Interfaces;

public class FakeMailSender : IMailSender
{
    public Task SendAsync(string to, string subject, string body)
    {
        return Task.CompletedTask;
    }
}
