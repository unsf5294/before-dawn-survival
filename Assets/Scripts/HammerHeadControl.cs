using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerHeadControl : MonoBehaviour
{
    [SerializeField] private Material hammerHeadMat;
    private bool isAttacking = false;
    // Update is called once per frame
    void Start()
    {
        hammerHeadMat.SetFloat("_AttackEffect", 0);
    }
    void Update()
    {
        if (isAttacking)
        {
            hammerHeadMat.SetFloat("_AttackEffect", 1);
        } else {
            hammerHeadMat.SetFloat("_AttackEffect", 0);
        }
    }
    
    public void setIsAttacking(bool isAttacking)
    {
        this.isAttacking = isAttacking;
    }
}
