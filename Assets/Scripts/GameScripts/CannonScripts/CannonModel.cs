using System.Collections.Generic;
using UnityEngine;

namespace GameScripts.CannonScripts
{
    public class CannonModel
    {
        public Queue<GameObject> AmmoQueue = new ();

        public void LoadLevelAmmo(List<GameObject> levelBugs)
        {
            foreach (var bug in levelBugs)
            {
                AmmoQueue.Enqueue(bug);
            }
        }
    }
}