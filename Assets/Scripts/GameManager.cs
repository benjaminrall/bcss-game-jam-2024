using System;
using System.Collections.Generic;
using System.Linq;
using Relics;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public PlayerController player;

    public Relic[] commonRelics;
    public Relic[] adaptiveRelics;
    public Relic[] pureRelics;

    [Serializable]
    public struct Test
    {
        public int i1;
        public int i2;
    }

    public Test testRelic;
    
    // Available relics
    private List<Relic> _availableCommonRelics;
    private List<Relic> _availableAdaptiveRelics;
    private List<Relic> _availablePureRelics;
    private List<Relic>[] _availableRelics;
    private int _availableRelicsCount;
    
    // Currently active relics
    private List<Relic> _activeRelics;

    private bool ValidateIndex((int, int) index) => 
        index.Item1 is >= 0 and < 3 && 
        index.Item2 >= 0 && index.Item2 < _availableRelics[index.Item1].Count;
    
    public Relic GetRelic((int, int) index)
    {
        return !ValidateIndex(index) ? null : _availableRelics[index.Item1][index.Item2];
    }

    private (int, int) RandomCommonRelic() => (0, Random.Range(0, _availableRelics[0].Count));
    private (int, int) RandomAdaptiveRelic() => (1, Random.Range(0, _availableRelics[1].Count));
    private (int, int) RandomPureRelic() => (2, Random.Range(0, _availableRelics[2].Count));
    
    
    public (int, int) RandomCommonRelic(PlayerColour colour)
    {
        if (colour == PlayerColour.White)
        {
            colour = Random.Range(0, 3) switch
            {
                0 => PlayerColour.Red,
                1 => PlayerColour.Green,
                2 => PlayerColour.Blue,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        List<int> colourIndices = _availableRelics[0]
            .Select((relic, index) => new {relic, index})
            .Where(x => x.relic.Colour == colour)
            .Select(x => x.index)
            .ToList();

        return colourIndices.Count == 0 ? RandomCommonRelic() : (0, colourIndices[Random.Range(0, colourIndices.Count)]);
    }

    public (int, int) RandomUncommonRelic(PlayerColour adaptiveColour)
    {
        float type = Random.Range(0.0f, 1.0f);
        
        // Handles random pure relic case
        if (type < (float) _availableRelics[2].Count / (_availableRelics[1].Count + _availableRelics[2].Count))
        {
            return RandomPureRelic();
        }
        
        // Handles random adaptive relic case
        if (adaptiveColour == PlayerColour.White)
        {
            adaptiveColour = Random.Range(0, 3) switch
            {
                0 => PlayerColour.Red,
                1 => PlayerColour.Green,
                2 => PlayerColour.Blue,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        (int, int) relicIndex = RandomAdaptiveRelic();
        _availableRelics[1][relicIndex.Item2].Colour = adaptiveColour;
        return relicIndex;
    }

    public void ActivateRelic((int, int) index)
    {
        Relic relic = GetRelic(index);
        
        if (!relic) return;
        
        _activeRelics.Add(relic);
        _availableRelics[index.Item1].RemoveAt(index.Item2);
        
        relic.ApplyEffect(player);
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        // Constructs available common relics
        _availableCommonRelics = new List<Relic>(commonRelics.Length * 3);
        foreach (Relic relic in commonRelics)
        {
            _availableCommonRelics.Add(relic.GetColourInstance(PlayerColour.Red));
            _availableCommonRelics.Add(relic.GetColourInstance(PlayerColour.Green));
            _availableCommonRelics.Add(relic.GetColourInstance(PlayerColour.Blue));
        }

        // Constructs available adaptive uncommon relics
        _availableAdaptiveRelics = new List<Relic>(adaptiveRelics.Length);
        _availableAdaptiveRelics.AddRange(adaptiveRelics);

        // Constructs available pure uncommon relics
        _availablePureRelics = new List<Relic>(pureRelics.Length);
        foreach (Relic relic in _availablePureRelics)
        {
            _availablePureRelics.Add(relic.GetColourInstance(PlayerColour.White));
        }

        // Constructs available relics
        _availableRelicsCount = _availableCommonRelics.Count + _availablePureRelics.Count + _availableAdaptiveRelics.Count;
        _availableRelics = new[] { _availableCommonRelics, _availableAdaptiveRelics, _availablePureRelics };
        _activeRelics = new List<Relic>();
        ActivateRelic((testRelic.i1, testRelic.i2));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
