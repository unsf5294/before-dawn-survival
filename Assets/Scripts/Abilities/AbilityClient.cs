using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class AbilityClient : MonoBehaviour
{
    [SerializeField] private string abilityName;
    [SerializeField] private GameObject player;
    [SerializeField] private float cooldown;
    [SerializeField] private float activeTime;
    [SerializeField] private float availableTime;
    [SerializeField] private UnityEvent eventListener;

    public float ActiveTime { get => activeTime; set => activeTime = value; }
    public float Cooldown { get => cooldown; set => cooldown = value; }
    public float AvailableTime { get => availableTime; set => availableTime = value; }

    public virtual void Activate(GameObject player)
    {
        eventListener.Invoke();
    }
}
