using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace ANPRWeb.Helpers
{
    public static class EmailUtility
    {
        public static bool SendMail(string emailTo, string subject, string body, string[] attachmentFiles = null, string emailFrom = "", string emailCC = "", string emailBCC = "", MailPriority priority = MailPriority.Normal)
        {
            if (string.IsNullOrWhiteSpace(emailTo))
                return false;
            try
            {
                MailMessage mail = new MailMessage();

                string[] add = emailTo.Split(';');
                foreach (var item in add)
                {
                    mail.To.Add(new MailAddress(item));
                }

                if (!string.IsNullOrWhiteSpace(emailCC))
                {
                    string[] cc = emailCC.Split(';');
                    foreach (var item in cc)
                    {
                        mail.CC.Add(new MailAddress(item));
                    }
                }
                if (!string.IsNullOrWhiteSpace(emailBCC))
                {
                    string[] bcc = emailBCC.Split(';');
                    foreach (var item in bcc)
                    {
                        mail.Bcc.Add(new MailAddress(item));
                    }
                }

                mail.Subject = subject;
                string Body = body + "</br></br>Please do not reply. This is an automated system message.";
                mail.Body = Body;
                mail.IsBodyHtml = true;
                mail.Priority = priority;

                if (attachmentFiles != null)
                {
                    foreach (var file in attachmentFiles)
                    {
                        if (File.Exists(file))
                        {
                            Attachment attachmentItem = new Attachment(file);
                            mail.Attachments.Add(attachmentItem);
                        }
                    }
                }

                mail.From = new MailAddress("abirami@gmail.com");

                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.UseDefaultCredentials = false;
                smtp.Send(mail);
                smtp.Dispose();
                mail.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                // Log.Error(ex);
                return false;
            }
        }

    }
}