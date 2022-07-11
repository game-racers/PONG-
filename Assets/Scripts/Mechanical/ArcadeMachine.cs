using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PONG.Mechanical
{
    public class ArcadeMachine : MonoBehaviour
    {
        [SerializeField] GameObject controlArm;
        [SerializeField] GameObject playerArm;
        [SerializeField] GameObject opponentArm;
        [SerializeField] GameObject controls2;
        [SerializeField] GameObject controls3;

        private void OnEnable()
        {
            EventManager.onStage += SetStage;
        }

        private void OnDisable()
        {
            EventManager.onStage -= SetStage;
        }

        private void SetStage(int stageNum)
        {
            if (stageNum == 3)
            {
                controls3.SetActive(true);
                controls2.SetActive(false);
            }
        }

        public void OpenDoor()
        {
            GetComponent<Animator>().SetTrigger("openLid");
            GetComponent<AudioSource>().Play();
        }

        public void MoveControlArm()
        {
            controlArm.GetComponent<Animator>().SetTrigger("moveArm");
            controlArm.GetComponent<AudioSource>().Play();
        }

        public void MovePlayerArm()
        {
            playerArm.GetComponent<Animator>().SetTrigger("moveArm");
            playerArm.GetComponent<AudioSource>().Play();
        }

        public void MoveEnemyArm()
        {
            opponentArm.GetComponent<Animator>().SetTrigger("moveArm");
            opponentArm.GetComponent<AudioSource>().Play();
        }
    }
}
