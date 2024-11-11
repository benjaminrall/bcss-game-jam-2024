using UnityEngine;

namespace Relics
{
    [CreateAssetMenu(fileName = "ShotgunRelic", menuName = "Relics/ShotgunRelic")]
    public class ShotgunRelic : Relic
    {
        public override string Name => "Shotgun";

        public override void ApplyEffect(PlayerController player)
        {
            player.Shotgun = true;
        }
    }
}