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
            player.MaxHealth = effectType switch
            {
                EffectType.Additive => player.MaxHealth + effectStrength,
                EffectType.Multiplicative => player.MaxHealth * effectStrength,
                _ => player.MaxHealth
            };
        }
    }
}
