using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSenderInterfaces
{
    public interface IEmailSender {
        bool SendEmail(string to, string body);
    }
}
