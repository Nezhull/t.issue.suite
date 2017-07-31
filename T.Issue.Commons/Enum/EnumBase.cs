using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace T.Issue.Commons.Enum
{
    public abstract class EnumBase<T> : IEquatable<EnumBase<T>> where T : EnumBase<T>
    {
        protected static List<T> EnumValues = new List<T>();

        public virtual int Key { get; }
        public virtual string Value { get; }

        protected EnumBase(int key, string value)
        {
            Key = key;
            Value = value;
            EnumValues.Add((T) this);
        }

        protected static ReadOnlyCollection<T> GetBaseValues()
        {
            return EnumValues.AsReadOnly();
        }

        protected static T GetBaseByKey(int key)
        {
            foreach (T value in EnumValues)
            {
                if (value.Key == key) return value;
            }
            return null;
        }

        public bool Equals(EnumBase<T> other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return Key == other.Key;
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
            return Equals((EnumBase<T>) obj);
        }

        public override int GetHashCode()
        {
            return Key;
        }

        public override string ToString()
        {
            return Value;
        }

        public static implicit operator int(EnumBase<T> enm)
        {
            return enm.Key;
        }

        public static implicit operator EnumBase<T>(int key)
        {
            return GetBaseByKey(key);
        }

        public static implicit operator int?(EnumBase<T> enm)
        {
            return enm.Key;
        }

        public static implicit operator EnumBase<T>(int? key)
        {
            return key.HasValue ? GetBaseByKey(key.Value) : null;
        }
    }
}
