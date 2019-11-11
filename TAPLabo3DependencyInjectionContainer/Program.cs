using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmailSenderInterfaces;
using TinyDependencyInjectionContainer;

namespace TAPLabo3DependencyInjectionContainer {
    class Program {
        static void Main(string[] args) {
            var resolver = new InterfaceResolver("TinyDICConfig.txt");
            var sender = resolver.Instantiate<IEmailSender>();
            sender.SendEmail("pippo", "pluto");
        }
    }
}
