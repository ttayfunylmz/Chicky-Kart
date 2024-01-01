using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackCheckpointsUI : MonoBehaviour
{
    [SerializeField] private CheckpointsTrack checkpointsTrack;

    private void Start() 
    {
        checkpointsTrack.OnPlayerCorrectCheckpoint += CheckpointsTrack_OnPlayerCorrectCheckpoint;
        checkpointsTrack.OnPlayerWrongCheckpoint += CheckpointsTrack_OnPlayerWrongCheckpoint;

        Hide();
    }

    private void CheckpointsTrack_OnPlayerWrongCheckpoint()
    {
        Show();
    }

    private void CheckpointsTrack_OnPlayerCorrectCheckpoint()
    {
        Hide();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
