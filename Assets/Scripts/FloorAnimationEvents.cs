using System;
using GameScripts.BuildingScripts;
using UnityEngine;

namespace GameScripts
{
    public class FloorAnimationEvents : MonoBehaviour
    {
        public FloorView ParentFloor; 

        public Action<FloorView> OnAnimationEventTriggered;

        public void OnHideFloorEvent()
        {
            OnAnimationEventTriggered?.Invoke(ParentFloor);
        }
    }
}