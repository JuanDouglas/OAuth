using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OAuth.Api.Models.Attributes
{
    public class RequireAuthenticationAttribute : Attribute, IEquatable<RequireAuthenticationAttribute>
    {
        public override bool Equals(object obj)
        {
            return Equals(obj as RequireAuthenticationAttribute);
        }

        public bool Equals(RequireAuthenticationAttribute other)
        {
            return other != null &&
                   base.Equals(other) &&
                   EqualityComparer<object>.Default.Equals(TypeId, other.TypeId);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), TypeId);
        }

        public static bool operator ==(RequireAuthenticationAttribute left, RequireAuthenticationAttribute right)
        {
            return EqualityComparer<RequireAuthenticationAttribute>.Default.Equals(left, right);
        }

        public static bool operator !=(RequireAuthenticationAttribute left, RequireAuthenticationAttribute right)
        {
            return !(left == right);
        }
    }
}
