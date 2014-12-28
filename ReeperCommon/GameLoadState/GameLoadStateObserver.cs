using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
