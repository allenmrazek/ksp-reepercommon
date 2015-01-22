using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReeperCommon.Gui.Window.Providers
{
    public class UniqueWindowIdProvider
    {
        private static int _id = 15000;

        public int Get()
        {
            return _id++;
        }
    }
}
