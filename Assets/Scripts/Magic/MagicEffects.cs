using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PONG.Magic
{
    public class MagicEffects : MonoBehaviour
    {
        Color retroYellow = new Color(.94f, .89f, .48f);
        Color retroBlue = new Color(.20f, .40f, .80f);
        Color retroRed = new Color(.69f, .14f, .07f);
        Color retroWhite;
        int magic = 0;
        [SerializeField] Image ballImage;
        [SerializeField] ParticleSystem fireParticles;
        [SerializeField] ParticleSystem snowParticles;
        [SerializeField] ParticleSystem thunderParticles;
        [SerializeField] AudioSource fireBall;
        [SerializeField] AudioSource snow;
        [SerializeField] AudioSource thunderBall;
        [SerializeField] AudioSource firePaddle;
        [SerializeField] AudioSource thunderPaddle;

        void Start()
        {
            retroWhite = ballImage.color;
        }

        public void FireMagic()
        {
            ballImage.color = retroRed;
            fireParticles.Play();
            snowParticles.Stop();
            thunderParticles.Stop();
            fireBall.Play();
            magic = 1;
        }

        public void SnowMagic()
        {
            ballImage.color = retroBlue;
            snowParticles.Play();
            fireParticles.Stop();
            thunderParticles.Stop();
            snow.Play();
            magic = 2;
        }

        public void ThunderMagic()
        {
            ballImage.color = retroYellow;
            thunderParticles.Play();
            fireParticles.Stop();
            snowParticles.Stop();
            thunderBall.Play();
            magic = 3;
        }

        public void EndMagic()
        {
            ballImage.color = retroWhite;
            fireParticles.Stop();
            snowParticles.Stop();
            thunderParticles.Stop();
            if (magic == 1)
            {
                firePaddle.Play();
                fireBall.Stop();
            }
            else if (magic == 2)
            {
                snow.Play();
            }
            else if (magic == 3)
            {
                thunderPaddle.Play();
                thunderBall.Stop();
            }

            magic = 0;
        }

        public int GetMagic()
        {
            return magic;
        }
    }
}
