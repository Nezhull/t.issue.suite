using T.Issue.Commons.Extensions;
using Xunit;

namespace T.Issue.Commons.Test.Utils.Extensions
{
    public class ObjectExtensionsTests
    {
        [Fact]
        public void NotNull_returns_true_when_object_is_not_null()
        {
            var o = new object();
            var s = "string";

            Assert.True(o.NotNull(), "object failed");
            Assert.True(s.NotNull(), "string failed");
        }

        [Fact]
        public void NotNull_returns_false_when_object_is_null()
        {
            object o = null;
            string s = null;

            Assert.False(o.NotNull(), "null object failed");
            Assert.False(s.NotNull(), "null string failed");
        }

        [Fact]
        public void Null_returns_false_when_object_is_not_null()
        {
            var o = new object();
            var s = "string";

            Assert.False(o.Null(), "object failed");
            Assert.False(s.Null(), "string failed");
        }

        [Fact]
        public void Null_returns_true_when_object_is_null()
        {
            object o = null;
            string s = null;

            Assert.True(o.Null(), "null object failed");
            Assert.True(s.Null(), "null string failed");
        }
    }
}
