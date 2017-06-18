using Microsoft.Exchange.WebServices.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Web;

namespace ProDashBoard.Repository
{
    public class EmailSenderRepository
    {
        public EmailSenderRepository()
        {
            SendEmail("ishan.mahamuthupelage@gmail.com", "test", "test email");
        }

        public bool SendEmail(string recipient, string subject, string body)
        {
            string name = "";
            int count = 0;
            try
            {
                
                    ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2010);
                    service.Credentials = new WebCredentials("ishanm", "Malshika12345", "seranet");
                    service.AutodiscoverUrl("ishanm@99x.lk", delegate { return true; });
                //, delegate { return true; }
                service.TraceEnabled = true;
                EmailMessage message = new EmailMessage(service);

                    string mailBody = "<html>test</html>";


                    message.Subject = "test";

                    message.Body = mailBody;

                    message.ToRecipients.Add("tharindraj@99x.lk");
                    message.Save();
                    message.SendAndSaveCopy();
                    System.Diagnostics.Debug.WriteLine(mailBody);
                

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("failed " + ex.Message);
                
            }
            
            return true;
        }
    }
}