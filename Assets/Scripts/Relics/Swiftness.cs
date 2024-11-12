using UnityEngine;

namespace Relics
{
    [CreateAssetMenu(fileName = "MoveSpeedRelic", menuName = "Relics/MoveSpeedRelic")]
    public class SwiftnessRelic : Relic
    {
        public override string Name => "Swiftness";

        [SerializeField]
        private EffectType effectType;
        
        [SerializeField]
        private float effectStrength;

        public override void ApplyEffect(PlayerController player)
        {
            player.MoveSpeed = effectType switch
            {
                EffectType.Additive => player.MoveSpeed + effectStrength,
                EffectType.Multiplicative => player.MoveSpeed * effectStrength,
                _ => player.MoveSpeed
            };
        }
    }
}
