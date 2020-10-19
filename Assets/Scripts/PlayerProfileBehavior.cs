using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerProfileBehavior : MonoBehaviour
{
    public int userID = 0;
    public string pUserName = "";
    public string pPassword = "";
    public string pGameName = "Shank";
    public int plevel = 9;
    public int pExp = 78;
    public int pWins = 12;
    public int pLoses = 5;

    // Start is called before the first frame update
    void Start()
    {
        //var input = gameObject.GetComponent<InputField>();
        //var submit = new InputField.SubmitEvent();
        //submit.AddListener(AssignUserName);
        //input.onEndEdit = submit;
        //input.onEndEdit.AddListener(AssignUserName);
    }

    // Update is called once per frame
    void Update()
    {
        //AssignUserProfile();
    }

    public PlayerProfileBehavior(int id, string username, string password, int level, int exp)
    {
        userID = id;
        pUserName = username;
        pPassword = password;
        plevel = level;
        pExp = exp;

    }

}
