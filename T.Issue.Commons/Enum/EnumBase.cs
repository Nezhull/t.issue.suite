﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace T.Issue.Commons.Enum
{
    /// <summary>
    /// Base class for simulating Java style enums.
    /// </summary>
    /// <typeparam name="T">Actual enum type.</typeparam>
    public abstract class EnumBase<T> where T : EnumBase<T>
    {
        protected static List<T> InnerValues = new List<T>();

        private static readonly ReadOnlyCollection<T> values = InnerValues.AsReadOnly();
        private static readonly Type enumType = typeof(T);

        public static ReadOnlyCollection<T> Values
        {
            get
            {
                Init();
                return values;
            }
        }

        public virtual int Value { get; }

        protected EnumBase(int value)
        {
            Value = value;
            InnerValues.Add((T) this);
        }

        public static T Resolve(int? value)
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

        private static void Init()
        {
            RuntimeHelpers.RunClassConstructor(enumType.TypeHandle);
        }
    }
}
