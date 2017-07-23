using System;

namespace T.Issue.Bootstrapper.Model
{
    public class AppliedItem
    {
        public object Id { get; set; }
        public ItemType Type { get; set; }
        public string Version { get; set; }
        public string Name { get; set; }
        public object Content { get; set; }
        public DateTime Executed { get; set; }
        public string Checksum { get; set; }
    }
}

