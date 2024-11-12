using UnityEngine;
using UnityEngine.Serialization;

public enum RelicType
{
    // Common Movement
    MoveSpeed,
    DashSpeed,
    DashDelay,
    
    // Common Melee
    MeleeDamage,
    MeleeSpeed,
    MeleeRange,
    
    // Common Ranged
    RangeDamage,
    RangeSpeed,
    RangeCapacity,
    RangeRecharge,
    RangeCooldown,
    
    // Common Others
    Health,
    Defence,
    
    // Uncommon Melee
    Combo,
    Stun,
    LifeSteal,
    
    // Uncommon Range
    Shotgun,
    Burst,
    Volatile,
    Homing,
    Momentum,
    Adrenaline,
    Resourceful,
    
    // Uncommon Movement
    Phase,
    
    // Uncommon Others
    Toxic,
}

