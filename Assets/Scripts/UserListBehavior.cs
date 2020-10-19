using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserListBehavior : MonoBehaviour
{
    public List<PlayerProfileBehavior> userList = new List<PlayerProfileBehavior>();
    //public Dictionary<string, PlayerProfileBehavior> users = new Dictionary<string, PlayerProfileBehavior>();

    int idCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AssignUserName(string data)
    {
        gameObject.GetComponentInParent<PlayerProfileBehavior>().pUserName = data;
        Debug.Log(gameObject.GetComponentInParent<PlayerProfileBehavior>().pUserName);
    }

    public void AssignPassWord(string data)
    {
        gameObject.GetComponentInParent<PlayerProfileBehavior>().pPassword = data;
        Debug.Log(gameObject.GetComponentInParent<PlayerProfileBehavior>().pPassword);

    }

    public void AssignPlayerData()
    {
        var GameUI = gameObject.GetComponent<PlayerProfileBehavior>();

        gameObject.GetComponentInParent<UserListBehavior>().userList.Add(new PlayerProfileBehavior( idCounter, GameUI.pUserName, GameUI.pPassword, GameUI.plevel, GameUI.pExp));
        idCounter++;
    }

}
