using System.Reflection;

namespace T.Issue.Bootstrapper.Model
{
    public class ItemLocation
    {
        public Assembly Assembly { get; set; }
        public string Location { get; set; }

        public override string ToString()
        {
            return Assembly.GetName().Name + "." + Location;
        }
    }
}
