using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmailSenderInterfaces;

namespace EmailSenderImplementation1
{
    public class EMailSenderImplementationOne:IEmailSender
    {
        public bool SendEmail(string to, string body){
            Console.WriteLine($"SendMailOne send mail to {to} with body:\n{body}");
            return true;
        }
    }
}
