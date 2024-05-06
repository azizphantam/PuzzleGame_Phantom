using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DifferentBuiding
{
    public List<GameObject> buildings;
    public bool isused;
}



[CreateAssetMenu(fileName = "Buildings", menuName = "MakeBuildings", order = 0)] 
public class DifferentCities : ScriptableObject
{
    public List<DifferentBuiding> Building = new();
}

