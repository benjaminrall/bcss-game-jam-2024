using UnityEngine;

namespace Relics
{
    [CreateAssetMenu(fileName = "ComboRelic", menuName = "Relics/ComboRelic")]
    public class ComboRelic : Relic
    {
        public override string Name => "Combo";

        public override void ApplyEffect(PlayerController player)
        {
            player.Combo = true;
        }
    }
}
