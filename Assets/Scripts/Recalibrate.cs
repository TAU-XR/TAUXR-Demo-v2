using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recalibrate : MonoBehaviour
{
    [SerializeField] float holdingDuration = 2f;
    float holdingTime = 0;

    bool isCorrectPositionSaved;

    Vector3 correctPosition;
    Quaternion correctRotation;

    TAUXRPlayer player;

    void Start()
    {
        player = TAUXRPlayer.Instance;
    }

    void Update()
    {
        if(player.IsHoldingTrigger(HandType.Any))
        {
            if(!isCorrectPositionSaved)
            {
                SaveCorrectPosition();
                isCorrectPositionSaved = true;
            }

            holdingTime += Time.deltaTime;
            if(holdingTime > holdingDuration)
            {
                RecalibratePlayer();
            }
        }
    }

    void SaveCorrectPosition()
    {
        correctPosition = player.PlayerHead.transform.position;
        correctRotation= player.PlayerHead.transform.rotation;
    }

    void RecalibratePlayer()
    {

    }
}
