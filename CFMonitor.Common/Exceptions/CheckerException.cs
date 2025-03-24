using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor.Exceptions
{
    public class CheckerException : Exception
    {          
        public CheckerException()
        {
        }

        public CheckerException(string message) : base(message)
        {
        }

        public CheckerException(string message, Exception innerException) : base(message, innerException)
        {
        }


        public CheckerException(string message, params object[] args)
            : base(string.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
