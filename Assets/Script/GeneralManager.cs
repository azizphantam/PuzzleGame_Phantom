
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Script
{
    [System.Serializable]
    public struct Positions
    {
        public List<Transform> differentPositions;
    }


    public class GeneralManager : MonoBehaviour
    {
        
        public DifferentCities[] Cities;

        public List<Positions> buildingpos = new();
        private int CityNumber;
        public Camera cam;

        public List<Transform> CamPositions;
        public void SelectCity(int User_Select_City)
        {
            CityNumber = User_Select_City;
        }

        public void moveGamePLay()
        {
            cam.transform.DOLocalMove(CamPositions[0].transform.position, 5);
        }

        public void moveMakeBuildings()
        {
            cam.transform.DOLocalMove(CamPositions[1].transform.position, 5);
        }
        public void OnButtonClickBuilding(int buildNumber)
        {
            BuildingPositions(buildNumber);
            
        }

        
        private void BuildingPositions(int building_number)
        {
            Instantiate(Cities[CityNumber].Building[building_number].buildings[0],
                buildingpos[CityNumber].differentPositions[building_number].transform.position, Quaternion.identity);
            //we save this value so we can interactable false in start of game 
            Cities[0].Building[0].isused.Equals(true);
        }

       
    }
}