using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class GameManager : Singleton<GameManager>
{
    /// <summary>
    /// Editor only
    /// </summary>
    public int tokendoCount;

    public UIManager uiManager;
    public InputManager inputManager;

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

    [HideInInspector]
    public List<IABehaviour> remainingEnemies = new List<IABehaviour>();

    void Start()
    {
        tokendoAmount = tokendoCount;
        player = FindObjectOfType<PlayerController>();

        IABehaviour.iaStateChangedDelegate += OnAIStateChanged;

        StartCoroutine(OnFirstFrame());
    }

    new void OnDestroy()
    {
        base.OnDestroy();
        IABehaviour.iaStateChangedDelegate -= OnAIStateChanged;
    }

    public void AddTokendo(int count)
    {
        tokendoAmount += count;
    }

    private IEnumerator OnFirstFrame()
    {
        IABehaviour[] allAI = GameObject.FindObjectsOfType<IABehaviour>();
        remainingEnemies.AddRange(allAI);
        yield return null;
    }

    private void OnAIStateChanged(IABehaviour entity, IAState oldState, IAState newState)
    {
        if (oldState == IAState.justSpawned)
        {
            remainingEnemies.Add(entity);
        }
        else if (newState == IAState.dead)
        {
            remainingEnemies.Remove(entity);
        }
    }

#if UNITY_EDITOR
    [Button(ButtonSizes.Medium), GUIColor(0.89f, 0.14f, 0.14f)]
    private void Populate()
    {
        uiManager = transform.GetComponentInChildren<UIManager>();
        uiManager.Populate();

        inputManager = transform.GetComponentInChildren<InputManager>();
    }
#endif
}
