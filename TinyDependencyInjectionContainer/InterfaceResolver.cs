using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyDependencyInjectionContainer
{
    public class InterfaceResolver{
        public InterfaceResolver(string configFileName){}
        public T Instantiate<T>() where T : class{
            return null;
        }
    }
}
