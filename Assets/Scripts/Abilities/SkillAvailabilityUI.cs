using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillAvailabilityUI : MonoBehaviour
{
    [SerializeField] private Image unavailable1;
    [SerializeField] private Image unavailable2;
    [SerializeField] private Image unavailable3;

    public void SetSkillUnavailable(int skillIndex, bool isUnavailable)
    {
        Image targetSkill = null;
        switch (skillIndex)
        {
            case 1:
                targetSkill = unavailable1;
                break;
            case 2:
                targetSkill = unavailable2;
                break;
            case 3:
                targetSkill = unavailable3;
                break;
        }

        if (targetSkill != null)
        {
            targetSkill.color = isUnavailable ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0);
        }
    }
}
