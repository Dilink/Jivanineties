using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterShield : MonoBehaviour
{
    public void playShieldSound()
    {
        GameManager.Instance.sM.PlaySoundPositioned(GameSound.WaterShied, transform.position);

    }
}
