using UnityEngine;
namespace ReeperCommon.GameLoadState
{


    public abstract class GameLoadStateObserver : MonoBehaviour
    {
        protected LoadStateMarkedTypeCreator Receiver;

        protected void Awake()
        {
            Receiver = GetComponent<LoadStateMarkedTypeCreator>();
        }
    }
}
