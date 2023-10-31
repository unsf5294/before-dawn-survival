using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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
    [SerializeField] private GameObject textParent;
    [SerializeField] private Text text;
    private int triggeredWave = 0;

    void Start()
    {
        textParent.SetActive(false);
        startTime = Time.time;
    }

    //If in-game time reaches 5min, transition scene
    void Update()
    {
        inGameTime = Time.time - startTime;

        if (triggeredWave < 1 && inGameTime >= wave1)
        {
            triggeredWave = 1;
            StartCoroutine(showNotif("Darkness summons more minions. Hold your ground!"));
            wave1Event.Invoke();
        }

        if (triggeredWave < 2 && inGameTime >= wave2)
        {
            triggeredWave = 2;
            StartCoroutine(showNotif("The darkness is angered by your resilience! More enemies incoming!"));
            wave2Event.Invoke();
        }

        if (triggeredWave < 3 && inGameTime >= wave3)
        {
            triggeredWave = 3;
            StartCoroutine(showNotif("The darkness strikes one last time in the face of dawn! More enemies incoming!"));
            wave3Event.Invoke();
        }

        if (inGameTime >= gameTime)
        {
            endofGame.Invoke();
        }
    }

    IEnumerator showNotif(string notif)
    {
        textParent.SetActive(true);
        text.text = notif;
        yield return new WaitForSeconds(3);
        textParent.SetActive(false);
    }
}
