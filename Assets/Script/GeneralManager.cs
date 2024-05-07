
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Entities.UniversalDelegates;
using UnityEngine;
using UnityEngine.UI;

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
        [Space]
        [Header("instructions Panel")]
        public GameObject Instruction_Panel;
        
        [Space]
        [Header("Building Names")]
        public TMP_Text Buiding_Names;
        [Space]
        [Header("Building Description")]
        public TMP_Text Buiding_Description;
        [Space]
        private int current_BuilingMake = 0;
        [Space]
        public List<GameObject> Main_btns; // gameplay buttons and make buildings buttons ....[0] make building button [1] play game button


        private void Start()
        {
            Main_btns[0].SetActive(true);
            Main_btns[1].SetActive(false);

            
            moveMakeBuildings();
        }

        public void moveGamePLay()
        {
            cam.transform.DOMove(CamPositions[0].transform.position, 10);
            Main_btns[0].SetActive(true);
            Main_btns[1].SetActive(false);
            Instruction_Panel.SetActive(false);
        }

        public void moveMakeBuildings()
        {
            cam.transform.DOMove(CamPositions[1].transform.position, 10).OnComplete(() => Appear_UI_Instructions());
            Main_btns[0].SetActive(false);
            Main_btns[1].SetActive(true);
        }
        

        private void  Appear_UI_Instructions()
        {
            for (int i = 0; i < 4; i++)
            {
                if (Cities[0].Building[i].isused == 0)
                {
                    current_BuilingMake = i;
                    break;
                }
            }
            Instruction_Panel.SetActive(true);
            Buiding_Names.text = Cities[0].Building[current_BuilingMake].Building_Name;
            Buiding_Description.text = Cities[0].Building[current_BuilingMake].Building_Description;
        }



        public void OnButtonClickBuilding()
        {
            BuildingPositions(current_BuilingMake);
            Instruction_Panel.SetActive(false);
        }

        
        private void BuildingPositions(int building_number)
        {
           GameObject buildings =  Instantiate(Cities[0].Building[building_number].buildings[0],
                buildingpos[0].differentPositions[building_number].transform.position, Quaternion.identity);

            buildings.transform.DOScale(1, 1).SetEase(Ease.InOutBounce);


        }

       
    }
}