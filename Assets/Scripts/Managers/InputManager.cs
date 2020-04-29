using UnityEngine;

public class InputManager: MonoBehaviour
{
    public bool UP { get { return Input.GetAxis("Vertical") > 0; } }
    public bool DOWN { get { return Input.GetAxis("Vertical") < 0; } }
    public bool RIGHT { get { return Input.GetAxis("Horizontal") > 0; } }
    public bool LEFT { get { return Input.GetAxis("Horizontal") < 0; } }

    public bool ATTACK { get { return Input.GetButtonDown("Attack"); } }
    public bool DODGE { get { return Input.GetButtonDown("Dodge"); } }
    public bool ATTACK_HOLD { get { return Input.GetButton("Attack"); } }
    public bool DODGE_HOLD { get { return Input.GetButton("Dodge"); } }
    public bool POWER_HOLD { get { return Input.GetButton("Power"); } }
}
