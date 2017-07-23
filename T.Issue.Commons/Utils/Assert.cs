using System;
using System.Collections;
using Generic = System.Collections.Generic;

namespace T.Issue.Commons.Utils
{
    /// <summary>
    /// Utility class for basic assertions.
    /// </summary>
    public static class Assert
    {
        public static void NotNull(object obj, string message = "Object is null!")
        {
            if (obj == null)
            {
                throw new AssertionException(AssertionType.NotNull, message);
            }
        }

        public static void IsTrue(bool expression, string message = "Expression is false!")
        {
            if (!expression)
            {
                throw new AssertionException(AssertionType.IsTrue, message);
            }
        }

        public static void HasText(string str, string message = "String is empty!")
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                throw new AssertionException(AssertionType.HasText, message);
            }
        }

        public static void IsNotEmpty(string str, string message = "String is empty!")
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new AssertionException(AssertionType.IsNotEmpty, message);
            }
        }

        public static void IsNotWhiteSpace(string str, string message = "String is empty!")
        {
            if (str == null || (string.Empty != str && string.Empty == str.Trim()))
            {
                throw new AssertionException(AssertionType.IsNotWhiteSpace, message);
            }
        }

        public static void IsNotEmpty<T>(T[] array, string message = "Array is empty!")
        {
            if (array == null || array.Length == 0)
            {
                throw new AssertionException(AssertionType.IsNotEmpty, message);
            }
        }

        public static void IsNotEmpty(ICollection collection, string message = "Collection is empty!")
        {
            if (collection == null || collection.Count == 0)
            {
                throw new AssertionException(AssertionType.IsNotEmpty, message);
            }
        }

        public static void IsNotEmpty<T>(Generic.ICollection<T> collection, string message = "Collection is empty!")
        {
            if (collection == null || collection.Count == 0)
            {
                throw new AssertionException(AssertionType.IsNotEmpty, message);
            }
        }
    }

    public class AssertionException : Exception
    {
        private const string MessageFormat = "{0}: Assertion failure! {1}";

        public AssertionType Type { get; }

        public AssertionException(AssertionType type) : this(type, string.Empty) { }

        public AssertionException(AssertionType type, string message) : this(type, message, null) { }

        public AssertionException(AssertionType type, string message, Exception cause) : base(string.Format(MessageFormat, type, message), cause)
        {
            Assert.NotNull(type);

            Type = type;
        }
    }

    public enum AssertionType
    {
        NotNull,
        IsTrue,
        HasText,
        IsNotEmpty,
        IsNotWhiteSpace
    }
}
