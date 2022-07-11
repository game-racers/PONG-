using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PONG.Control
{
    public class OpponentController : MonoBehaviour
    {
        [SerializeField] float vertSpeed = 4f;
        [SerializeField] float horizSpeed = 3f;
        [SerializeField] float health = 3f; // currently length of block
        [SerializeField] Transform edges;
        Transform northEdge;
        Transform southEdge;
        Transform topEdge;
        Transform bottomEdge;
        Transform forwardEdge;
        Transform backEdge;
        [SerializeField] float edgeRadius = .1f;
        [SerializeField] LayerMask edgeMask;
        [SerializeField] LayerMask groundMask;
        [SerializeField] LayerMask goalMask;
        [SerializeField] LayerMask fieldMask;
        [SerializeField] LayerMask ballMask;
        [SerializeField] Vector3 checkDistance = new Vector3(2f, 5f, 10f);

        bool isStage2 = false;
        float startingX;
        [SerializeField] float buffer = 0.4f;
        [SerializeField] PaddleMagicTrigger magicPaddle;
        [SerializeField] Image castingImage;
        [SerializeField] Image damageImage;
        Color retroYellow = new Color(.94f, .89f, .48f);
        Color retroBlue = new Color(.20f, .40f, .80f);
        Color retroRed = new Color(.69f, .14f, .07f);
        Color noAlpha = new Color(1f, 1f, 1f, 0f);

        bool chooseMagic = false;
        int magicNum = 0;
        float magicTimer;
        [SerializeField] float magicTimerMax = 1f;
        float magicDuration;
        [SerializeField] float magicDurationMax = 1f;
        bool isFrozen = false;
        bool isShocked = false;

        bool isStage3 = false;

        float vert;
        float horiz;
        float elevation;
        float elevBuffer = .25f;
        Vector3 direction;
        Vector3 fastDir;

        private void OnEnable()
        {
            EventManager.onStage += NextStage;
            EventManager.onBoxHeal += Heal;
        }

        private void OnDisable()
        {
            EventManager.onStage -= NextStage;
            EventManager.onBallMagic -= SetDamage;
            EventManager.onBoxHeal -= Heal;
        }

        private void NextStage(int num)
        {
            if (num == 2)
            {
                isStage2 = true;
                EventManager.onBallMagic += SetDamage;
            }
            if (num == 3)
            {
                isStage3 = true;
            }
        }

        private void SetDamage(GameObject paddle, int ballMagic)
        {
            if (!GameObject.ReferenceEquals(gameObject, paddle)) return;
            if (ballMagic == 1) // Fire
                FireDamage();
            if (ballMagic == 2) // Snow
                SnowDamage();
            if (ballMagic == 3) // Thunder
                ThunderDamage();

            magicTimer = Time.time;
            chooseMagic = false;
        }

        private void Heal(bool isPlayer)
        {
            if (isPlayer == false)
            {
                if (health == 3) return;
                health += 1;
                gameObject.transform.localScale += new Vector3(0f, 0f, .5f);
            }
        }

        private void Start()
        {
            startingX = transform.position.x;

            topEdge = edges.Find("Top Edge").GetComponent<Transform>();
            bottomEdge = edges.Find("Bottom Edge").GetComponent<Transform>();
            northEdge = edges.Find("North Edge").GetComponent<Transform>();
            southEdge = edges.Find("South Edge").GetComponent<Transform>();
            forwardEdge = edges.Find("Forward Edge").GetComponent<Transform>();
            backEdge = edges.Find("Back Edge").GetComponent<Transform>();
        }

        void Update()
        {
            UpdateMove();
            UpdateMagic();
            UpdateDamage();
        }

        private void UpdateMove()
        {
            if (isFrozen == true) return;
            if (isShocked == true)
            {
                transform.Translate(new Vector3(-1f, 0f, 0f) * Time.deltaTime);
                return;
            }

            Collider[] balls = Physics.OverlapBox(transform.position, checkDistance, Quaternion.identity, ballMask);
            if (balls.Length > 0)
            {
                vert = balls[0].transform.position.z - transform.position.z;
                if (Mathf.Abs(vert) < health / 3)
                {
                    vert = 0;
                }
                else
                {
                    vert = vert / Mathf.Abs(vert);
                }

                if (Physics.CheckSphere(northEdge.position, edgeRadius, edgeMask))
                {
                    if (vert > 0)
                        vert = 0;
                }
                if (Physics.CheckSphere(southEdge.position, edgeRadius, edgeMask))
                {
                    if (vert < 0)
                        vert = 0;
                }

                if (transform.position.x - buffer > startingX)
                {
                    horiz = -1f;
                }
                if (transform.position.x + buffer < startingX)
                {
                    horiz = 1f;
                }

                if (isStage3 == true)
                {
                    elevation = balls[0].transform.position.y - transform.position.y;
                    if (Mathf.Abs(elevation) < elevBuffer)
                    {
                        elevation = 0;
                    }
                    else
                    {
                        elevation = elevation / Mathf.Abs(elevation);
                    }
                    if (Physics.CheckSphere(topEdge.position, edgeRadius, groundMask))
                    {
                        if (elevation > 0)
                            elevation = 0;
                    }
                    if (Physics.CheckSphere(bottomEdge.position, edgeRadius, groundMask))
                    {
                        if (elevation < 0)
                            elevation = 0;
                    }
                }

                direction = new Vector3(horiz, elevation, vert).normalized;
                fastDir = new Vector3(direction.x * horizSpeed, direction.y * vertSpeed, direction.z * vertSpeed);
                transform.Translate(fastDir * Time.deltaTime);
            }
        }

        private void UpdateMagic()
        {
            if (isStage2 == false) return;
            if (chooseMagic == true) return;
            if (Time.time - magicTimer < magicTimerMax) return;

            magicNum = Random.Range(0, 4);
            if (magicNum ==  1)
            {
                castingImage.color = retroRed;
            }
            else if (magicNum == 2)
            {
                castingImage.color = retroBlue;
            }
            else if (magicNum == 3)
            {
                castingImage.color = retroYellow;
            }
            else if (magicNum == 0)
            {
                castingImage.color = noAlpha;
            }
            magicPaddle.SetMagic(magicNum);
            chooseMagic = true;
        }

        private void UpdateDamage()
        {
            if (Time.time - magicDuration < magicDurationMax) return;
            damageImage.color = noAlpha;
            isFrozen = false;
            isShocked = false;
        }    

        private void FireDamage()
        {
            magicDuration = Time.time;
            damageImage.color = retroRed;
            if (health <= 0) return;
            gameObject.transform.localScale -= new Vector3(0f, 0f, .5f);
            health -= 1;
        }

        private void SnowDamage()
        {
            magicDuration = Time.time;
            damageImage.color = retroBlue;
            isFrozen = true;
        }

        private void ThunderDamage()
        {
            magicDuration = Time.time;
            damageImage.color = retroYellow;
            isShocked = true;
        }
    }
}

