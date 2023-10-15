using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHolder : MonoBehaviour
{
    [SerializeField] private AbilityClient ability;
    private float startTime;
    private float cooldownTime;
    private float activeTime;
    public KeyCode key;

    enum AbilityState
    {
        ready,
        active,
        cooldown
    } 

    AbilityState state = AbilityState.ready;

    private void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - activeTime < ability.AvailableTime)
        {
            return;
        }
        //Activate ability
        if (state == AbilityState.ready && Input.GetKeyDown(key))
        {
            ability.Activate(gameObject);
            state = AbilityState.active;
            activeTime = ability.ActiveTime;
        }
        //Active time
        else if (state == AbilityState.active)
        {
            if (activeTime > 0)
            {
                activeTime -= Time.deltaTime;
            }
            else
            {
                state = AbilityState.cooldown;
                cooldownTime = ability.Cooldown;
            }
        }
        else if (state == AbilityState.cooldown)
        {
            if (cooldownTime > 0)
            {
                cooldownTime -= Time.deltaTime;
            }
            else 
            {   
                state = AbilityState.ready;
            }
        }
    }
}
