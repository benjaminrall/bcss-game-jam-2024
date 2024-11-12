using UnityEngine;

namespace Relics
{
    [CreateAssetMenu(fileName = "LifeStealRelic", menuName = "Relics/LifeStealRelic")]
    public class LifeStealRelic : Relic
    {
        public override string Name => "Life Steal";

        public override void ApplyEffect(PlayerController player)
        {
            player.LifeSteal = true;
        }
    }
}
