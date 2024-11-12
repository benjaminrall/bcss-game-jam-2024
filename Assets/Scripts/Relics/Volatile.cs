using UnityEngine;

namespace Relics
{
    [CreateAssetMenu(fileName = "VolatileRelic", menuName = "Relics/VolatileRelic")]
    public class VolatileRelic : Relic
    {
        public override string Name => "Volatile";
        
        public override void ApplyEffect(PlayerController player)
        {
            player.Volatile = true;
        }
    }
}
