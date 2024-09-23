using Common.Dtos;
using Common.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Utils;
using UserManagement.Application.Configuration;
using UserManagement.Infrastructure.Services.Configurations;

namespace UserManagement.Infrastructure.Services
{
    public class SmtpEmailService : IEmailService
    {
        public SmtpEmailService()
        {
        }
        private static string ReplaceFooterContent(BodyBuilder builder, string stringInput)
        {
            stringInput = stringInput.Replace("[Year]", DateTime.Now.Year.ToString());
            stringInput = stringInput.Replace("[OrganizationName]", OrganizationSetting.Name);
            stringInput = stringInput.Replace("[UrlOrganizationFacebook]", OrganizationSetting.Facebook);
            stringInput = stringInput.Replace("[UrlOrganizationTwitter]", OrganizationSetting.Twitter);
            stringInput = stringInput.Replace("[UrlOrganizationInstagram]", OrganizationSetting.Instagram);
            stringInput = stringInput.Replace("[UrlOrganizationTiktok]", OrganizationSetting.Tiktok);
            stringInput = stringInput.Replace("[UrlOrganizationYoutube]", OrganizationSetting.Youtube);
            stringInput = stringInput.Replace("[UrlOrganizationWhatsapp]", OrganizationSetting.Whatsapp);

            MimeEntity mimeEntity = builder.LinkedResources.Add($"{DirectorySetting.PathFileApp}/icons8-facebook-96.png");
            mimeEntity.ContentId = MimeUtils.GenerateMessageId();
            stringInput = stringInput.Replace("[IconFacebook]",  mimeEntity.ContentId);

            mimeEntity = builder.LinkedResources.Add($"{DirectorySetting.PathFileApp}/icons8-twitterx-96.png");
            mimeEntity.ContentId = MimeUtils.GenerateMessageId();
            stringInput = stringInput.Replace("[IconTwitter]", mimeEntity.ContentId);

            mimeEntity = builder.LinkedResources.Add($"{DirectorySetting.PathFileApp}/icons8-instagram-96.png");
            mimeEntity.ContentId = MimeUtils.GenerateMessageId();
            stringInput = stringInput.Replace("[IconInstagram]", mimeEntity.ContentId);

            mimeEntity = builder.LinkedResources.Add($"{DirectorySetting.PathFileApp}/icons8-tiktok-96.png");
            mimeEntity.ContentId = MimeUtils.GenerateMessageId();
            stringInput = stringInput.Replace("[IconTiktok]", mimeEntity.ContentId);

            mimeEntity = builder.LinkedResources.Add($"{DirectorySetting.PathFileApp}/icons8-youtube-96.png");
            mimeEntity.ContentId = MimeUtils.GenerateMessageId();
            stringInput = stringInput.Replace("[IconYoutube]", mimeEntity.ContentId);

            mimeEntity = builder.LinkedResources.Add($"{DirectorySetting.PathFileApp}/icons8-whatsapp-96.png");
            mimeEntity.ContentId = MimeUtils.GenerateMessageId();
            stringInput = stringInput.Replace("[IconWhatsapp]", mimeEntity.ContentId);
            return stringInput;
        }
        public async Task SendEmailAsync(EmailMessageDto message)
        {
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress(SmtpSettings.EmailSender, SmtpSettings.Username));
            for (int i = 0; i < message.To.Count; i++)
            {
                mimeMessage.To.Add(new MailboxAddress(message.Title, message.To[i]));
            }
            for (int i = 0; i < message.Cc.Count; i++)
            {
                mimeMessage.Cc.Add(new MailboxAddress(message.Title, message.Cc[i]));
            }

            mimeMessage.Subject = message.Subject;

            var builder = new BodyBuilder();
            builder.TextBody = message.Content;
            if (message.Attachments != null)
            {
                foreach (var attacthment in message.Attachments)
                {
                    builder.Attachments.Add(attacthment);
                }
            }
            mimeMessage.Body = builder.ToMessageBody();
            try
            {
                using (var client = new SmtpClient())
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    await client.ConnectAsync(SmtpSettings.Host, SmtpSettings.Port, SecureSocketOptions.Auto);
                    await client.AuthenticateAsync(SmtpSettings.Username, SmtpSettings.Password);
                    client.AuthenticationMechanisms.Remove("XOAUTH");
                    await client.SendAsync(mimeMessage);
                    await client.DisconnectAsync(true);

                }
            }
            catch (Exception ex)
            {
                var e = ex;
                throw;
            }
        }

        public async Task SendHtmlEmailAsync(EmailMessageDto message)
        {
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress(SmtpSettings.EmailSender, SmtpSettings.Username));
            for (int i = 0; i < message.To.Count; i++)
            {
                mimeMessage.To.Add(new MailboxAddress(message.Title, message.To[i]));
            }
            for (int i = 0; i < message.Cc.Count; i++)
            {
                mimeMessage.Cc.Add(new MailboxAddress(message.Title, message.Cc[i]));
            }
            mimeMessage.Subject = message.Subject;

            var builder = new BodyBuilder();
            builder.HtmlBody = ReplaceFooterContent(builder, message.Content);

            if (message.Attachments != null)
            {
                foreach (var attacthment in message.Attachments)
                {
                    builder.Attachments.Add(attacthment);
                }
            }

            mimeMessage.Body = builder.ToMessageBody();
            try
            {
                using (var client = new SmtpClient())
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    await client.ConnectAsync(SmtpSettings.Host, SmtpSettings.Port, SecureSocketOptions.Auto);
                    await client.AuthenticateAsync(SmtpSettings.Username, SmtpSettings.Password);
                    client.AuthenticationMechanisms.Remove("XOAUTH");
                    await client.SendAsync(mimeMessage);
                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                var e = ex;
                throw;
            }
        }
    }
}