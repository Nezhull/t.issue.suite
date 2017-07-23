using T.Issue.Commons.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace T.Issue.Commons.Tests.Utils.Extensions
{
    [TestClass]
    public class ObjectExtensionsTests
    {
        [TestMethod]
        public void NotNull_returns_true_when_object_is_not_null()
        {
            var o = new object();
            var s = "string";

            Assert.IsTrue(o.NotNull(), "object failed");
            Assert.IsTrue(s.NotNull(), "string failed");
        }

        [TestMethod]
        public void NotNull_returns_false_when_object_is_null()
        {
            object o = null;
            string s = null;

            Assert.IsFalse(o.NotNull(), "null object failed");
            Assert.IsFalse(s.NotNull(), "null string failed");
        }

        [TestMethod]
        public void Null_returns_false_when_object_is_not_null()
        {
            var o = new object();
            var s = "string";

            Assert.IsFalse(o.Null(), "object failed");
            Assert.IsFalse(s.Null(), "string failed");
        }

        [TestMethod]
        public void Null_returns_true_when_object_is_null()
        {
            object o = null;
            string s = null;

            Assert.IsTrue(o.Null(), "null object failed");
            Assert.IsTrue(s.Null(), "null string failed");
        }
    }
}
