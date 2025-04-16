using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminsWorkSystem.Utilidades
{
    public class EmailSender : IEmailSender
    {
        public string SenGridSecret { get; set; }
        public EmailSender(IConfiguration _config)
        {
            SenGridSecret = _config.GetValue<string>("SenGrid:SecretKey");
        }
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SendGridClient(SenGridSecret);
            var from = new EmailAddress("vicntrobler360@hotmail.com");
            var to = new EmailAddress(email);
            var msg = MailHelper.CreateSingleEmail(from,to,subject, "",htmlMessage);

            return client.SendEmailAsync(msg);
        }
    }
}
