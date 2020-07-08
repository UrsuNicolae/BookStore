using BookStore.Infra.Messaging;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Messaging
{
    public class MessageService : IMessageService
    {
        private readonly Mail mail;
        public MessageService(IOptions<Mail> mail)
        {
            this.mail = mail.Value;
        }

        public async Task SendEmailAsync(MessageOptions message,
            params Attachment[] attachments)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(message.fromDisplayName, message.fromEmailAddress));
            email.To.Add(new MailboxAddress(message.toEamilAddress));
            email.Subject = message.subjcet;
            var body = new BodyBuilder()
            {
                HtmlBody = message.message
            };

            foreach (var attachment in attachments)
            {
                using (var stream = await attachment.ContentToStreamAsync())
                {
                    body.Attachments.Add(attachment.FileName, stream);
                }
            }
            email.Body = body.ToMessageBody();
            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                client.ServerCertificateValidationCallback =
                    (sender, certificate, certchainType, errors) => true;
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                await client.ConnectAsync(mail.Server, mail.Port, false).ConfigureAwait(false);
                await client.AuthenticateAsync(mail.UserName, mail.Password).ConfigureAwait(false);
                await client.SendAsync(email).ConfigureAwait(false);
                await client.DisconnectAsync(true).ConfigureAwait(false);

            }
        }


    }
}
