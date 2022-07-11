using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PONG.Game
{
    public class Goal : MonoBehaviour
    {
        [SerializeField] LayerMask ballMask;
        [SerializeField] bool isPlayerGoal;

        private void OnTriggerExit(Collider other)
        {
            if ((ballMask & (1 << other.gameObject.layer)) != 0)
            {
                EventManager.GoalHit(other.gameObject, isPlayerGoal);
            }
        }
    }
}
