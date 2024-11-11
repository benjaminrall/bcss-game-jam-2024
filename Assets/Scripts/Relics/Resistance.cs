using UnityEngine;

namespace Relics
{
    [CreateAssetMenu(fileName = "DefenceRelic", menuName = "Relics/DefenceRelic")]
    public class ResistanceRelic : Relic
    {
        public override string Name => "Resistance";

        [SerializeField]
        private EffectType effectType;
        
        [SerializeField]
        private float effectStrength;

        public override void ApplyEffect(PlayerController player)
        {
            player.Defence = effectType switch
            {
                EffectType.Additive => player.Defence + effectStrength,
                EffectType.Multiplicative => player.Defence * effectStrength,
                _ => player.Defence
            };
        }
    } 
}
