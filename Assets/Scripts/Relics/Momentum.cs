using UnityEngine;

namespace Relics
{
    [CreateAssetMenu(fileName = "MomentumRelic", menuName = "Relics/MomentumRelic")]
    public class MomentumRelic : Relic
    {
        public override string Name => "Momentum";

        public override void ApplyEffect(PlayerController player)
        {
            player.Momentum = true;
        }
    }
}
