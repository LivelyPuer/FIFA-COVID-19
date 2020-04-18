using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class launcher : MonoBehaviourPunCallbacks
{
    [SerializeField] string gameVersion = "1";
    [SerializeField] byte maxPlayer = 8;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;  
    }

    private void Start()
    {
        Connect();
    }

    private void Connect()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = gameVersion;
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }
    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(null, new RoomOptions {MaxPlayers = maxPlayer});
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(1);
    }
}
