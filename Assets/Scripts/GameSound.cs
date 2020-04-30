/// <summary>
/// This enum defines the sound types available to play.
/// Each enum value can have AudioClips assigned to it in the SoundManager's Inspector pane.
/// </summary>
public enum GameSound
{
    // It's advisable to keep 'None' as the first option, since it helps exposing this enum in the Inspector.
    // If the first option is already an actual value, then there is no "nothing selected" option.
    None,

    Payer_Attack,
    Payer_Attack_Up,
    Payer_Dash,//
    Payer_Dash_Up,//
    Payer_Death,
    Payer_TakeDamage,//

    WaterShied,
    WaterIn,//
    WaterOut,//

    Pickup_Tokkendo,

    EnemyAttack,//
    EnemyAttack_up,
    Enemy_stun,//
    Enemy_noise,//
    Enemy_takeDamage,//
    Enemy_Death,//

    Death_Smoke,

    Enemy_Noise2,

}
