using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Unity.VisualScripting;

public class GameManager : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI winnerText;

    private int score = 0;
    private string winningPlayerUsername;

    public GameObject boxPrefab;

    int boxCount;
    private const int maxBoxes = 10;
    public int width;
    public int height;

    void Start()
    {
        boxCount=0;
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(SpawnBoxRoutine());
        }
    }

    IEnumerator SpawnBoxRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(.25f);

            if (boxCount < maxBoxes)
            {
                PhotonNetwork.Instantiate(boxPrefab.name, new Vector3(Random.Range(-width,width),Random.Range(-height,height),0) , Quaternion.identity);
                boxCount++;
            }
        }
    }

    public void DestroyBox(GameObject box)
    {
        
        PhotonNetwork.Destroy(box);
        boxCount--;
        UpdateScore(1);
        
    }

    public void DestroyBoxByMaster(GameObject box)
    {
        
        PhotonNetwork.Destroy(box);
        boxCount--;
        
    }

    public void UpdateScore(int increment)
    {
        score += increment;
        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }
        else
        {
            Debug.LogError("scoreText is null!");
        }   

        if (score >= 8 && winningPlayerUsername == null)
        {
            // Set the winning player's username
            winningPlayerUsername = PhotonNetwork.NickName;

            this.photonView.RPC("RPC_DisplayWinner", RpcTarget.AllViaServer, winningPlayerUsername);
            Debug.Log(winningPlayerUsername);
        }
    }

    [PunRPC]
    public void RPC_DisplayWinner(string winnerUsername)
    {
        winnerText.text = "Winner: " + winnerUsername;
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log("A new player has entered the room");
    }
}