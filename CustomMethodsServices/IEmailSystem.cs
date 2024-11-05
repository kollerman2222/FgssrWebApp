namespace fgssr.CustomMethodsServices
{
    public interface IEmailSystem
    {
        Task<bool> SendEmail(string receiverMail, string subject , string? code , string fullnameenglish);
    }
}
