using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Script
{
    public class GeneralManager : MonoBehaviour
    {
        public City_1 city1;
        private int BuildnumberInstantiate = 0;
        public List<Transform> buildingpos = new();
        private int BuildingPositions=0;
        
    }
}