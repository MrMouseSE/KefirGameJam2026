using System;
using GameScripts.BuildingScripts;
using UnityEngine;

namespace GameScripts.Descriptions
{
    [Serializable]
    public class SpriteColorByBuildingColor
    {
        public BuildingColors BuildingColor;
        public Color SpriteColorValue;
    }
}