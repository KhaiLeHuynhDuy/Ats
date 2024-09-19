using HtmlAgilityPack;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Ats.Models.Commons;
using Microsoft.Extensions.Options;

namespace Ats.Service.EmailEngine
{
    public interface IEmailSender
    {
        Task<MailMessage> BuildMailMessageAsync(string viewpath, object model);
        Task SendAsync(MailMessage msg);
    }

    public class EmailSender : IEmailSender
    {
        private IViewRenderService _viewRenderService;
        //private ILoggerManager Logger { get; set; }
        private SmtpConfig SmtpConfig { get; }

        public EmailSender(IViewRenderService viewRenderService/*, ILoggerManager logger*/, IOptions<SmtpConfig> smtpConfig)
        {
            _viewRenderService = viewRenderService;
            //Logger = logger;
            SmtpConfig = smtpConfig.Value;
        }

        public async Task<MailMessage> BuildMailMessageAsync(string viewpath, object model)
        {
            MailMessage msg = null;

            try
            {
                var content = await _viewRenderService.RenderToStringAsync(viewpath, model);

                if (!string.IsNullOrEmpty(content))
                {
                    msg = new MailMessage();

                    HtmlDocument document = new HtmlDocument();
                    document.LoadHtml(content);
                    HtmlNodeCollection collection = document.DocumentNode.SelectNodes("//section");
                    foreach (HtmlNode section in collection)
                    {
                        if (section.Id.ToLower() == "header")
                        {
                            HtmlNodeCollection divs = document.DocumentNode.SelectNodes("//div");

                            string emailFromDisplayName = string.Empty;
                            string emailFromAddress = string.Empty;
                            string emailToAddresses = string.Empty;
                            string emailCcAddresses = string.Empty;
                            string emailBccAddresses = string.Empty;
                            string emailSubject = string.Empty;
                            bool isHtml = true;

                            foreach (HtmlNode div in divs)
                            {
                                if (div.Attributes.Count == 0 || div.Attributes["name"] == null) continue;

                                string target = div.Attributes["name"].Value;
                                string val = div.InnerText;

                                switch (target.ToLower())
                                {
                                    case "name":
                                        emailFromDisplayName = val;
                                        break;

                                    case "from":
                                        emailFromAddress = val;
                                        break;

                                    case "to":
                                        emailToAddresses = val;
                                        break;

                                    case "cc":
                                        emailCcAddresses = val;
                                        break;

                                    case "bcc":
                                        emailBccAddresses = val;
                                        break;

                                    case "subject":
                                        emailSubject = val;
                                        break;

                                    case "ishtml":
                                        bool.TryParse(val, out isHtml);
                                        break;
                                };
                            }

                            msg.From = new MailAddress(emailFromAddress, emailFromDisplayName);

                            if (!string.IsNullOrEmpty(emailToAddresses))
                            {
                                string[] emailAddresses = emailToAddresses.Split(';');
                                foreach (string emailAddress in emailAddresses)
                                {
                                    if (Commons.Ultility.IsValidEmailAddress(emailAddress))
                                        msg.To.Add(new MailAddress(emailAddress.Trim()));
                                }
                            }

                            if (!string.IsNullOrEmpty(emailCcAddresses))
                            {
                                string[] emailAddresses = emailCcAddresses.Split(';');
                                foreach (string emailAddress in emailAddresses)
                                {
                                    if (Commons.Ultility.IsValidEmailAddress(emailAddress))
                                        msg.CC.Add(new MailAddress(emailAddress.Trim()));
                                }
                            }

                            if (!String.IsNullOrEmpty(emailBccAddresses))
                            {
                                string[] emailAddresses = emailBccAddresses.Split(';');
                                foreach (string emailAddress in emailAddresses)
                                {
                                    if (Commons.Ultility.IsValidEmailAddress(emailAddress))
                                        msg.Bcc.Add(new MailAddress(emailAddress.Trim()));
                                }
                            }

                            msg.Subject = emailSubject;
                            msg.IsBodyHtml = isHtml;
                        }
                        else if (section.Id.ToLower() == "body")
                        {
                            msg.Body = section.InnerHtml;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return msg;
        }

        public async Task SendAsync(MailMessage msg)
        {
            if (msg == null)
                throw new ArgumentNullException("msg");
            try
            {
                msg.From = new MailAddress(SmtpConfig.User, SmtpConfig.DisplayName);

                using (var client = new SmtpClient())
                {
                    client.Host = SmtpConfig.Server;
                    client.Port = SmtpConfig.Port;
                    client.UseDefaultCredentials = SmtpConfig.UseDefaultCredentials;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.EnableSsl = SmtpConfig.EnableSsl;
                    client.Credentials = new NetworkCredential(SmtpConfig.User, SmtpConfig.Password);

                    await client.SendMailAsync(msg);
                }
            }
            catch (Exception ex)
            {
                //Logger.LogError($"Error send email: {ex}");
            }

        }
    }
}