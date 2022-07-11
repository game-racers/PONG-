using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PONG.Control
{
    public class PaddleMagicTrigger : MonoBehaviour
    {
        int magicNum = 0;

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Ball>() != null)
                EventManager.MagicBall(transform.parent.gameObject, other.GetComponent<Ball>().GetMagicBall());
        }

        private void OnTriggerExit(Collider other)
        {
            EventManager.PaddleBallCollision(other.gameObject, magicNum);
        }

        public void SetMagic(int newMagic)
        {
            magicNum = newMagic;
        }
    }

}
