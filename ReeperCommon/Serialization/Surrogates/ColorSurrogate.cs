﻿using UnityEngine;

namespace ReeperCommon.Serialization.Surrogates
{
// ReSharper disable once UnusedMember.Global
    public class ColorSurrogate : FieldSurrogateToSingleValueBase<Color>
    {
        protected override string GetFieldContentsAsString(Color instance)
        {
            return ConfigNode.WriteColor(instance);
        }

        protected override Color GetFieldContentsFromString(string value)
        {
            return ConfigNode.ParseColor(value);
        }
    }
}
