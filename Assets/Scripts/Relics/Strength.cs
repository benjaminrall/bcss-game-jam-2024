using UnityEngine;

namespace Relics
{
    [CreateAssetMenu(fileName = "MeleeDamageRelic", menuName = "Relics/MeleeDamageRelic")]
    public class StrengthRelic : Relic
    {
        public override string Name => "Strength";

        [SerializeField]
        private EffectType effectType;
        
        [SerializeField]
        private float effectStrength;

        public override void ApplyEffect(PlayerController player)
        {
            player.MeleeDamage = effectType switch
            {
                EffectType.Additive => player.MeleeDamage + effectStrength,
                EffectType.Multiplicative => player.MeleeDamage * effectStrength,
                _ => player.MeleeDamage
            };
        }
    }
}
