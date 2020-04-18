using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviourPunCallbacks
{
    #region Singleton
    private static GameManager _instance;
    public static GameManager Instance() { return _instance; }
    private void Awake()
    {
        if (!_instance)
        {
            _instance = this;
            DontDestroyOnLoad(_instance);
        }
    }
    #endregion Singleton

    [SerializeField] GameObject playerPrefab;
    public PlayerController currentPlayer;

    [Header("UI")]
    [SerializeField] Text logText;
    public Image hpBar;
    public InputField chatField;

    static List<string> chat = new List<string>();

    void Start()
    {
        PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(5, 5, 5), Quaternion.identity, 0); 
    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        print(newPlayer.NickName + " join room");
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        print(otherPlayer.NickName + " left room");
    }
    public void SendChatMessage(string val)
    {
        //Log(chatField.text);
        //chatField.text = "";
    }
    public static void Log(string message)
    {
        chat.Add(message);
        if  (chat.Count > 5)
        {
            chat.RemoveAt(0);
        }
        GameManager.Instance().logText.text = string.Join("\n", chat);
    }
}