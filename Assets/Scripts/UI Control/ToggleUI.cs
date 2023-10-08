using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleUI : MonoBehaviour
{
    private GameObject UI;
    // Start is called before the first frame update
    void Start()
    {
        UI = transform.gameObject;
        UI.SetActive(false);
    }

    void activate(Boolean state)
    {
        UI.SetActive(state);
    }
}
