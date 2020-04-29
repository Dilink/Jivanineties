using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    /// <summary>
    /// Editor only
    /// </summary>
    public int tokendoCount;

    private UIManager uiManager;

    /// <summary>
    /// Gameplay only
    /// </summary>
    private int _tokendoAmount = 0;
    private int tokendoAmount
    {
        get => _tokendoAmount;
        set
        {
            uiManager.UpdateTokendoCount(_tokendoAmount);
        }
    }

    void Start()
    {
        _tokendoAmount = tokendoCount;
        uiManager = GetComponent<UIManager>();
    }

    public void AddTokendo(int count)
    {
        tokendoAmount += count;
    }
}
