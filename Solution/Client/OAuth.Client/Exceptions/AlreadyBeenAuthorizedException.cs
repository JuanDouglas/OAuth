using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Client.Exceptions
{

    [Serializable]
    public class AlreadyBeenAuthorizedException : Exception
    {
        public AlreadyBeenAuthorizedException() { }
        public AlreadyBeenAuthorizedException(string message) : base(message) { }
        public AlreadyBeenAuthorizedException(string message, Exception inner) : base(message, inner) { }
        protected AlreadyBeenAuthorizedException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
