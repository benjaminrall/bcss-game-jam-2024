using UnityEngine;

namespace Relics
{
    [CreateAssetMenu(fileName = "HomingRelic", menuName = "Relics/HomingRelic")]
    public class HomingRelic : Relic
    {
        public override string Name => "Homing";

        public override void ApplyEffect(PlayerController player)
        {
            player.Homing = true;
        }
    }
}
