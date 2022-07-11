using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PONG.Game
{
    public class Bounds : MonoBehaviour
    {
        [SerializeField] LayerMask ballMask;

        private void OnTriggerExit(Collider other)
        {
            if ((ballMask & (1 << other.gameObject.layer)) != 0)
            {
                EventManager.BallBreak(other.gameObject);
            }
        }
    }
}
