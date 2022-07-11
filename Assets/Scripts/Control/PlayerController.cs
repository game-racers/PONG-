using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PONG.Control
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] float vertSpeed = 1f;
        [SerializeField] float horizSpeed = 1f;
        [SerializeField] float health = 3f; // currently length of block
        [SerializeField] Transform edges;
        Transform northEdge;
        Transform southEdge;
        Transform topEdge;
        Transform bottomEdge;
        Transform forwardEdge;
        Transform backEdge;
        [SerializeField] float edgeRadius = .3f;
        [SerializeField] LayerMask edgeMask;
        [SerializeField] LayerMask groundMask;
        [SerializeField] LayerMask goalMask;
        [SerializeField] LayerMask fieldMask;
        float horiz;
        float vert;
        float elevation = 0f;

        bool isStage2 = false;
        int magicNum = 0;
        [SerializeField] PaddleMagicTrigger magicPaddle;
        [SerializeField] Image castingImage;
        [SerializeField] Image damageImage;
        Color retroYellow = new Color(.94f, .89f, .48f);
        Color retroBlue = new Color(.20f, .40f, .80f);
        Color retroRed = new Color(.69f, .14f, .07f);
        Color noAlpha = new Color(1f, 1f, 1f, 0f);
        float magicDuration;
        [SerializeField] float magicDurationMax = 1f;
        bool isFrozen = false;
        bool isShocked = false;

        bool isStage3;

        Vector3 direction;

        private void OnEnable()
        {
            EventManager.onStage += SetStage;
            EventManager.onBoxHeal += Heal;
        }

        private void OnDisable()
        {
            EventManager.onStage -= SetStage;
            EventManager.onBallMagic -= SetDamage;
            EventManager.onBoxHeal -= Heal;
        }

        private void SetStage(int num)
        {
            if (num == 2)
            {
                isStage2 = true;
                EventManager.onBallMagic += SetDamage;
            }
            if (num == 3)
                isStage3 = true;
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
        }

        private void Heal(bool isPlayer)
        {
            if (isPlayer == true)
            {
                if (health == 3) return;
                health += 1;
                gameObject.transform.localScale += new Vector3(0f, 0f, .5f);
            }
        }

        private void Start()
        {
            topEdge = edges.Find("Top Edge").GetComponent<Transform>();
            bottomEdge = edges.Find("Bottom Edge").GetComponent<Transform>();
            northEdge = edges.Find("North Edge").GetComponent<Transform>();
            southEdge = edges.Find("South Edge").GetComponent<Transform>();
            forwardEdge = edges.Find("Forward Edge").GetComponent<Transform>();
            backEdge = edges.Find("Back Edge").GetComponent<Transform>();
        }

        void Update()
        {
            UpdateMover();
            if (isStage2 == true)
            {
                Stage2Input();
                UpdateDamage();
            }
        }

        private void UpdateMover()
        {
            if (isFrozen == true) return;
            if (isShocked == true)
            {
                transform.Translate(new Vector3(-1f, 0f, 0f) * Time.deltaTime);
                return;
            }

            vert = Input.GetAxis("Vertical");

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

            if (isStage2 == true)
            {
                horiz = Input.GetAxis("Elevation");
                if (Physics.CheckSphere(forwardEdge.position, edgeRadius, fieldMask))
                {
                    if (horiz > 0)
                        horiz = 0;
                }
                if (Physics.CheckSphere(backEdge.position, edgeRadius, goalMask))
                    if (horiz < 0)
                        horiz = 0;
            }
            else
                horiz = 0;

            if (isStage3 == true)
            {
                elevation = Input.GetAxis("Horizontal");
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

            direction = new Vector3(horiz, elevation, vert);
            direction = new Vector3(direction.x * horizSpeed, direction.y * vertSpeed, direction.z * vertSpeed);
            transform.Translate(direction * Time.deltaTime);
        }

        private void Stage2Input()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (magicNum != 1)
                {
                    magicNum = 1;
                    castingImage.color = retroRed;
                }
                else
                {
                    magicNum = 0;
                    castingImage.color = noAlpha;
                }
                magicPaddle.SetMagic(magicNum);
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (magicNum != 2)
                {
                    magicNum = 2;
                    castingImage.color = retroBlue;
                }
                else
                {
                    magicNum = 0;
                    castingImage.color = noAlpha;
                }
                magicPaddle.SetMagic(magicNum);
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (magicNum != 3)
                {
                    magicNum = 3;
                    castingImage.color = retroYellow;
                }
                else
                {
                    magicNum = 0;
                    castingImage.color = noAlpha;
                }
                magicPaddle.SetMagic(magicNum);
            }
        }

        private int GetEffect()
        {
            return magicNum;
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
