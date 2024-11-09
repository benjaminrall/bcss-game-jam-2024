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

public abstract class Relic : ScriptableObject
{
    public PlayerColour Colour { get; set; }
    
    public abstract void ApplyEffect(PlayerController player);
}

// Movement speed
[CreateAssetMenu(fileName = "MoveSpeedRelic", menuName = "Relics/MoveSpeedRelic")]
public class SwiftnessRelic : Relic
{
    public override void ApplyEffect(PlayerController player)
    {
        throw new System.NotImplementedException();
    }
}

// Dash speed
[CreateAssetMenu(fileName = "DashSpeedRelic", menuName = "Relics/DashSpeedRelic")]
public class BoostRelic : Relic
{
    public override void ApplyEffect(PlayerController player)
    {
        throw new System.NotImplementedException();
    }
}

// Dash delay
[CreateAssetMenu(fileName = "DashDelayRelic", menuName = "Relics/DashDelayRelic")]
public class RefreshRelic: Relic
{
    public override void ApplyEffect(PlayerController player)
    {
        throw new System.NotImplementedException();
    }
}

// Melee damage
[CreateAssetMenu(fileName = "MeleeDamageRelic", menuName = "Relics/MeleeDamageRelic")]
public class StrengthRelic : Relic
{
    public override void ApplyEffect(PlayerController player)
    {
        throw new System.NotImplementedException();
    }
}

// Melee speed
[CreateAssetMenu(fileName = "MeleeSpeedRelic", menuName = "Relics/MeleeSpeedRelic")]
public class SpeedRelic : Relic
{
    public override void ApplyEffect(PlayerController player)
    {
        throw new System.NotImplementedException();
    }
}

// Melee range
[CreateAssetMenu(fileName = "MeleeRangeRelic", menuName = "Relics/MeleeRangeRelic")]
public class ReachRelic : Relic
{
    public override void ApplyEffect(PlayerController player)
    {
        throw new System.NotImplementedException();
    }
}

// Range damage
[CreateAssetMenu(fileName = "RangeDamageRelic", menuName = "Relics/RangeDamageRelic")]
public class PowerRelic : Relic
{
    public override void ApplyEffect(PlayerController player)
    {
        throw new System.NotImplementedException();
    }
}

// Range speed
[CreateAssetMenu(fileName = "RangeSpeedRelic", menuName = "Relics/RangeSpeedRelic")]
public class QuickshotRelic : Relic
{
    public override void ApplyEffect(PlayerController player)
    {
        throw new System.NotImplementedException();
    }
}

// Max ammo
[CreateAssetMenu(fileName = "MaxAmmoRelic", menuName = "Relics/MaxAmmoRelic")]
public class CapacityRelic : Relic
{
    public override void ApplyEffect(PlayerController player)
    {
        throw new System.NotImplementedException();
    }
}

// Ammo recharge delay
[CreateAssetMenu(fileName = "AmmoRechargeDelayRelic", menuName = "Relics/AmmoRechargeDelayRelic")]
public class RecuperateRelic : Relic
{
    public override void ApplyEffect(PlayerController player)
    {
        throw new System.NotImplementedException();
    }
}

// Ammo recharge rate
[CreateAssetMenu(fileName = "AmmoRechargeRateRelic", menuName = "Relics/AmmoRechargeRateRelic")]
public class RechargeRelic : Relic
{
    public override void ApplyEffect(PlayerController player)
    {
        throw new System.NotImplementedException();
    }
}

// Health
[CreateAssetMenu(fileName = "HealthRelic", menuName = "Relics/HealthRelic")]
public class VitalityRelic : Relic
{
    public override void ApplyEffect(PlayerController player)
    {
        throw new System.NotImplementedException();
    }
}

// Defence
[CreateAssetMenu(fileName = "DefenceRelic", menuName = "Relics/DefenceRelic")]
public class ResistanceRelic : Relic
{
    public override void ApplyEffect(PlayerController player)
    {
        throw new System.NotImplementedException();
    }
} 

// Combo
[CreateAssetMenu(fileName = "ComboRelic", menuName = "Relics/ComboRelic")]
public class ComboRelic : Relic
{
    public override void ApplyEffect(PlayerController player)
    {
        throw new System.NotImplementedException();
    }
}

// Stun
[CreateAssetMenu(fileName = "StunRelic", menuName = "Relics/StunRelic")]
public class StunRelic : Relic
{
    public override void ApplyEffect(PlayerController player)
    {
        throw new System.NotImplementedException();
    }
}

// Life steal
[CreateAssetMenu(fileName = "LifeStealRelic", menuName = "Relics/LifeStealRelic")]
public class LifeStealRelic : Relic
{
    public override void ApplyEffect(PlayerController player)
    {
        throw new System.NotImplementedException();
    }
}

// Shotgun
[CreateAssetMenu(fileName = "ShotgunRelic", menuName = "Relics/ShotgunRelic")]
public class ShotgunRelic : Relic
{
    public override void ApplyEffect(PlayerController player)
    {
        throw new System.NotImplementedException();
    }
}

// Burst
[CreateAssetMenu(fileName = "BurstRelic", menuName = "Relics/BurstRelic")]
public class BurstRelic : Relic
{
    public override void ApplyEffect(PlayerController player)
    {
        throw new System.NotImplementedException();
    }
}

// Volatile
[CreateAssetMenu(fileName = "VolatileRelic", menuName = "Relics/VolatileRelic")]
public class VolatileRelic : Relic
{
    public override void ApplyEffect(PlayerController player)
    {
        throw new System.NotImplementedException();
    }
}

// Homing
[CreateAssetMenu(fileName = "HomingRelic", menuName = "Relics/HomingRelic")]
public class HomingRelic : Relic
{
    public override void ApplyEffect(PlayerController player)
    {
        throw new System.NotImplementedException();
    }
}

// Momentum
[CreateAssetMenu(fileName = "MomentumRelic", menuName = "Relics/MomentumRelic")]
public class MomentumRelic : Relic
{
    public override void ApplyEffect(PlayerController player)
    {
        throw new System.NotImplementedException();
    }
}

// Adrenaline
[CreateAssetMenu(fileName = "AdrenalineRelic", menuName = "Relics/AdrenalineRelic")]
public class AdrenalineRelic : Relic
{
    public override void ApplyEffect(PlayerController player)
    {
        throw new System.NotImplementedException();
    }
}

// Resourceful
[CreateAssetMenu(fileName = "ResourcefulRelic", menuName = "Relics/ResourcefulRelic")]
public class ResourcefulRelic : Relic
{
    public override void ApplyEffect(PlayerController player)
    {
        throw new System.NotImplementedException();
    }
}

// Phase
[CreateAssetMenu(fileName = "PhaseRelic", menuName = "Relics/PhaseRelic")]
public class PhaseRelic : Relic
{
    public override void ApplyEffect(PlayerController player)
    {
        throw new System.NotImplementedException();
    }
}

// Toxic
[CreateAssetMenu(fileName = "ToxicRelic", menuName = "Relics/ToxicRelic")]
public class ToxicRelic : Relic
{
    public override void ApplyEffect(PlayerController player)
    {
        throw new System.NotImplementedException();
    }
}