using System;
using T.Issue.Commons.Enum;
using Xunit;

namespace T.Issue.Commons.Test.Enum
{
    public class EnumBaseTests
    {
        [Fact]
        public void TestEnumBaseEquals()
        {
            int? a = EnumTestType.A;
            int b = EnumTestType.B.Value;
            Int32 c = EnumTestType.C.Value;

            EnumTestType ae = (EnumTestType) a;
            EnumTestType be = (EnumTestType) b;
            EnumTestType bc = (EnumTestType) c;

            Assert.True(EnumTestType.A == ae);
            Assert.True(EnumTestType.B == be);
            Assert.True(EnumTestType.C == bc);

            Assert.False(EnumTestType.A == 1);
            Assert.False(1 == EnumTestType.A);
        }

        [Fact]
        public void TestLabeledEquals()
        {
            int? a = EnumTestType2.A;
            int b = EnumTestType2.B.Value;

            EnumTestType2 ae = (EnumTestType2) a;
            EnumTestType2 be = (EnumTestType2) b;

            Assert.True(EnumTestType2.A == ae);
            Assert.True(EnumTestType2.B == be);

            Assert.False(EnumTestType2.A == 1);
            Assert.False(1 == EnumTestType2.A);
        }

        [Fact]
        public void TestEnumBaseValues()
        {
            Assert.True(EnumTestType.Values.Contains(EnumTestType.A));
            Assert.True(EnumTestType.Values.Contains(EnumTestType.B));
            Assert.True(EnumTestType.Values.Contains(EnumTestType.C));
            Assert.True(EnumTestType.Values.Contains(EnumTestType.D));
        }
    }

    public class EnumTestType : EnumBase<EnumTestType>
    {
        public static readonly EnumTestType A = new EnumTestType(0, "A");
        public static readonly EnumTestType B = new EnumTestType(1, "B");
        public static readonly EnumTestType C = new EnumTestType(2, "C");
        public static readonly EnumTestType D = new EnumTestType(3, "D");

        public string Description { get; }

        public EnumTestType(int value, string description) : base(value)
        {
            Description = description;
        }
    }

    public class EnumTestType2 : LabeledEnum<EnumTestType2>
    {
        public static readonly EnumTestType2 A = new EnumTestType2(0, "a", "A");
        public static readonly EnumTestType2 B = new EnumTestType2(1, "b", "B");
        public static readonly EnumTestType2 C = new EnumTestType2(2, "c", "C");
        public static readonly EnumTestType2 D = new EnumTestType2(3, "d", "D");

        public string Description { get; }

        public EnumTestType2(int value, string label, string description) : base(value, label)
        {
            Description = description;
        }
    }
}
