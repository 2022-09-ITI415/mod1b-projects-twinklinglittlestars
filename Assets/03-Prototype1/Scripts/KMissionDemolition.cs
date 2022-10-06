using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum GameMode
    {
        idle,
        playing,
        levelEnd
    }

    public class KMissionDemolition : MonoBehaviour
    {
        static private KMissionDemolition S; // a private Singleton

        [Header("Set in Inspector")]
        public Text uitLevel; // The UIText_Level Text
        public Text uitShots; //The UIText_Shots Text
        public Text uitButton; // The Text on UIButton_View
        public Vector3 castlePos; // the place to put castles
        public GameObject[] castles; // an array of the castles

        [Header("Set Dynamically")]
        public int level; // the current level
        public int levelMax; // the number of levels
        public int shotsTaken;
        public GameObject castle; // the current castle
        public GameMode mode = GameMode.idle;
        public string showing = "Show Slingshot"; // followcam mode

        // Start is called before the first frame update
        void Start()
        {
            S = this; // define the singleton

            level = 0;
            levelMax = castles.Length;
            StartLevel();
        }

        void StartLevel()
        {
            // get rid of the old castle if one exists
            if (castle != null)
            {
                Destroy(castle);
            }

            // destroy old projectiles if they exist
            GameObject[] gos = GameObject.FindGameObjectsWithTag("KirbyEdited"); // changed projectile to kirbyedited
            foreach (GameObject pTemp in gos)
            {
                Destroy(pTemp);
            }

            //instantiate the new castle
            castle = Instantiate<GameObject>(castles[level]);
            castle.transform.position = castlePos;
            shotsTaken = 0;

            //reset the camera
            SwitchView("Show Both");
            KProjectileLine.S.Clear();

            //reset the goal
            Goal.goalMet = false;

            UpdateGUI();

            mode = GameMode.playing;
        }



        // Update is called once per frame
        void UpdateGUI()
        {
            //Show the data in the GUITexts
            uitLevel.text = "Level: " + (level + 1) + " of " + levelMax;
            uitShots.text = "Shots Taken: " + shotsTaken;

        }

        void Update()
        {
            UpdateGUI();

            //check for level end
            if ((mode == GameMode.playing) && Goal.goalMet)
            {
                // change mode to stop checking for level end
                mode = GameMode.levelEnd;
                // zoom out
                SwitchView("Show Both");
                // start the next level in 2 seconds
                Invoke("NextLevel", 2f);
            }
        }

        void NextLevel()
        {
            level++;
            if (level == levelMax)
            {
                level = 0;
            }
            StartLevel();
        }

        public void SwitchView(string eView = "")
        {
            if (eView == "")
            {
                eView = uitButton.text;
            }
            showing = eView;
            switch (showing)
            {
                case "Show Slingshot":
                    FollowCam.POI = null;
                    uitButton.text = "Show Castle";
                    break;

                case "Show Castle":
                    FollowCam.POI = S.castle;
                    uitButton.text = "Show Both";
                    break;

                case "Show Both":
                    FollowCam.POI = GameObject.Find("ViewBoth");
                    uitButton.text = "Show Slingshot";
                    break;
            }
        }

        // static method that allows code anywhere to increment shotsTaken
        public static void ShotsFired()
        {
            S.shotsTaken++;
        }
    } 

