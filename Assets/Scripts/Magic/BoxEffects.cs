using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PONG.Magic
{
    public class BoxEffects : MonoBehaviour
    {
        [SerializeField] LayerMask ballMask;
        [SerializeField] LayerMask paddlesMask;
        [SerializeField] bool isHeal = false;

        [SerializeField] float spawnX = 4f;
        [SerializeField] float spawnY = 2f;
        [SerializeField] float spawnZ = 2.5f;
        bool isStage3 = false;

        private void Start()
        {
            Vector3 spawnPoint = new Vector3(Random.Range(-spawnX, spawnX), Random.Range(-spawnY, spawnY), Random.Range(-spawnZ, spawnZ));
            if (isStage3 == false)
            {
                spawnPoint = new Vector3(spawnPoint.x, 0, spawnPoint.z);
            }
            transform.position = spawnPoint;
        }

        private void OnTriggerEnter(Collider other)
        {
            if ((ballMask & (1 << other.gameObject.layer)) != 0)
            {
                bool isPlayer = other.gameObject.transform.position.x < transform.position.x;

                if (isHeal == true)
                {
                    EventManager.BoxRegen(isPlayer);
                }
                else
                {
                    EventManager.BoxBall(isPlayer);
                }

                EventManager.BoxCollide(gameObject);
            }

            if ((paddlesMask & (1 << other.gameObject.layer)) != 0)
            {
                bool isPlayer = other.gameObject.transform.position.x < transform.position.x;

                if (isHeal == true)
                {
                    EventManager.BoxRegen(isPlayer);
                    EventManager.BoxCollide(gameObject);
                }
            }
        }
    }
}
