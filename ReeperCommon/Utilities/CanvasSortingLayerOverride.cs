using System;
using System.Linq;
using ReeperCommon.Containers;
using ReeperCommon.Logging;
using UnityEngine;

namespace ReeperCommon.Utilities
{
    /// <summary>
    /// There are a number of sorting layers defined inside KSP. Unfortunately, if defined manually inside Unity
    /// and loaded via AssetBundle, the ids become invalid and useless
    /// </summary>
    [RequireComponent(typeof(Canvas)), DisallowMultipleComponent]
    public class CanvasSortingLayerOverride : MonoBehaviour
    {
        public enum SortingLayerNames
        {
            Default = 0,
            Main,
            Apps,
            Actions,
            Dialogs,
            DragDrop,
            Tweening,
            Tooltips
        }

        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        // ReSharper disable once ConvertToConstant.Local
        [SerializeField] private SortingLayerNames _sortingLayer = SortingLayerNames.Default;


        private void Start()
        {
            GetComponent<Canvas>().Do(c =>
            {
                var layer = GetSortingLayer(_sortingLayer.ToString());

                if (!layer.Any())
                    Log.Error("Unable to set sorting layer to " + _sortingLayer + ": unrecognized");
                else c.sortingLayerID = layer.Value;
            });
        }


        private static Maybe<int> GetSortingLayer(string layerName)
        {
            if (string.IsNullOrEmpty(layerName)) throw new ArgumentException("cannot be null or empty", "layerName");

            var layers = SortingLayer.layers;

            for (int i = 0; i < layers.Length; ++i)
                if (string.CompareOrdinal(layerName, layers[i].name) == 0)
                    return layers[i].id.ToMaybe();
                    
            return Maybe<int>.None;
        }
    }
}
