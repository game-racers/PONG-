using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace PONG.UI
{
    public class MixerController : MonoBehaviour
    {
        [SerializeField] AudioMixer mixer;

        public void SetVolume(float sliderValue)
        {
            mixer.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 20);
        }
    }
}
