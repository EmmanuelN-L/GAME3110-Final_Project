using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ButtonBehavior : MonoBehaviour
{
    public GameObject userInput;
    public string validUsername = " ";
    public string validPassword = " ";
    bool loginSuccess = false;
    bool registerSuccess = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RegistrationBehavior()
    {
        // Grab User name and password to Process a new user account
        if (userInput.GetComponent<UserListBehavior>().userList.Count != 0)
        {
            foreach (PlayerProfileBehavior Account in userInput.GetComponent<UserListBehavior>().userList)
            {
                if (Account.pUserName == userInput.GetComponent<PlayerProfileBehavior>().pUserName)
                {
                    registerSuccess = false;
                    break;
                }
                else
                    registerSuccess = true;
            }
        }
        else
            registerSuccess = true;

        if (registerSuccess == true)
        {
            userInput.GetComponent<UserListBehavior>().AssignPlayerData();


            Debug.Log("Success!, Your Account Has Been Made!");
            Debug.Log("All Acounts Currently");
            foreach(PlayerProfileBehavior Account in userInput.GetComponent<UserListBehavior>().userList)
            {
                Debug.Log("UserID: " + Account.userID + " UserName: " + Account.pUserName + "Password: " + Account.pPassword);
            }
            //Debug.Log(userInput.GetComponent<UserListBehavior>().userList);
        }
        else
        {
            Debug.Log("Error!, That Username is already taken!");
        }
       

    }

    public void LoginBehavior()
    {
        // Grab User name and Password to validate Login Procedure
        //if (userInput.GetComponent<UserListBehavior>().userList.Count != 0)
        //{
        //    foreach (PlayerProfileBehavior Account in userInput.GetComponent<UserListBehavior>().userList)
        //    {
        //        if (Account.pUserName == userInput.GetComponent<PlayerProfileBehavior>().pUserName && 
        //            Account.pPassword == userInput.GetComponent<PlayerProfileBehavior>().pPassword)
        //        {
        //            loginSuccess = true;
        //            break;
        //        }
        //        else
        //            loginSuccess = false;
        //    }
        //}
        //else
        //    loginSuccess = true;

        foreach (PlayerProfileBehavior Account in userInput.GetComponent<UserListBehavior>().userList)
        {
            if (Account.pUserName == userInput.GetComponent<PlayerProfileBehavior>().pUserName &&
                Account.pPassword == userInput.GetComponent<PlayerProfileBehavior>().pPassword)
            {
                loginSuccess = true;
                break;
            }
            else
                loginSuccess = false;
        }

        if (loginSuccess == true)
        {
            Debug.Log("Login Successful!");

            SceneManager.LoadScene("GameScene");

        }
        else
        {
            Debug.Log("Error!, Login Unsuccessful!");
        }
    }

    public void ExitGameApplication()
    {
        // Quits The Game Application
        Application.Quit();
    }

}
