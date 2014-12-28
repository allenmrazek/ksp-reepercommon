using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReeperCommon.GameLoadState
{
    class GameDatabaseFinished : LoadingSystem
    {
        public override bool IsReady()
        {
            return true;
        }

        public override void StartLoad()
        {
            gameObject.GetComponent<LoadState_TexturesLoaded>().Ready();
        }
    }

    class LoadState_TexturesLoaded : GameLoadStateObserver
    {
        void Start()
        {
            var ls = FindObjectOfType<LoadingScreen>();
            var fakeLoader = gameObject.AddComponent<GameDatabaseFinished>();
            

            for (int i = 0; i < ls.loaders.Count; ++i)
                if (ls.loaders[i].GetType() == typeof (GameDatabase))
                {
                    ls.loaders.Insert(i + 1, fakeLoader);
                    break;
                }
        }

        public void Ready()
        {
            Receiver.CreateTypesFor(Attributes.LoadStateMarker.State.AfterTexturesLoaded);
        }
    }
}
