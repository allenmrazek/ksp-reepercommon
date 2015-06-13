//using System;
//using ReeperCommon.Serialization;
//using UnityEngine;

//namespace ReeperCommon.Utility
//{
//    [Serializable]
//    public struct PersistentRect
//    {
//        [ReeperPersistent] public float x, y, width, height;

//        public PersistentRect(float x, float y, float width, float height)
//        {
//            this.x = x;
//            this.y = y;
//            this.width = width;
//            this.height = height;
//        }

//        public static implicit operator Rect(PersistentRect pr)
//        {
//            return new Rect(pr.x, pr.y, pr.width, pr.height);
//        }

//        public static implicit operator PersistentRect(Rect r)
//        {
//            return new PersistentRect(r.x, r.y, r.width, r.height);
//        }
//    }
//}
