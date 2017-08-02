using Microsoft.VisualStudio.TestTools.UnitTesting;
using T.Issue.Commons.Enum;

namespace T.Issue.Commons.Utils.Tests
{
    [TestClass]
    public class EnumBaseTests
    {
        [TestMethod]
        public void TestEnumBaseEquals()
        {
            int? a = EnumTestType.A;
            int b = EnumTestType.B.Value;

            EnumTestType ae = (EnumTestType) a;
            EnumTestType be = (EnumTestType) b;

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(EnumTestType.A == ae);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(EnumTestType.B == be);

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsFalse(EnumTestType.A == 1);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsFalse(1 == EnumTestType.A);
        }

        [TestMethod]
        public void TestLabeledEquals()
        {
            int? a = EnumTestType2.A;
            int b = EnumTestType2.B.Value;

            EnumTestType2 ae = (EnumTestType2) a;
            EnumTestType2 be = (EnumTestType2) b;

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(EnumTestType2.A == ae);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(EnumTestType2.B == be);

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsFalse(EnumTestType2.A == 1);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsFalse(1 == EnumTestType2.A);
        }

        [TestMethod]
        public void TestEnumBaseValues()
        {
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(EnumTestType.Values.Contains(EnumTestType.A));
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(EnumTestType.Values.Contains(EnumTestType.B));
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(EnumTestType.Values.Contains(EnumTestType.C));
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(EnumTestType.Values.Contains(EnumTestType.D));
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
