using UnityEngine;

namespace Relics
{
    [CreateAssetMenu(fileName = "BurstRelic", menuName = "Relics/BurstRelic")]
    public class BurstRelic : Relic
    {
        public override string Name => "Burst";

        public override void ApplyEffect(PlayerController player)
        {
            player.Burst = true;
        }
    }
}