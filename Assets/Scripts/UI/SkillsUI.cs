using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SkillsUI : MonoBehaviour
{
    public static SkillsUI Instance { get; private set; }

    [SerializeField] private RectTransform content;
    [SerializeField] private float spinDuration = 2f;
    [SerializeField] private Ease spinEase = Ease.OutQuint;

    public bool isSpinning = false;
    private Dictionary<string, RectTransform> skillSlotDictionary = new Dictionary<string, RectTransform>();

    private void Awake()
    {
        Instance = this;

        foreach (Transform child in content)
        {
            SkillSlot skillSlot = child.GetComponent<SkillSlot>();
            if (skillSlot != null)
            {
                skillSlotDictionary[skillSlot.skillData.skillName] = skillSlot.GetComponent<RectTransform>();
            }
        }
    }

    private void Start() 
    {
        Hide();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    public IEnumerator SpinWheel(AbilityDataSO selectedSkill)
    {
        if (!isSpinning)
        {
            Show();

            content.DOAnchorPosY(0, 0);
            yield return null; //Wait 1 frame


            isSpinning = true;

            if (skillSlotDictionary.TryGetValue(selectedSkill.skillName, out RectTransform skillSlotRect))
            {
                float targetPosY = -skillSlotRect.localPosition.y - content.anchoredPosition.y;

                content.DOAnchorPosY(targetPosY, spinDuration)
                    .SetEase(spinEase)
                    .OnComplete(() =>
                    {
                        isSpinning = false;
                    });
            }
            else
            {
                Debug.LogError("Selected skill not found in skill slots.");
            }
        }
    }

    public void ResetUI()
    {
        content.DOAnchorPosY(0, 0);

        if(!isSpinning)
        {
            Hide();
        }
    }
}
