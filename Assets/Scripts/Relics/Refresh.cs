using UnityEngine;

namespace Relics
{
    [CreateAssetMenu(fileName = "DashDelayRelic", menuName = "Relics/DashDelayRelic")]
    public class RefreshRelic: Relic
    {
        public override string Name => "Refresh";

        [SerializeField]
        private EffectType effectType;
        
        [SerializeField]
        private float effectStrength;

        public override void ApplyEffect(PlayerController player)
        {
            player.DashDelay = effectType switch
            {
                EffectType.Additive => player.DashDelay + effectStrength,
                EffectType.Multiplicative => player.DashDelay * effectStrength,
                _ => player.DashDelay
            };
        }
    }
}
