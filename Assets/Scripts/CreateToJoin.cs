using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class CreateToJoin : MonoBehaviourPunCallbacks
{
    public TMP_InputField input_Create;
    public TMP_InputField input_Join;
    public TMP_InputField usernameInput;

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(input_Create.text);
        SetUsername();
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(input_Join.text);
        SetUsername();
    }

    public void SetUsername()
    {
        PhotonNetwork.NickName = usernameInput.text;
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("GamePlay");
    }
}