using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace Script
{
    [System.Serializable]
    public struct Positions
    {
        public List<Transform> differentPositions;
    }


    public class GeneralManager : MonoBehaviour
    {
        public static GeneralManager instance;
        [Header("Cities Scriptable Objects")] public DifferentCities[] Cities;

        [Space] [Header("Building Positions for Spawn")]
        public List<Positions> buildingpos = new();

        [Space] private int CityNumber;
        [Header("Camera for Move")] public Camera cam;

        [Space] [Header("Camera delay for  Move")]
        public int Cam_Movement_Delay;

        [Space] [Header("Camera Positions for Gameplay and Make Buildings")]
        public List<Transform> CamPositions;

        [Space] [Header("instructions Panel")] public GameObject Instruction_Panel;

        [Space] [Header("Building Names")] public TMP_Text Buiding_Names;

        [Space] [Header("Building Description")]
        public TMP_Text Buiding_Description;

        [Space] private int current_BuilingMake = 0;

        [Space] public List<GameObject>
            Main_btns; // gameplay buttons and make buildings buttons ....[0] make building button [1] play game button

        [Space] public GameObject WarningMessage;
        [Space] public TMP_Text starscount;

        [Space] public TMP_Text starsRequired_forBuilding;
        [Space] public GameObject FirstTimeNarration;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            Stars();
            MoveBuildingScreen();
        }

        public void MoveBuildingScreen()
        {
            cam.transform.DOMove(CamPositions[1].transform.position, 5).OnComplete(FirstNarration);
            ;
        }

        public void Stars()
        {
            starscount.text = "Stars :" + PlayerPrefs.GetInt("Stars");
        }

        public void FirstNarration()
        {
            FirstTimeNarration.SetActive(true);
            StartCoroutine(nameof(movegamefirstnarration));
        }

        IEnumerator movegamefirstnarration()
        {
            yield return new WaitForSeconds(3);
            FirstTimeNarration.SetActive(false);
            moveGamePLay();
        }

        public void moveGamePLay()
        {
            cam.transform.DOMove(CamPositions[0].transform.position, 5);
            //Move_Gameplay_MakeBuilding(0);
            cam.transform.DORotate(new Vector3(60,-10,0), 1).SetDelay(.5f);
            Instruction_Panel.SetActive(false);
            WarningMessage.SetActive(false);
        }

        public void moveMakeBuildings()
        {
            starsRequired_forBuilding.text = "Stars Required :" +
                                             Cities[0].Building[PlayerPrefs.GetInt("TotalBuildingDone")].Stars_Required;
            cam.transform.DOMove(CamPositions[1].transform.position, 7).OnComplete(MoveCam_OnEachBuilding);
            // Move_Gameplay_MakeBuilding(1);
        }

        public void MoveCam_OnEachBuilding()
        {
            int buildingclose_pos = PlayerPrefs.GetInt("TotalBuildingDone");
            cam.transform.DOMove(buildingpos[0].differentPositions[buildingclose_pos].GetChild(0).transform.position, 5).OnComplete(Appear_UI_Instructions);
            cam.transform.DORotate(new Vector3(40,0,0), 1).SetDelay(.5f);
           
        }

        private void Appear_UI_Instructions()
        {
            for (int i = 0; i < 4; i++)
            {
                /*if (Cities[0].Building[i].isused == 0)
                {
                    current_BuilingMake = i;
                    break;
                }*/
                if (i == PlayerPrefs.GetInt("TotalBuildingDone"))
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
            if (PlayerPrefs.GetInt("Stars") < Cities[0].Building[current_BuilingMake].Stars_Required)
            {
                WarningMessage.SetActive(true);
                StartCoroutine(nameof(MoveGAmePlay));
            }
            else
            {
                BuildingPositions(current_BuilingMake);
                PlayerPrefs.SetInt("Stars",
                    PlayerPrefs.GetInt("Stars") - Cities[0].Building[current_BuilingMake].Stars_Required);
                Stars();
                Instruction_Panel.SetActive(false);
            }
        }

        IEnumerator MoveGAmePlay()
        {
            yield return new WaitForSeconds(2);
            moveGamePLay();
        }

        private void Move_Gameplay_MakeBuilding(int game_moves)
        {
            if (game_moves == 0)
            {
                Main_btns[0].SetActive(true);
                Main_btns[1].SetActive(false);
            }
            else
            {
                Main_btns[0].SetActive(false);
                Main_btns[1].SetActive(true);
            }
        }

        private void BuildingPositions(int building_number)
        {
            GameObject buildings = Instantiate(Cities[0].Building[building_number].buildings[0],
                buildingpos[0].differentPositions[building_number].transform.position, Quaternion.identity);

            buildings.transform.DOScale(2, 1.5f).SetEase(Ease.InOutBounce);
            PlayerPrefs.SetInt("TotalBuildingDone", PlayerPrefs.GetInt("TotalBuildingDone") + 1);
        }
    }
}