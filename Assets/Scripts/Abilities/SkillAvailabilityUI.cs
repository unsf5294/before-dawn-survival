using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillAvailabilityUI : MonoBehaviour
{
    [Header("Skill Unavailability Icons")]
    [SerializeField] private Image unavailable1;
    [SerializeField] private Image unavailable2;
    [SerializeField] private Image unavailable3;

    [Header("Skill Notification")]
    [SerializeField] private GameObject[] skillNotifications;

    [SerializeField] private Animator skillNotificationAnimator;
    private const string PLAY_NOTIFICATION_TRIGGER = "PlaySkillNotification";



    private void Start()
    {
        foreach (GameObject notification in skillNotifications)
        {
            notification.SetActive(false);
        }
    }


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

    public void ShowSkillNotification(int skillIndex)
    {
        skillNotifications[skillIndex].SetActive(true);
        StartCoroutine(HideNotificationAfterDelay(skillNotifications[skillIndex], 5f));
    }

    private IEnumerator HideNotificationAfterDelay(GameObject notification, float delay)
    {
        yield return new WaitForSeconds(delay);
        skillNotificationAnimator.SetTrigger(PLAY_NOTIFICATION_TRIGGER);
    }

}
