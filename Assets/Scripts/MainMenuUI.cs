using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MainMenuUI : MonoBehaviour
{
    [Header("Background Displacement")]
    [SerializeField]
    [ReadOnly]
    private RawImage backgroundImage;

    [SerializeField]
    private float backgroundDisplacementXSpeed = 0.13f;

    [SerializeField]
    private float backgroundDisplacementYSpeed = 0.13f;

    [Header("Title Animation")]
    [SerializeField]
    [ReadOnly]
    private RectTransform titleRect;

    [SerializeField]
    [ReadOnly]
    private Image titleImage;

    [SerializeField]
    private float titleAnimationSpeed = 0.8f;

    [SerializeField]
    private float scaleRatio = 10.0f;

    [Header("Title Animation")]
    [SerializeField]
    [ReadOnly]
    private List<RectTransform> buttonsRects = new List<RectTransform>();

    [SerializeField]
    private float buttonsAnimationSpeed = 0.8f;

    private float backgroundDisplacementX = 0;
    private float backgroundDisplacementY = 0;
    private Vector3 animEndTitleScale;

    void Start()
    {
        animEndTitleScale = titleRect.localScale;
        titleRect.localScale *= scaleRatio;
        
        PlayTitleAnimation();
        PlayButtonsAnimation();
    }

    private void PlayTitleAnimation()
    {
        titleImage.DOFade(1, titleAnimationSpeed).SetDelay(titleAnimationSpeed / 1.5f);
        titleRect.DOScale(animEndTitleScale, titleAnimationSpeed).SetDelay(titleAnimationSpeed / 2.0f).SetEase(Ease.OutBounce);
    }

    private void PlayButtonsAnimation()
    {
        for (int i = 0; i < buttonsRects.Count; i++)
        {
            RectTransform btn = buttonsRects[i];
            btn.DOAnchorPosY( 100 - 100 * (i + 1), buttonsAnimationSpeed, false).SetDelay(titleAnimationSpeed + (buttonsAnimationSpeed / 2.0f) * (i + 1)).SetEase(Ease.OutBounce);
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void PlayTuto()
    {
        SceneManager.LoadScene(2);
    }

    public void CloseGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }

#if UNITY_EDITOR
    [Button(ButtonSizes.Medium), GUIColor(0.89f, 0.14f, 0.14f)]
    private void Populate()
    {
        backgroundImage = transform.Find("Background").GetComponent<RawImage>();

        Transform titleTransform = transform.Find("Title");
        titleImage = titleTransform.GetComponent<Image>();
        titleRect = titleTransform.GetComponent<RectTransform>();

        Button[] buttons = transform.GetComponentsInChildren<Button>();
        buttonsRects.Clear();
        foreach (Button btn in buttons)
        {
            buttonsRects.Add(btn.GetComponent<RectTransform>());
        }
    }
#endif
}
