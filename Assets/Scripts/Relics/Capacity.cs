using UnityEngine;

namespace Relics
{
    [CreateAssetMenu(fileName = "MaxAmmoRelic", menuName = "Relics/MaxAmmoRelic")]
    public class CapacityRelic : Relic
    {
        public override string Name => "Capacity";
        
        [SerializeField]
        private EffectType effectType;
        
        [SerializeField]
        private float effectStrength;

        public override void ApplyEffect(PlayerController player)
        {
            player.MaxAmmo = effectType switch
            {
                EffectType.Additive => player.MaxAmmo + effectStrength,
                EffectType.Multiplicative => player.MaxAmmo * effectStrength,
                _ => player.MaxAmmo
            };
        }
    }
}
