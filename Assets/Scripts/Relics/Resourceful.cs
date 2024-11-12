using UnityEngine;

namespace Relics
{
    [CreateAssetMenu(fileName = "ResourcefulRelic", menuName = "Relics/ResourcefulRelic")]
    public class ResourcefulRelic : Relic
    {
        public override string Name => "Resourceful";

        public override void ApplyEffect(PlayerController player)
        {
            player.Resourceful = true;
        }
    }
}
