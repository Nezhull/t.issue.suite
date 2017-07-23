using T.Issue.Commons.Utils;

namespace T.Issue.Commons.Check
{
    public class Check : ICheck
	{
		public void NotNull(object obj, string paramName)
		{
			Assert.NotNull(obj, paramName);
		}

		public void NotNullEmpty(string s, string paramName)
		{
			Assert.IsNotEmpty(s, paramName);
		}

		public void NotNullWhiteSpace(string s, string paramName)
		{
            Assert.IsNotWhiteSpace(s, paramName);
		}

		public void NotNullEmptyWhiteSpace(string s, string paramName)
		{
            Assert.HasText(s, paramName);
		}
	}
}
