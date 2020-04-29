using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Sirenix.OdinInspector;

public class UIManager : MonoBehaviour
{
    public RectTransform tokendoRect;
    public TMP_Text tokendoCountText;
    public RectTransform newPhaseRect;
    public TMP_Text newPhaseText;
    public RectTransform movementLimitAnchor;

    private Vector3 InitialNewPhaseTransform;

    private void Awake()
    {
        Vector3 v = newPhaseRect.position;
        InitialNewPhaseTransform = new Vector3(v.x, v.y, v.z);
    }

    public void UpdateTokendoCount(int count)
    {
        tokendoCountText.SetText("" + count);
        tokendoRect.DOShakeScale(0.5f);
    }

    public void ShowAlertText(string text)
    {
        newPhaseText.SetText(text);
        
        var el = newPhaseRect;
        float midScreen = ((InitialNewPhaseTransform.x - movementLimitAnchor.position.x) / 2.0f) - (el.sizeDelta.x / 2.0f);
        el.DOMoveX(midScreen, 0.7f, false).SetEase(Ease.OutQuint);
        el.DOMoveX(movementLimitAnchor.position.x, 0.3f, false).SetDelay(0.7f).SetEase(Ease.InQuint).OnComplete(() =>
        {
            el.DOMoveX(InitialNewPhaseTransform.x + el.sizeDelta.x / 2.0f, 0);
        });
    }

#if UNITY_EDITOR
    [Button(ButtonSizes.Medium), GUIColor(0.89f, 0.14f, 0.14f)]
    public void Populate()
    {
        tokendoRect = transform.Find("Canvas/Tokendo/Image").GetComponent<RectTransform>();
        tokendoCountText = transform.Find("Canvas/Tokendo/Count").GetComponent<TMP_Text>();

        var newPhaseTransform = transform.Find("Canvas/Phases/Text");
        newPhaseText = newPhaseTransform.GetComponent<TMP_Text>();
        newPhaseRect = newPhaseTransform.GetComponent<RectTransform>();

        movementLimitAnchor = transform.Find("Canvas/MovementLimitAnchor").GetComponent<RectTransform>();
    }
#endif
}
