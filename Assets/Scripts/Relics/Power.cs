using UnityEngine;

namespace Relics
{
    [CreateAssetMenu(fileName = "RangeDamageRelic", menuName = "Relics/RangeDamageRelic")]
    public class PowerRelic : Relic
    {
        public override string Name => "Power";

        [SerializeField]
        private EffectType effectType;
        
        [SerializeField]
        private float effectStrength;

        public override void ApplyEffect(PlayerController player)
        {
            player.RangeDamage = effectType switch
            {
                EffectType.Additive => player.RangeDamage + effectStrength,
                EffectType.Multiplicative => player.RangeDamage * effectStrength,
                _ => player.RangeDamage
            };
        }
    }
}
