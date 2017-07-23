namespace T.Issue.Bootstrapper.Model
{
    public class ClasspathItem
    {
        public object Id { get; set; }
        public ItemType Type { get; set; }
        public string Version { get; set; }
        public string Name { get; set; }
        public object Content { get; set; }
        public string Checksum { get; set; }
    }
}