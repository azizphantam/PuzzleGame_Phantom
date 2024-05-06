
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
        [Header("Cities Scriptable Objects")]
        public DifferentCities[] Cities;
        [Space]
        [Header("Building Positions for Spawn")]
        public List<Positions> buildingpos = new();
        [Space]
        private int CityNumber;
        [Header("Camera for Move")]
        public Camera cam;
        [Space]
        [Header("Camera delay for  Move")]
        public int Cam_Movement_Delay;
        [Space]
        [Header("Camera Positions for Gameplay and Make Buildings")]
        public List<Transform> CamPositions;
        public void SelectCity(int User_Select_City)
        {
            CityNumber = User_Select_City;
        }

        public void moveGamePLay()
        {
            cam.transform.DOMove(CamPositions[0].transform.position, 10);
        }

        public void moveMakeBuildings()
        {
            cam.transform.DOMove(CamPositions[1].transform.position, 10);
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