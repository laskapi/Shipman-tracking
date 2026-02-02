using shipman.Server.Application.Interfaces;

public class FakeMailSender : IMailSender
{
    public Task SendAsync(string to, string subject, string body)
    {
        Console.WriteLine($"Email to {to}: {subject}");
        return Task.CompletedTask;
    }
}
