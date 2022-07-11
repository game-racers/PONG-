using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public delegate void OutOfBounds(GameObject ball);
    public static event OutOfBounds onBallBreak;

    public delegate void Goal(GameObject ball, bool isPlayerGoal);
    public static event Goal onGoal;

    public delegate void Stage(int stage);
    public static event Stage onStage;

    public delegate void PaddleBall(GameObject ball, int magicNum);
    public static event PaddleBall onPaddleBall;

    public delegate void BallMagic(GameObject paddle, int magicNum);
    public static event BallMagic onBallMagic;

    public delegate void BoxSpawn(bool dir);
    public static event BoxSpawn onBoxSpawn;

    public delegate void BoxHeal(bool isPlayer);
    public static event BoxHeal onBoxHeal;

    public delegate void BoxBreak(GameObject box);
    public static event BoxBreak onBoxBreak;

    public static void BallBreak(GameObject ball)
    {
        if (onBallBreak != null)
            onBallBreak(ball);
    }
    
    public static void GoalHit(GameObject ball, bool isPlayerGoal)
    {
        if (onGoal != null)
            onGoal(ball, isPlayerGoal);
    }

    public static void NextStage(int stage)
    {
        if (onStage != null)
            onStage(stage);
    }

    public static void PaddleBallCollision(GameObject ball, int magicNum)
    {
        if (onPaddleBall != null)
            onPaddleBall(ball, magicNum);
    }

    public static void MagicBall(GameObject paddle, int magicNum)
    {
        if (onBallMagic != null)
            onBallMagic(paddle, magicNum);
    }

    public static void BoxBall(bool dir)
    {
        if (onBoxSpawn != null)
            onBoxSpawn(dir);
    }

    public static void BoxRegen(bool isPlayer)
    {
        if (onBoxHeal != null)
            onBoxHeal(isPlayer);
    }

    public static void BoxCollide(GameObject box)
    {
        if (onBoxBreak != null)
            onBoxBreak(box);
    }
}
