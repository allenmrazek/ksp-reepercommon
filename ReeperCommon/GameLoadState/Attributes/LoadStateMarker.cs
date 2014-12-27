using System;

namespace ReeperCommon.GameLoadState.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class LoadStateMarker : Attribute
    {
        public enum State
        {
            Immediate,
            FileTreeReady,
            AfterTexturesLoaded,
            AfterModelsLoaded,
            MainMenu,
            SpaceCenter,
            Flight
        }

        public State when;

        public LoadStateMarker(State when = State.Immediate)
        {
            this.when = when;
        }
    }
}
