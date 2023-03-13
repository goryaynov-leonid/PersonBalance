using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonBalance.Web.Exceptions
{
    public class EntityHasChangedException : Exception
    {
        public EntityHasChangedException(string message) : base(message) { }
        public EntityHasChangedException(string message, Exception innerException) : base(message, innerException) { }
    }
}