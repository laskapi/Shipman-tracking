namespace shipman.Server.Application.Interfaces
{
    public interface IMailSender
    {
        Task SendAsync(string to, string subject,string body);
    }
}
