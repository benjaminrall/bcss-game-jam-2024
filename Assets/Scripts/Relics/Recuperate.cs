using UnityEngine;

namespace Relics
{
    [CreateAssetMenu(fileName = "AmmoRechargeDelayRelic", menuName = "Relics/AmmoRechargeDelayRelic")]
    public class RecuperateRelic : Relic
    {
        public override string Name => "Recuperate";

        [SerializeField]
        private EffectType effectType;
        
        [SerializeField]
        private float effectStrength;

        public override void ApplyEffect(PlayerController player)
        {
            player.AmmoRechargeDelay = effectType switch
            {
                EffectType.Additive => player.AmmoRechargeDelay + effectStrength,
                EffectType.Multiplicative => player.AmmoRechargeDelay * effectStrength,
                _ => player.AmmoRechargeDelay
            };
        }
    }
}
