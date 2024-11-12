using UnityEngine;

namespace Relics
{
    [CreateAssetMenu(fileName = "AdrenalineRelic", menuName = "Relics/AdrenalineRelic")]
    public class AdrenalineRelic : Relic
    {
        public override string Name => "Adrenaline";

        public override void ApplyEffect(PlayerController player)
        {
            player.Adrenaline = true;
        }
    }
}
