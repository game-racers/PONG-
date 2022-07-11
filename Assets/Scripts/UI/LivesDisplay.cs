using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PONG.UI
{
    public class LivesDisplay : MonoBehaviour
    {
        [SerializeField] Image[] lives;

        public void UpdateLives(int newLives)
        {
            for (int i = 0; i < lives.Length; i++)
            {
                if (i < newLives)
                {
                    lives[i].enabled = true;
                }
                else
                {
                    lives[i].enabled = false;
                }
            }
        }
    }
}
