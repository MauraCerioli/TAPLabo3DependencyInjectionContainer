using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmailSenderInterfaces;

namespace EMailSenderImplementation2
{
    public class EmailSenderImplementationTwo:IEmailSender
    {
        public bool SendEmail(string to, string body){
            Console.WriteLine($"SendMailTwo send mail to {to} with body:\n{body}");
            return true;
        }
    }
}
