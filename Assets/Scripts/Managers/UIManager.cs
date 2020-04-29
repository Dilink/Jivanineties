using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Sirenix.OdinInspector;

public class UIManager : MonoBehaviour
{
    public RectTransform tokendoRect;
    public TMP_Text tokendoCountText;

    public void UpdateTokendoCount(int count)
    {
        tokendoCountText.SetText("" + count);
        tokendoRect.DOShakeScale(0.5f);
    }

#if UNITY_EDITOR
    [Button(ButtonSizes.Medium), GUIColor(0.89f, 0.14f, 0.14f)]
    private void Populate()
    {
        tokendoRect = transform.Find("Canvas/Tokendo/Image").GetComponent<RectTransform>();
        tokendoCountText = transform.Find("Canvas/Tokendo/Count").GetComponent<TMP_Text>();
    }
#endif
}
