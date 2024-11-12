using UnityEngine;

namespace Relics
{
    [CreateAssetMenu(fileName = "StunRelic", menuName = "Relics/StunRelic")]
    public class StunRelic : Relic
    {
        public override string Name => "Stun";

        public override void ApplyEffect(PlayerController player)
        {
            player.Stun = true;
        }
    }

}
