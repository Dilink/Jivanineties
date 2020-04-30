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

    [ShowInInspector]
    [ReadOnly]
    [SerializeField]
    private CameraController cameraController;

    public Image enemyLocatorImage;
    public RectTransform enemyLocatorRect;

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


    float EDGE = 0.45f;
    Color color = new Color(1, 1, 1, 1);

    void Update()
    {
        var enemies = GameManager.Instance.remainingEnemies;

        if (enemies.Count > 0)
        {
            GameObject lookat = enemies[0].gameObject;
            Camera camera = cameraController.playerCamera;

            //Vector3 viewpos = camera.transform.InverseTransformPoint(lookat.transform.position);
            //viewpos.Normalize();

            // get a position from -0.5 to 0.5 in xy space - which actually doesn't do what it claims sigh.
            Vector3 viewpos = camera.WorldToViewportPoint(lookat.transform.position);
            viewpos.x = (viewpos.x - 0.5f);
            viewpos.y = (viewpos.y - 0.5f);

            // fade if not needed
            if (((viewpos.x > -EDGE && viewpos.x < EDGE) && (viewpos.y > -EDGE && viewpos.y < EDGE)) && viewpos.z > 0)
            {
                color.a = color.a > 0.0f ? color.a - 0.01f : 0.0f;
                enemyLocatorImage.color = color;
            }
            else
            {
                color.a = color.a < 0.5f ? color.a + 0.01f : 0.5f;
                enemyLocatorImage.color = color;
            }

            // rotate the arrow around lookat axis
            float angle = Mathf.Atan2(viewpos.y, viewpos.x) * Mathf.Rad2Deg + 90;
            //transform.localRotation = Quaternion.AngleAxis(angle, Vector3.forward) * Quaternion.Euler(270, 0, 0);

            // constrain the arrow to the screen; it should fade away when on screen; we allow it to come off the edge in that case
            if (viewpos.x > EDGE) viewpos.x = EDGE;
            if (viewpos.x < -EDGE) viewpos.x = -EDGE;
            if (viewpos.y > EDGE) viewpos.y = EDGE;
            if (viewpos.y < -EDGE) viewpos.y = -EDGE;

            // place the arrow
            //transform.localPosition = new Vector3(viewpos.x * DIST, viewpos.y * DIST, DIST / 2.0f);
            enemyLocatorRect.localPosition = new Vector3(viewpos.x * Screen.width, viewpos.y * Screen.height, 0);
        }
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

        cameraController = GameObject.FindObjectOfType<CameraController>();

        var enemyLocatorTransform = transform.Find("Canvas/EnemyLocator");
        enemyLocatorImage = enemyLocatorTransform.GetComponent<Image>();
        enemyLocatorRect = enemyLocatorTransform.GetComponent<RectTransform>();
    }
#endif
}
