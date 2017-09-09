using System;

namespace T.Issue.Commons.Model
{
    public abstract class EqualityAndHashCodeProvider<T, TKey> : IEquatable<T> where T : EqualityAndHashCodeProvider<T, TKey>
    {
        public abstract TKey Id { get; set; }

        public override int GetHashCode()
        {
            return Id?.GetHashCode() ?? base.GetHashCode();
        }

        public bool Equals(T other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return Equals(Id, other.Id) && Id != null && other.Id != null;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != GetType())
            {
                return false;
            }
            return Equals((T) obj);
        }
    }
}