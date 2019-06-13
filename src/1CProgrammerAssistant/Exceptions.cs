using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1CProgrammerAssistant
{
    internal class CreateConnectionStringException : Exception
    {
    }
    internal class PingConnectionException : Exception
    {
        public PingConnectionException()
        {
        }

        public PingConnectionException(string message) : base(message)
        {
        }
    }
}
