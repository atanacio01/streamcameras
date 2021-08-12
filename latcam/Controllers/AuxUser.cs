using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using latcam.Models;

using System.Net.Mail;
using latcam.Models;
using latcam.Results;


namespace latcam.Controllers
{
    public static class AuxUser
    {
        public static void getUser(string email)
        {
            try
            {
                latincamEntities context = new latincamEntities();
                Random random = new Random();
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                string token = new string(Enumerable.Repeat(chars, 4)
                  .Select(s => s[random.Next(s.Length)]).ToArray());

                AspNetUsers getUser = (from c in context.AspNetUsers where c.Email == email select c).FirstOrDefault();
                getUser.ActivationCode = token;
                context.SaveChanges();

                try
                {
                    //Set SmtpClient to send Email
                    string stFromUserName = "noreply@mail.com";
                    string stFromPassword = "asdasda";
                    int inPort = Convert.ToInt32(587);
                    string stHost = "mail.mail.com";
                    bool btIsSSL = true;

                    MailAddress to = new MailAddress(email);
                    MailAddress from = new MailAddress("noreply@mail.com");

                    MailMessage objEmail = new MailMessage(from, to);
                    objEmail.Subject = "Email validation";
                    objEmail.Body = "please use the following code to confirm your identity: " + token;
                    objEmail.IsBodyHtml = true;
                    objEmail.Priority = MailPriority.High;

                    SmtpClient client = new SmtpClient();
                    System.Net.NetworkCredential auth = new System.Net.NetworkCredential(stFromUserName, stFromPassword);
                    client.Host = stHost;
                    client.Port = inPort;
                    client.UseDefaultCredentials = false;
                    client.Credentials = auth;
                    client.EnableSsl = btIsSSL;
                    client.Send(objEmail);

                }
                catch (Exception ex)
                {

                }



            }
            catch (Exception ex)
            {

            }
        }
    }
}