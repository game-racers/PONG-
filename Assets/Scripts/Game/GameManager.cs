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
        [SerializeField] int lives2 = 5;
        [SerializeField] int lives3 = 5;

        [SerializeField] GameObject healthPack;
        [SerializeField] GameObject ballPack;
        [SerializeField] float packTimerMax = 5f;
        float packTimer;
        [SerializeField] int maxPacks = 3;
        List<GameObject> packs = new List<GameObject>();

        [SerializeField] CameraManager cameraManager;

        [SerializeField] GameObject gameOverScreen;
        [SerializeField] TextMeshProUGUI r1Winner;
        [SerializeField] TextMeshProUGUI r2Winner;
        [SerializeField] TextMeshProUGUI r3Winner;
        [SerializeField] TextMeshProUGUI finalWinner;
        int playerWins = 0;


        private void Update()
        {
            SpawnPack();
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
                    return;
                }
            }
        }

        private void NewRound(int ballNum)
        {
            Destroy(balls[ballNum]);
            balls.RemoveAt(ballNum);
            if (balls.Count > 0) return;
            if (lives <= playerPoints || lives <= opponentPoints)
            {
                StartCoroutine(NewStage());
            }
            else
            {
                SpawnBall();
                ChangeBallDir();
            }
        }

        private IEnumerator NewStage()
        {
            if (currentStage == 3)
            {
                if (playerPoints > opponentPoints)
                { 
                    r3Winner.SetText("Player 1 Wins");
                    playerWins += 1;
                }
                else
                    r3Winner.SetText("Player 1 Loses");
                if (playerWins >= 2)
                    finalWinner.SetText("Player 1 Wins!");
                else
                    finalWinner.SetText("Player 1 Loses");

                gameOverScreen.SetActive(true);
                currentStage += 1;
                Debug.Log("Game Over");
                yield break;
            }

            currentStage += 1;
            if (currentStage == 2)
            {
                if (playerPoints > opponentPoints)
                {
                    r1Winner.SetText("Player 1 Wins");
                    playerWins += 1;
                }
                else
                    r1Winner.SetText("Player 1 Loses");
                packTimer = Time.time + 8f;
                cameraManager.StartMove(currentStage);
                arcadeMachine.OpenDoor();
                arcadeMachine.MoveControlArm();
                yield return new WaitForSeconds(4f);
            }
            if (currentStage == 3)
            {
                if (playerPoints > opponentPoints)
                {
                    r2Winner.SetText("Player 1 Wins");
                    playerWins += 1;
                }
                else
                    r2Winner.SetText("Player 1 Loses");
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
