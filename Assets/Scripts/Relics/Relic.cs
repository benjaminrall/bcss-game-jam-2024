using UnityEngine;

namespace Relics
{
    public abstract class Relic : ScriptableObject
    {
        public abstract string Name { get; }
        public PlayerColour Colour { get; set; }

        public abstract void ApplyEffect(PlayerController player);
        
        public Relic GetColourInstance(PlayerColour colour)
        {
            Relic newRelic = (Relic) MemberwiseClone();
            newRelic.Colour = colour;
            return newRelic;
        }
    }
}