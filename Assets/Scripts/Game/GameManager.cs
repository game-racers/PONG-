using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PONG.Control;
using PONG.UI;
using System;
using TMPro;
using PONG.Mechanical;

namespace PONG.Game
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] ArcadeMachine arcadeMachine;
        [SerializeField] TextMeshProUGUI playerText;
        [SerializeField] TextMeshProUGUI opponentText;
        [SerializeField] LivesDisplay livesUI;
        List<GameObject> balls = new List<GameObject>();

        [SerializeField] GameObject ballPrefab;
        [SerializeField] AudioSource scoreSound;
        [SerializeField] int lives = 1;
        int playerPoints = 0;
        int opponentPoints = 0;
        int currentStage = 1;

        bool changeDir = false;
        [SerializeField] bool stage2 = false;
        [SerializeField] int lives2 = 5;
        [SerializeField] bool stage3 = false;
        [SerializeField] int lives3 = 5;

        [SerializeField] GameObject healthPack;
        [SerializeField] GameObject ballPack;
        [SerializeField] float packTimerMax = 5f;
        float packTimer;
        [SerializeField] int maxPacks = 3;
        List<GameObject> packs = new List<GameObject>();

        [SerializeField] CameraManager cameraManager;

        void Start()
        {
            if (stage2 == true)
            {
                NewStage();
            }

            if (stage3 == true)
            {
                if (stage2 != true) NewStage();
                NewStage();
            }
        }

        private void Update()
        {
            SpawnPack();
        }

        private void LateUpdate()
        {
            //ChangeBallDir();
        }

        private void ChangeBallDir()
        {
            if (changeDir)
            {
                balls[balls.Count - 1].GetComponent<Ball>().ChangeDir();
            }
            changeDir = false;
        }

        private void OnEnable()
        {
            EventManager.onBallBreak += RestartRound;
            EventManager.onGoal += SetPoint;
            EventManager.onBoxSpawn += SpawnBall;
            EventManager.onBoxBreak += DestroyBox;
        }

        private void OnDisable()
        {
            EventManager.onBallBreak -= RestartRound;
            EventManager.onGoal -= SetPoint;
            EventManager.onBoxSpawn -= SpawnBall;
            EventManager.onBoxBreak -= DestroyBox;
        }

        private void RestartRound(GameObject ball)
        {
            for (int i = 0; i < balls.Count; i++)
            {
                if (GameObject.ReferenceEquals(balls[i], ball))
                {
                    NewRound(i);
                }
            }
        }

        private void NewRound(int ballNum)
        {
            Destroy(balls[ballNum]);
            balls.RemoveAt(ballNum);
            if (balls.Count > 0) return;
            lives -= 1;
            livesUI.UpdateLives(lives);
            if (lives > 0)
            {
                SpawnBall();
                ChangeBallDir();
            }
            else
            {
                StartCoroutine(NewStage());
            }
        }

        private IEnumerator NewStage()
        {
            if (currentStage == 3) yield break;
            currentStage += 1;
            if (currentStage == 2)
            {
                packTimer = Time.time + 8f;
                cameraManager.StartMove(currentStage);
                arcadeMachine.OpenDoor();
                arcadeMachine.MoveControlArm();
                yield return new WaitForSeconds(4f);
            }
            if (currentStage == 3)
            {
                arcadeMachine.MovePlayerArm();
                yield return new WaitForSeconds(4f);
                arcadeMachine.MoveEnemyArm();
                yield return new WaitForSeconds(4f);
            }
            EventManager.NextStage(currentStage);

            playerPoints = 0;
            opponentPoints = 0;
            playerText.SetText(playerPoints.ToString("00"));
            opponentText.SetText(opponentPoints.ToString("00"));
            if (currentStage == 2)
                lives = lives2;
            if (currentStage == 3)
                lives = lives3;
            livesUI.UpdateLives(lives);
            SpawnBall();
            ChangeBallDir();
        }

        private void SetPoint(GameObject ball, bool isPlayerGoal)
        {
            if (isPlayerGoal)
            {
                opponentPoints += 1;
                opponentText.SetText(opponentPoints.ToString("00"));
            }
            else
            {
                playerPoints += 1;
                playerText.SetText(playerPoints.ToString("00"));
                changeDir = true;
            }
            scoreSound.Play();
            RestartRound(ball);
        }

        private void SpawnBall()
        {
            balls.Add(Instantiate(ballPrefab));
            balls[balls.Count - 1].GetComponent<Ball>().SetStage(currentStage);
        }

        private void SpawnBall(bool dir)
        {
            balls.Add(Instantiate(ballPrefab));
            balls[balls.Count - 1].GetComponent<Ball>().SetStage(currentStage);
            changeDir = dir;
        }

        private void SpawnPack()
        {
            if (currentStage >= 2)
            {
                if (Time.time - packTimer > packTimerMax)
                {
                    packTimer = Time.time;
                    if (UnityEngine.Random.Range(0, 2) == 0)
                        packs.Add(Instantiate(healthPack));
                    else
                        packs.Add(Instantiate(ballPack));

                    if (packs.Count > maxPacks)
                        DestroyBox(packs[0]);
                }
            }
        }

        private void DestroyBox(GameObject pack)
        {
            for (int i = 0; i < packs.Count; i++)
            {
                if (GameObject.ReferenceEquals(packs[i], pack))
                {
                    Destroy(packs[i]);
                    packs.RemoveAt(i);
                }
            }
        }

        public void GameBegin()
        {
            SpawnBall();
            playerText.SetText(playerPoints.ToString("00"));
            opponentText.SetText(opponentPoints.ToString("00"));
            livesUI.UpdateLives(lives);
        }

        public void SetR1Lives(int num)
        {
            lives = num;
        }

        public void SetR2Lives(int num)
        {
            lives2 = num;
        }
        public void SetR3Lives(int num)
        {
            lives3 = num;
        }
    }
}
