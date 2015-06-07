namespace ReeperCommon.Gui
{
    public class UniqueWindowIdProvider : IWindowIdProvider
    {
        private static int _id = 15000;

        public int Get()
        {
            return _id++;
        }
    }
}
