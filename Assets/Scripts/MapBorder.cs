using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBorder : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject UI;

    private void Start()
    {
        UI.SetActive(false);
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject == Player)
        {
            StopAllCoroutines();
            UI.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject == Player)
        {
            StartCoroutine(OutOfBorderDmg());
            UI.SetActive(true);
        }
    }

    IEnumerator OutOfBorderDmg()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            Player.GetComponent<PlayerHealth>().TakeDamage(10);
        }
    }
}