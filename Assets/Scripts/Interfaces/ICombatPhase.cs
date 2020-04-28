using System;
using System.Collections;

public interface ICombatPhase
{
    IEnumerator Execute(Action onEnd);
}
