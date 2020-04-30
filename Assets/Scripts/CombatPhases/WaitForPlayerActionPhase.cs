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

    public IEnumerator Execute(Action onEnd, CombatPhaseData data)
    {
        yield return null;

        if (actionType == PlayerActionType.MOVEMENT)
        {
            PlayerController player = GameManager.Instance.player;

            Vector3 initialPosition = player.transform.position;

            while (initialPosition == player.transform.position)
            {
                yield return null;
            }
        }

        else if (actionType == PlayerActionType.ATTACK)
        {

        }

        onEnd();
    }

    public CombatPhaseType ReturnType()
    {
        return CombatPhaseType.WaitPhase;
    }
}
