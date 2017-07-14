using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailClient
{
    class Program
    {
        // Using this for mainly test purposes to confirm the email sending works
        static void Main(string[] args)
        {
            EmailClient emailClient = new EmailClient();
            emailClient.ConstructEmail(new List<string>(new string[] { "email@email.com" }), "Hello from visual studio!", 
                "testing this email refactor");

            emailClient.Send();
        }
    }
}
