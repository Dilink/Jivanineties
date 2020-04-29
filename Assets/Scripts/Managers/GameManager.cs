using UnityEngine;
using Sirenix.OdinInspector;

public class GameManager : Singleton<GameManager>
{
    /// <summary>
    /// Editor only
    /// </summary>
    public int tokendoCount;

    public UIManager uiManager;

    /// <summary>
    /// Gameplay only
    /// </summary>
    private int _tokendoAmount = 0;
    private int tokendoAmount
    {
        get => _tokendoAmount;
        set
        {
            _tokendoAmount = value;
            uiManager.UpdateTokendoCount(_tokendoAmount);
        }
    }

    [HideInInspector]
    public PlayerController player;

    void Start()
    {
        tokendoAmount = tokendoCount;
        player = FindObjectOfType<PlayerController>();
    }

    public void AddTokendo(int count)
    {
        tokendoAmount += count;
    }

#if UNITY_EDITOR
    [Button(ButtonSizes.Medium), GUIColor(0.89f, 0.14f, 0.14f)]
    private void Populate()
    {
        uiManager = transform.GetComponentInChildren<UIManager>();
        uiManager.Populate();
    }
#endif
}
