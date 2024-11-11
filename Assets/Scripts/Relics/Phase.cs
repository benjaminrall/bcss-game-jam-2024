using UnityEngine;

namespace Relics
{
    [CreateAssetMenu(fileName = "PhaseRelic", menuName = "Relics/PhaseRelic")]
    public class PhaseRelic : Relic
    {
        public override string Name => "Phase";

        public override void ApplyEffect(PlayerController player)
        {
            player.Phase = true;
        }
    }
}
