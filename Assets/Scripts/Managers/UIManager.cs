using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Sirenix.OdinInspector;

[System.Serializable]
public struct EnemyLocator
{
    public GameObject go;
    public Image image;
    public RectTransform rect;
}

public class UIManager : MonoBehaviour
{
    public Canvas canvas;
    public RectTransform tokendoRect;
    public TMP_Text tokendoCountText;
    public RectTransform newPhaseRect;
    public TMP_Text newPhaseText;
    public RectTransform movementLimitAnchor;

    [ShowInInspector]
    [ReadOnly]
    [SerializeField]
    private CameraController cameraController;

    private Vector3 InitialNewPhaseTransform;

    public GameObject enemyLocatorPrefab;

    private Dictionary<IABehaviour, EnemyLocator> enemyLocators = new Dictionary<IABehaviour, EnemyLocator>();

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

    public void OnEnemySpawned(IABehaviour entity)
    {
        GameObject go = Instantiate(enemyLocatorPrefab, canvas.transform);
        go.name = "EnemyLocator (" + enemyLocators.Count + ")";

        EnemyLocator enemyLocator = new EnemyLocator();
        enemyLocator.go = go;
        enemyLocator.image = go.GetComponent<Image>();
        enemyLocator.rect = go.GetComponent<RectTransform>();

        enemyLocators.Add(entity, enemyLocator);
    }

    public void OnEnemyDied(IABehaviour entity)
    {
        Destroy(enemyLocators[entity].go);
        enemyLocators.Remove(entity);
    }

    float EDGE = 0.45f;

    void Update()
    {
        UpdateEnemyLocators();
    }

    private void UpdateEnemyLocators()
    {
        if (enemyLocators.Count == 0)
        {
            return;
        }

        Camera camera = cameraController.playerCamera;

        foreach (var item in enemyLocators)
        {
            GameObject lookat = item.Key.gameObject;
            EnemyLocator locator = item.Value;

            //Vector3 viewpos = camera.transform.InverseTransformPoint(lookat.transform.position);
            //viewpos.Normalize();

            // get a position from -0.5 to 0.5 in xy space - which actually doesn't do what it claims sigh.
            Vector3 viewpos = camera.WorldToViewportPoint(lookat.transform.position);
            viewpos.x = (viewpos.x - 0.5f);
            viewpos.y = (viewpos.y - 0.5f);

            // fade if not needed
            if (((viewpos.x > -EDGE && viewpos.x < EDGE) && (viewpos.y > -EDGE && viewpos.y < EDGE)) && viewpos.z > 0)
            {
                Color color = locator.image.color;
                color.a = color.a > 0.0f ? color.a - 0.01f : 0.0f;
                locator.image.color = color;
            }
            else
            {
                Color color = locator.image.color;
                color.a = color.a < 0.5f ? color.a + 0.01f : 0.5f;
                locator.image.color = color;
            }

            // rotate the arrow around lookat axis
            //float angle = Mathf.Atan2(viewpos.y, viewpos.x) * Mathf.Rad2Deg + 90;
            //transform.localRotation = Quaternion.AngleAxis(angle, Vector3.forward) * Quaternion.Euler(270, 0, 0);

            // constrain the arrow to the screen; it should fade away when on screen; we allow it to come off the edge in that case
            if (viewpos.x > EDGE) viewpos.x = EDGE;
            if (viewpos.x < -EDGE) viewpos.x = -EDGE;
            if (viewpos.y > EDGE) viewpos.y = EDGE;
            if (viewpos.y < -EDGE) viewpos.y = -EDGE;

            // place the arrow
            //transform.localPosition = new Vector3(viewpos.x * DIST, viewpos.y * DIST, DIST / 2.0f);
            locator.rect.localPosition = new Vector3(viewpos.x * Screen.width, viewpos.y * Screen.height, 0);
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
        var canvasTransform = transform.Find("Canvas");
        canvas = canvasTransform.GetComponent<Canvas>();

        tokendoRect = canvasTransform.Find("Tokendo/Image").GetComponent<RectTransform>();
        tokendoCountText = canvasTransform.Find("Tokendo/Count").GetComponent<TMP_Text>();

        var newPhaseTransform = canvasTransform.Find("Phases/Text");
        newPhaseText = newPhaseTransform.GetComponent<TMP_Text>();
        newPhaseRect = newPhaseTransform.GetComponent<RectTransform>();

        movementLimitAnchor = canvasTransform.Find("MovementLimitAnchor").GetComponent<RectTransform>();

        cameraController = GameObject.FindObjectOfType<CameraController>();
    }
#endif
}
