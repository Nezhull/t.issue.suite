namespace T.Issue.Commons.Extensions
{
    public static class ObjectExtensions
    {
        public static bool NotNull(this object obj)
        {
            return null != obj;
        }

        public static bool Null(this object obj)
        {
            return null == obj;
        }
    }
}
