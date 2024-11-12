using UnityEngine;

namespace Relics
{
    [CreateAssetMenu(fileName = "MeleeSpeedRelic", menuName = "Relics/MeleeSpeedRelic")]
    public class SpeedRelic : Relic
    {
        public override string Name => "Speed";

        [SerializeField]
        private EffectType effectType;
        
        [SerializeField]
        private float effectStrength;

        public override void ApplyEffect(PlayerController player)
        {
            player.MeleeSpeed = effectType switch
            {
                EffectType.Additive => player.MeleeSpeed + effectStrength,
                EffectType.Multiplicative => player.MeleeSpeed * effectStrength,
                _ => player.MeleeSpeed
            };
        }
    }
}
