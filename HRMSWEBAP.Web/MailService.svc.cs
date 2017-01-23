using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.ServiceModel;
using System.ServiceModel.Activation;

namespace HRMSWEBAP.Web
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class MailService
    {
        [OperationContract]
        public bool SendEmail(String UserName = null, String UserEmail = null, String Message = null, string imgAttachment = null)
        {
            bool flag = false;
            try
            {
                #region
                //string Filler = "";
                //string MsgBody = "";

                //MsgBody += "Name:" + Filler.PadLeft(5) + ClientName.Trim() + Environment.NewLine + Environment.NewLine;
                //MsgBody += "Email:" + Filler.PadLeft(4) + ClientEmail.ToString() + Environment.NewLine + Environment.NewLine;
                //MsgBody += "Phone:" + Filler.PadLeft(4) + ClientPhone + Environment.NewLine + Environment.NewLine;
                //MsgBody += "Comments:" + Environment.NewLine + Environment.NewLine;
                //MsgBody += Comment + Environment.NewLine;

                //MailMessage mail = new MailMessage("nithinvelluva@gmail.com", "mohithp003@gmail.com");
                //SmtpClient client = new SmtpClient();
                //client.Port = 25;
                //client.DeliveryMethod = SmtpDeliveryMethod.Network;
                //client.UseDefaultCredentials = false;
                //client.Host = "smtp.gmail.com";
                //mail.Subject = "Monthly Report-" + Comment + "HRMS";
                //mail.Body = "this is my test email body";
                //client.Send(mail);
                #endregion

                string mailfrom = "tnoreply001@gmail.com";
                string mailTo = UserEmail;

                MailMessage email = new MailMessage();

                email.Subject = "Monthly Report-" + UserName + " HRMS";
                email.From = new MailAddress(mailfrom);
                email.To.Add(new MailAddress(mailTo));
                email.IsBodyHtml = true;
                email.Body = "<div style=\"font-family:Arial\">Report Is Attached Below<br /><br /><img src=\"@@IMAGE@@\" alt=\"INLINE attachment\"><br /><br /></div>";

                // create the INLINE attachment

                //  string attachmentPath = Environment.CurrentDirectory + @"\test.png";
                string attachmentPath = imgAttachment;

                //Attachment inline = new Attachment(new MemoryStream(imgAttachment[0]),"");
                Attachment inline = new Attachment(attachmentPath);

                // generate the contentID string using the datetime
                string contentID = Path.GetFileName(attachmentPath).Replace(".", "") + "@zofm";

                inline.ContentDisposition.Inline = true;
                inline.ContentDisposition.DispositionType = DispositionTypeNames.Inline;
                inline.ContentId = contentID;
                inline.ContentType.MediaType = "image/png";
                inline.ContentType.Name = Path.GetFileName(attachmentPath);
                email.Attachments.Add(inline);

                // replace the tag with the correct content ID
                email.Body = email.Body.Replace("@@IMAGE@@", "cid:" + contentID);

                SmtpClient client = new SmtpClient("smtp.gmail.com", 25);
                //client.Port = 25;
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                //client.Host = "smtp.gmail.com";
                client.Credentials = new NetworkCredential("tnoreply001@gmail.com", "nithin837");
                client.Timeout = 360000;
                client.Send(email);
               
                flag = true;
            }
            catch (Exception ex)
            {
                flag = false;
            }


            return flag;
        }

          [OperationContract]

        public bool SendQueryMail(string Username=null,string userEmail=null,string subject=null,string message=null)
        {
            bool flag = false;
            try
            {
                string  mailTo= "tnoreply001@gmail.com";
                string mailfrom = userEmail;

                MailMessage email = new MailMessage();

                email.Subject = subject;
                email.From = new MailAddress(mailfrom);
                email.To.Add(new MailAddress(mailTo));
                email.Body = message;

                SmtpClient client = new SmtpClient("smtp.gmail.com", 25);
                //client.Port = 25;
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                //client.Host = "smtp.gmail.com";
                client.Credentials = new NetworkCredential("tnoreply001@gmail.com", "nithin837");
                client.Timeout = 360000;
                client.Send(email);

                flag = true;

            }
              catch(Exception ex)
            {
                flag = false;
            }

            return flag;
        }
    }
}
