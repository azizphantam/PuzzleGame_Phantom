using System.Collections.Generic;
using UnityEngine;




[System.Serializable]
public struct BuildingsPrefab
{
    public List<GameObject> buildingprefabs;
    
}
[System.Serializable]
public struct DifferentBuiding
{
    public List<GameObject> Buildings;
}



[CreateAssetMenu(fileName = "Buildings", menuName = "MakeBuildings", order = 0)] 
public class City_1 : ScriptableObject
{
    public List<DifferentBuiding> Building = new();
}