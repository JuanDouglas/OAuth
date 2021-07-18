using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Client.Android.Exceptions
{
    [Serializable]
    public class AuthorizationNotFoundException : Exception
    {
        public AuthorizationNotFoundException() { }
        public AuthorizationNotFoundException(string message) : base(message) { }
        public AuthorizationNotFoundException(string message, Exception inner) : base(message, inner) { }
        protected AuthorizationNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
