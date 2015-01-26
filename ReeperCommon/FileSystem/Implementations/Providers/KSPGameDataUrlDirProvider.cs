using System.Linq;

namespace ReeperCommon.FileSystem.Implementations.Providers
{
    public class KSPGameDataUrlDirProvider : IUrlDirProvider
    {
        public UrlDir Get()
        {
            return GameDatabase.Instance.root.children.
                FirstOrDefault(u => u.path.EndsWith("\\GameData") || u.path.EndsWith("/GameData"));
        }
    }
}
