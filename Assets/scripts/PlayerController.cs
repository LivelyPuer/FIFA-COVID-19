using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] float maxHp;
    public GameObject camera;
    bool inChat = false;
    public PlayerController currentPlayer;
    float directionDampTime = .25f;
    [SerializeField] float maxHealth;
    float health = 100;
    Image hpBar;
    void Start()
    {
        if (photonView.IsMine)
        {
            Instantiate(camera);
            GameManager.Instance().currentPlayer = this;
            this.hpBar = GameManager.Instance().hpBar;
            camera.GetComponentInChildren<Camera>().enabled = true;
            camera.GetComponentInChildren<AudioListener>().enabled = true;

            // get the third person character ( this should never be null due to require component )
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine && PhotonNetwork.IsConnected)
        {
            return;
        }

            //inChat = !inChat;
            //Cursor.visible = inChat;
            //Cursor.lockState = inChat ? CursorLockMode.None : CursorLockMode.Locked;
            //GameManager.Instance().chatField.Select();
    }

    //public void Damage(int dmg)
    //{
    //    health -= dmg;
    //    hpBar.fillAmount = maxHealth / health;
    //    print(health);
    //}

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(health);
        }
        else
        {
            this.health = (float)stream.ReceiveNext();
        }
    }
}
