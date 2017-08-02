using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace T.Issue.Commons.Enum
{
    /// <summary>
    /// Base class for simulating Java style enums.
    /// </summary>
    /// <typeparam name="T">Actual enum type.</typeparam>
    public abstract class EnumBase<T> where T : EnumBase<T>
    {
        protected static List<T> InnerValues = new List<T>();

        public static ReadOnlyCollection<T> Values { get; } = InnerValues.AsReadOnly();

        public virtual int Value { get; }

        protected EnumBase(int value)
        {
            Value = value;
            InnerValues.Add((T) this);
        }

        protected static T Resolve(int? value)
        {
            return Values.SingleOrDefault(v => v.Value == value);
        }

        public static implicit operator EnumBase<T>(int value)
        {
            return Resolve(value);
        }

        public static implicit operator int?(EnumBase<T> enm)
        {
            return enm?.Value;
        }

        public static implicit operator EnumBase<T>(int? value)
        {
            return Resolve(value);
        }
    }
}
