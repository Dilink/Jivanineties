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
    public CombatController combatController;
    public Ma_SoundManager sM;
    public GameObject lifeBar;
    public GameObject VFXSmoke; 

    /// <summary>
    /// Gameplay only
    /// </summary>
    private int _tokendoAmount = 0;
    public int tokendoAmount
    {
        get => _tokendoAmount;
        set
        {
            _tokendoAmount = value;
            uiManager.UpdateTokendoCount(_tokendoAmount);
            sM.PlaySound(GameSound.Pickup_Tokkendo);
        }
    }

    [HideInInspector]
    public PlayerController player;

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
        yield return null;

        combatController.MoveToNextPhase();
    }

    private void OnAIStateChanged(IABehaviour entity, IAState oldState, IAState newState)
    {
        if (oldState == IAState.justSpawned && newState != IAState.dead)
        {
            remainingEnemies.Add(entity);
            uiManager.OnEnemySpawned(entity);
        }
        else if (newState == IAState.dead)
        {
            remainingEnemies.Remove(entity);
            uiManager.OnEnemyDied(entity);
            StartCoroutine(DelayDestroy(entity));
        }
    }

    IEnumerator DelayDestroy(IABehaviour entity)
    {
        yield return new WaitForSeconds(1f);
        Destroy(entity.gameObject);
    }

#if UNITY_EDITOR
    [Button(ButtonSizes.Medium), GUIColor(0.89f, 0.14f, 0.14f)]
    private void Populate()
    {
        uiManager = transform.GetComponentInChildren<UIManager>();
        uiManager.Populate();

        inputManager = transform.GetComponentInChildren<InputManager>();
        combatController = transform.GetComponentInChildren<CombatController>();
    }
#endif
}
