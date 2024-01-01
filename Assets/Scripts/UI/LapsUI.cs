using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LapsUI : MonoBehaviour
{
    private const int MAX_LAPS = 4;

    [SerializeField] private TMP_Text lapsText;

    private void Start() 
    {
        CheckpointsTrack.Instance.OnPlayerLapCompleted += CheckpointsTrack_OnPlayerLapCompleted;
        CheckpointsTrack.Instance.OnPlayerRaceCompleted += CheckpointsTrack_OnPlayerRaceCompleted;

        UpdateLapsText(0, MAX_LAPS - 1);
    }

    private void CheckpointsTrack_OnPlayerLapCompleted(int lapCounter)
    {
        UpdateLapsText(lapCounter, MAX_LAPS - 1);
    }

    private void CheckpointsTrack_OnPlayerRaceCompleted()
    {
        //TODO : RACE COMPLETED
        Hide();
    }

    private void UpdateLapsText(int lap, int maxLaps)
    {
        lapsText.text = "Lap: " + lap.ToString() + "/" + maxLaps.ToString();
    }
    
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
