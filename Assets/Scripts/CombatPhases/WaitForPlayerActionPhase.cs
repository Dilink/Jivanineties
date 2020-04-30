using System;
using System.Collections;
using UnityEngine;

[System.Serializable]
public enum PlayerActionType
{
    ATTACK,
    DASH,
    MOVEMENT,
}

[System.Serializable]
public class WaitForPlayerActionPhase : ICombatPhase
{
    public PlayerActionType actionType;
    public bool triggerOnSpecialAction;

    private PlayerController player;
    private Action onPhaseEnd;

    public IEnumerator Execute(Action onEnd, CombatPhaseData data)
    {
        yield return null;

        onPhaseEnd = onEnd;


        player = GameManager.Instance.player;


        if (actionType == PlayerActionType.MOVEMENT)
        {
            Vector3 initialPosition = player.transform.position;

            while (initialPosition == player.transform.position)
            {
                yield return null;
            }

            onEnd();
        }

        else if (actionType == PlayerActionType.ATTACK)
        {
            player.onAttackDelegate += OnAttack;
        }

        else if (actionType == PlayerActionType.DASH)
        {
            player.onDashDelegate += OnDash;
        }
    }

    private void OnAttack(bool isPoweredAttack)
    {
        if (triggerOnSpecialAction && isPoweredAttack)
        {
            player.onAttackDelegate -= OnAttack;
            onPhaseEnd();
        }
        else if (!triggerOnSpecialAction && !isPoweredAttack)
        {
            player.onAttackDelegate -= OnAttack;
            onPhaseEnd();
        }
    }

    private void OnDash(Transform destination)
    {
        if (triggerOnSpecialAction && destination != null)
        {
            player.onDashDelegate -= OnDash;
            onPhaseEnd();
        }
        else if (!triggerOnSpecialAction && destination == null)
        {
            player.onDashDelegate -= OnDash;
            onPhaseEnd();
        }
    }

    public CombatPhaseType ReturnType()
    {
        return CombatPhaseType.WaitPhase;
    }
}
