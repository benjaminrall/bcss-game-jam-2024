using UnityEngine;

namespace Relics
{
    [CreateAssetMenu(fileName = "RangeSpeedRelic", menuName = "Relics/RangeSpeedRelic")]
    public class QuickshotRelic : Relic
    {
        public override string Name => "Quickshot";

        [SerializeField]
        private EffectType effectType;
        
        [SerializeField]
        private float effectStrength;

        public override void ApplyEffect(PlayerController player)
        {
            player.RangeSpeed = effectType switch
            {
                EffectType.Additive => player.RangeSpeed + effectStrength,
                EffectType.Multiplicative => player.RangeSpeed * effectStrength,
                _ => player.RangeSpeed
            };
        }
    }
}
