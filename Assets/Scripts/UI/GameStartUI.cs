using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartUI : MonoBehaviour
{
    private const string COUNTDOWN_START = "CountdownStart";

    private Animator animator;
    
    private void Awake() 
    {
        animator = GetComponent<Animator>();
    }

    private void Start() 
    {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
    }

    private void GameManager_OnStateChanged()
    {
        if(GameManager.Instance.IsCountdownToStartActive())
        {
            animator.SetTrigger(COUNTDOWN_START);
        }
    }
}
