using UnityEngine;

namespace Relics
{
    [CreateAssetMenu(fileName = "AmmoRechargeRateRelic", menuName = "Relics/AmmoRechargeRateRelic")]
    public class RechargeRelic : Relic
    {
        public override string Name => "Recharge";

        [SerializeField]
        private EffectType effectType;
        
        [SerializeField]
        private float effectStrength;

        public override void ApplyEffect(PlayerController player)
        {
            player.AmmoRechargeRate = effectType switch
            {
                EffectType.Additive => player.AmmoRechargeRate + effectStrength,
                EffectType.Multiplicative => player.AmmoRechargeRate * effectStrength,
                _ => player.AmmoRechargeRate
            };
        }
    }

}
