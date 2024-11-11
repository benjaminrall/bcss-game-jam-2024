using System;
using UnityEngine;

namespace Relics
{
    [CreateAssetMenu(fileName = "DashSpeedRelic", menuName = "Relics/DashSpeedRelic")]
    public class BoostRelic : Relic
    {
        public override string Name => "Boost";
        
        [SerializeField]
        private EffectType effectType;
        
        [SerializeField]
        private float effectStrength;

        public override void ApplyEffect(PlayerController player)
        {
            player.DashSpeed = effectType switch
            {
                EffectType.Additive => player.DashSpeed + effectStrength,
                EffectType.Multiplicative => player.DashSpeed * effectStrength,
                _ => player.DashSpeed
            };
        }
    }
}
