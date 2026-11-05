using System;
using System.Collections.Generic;
using System.Linq;

namespace DonationHub.Domain.Common
{
    public abstract class ValueObject
    {
        protected abstract IEnumerable<object> GetValues();

        public override bool Equals(object? obj)
        {
            if (obj is not ValueObject other)
                return false;

            if (GetType() != other.GetType())
                return false;

            return GetValues().SequenceEqual(other.GetValues());
        }

        public override int GetHashCode()
        {
            return GetValues()
                .Aggregate( 0, (hash, value) => HashCode.Combine(hash, value));
        }

        public static bool operator ==( ValueObject? left, ValueObject? right)
        {
            return left is null? right is null: left.Equals(right);
        }

        public static bool operator !=( ValueObject? left, ValueObject? right)
        {
            return !(left == right);
        }
    }
}
