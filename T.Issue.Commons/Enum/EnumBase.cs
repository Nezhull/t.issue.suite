using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace T.Issue.Commons.Enum
{
    public abstract class EnumBase<T> where T : EnumBase<T>
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

        public override string ToString()
        {
            return Value;
        }
    }
}
