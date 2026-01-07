using System;
using System.Collections.Generic;
using GameScripts.BuildingScripts;
using UnityEngine;

namespace GameScripts.Descriptions
{
    [Serializable]
    public class FloorSpriteColorPair
    {
        public FloorView FloorPrefab;
        public float FloorHeight;
        public List<BuildingColors> BuildingColors;
        public Sprite FloorSprite;
    }
}