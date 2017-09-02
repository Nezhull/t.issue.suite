namespace T.Issue.Bootstrapper.Model
{
    public class BootstrapItem
    {
        public virtual object Id { get; set; }
        public virtual ItemType Type { get; set; }
        public virtual string Version { get; set; }
        public virtual string Name { get; set; }
        public virtual object Content { get; set; }
        public virtual string Checksum { get; set; }
    }
}