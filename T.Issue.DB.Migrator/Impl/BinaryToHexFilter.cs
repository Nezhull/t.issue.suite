using System.IO;
using System.Linq;
using DotLiquid;
using T.Issue.Bootstrapper.Model;
using T.Issue.Commons.Utils;

namespace T.Issue.DB.Migrator.Impl
{
    public static class BinaryToHexFilter
    {
        private const string HexPrefix = "0x";

        public const string CurrentLocation = "_currentLocation";

        public static string ResourceToHex(Context context, string resourceName, string location)
        {
            Assert.HasText(resourceName);

            ItemLocation currentlocation = FindCurrentLocation(context);
            Assert.NotNull(currentlocation);

            string fullName = currentlocation + (!string.IsNullOrEmpty(location) ? "." + location : "") + "." + resourceName;
            Assert.IsTrue(currentlocation.Assembly.GetManifestResourceNames().Contains(fullName));

            using (Stream stream = currentlocation.Assembly.GetManifestResourceStream(fullName))
            {
                Assert.NotNull(stream);

                using (MemoryStream ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    return HexPrefix + StringUtils.ByteArrayToHex(ms.ToArray());
                }
            }
        }

        private static ItemLocation FindCurrentLocation(Context context)
        {
            foreach (var environment in context.Environments)
            {
                object location;
                if (environment.TryGetValue(CurrentLocation, out location))
                {
                    return location as ItemLocation;
                }
            }

            return null;
        }
    }
}
