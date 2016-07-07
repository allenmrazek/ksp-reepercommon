using ReeperCommon.Containers;
using UnityEngine;

namespace ReeperCommon.Utilities
{
    public class CoroutineHoster : MonoBehaviour
    {
        private static CoroutineHoster _instance;

        public static CoroutineHoster Instance
        {
            get
            {
                _instance = _instance ??
                            new GameObject("ReeperCommon.Utilities.CoroutineHoster").AddComponent<CoroutineHoster>()
                                .Do(ch => DontDestroyOnLoad(ch.gameObject));

                return _instance;
            }
        }
    }
}
