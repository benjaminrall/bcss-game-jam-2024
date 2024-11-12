using UnityEngine;

namespace Relics
{
    [CreateAssetMenu(fileName = "MeleeRangeRelic", menuName = "Relics/MeleeRangeRelic")]
    public class ReachRelic : Relic
    {
        public override string Name => "Reach";

        [SerializeField]
        private EffectType effectType;
        
        [SerializeField]
        private float effectStrength;

        public override void ApplyEffect(PlayerController player)
        {
            player.MeleeRange = effectType switch
            {
                EffectType.Additive => player.MeleeRange + effectStrength,
                EffectType.Multiplicative => player.MeleeRange * effectStrength,
                _ => player.MeleeRange
            };
        }
    }
}
