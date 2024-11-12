using UnityEngine;

namespace Relics
{
    [CreateAssetMenu(fileName = "HealthRelic", menuName = "Relics/HealthRelic")]
    public class VitalityRelic : Relic
    {
        public override string Name => "Health";

        [SerializeField]
        private EffectType effectType;
        
        [SerializeField]
        private float effectStrength;

        public override void ApplyEffect(PlayerController player)
        {
            player.Health = effectType switch
            {
                EffectType.Additive => player.Health + effectStrength,
                EffectType.Multiplicative => player.Health * effectStrength,
                _ => player.Health
            };
        }
    }
}
