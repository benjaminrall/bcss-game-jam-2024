using UnityEngine;

namespace Relics
{
    [CreateAssetMenu(fileName = "ToxicRelic", menuName = "Relics/ToxicRelic")]
    public class ToxicRelic : Relic
    {
        public override string Name => "Toxic";

        public override void ApplyEffect(PlayerController player)
        {
            player.Toxic = true;
        }
    }
}
