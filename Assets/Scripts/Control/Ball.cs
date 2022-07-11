using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PONG.Magic;

namespace PONG.Control
{
    public class Ball : MonoBehaviour
    {
        [SerializeField] float defSpeed = 4f;
        [SerializeField] LayerMask edgeMask;
        [SerializeField] LayerMask paddleMask;
        [SerializeField] LayerMask groundMask;
        [SerializeField] float radius = .6f;
        [SerializeField] Canvas canvas;
        [SerializeField] AudioSource wall;
        [SerializeField] AudioSource paddle;

        MagicEffects magicSystem;
        int magicNum = 0;
        bool isStage3 = false;

        Vector3 direction;

        void Start()
        {
            if (isStage3 == true)
                direction = new Vector3(1f, Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            else
                direction = new Vector3(1f, 0f, Random.Range(-1f, 1f)).normalized;
            magicSystem = GetComponent<MagicEffects>();
            wall.Play();
        }

        void Update()
        {
            CheckPhysics();
            UpdateMove();
        }

        private void OnEnable()
        {
            EventManager.onPaddleBall += SetMagic;
        }

        private void OnDisable()
        {
            EventManager.onPaddleBall -= SetMagic;
        }

        private void SetMagic(GameObject ball, int magicNum)
        {
            if (!GameObject.ReferenceEquals(ball, gameObject)) return;
            if (magicNum == 1)
            {
                magicSystem.FireMagic();
            }
            if (magicNum == 2)
            {
                magicSystem.SnowMagic();
            }
            if (magicNum == 3)
            {
                magicSystem.ThunderMagic();
            }
            this.magicNum = magicNum;
        }

        private void NextStage(int num)
        {
            if (num == 3)
                isStage3 = true;
        }

        private void CheckPhysics()
        {
            // Hit map Edge
            if (Physics.CheckSphere(transform.position, radius, edgeMask))
            {
                direction = new Vector3(direction.x, direction.y, direction.z * -1).normalized;
                wall.Play();
            }

            // Hit upper/lower bounds
            if (Physics.CheckSphere(transform.position, radius, groundMask))
            {
                direction = new Vector3(direction.x, direction.y * -1, direction.z).normalized;
                wall.Play();
            }

            // Hit Paddle
            Collider[] paddles = Physics.OverlapSphere(transform.position, radius, paddleMask);
            if (paddles.Length > 0)
            {
                direction = (transform.position - paddles[0].transform.position);
                if (isStage3 == false)
                {
                    direction = new Vector3(direction.x, 0f, direction.z);
                }
                direction = direction.normalized;
                magicSystem.EndMagic();
                SetMagic(gameObject, 0);
                paddle.Play();
            }
        }

        private void UpdateMove()
        {
            transform.Translate(direction * defSpeed * Time.deltaTime);
        }

        public void SetStage(int stageNum)
        {
            if (stageNum == 3)
            {
                isStage3 = true;
            }
        }

        public void ChangeDir()
        {
            direction = new Vector3(direction.x * -1, direction.y, direction.z).normalized;
        }

        public int GetMagicBall()
        {
            return magicNum;
        }
    }
}
