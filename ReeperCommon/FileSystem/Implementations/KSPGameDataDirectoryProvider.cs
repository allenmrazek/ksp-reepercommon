using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReeperCommon.FileSystem.Implementations
{
    public class KSPGameDataDirectoryProvider : IGameDataPathQuery
    {
        public UrlDir Directory()
        {
            return GameDatabase.Instance.root.children.
                FirstOrDefault(u => u.path.EndsWith("\\GameData") || u.path.EndsWith("/GameData"));
        }
    }
}
