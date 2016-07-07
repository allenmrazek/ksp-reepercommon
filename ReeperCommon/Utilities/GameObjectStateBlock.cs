using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace ReeperCommon.Utilities
{
    [Serializable]
    public class GameObjectStateBlock
    {
        public List<GameObject> ItemsToHide = new List<GameObject>();

        [HideInInspector]
        private readonly List<KeyValuePair<GameObject, bool>> _cachedStates = new List<KeyValuePair<GameObject, bool>>();

        public GameObjectStateBlock()
        {
        }

        public GameObjectStateBlock([NotNull] IList<GameObject> gameObjects)
        {
            if (gameObjects == null) throw new ArgumentNullException("gameObjects");
            ItemsToHide = gameObjects.ToList();
        }


        public void Hide()
        {
            if (_cachedStates.Any()) Restore();

            foreach (var item in ItemsToHide)
            {
                _cachedStates.Add(new KeyValuePair<GameObject, bool>(item, item.activeSelf));
                item.SetActive(false);
            }
        }

        public void Restore()
        {
            if (_cachedStates.Any())
                _cachedStates.ForEach(state => state.Key.SetActive(state.Value));
            _cachedStates.Clear();
        }
    }
}
