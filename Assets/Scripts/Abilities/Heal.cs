using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class Heal : AbilityClient
{
    public override void Activate(GameObject player)
    {
        base.Activate(player);
        player.GetComponent<PlayerHealth>().AddHealth(10);
    }
}
