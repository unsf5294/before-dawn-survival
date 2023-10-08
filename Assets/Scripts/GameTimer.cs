using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameTimer : MonoBehaviour
{
    private float startTime;
    private float inGameTime;
    [SerializeField] private UnityEvent endofGame;
    [SerializeField] private UnityEvent wave1Event;
    [SerializeField] private UnityEvent wave2Event;
    [SerializeField] private UnityEvent wave3Event;
    [SerializeField] private float gameTime;
    [SerializeField] private float wave1 = 90;
    [SerializeField] private float wave2 = 180;
    [SerializeField] private float wave3 = 240;
    [SerializeField] private GameObject Player;
    private int triggeredWave = 0;

    void Start()
    {
        startTime = Time.time;
    }

    //If in-game time reaches 5min, transition scene
    void Update()
    {
        inGameTime = Time.time - startTime;

        if (triggeredWave < 1 && inGameTime >= wave1)
        {
            triggeredWave = 1;
            wave1Event.Invoke();
        }

        if (triggeredWave < 2 && inGameTime >= wave2)
        {
            triggeredWave = 2;
            wave2Event.Invoke();
        }

        if (triggeredWave < 3 && inGameTime >= wave3)
        {
            triggeredWave = 3;
            wave3Event.Invoke();
        }

        if (inGameTime >= gameTime)
        {
            endofGame.Invoke();
        }
    }
}
