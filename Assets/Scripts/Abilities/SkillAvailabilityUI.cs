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
    
    [SerializeField] private float shakeAmount = 3f;
    [SerializeField] private float shakeDuration = 0.75f;
    [SerializeField] private float delayBeforeShake = 2.5f;


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
            if (!isUnavailable)
            {
                StartCoroutine(ShakeImage(targetSkill));
            }
            else
            {
                targetSkill.color = new Color(1, 1, 1, 1);
            }
        }
    }

    private IEnumerator ShakeImage(Image targetImage)
    {
        Vector3 originalPosition = targetImage.rectTransform.localPosition;
        Vector3 originalScale = targetImage.rectTransform.localScale; 
        float elapsed = 0.0f;

        Vector3 enlargedScale = originalScale * 1.1f; 

        yield return new WaitForSeconds(delayBeforeShake);

        targetImage.rectTransform.localScale = enlargedScale;

        while (elapsed < shakeDuration)
        {
            float x = originalPosition.x + Random.Range(-1f, 1f) * shakeAmount;
            float y = originalPosition.y + Random.Range(-1f, 1f) * shakeAmount;

            targetImage.rectTransform.localPosition = new Vector3(x, y, originalPosition.z);
            elapsed += Time.deltaTime;

            yield return null;
        }

        yield return new WaitForSeconds(0.25f);


        targetImage.rectTransform.localPosition = originalPosition;
        targetImage.rectTransform.localScale = originalScale; 
        targetImage.color = new Color(1, 1, 1, 0); 

        Debug.Log("Shake ended."); 
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
