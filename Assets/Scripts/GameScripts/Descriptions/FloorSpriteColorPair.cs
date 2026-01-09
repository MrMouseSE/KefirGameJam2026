using System;
using System.Collections.Generic;
using GameScripts.BuildingScripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameScripts.Descriptions
{
    [Serializable]
    public class FloorSpriteColorPair
    {
        public FloorView FloorPrefab; 
        public float FloorHeight;
        public float BugYOffsetAtFloor;
        public float FloorXOffset;
        public Vector3 ColliderOffset = new (0f, 0f, 0f);
        public List<BuildingColors> BuildingColors;
        public Sprite FloorSprite;
        public Sprite FloorDestructionSprite;
    }
}