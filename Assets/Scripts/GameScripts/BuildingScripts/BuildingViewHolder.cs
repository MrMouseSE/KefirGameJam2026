using System;
using UnityEngine;

namespace GameScripts.BuildingScripts
{
    [Serializable]
    public class BuildingViewHolder
    {
        public Transform BuildingPlace;
        public bool IsHolderOccupied;
        public BuildingView BuildingView;
    }
}