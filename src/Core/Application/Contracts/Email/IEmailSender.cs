using Application.Models.Email;

namespace Application.Contracts.Email;

public interface IEmailSender
{
  Task<bool> SendEmail(EmailMessage email);
}