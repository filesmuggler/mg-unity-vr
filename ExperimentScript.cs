﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class ExperimentScript : MonoBehaviour
{
    // levels of crystals to count by the subject
    public GameObject crystalLevels;
    // what is current crystal level
    public int levelCounter;

    // to calculate time of one probe
    float startTime, currentTime;
    String currentDateTime;

    // file storing data from whole research session
    String fileName;
    StreamWriter fileHandler;

    // keys used for navigation
    KeyCode nextLevel;
    bool nextLevelPressed;
    KeyCode newSubject;
    bool newSubjectPressed;
    KeyCode subjectSexM;
    bool subjectSexMPressed;
    KeyCode subjectSexF;
    bool subjectSexFPressed;
    KeyCode subjectSexD;
    bool subjectSexDPressed;
    KeyCode startSubject;
    bool startSubjectPressed;
    KeyCode endSubject;
    bool endSubjectPressed;
    KeyCode restartProbe;
    bool restartProbePressed;

    KeyCode quitApp;
    bool quitAppPressed;

    bool choosingSex;
    int noSubject;
    String subjectData;
    bool if_active;

    public GameObject womanFPV;
    public GameObject manFPV;

    // Start is called before the first frame update
    void Start()
    {
        nextLevel = KeyCode.Space;
        newSubject = KeyCode.N;
        subjectSexM = KeyCode.M;
        subjectSexF = KeyCode.F;
        subjectSexD = KeyCode.D;
        startSubject = KeyCode.S;
        endSubject = KeyCode.E;
        restartProbe = KeyCode.R;
        quitApp = KeyCode.Q;
        noSubject = 1;
        if_active = false;
        womanFPV.SetActive(false);
        manFPV.SetActive(false);

        // turn off all levels of crystals
        for (int i = 0; i < crystalLevels.transform.childCount - 1; i++)
        {
            crystalLevels.transform.GetChild(i).transform.gameObject.SetActive(false);
        }
        print("Experiment started.\n");

        levelCounter = 0;

        startTime = Time.time;

        // create the name of the file used further in program
        currentDateTime = DateTime.UtcNow.ToString("yyyyMMddTHH-mm-ss");
        fileName = "Research_results-" + currentDateTime + ".csv";
        print("Filename is " + fileName + "\n");


        fileHandler = new StreamWriter(fileName, true);
        String header = "no,sex,test1,test2,test3,sample1,sample2,sample3,sample4,sample5,sample6,sample7,sample8,sample9,sample10,\n";
        fileHandler.Write(header);
        fileHandler.Close();
    }

    // Update is called once per frame
    void Update()
    {

        newSubjectPressed = Input.GetKeyDown(newSubject);
        startSubjectPressed = Input.GetKeyDown(startSubject);
        endSubjectPressed = Input.GetKeyDown(endSubject);
        nextLevelPressed = Input.GetKeyDown(nextLevel);
        restartProbePressed = Input.GetKeyDown(restartProbe);
        quitAppPressed = Input.GetKeyDown(quitApp);

        // TO-DO: fill in opening new file, adding number 
        if (newSubjectPressed)
        {

            // add new number of subject
            subjectData = "";
            subjectData = subjectData + noSubject + ",";
            noSubject++;
            Debug.Log(subjectData);
            // enable choosing sex
            choosingSex = true;
        }

        subjectSexMPressed = Input.GetKeyDown(subjectSexM);
        subjectSexFPressed = Input.GetKeyDown(subjectSexF);
        subjectSexDPressed = Input.GetKeyDown(subjectSexD);
        if (subjectSexMPressed && choosingSex)
        {
            //assign proper letter to string
            //disable choosing sex
            subjectData = subjectData + "M,";
            manFPV.SetActive(true);
            womanFPV.SetActive(false);
            choosingSex = false;
        }
        else if (subjectSexFPressed && choosingSex)
        {
            //assign proper letter to string
            //disable choosing sex
            subjectData = subjectData + "F,";
            manFPV.SetActive(false);
            womanFPV.SetActive(true);
            choosingSex = false;
        }
        else if (subjectSexDPressed && choosingSex)
        {
            //assign proper letter to string
            //disable choosing sex
            subjectData = subjectData + "D,";
            manFPV.SetActive(false);
            womanFPV.SetActive(false);
            choosingSex = false;
        }



        /*
        // TO-DO: implement ignoring space butont
        if (levelCounter > 13)
        {
            // ignore space clicking
        }
        */

        if (startSubjectPressed)
        {
            startTime = Time.time;
            //wczytaj pierwszy level
            for (int i = 0; i < crystalLevels.transform.childCount; i++)
            {
                if (i == levelCounter)
                {
                    crystalLevels.transform.GetChild(i).transform.gameObject.SetActive(true);
                }
                else
                {
                    crystalLevels.transform.GetChild(i).transform.gameObject.SetActive(false);
                }
            }
            //incrementuj counter 
            Debug.Log("poziom nr " + levelCounter);
            levelCounter++;

            if_active = true;

        }

        if (nextLevelPressed && if_active)
        {
            // getting time from previous level
            currentTime = Time.time - startTime;
            subjectData = subjectData + currentTime + ",";

            startTime = Time.time;

            for (int i = 0; i < crystalLevels.transform.childCount; i++)
            {
                if (i == levelCounter)
                {
                    crystalLevels.transform.GetChild(i).transform.gameObject.SetActive(true);
                }
                else
                {
                    crystalLevels.transform.GetChild(i).transform.gameObject.SetActive(false);
                }
            }
            Debug.Log("poziom nr " + levelCounter);
            if (levelCounter < 13)
            {
                levelCounter++;
            }
            else
            {
                levelCounter = 0;
                //ignore further signals
                if_active = false;
            }

        }

        if (endSubjectPressed)
        {
            //terminate string

            //append to file
            if (!File.Exists(fileName))
            {
                print("file does not exist");
            }
            else
            {
                fileHandler = File.AppendText(fileName);
                fileHandler.WriteLine(subjectData);
                fileHandler.Close();
                subjectData = "";
                levelCounter = 0;
            }
        }

        if (quitAppPressed)
        {
            //append to file
            if (!File.Exists(fileName))
            {
                print("file does not exist");
            }
            else
            {
                fileHandler = File.AppendText(fileName);
                fileHandler.WriteLine(subjectData);
                fileHandler.Close();
                levelCounter = 0;
                Debug.Log("Experiment terminated.");
            }
        }
    }
}

