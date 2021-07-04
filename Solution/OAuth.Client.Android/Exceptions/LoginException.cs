using System;

namespace OAuth.Client.Android.Exceptions
{
    [Serializable]
    public class LoginException : Exception
    {
        public string Field { get; set; }
        public const string UserField = "Login";
        public const string PasswordField = "Password";
        public const string NoneField = "NoField";
        public LoginException(string message) : this(NoneField, message)
        {

        }

        public LoginException(string field, string message) : base(message)
        {
            Field = field;
        }
        public LoginException(string message, Exception inner) : base(message, inner) { }
        protected LoginException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}