using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MoveBlock : MonoBehaviourPunCallbacks
{
    private GameManager gameManager;
    Vector2 startPosition;
    public float speed;
    public float amplitude;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        startPosition = this.transform.position;
    }

    void Update()
    {  
        float newY = startPosition.y + Mathf.Sin(Time.time * speed) * amplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    void OnMouseDown()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            gameManager.DestroyBox(gameObject);
        }
        else
        {
            gameManager.UpdateScore(1); // Update score before requesting destruction
            photonView.RPC("RequestDestroyBox", RpcTarget.MasterClient);  
        }
    }

    [PunRPC]
    private void RequestDestroyBox()
    {
        // Only the Master Client can handle the destruction of GameObjects
        if (PhotonNetwork.IsMasterClient)
        {
            gameManager.DestroyBoxByMaster(gameObject);
        }
    }
}
