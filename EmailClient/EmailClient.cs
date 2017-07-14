using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Net;
using System.Configuration;

namespace EmailClient
{
    public class EmailClient : SmtpClient
    {
        string username;
        string password;
        int port;
        bool emailConstructed;
        List<MailMessage> email;

        /// <summary>
        /// This needs to be updated with the ONYX from Office 365
        /// </summary>
        public EmailClient()
        {
            username = ConfigurationManager.AppSettings.Get("EmailLogin");
            password = ConfigurationManager.AppSettings.Get("EmailPassword");
            Host = ConfigurationManager.AppSettings.Get("EmailHost");
            port = int.Parse(ConfigurationManager.AppSettings.Get("EmailPort"));
            EnableSsl = true;
            emailConstructed = false;
            Credentials = new NetworkCredential(username, password);
            email = new List<MailMessage>();
        }


        //This method creates an email to be sent. Note you can construct more than one and batch send
        public bool ConstructEmail(IEnumerable<string> toEmail, string subject, string body, string toAttach = null, List<string> toCCs = null, List<string> toBCCs = null)
        {
            try
            {
                MailMessage email = new MailMessage();

                foreach(string to in toEmail)
                    email.To.Add(new MailAddress(to));

                email.From = new MailAddress(username);
                email.Subject = subject;
                email.Body = body;
                email.IsBodyHtml = true;

                if (toCCs != null)
                {
                    if (toCCs.Count > 0)
                    {
                        foreach (string emailAddress in toCCs)
                        {
                            email.CC.Add(new MailAddress(emailAddress));
                        }
                    }
                }

                if (toBCCs != null)
                {
                    if (toBCCs.Count > 0)
                    {
                        foreach (string emailAddress in toBCCs)
                        {
                            email.Bcc.Add(new MailAddress(emailAddress));
                        }
                    }
                }

                if (toAttach != null)
                {
                    if (File.Exists(toAttach))
                    {
                        Attachment att = new Attachment(toAttach, MediaTypeNames.Application.Octet);
                        email.Attachments.Add(att);
                    }
                    else
                    {
                        Console.WriteLine(toAttach + " NOT FOUND");
                        return false;
                    }
                }
                this.email.Add(email);
                emailConstructed = true;
                return true;
            }

            catch (Exception e)
            {
                Console.WriteLine(toEmail + " is not a valid email address.");
                return false;
            }
        }

        //Send all messages
        public bool Send()
        {
            try
            {
                if (emailConstructed)
                {
                    foreach (MailMessage mail in email)
                    {
                        this.Send(mail);
                        string sentTo = mail.To.ToString();
                        Console.WriteLine("Email sent to: " + sentTo + '.');
                    }
                    return true;
                }
                else
                {
                    Console.WriteLine("No emails to send");
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Enter something to continue:");
                Console.ReadLine();
                return false;
            }
        }
    }
}