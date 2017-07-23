using T.Issue.Bootstrapper.Model;

namespace T.Issue.DB.Migrator.Utils
{
    internal static class ScriptTypeUtils
    {
        public static ItemType ResolveType(string typeStr)
        {
            switch (typeStr)
            {
                case "V":
                    return ItemType.Versioned;
                case "R":
                    return ItemType.Repeatable;
                default:
                    return ItemType.Versioned;
            }
        }

        public static string ResolveString(ItemType type)
        {
            switch (type)
            {
                case ItemType.Repeatable:
                    return "R";
                case ItemType.Versioned:
                    return "V";
                default:
                    return "V";
            }
        }
    }
}
