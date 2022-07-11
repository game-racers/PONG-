using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PONG.UI
{
    public class CameraManager : MonoBehaviour
    {
        //[SerializeField] Transform lookAtMe;
        [SerializeField] Transform secondStagePos;
        bool shouldMove = false;
        [SerializeField] float rotSpeed = .001f;
        [SerializeField] float moveSpeed = .1f;
        int stageNum = 1;

        Vector3 startPos;
        float journeyLength;
        float moveTime;

        private void Start()
        {
            startPos = transform.position;
            journeyLength = Vector3.Distance(startPos, secondStagePos.position);
        }

        private void Update()
        {
            if (shouldMove == false) return;
            if (stageNum == 2)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, secondStagePos.rotation, rotSpeed * Time.time);
                float distCovered = (Time.time - moveTime) * moveSpeed;
                float fracOfJourney = distCovered / journeyLength;
                transform.position = Vector3.Lerp(startPos, secondStagePos.position, fracOfJourney);
                if (Time.time - moveTime > 8f)
                    shouldMove = false;
            }
        }

        public void StartMove(int num)
        {
            if (num == 2)
            {
                shouldMove = true;
                stageNum = num;
                moveTime = Time.time;
            }
        }
    }
}