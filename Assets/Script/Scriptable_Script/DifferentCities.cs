using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DifferentBuiding
{
    public List<GameObject> buildings;
    public int isused;
    public int Stars_Required;
    public string Building_Description;
    public string Building_Name;
}
[CreateAssetMenu(fileName = "Buildings", menuName = "MakeBuildings", order = 0)]
public class DifferentCities : ScriptableObject
{
    public List<DifferentBuiding> Building = new();
}

