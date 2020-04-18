using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviourPunCallbacks, IPunObservable
{
    Animator animator;
    public UnityStandardAssets.Characters.ThirdPerson.ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
    private Transform m_Cam;                  // A reference to the main camera in the scenes transform
    private Vector3 m_CamForward;             // The current forward direction of the camera
    private Vector3 m_Move;
    private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.
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
        animator = GetComponent<Animator>();
        if (photonView.IsMine)
        {
            camera.GetComponent<Camera>().enabled = true;
        GameManager.Instance().currentPlayer = this;
            this.hpBar = GameManager.Instance().hpBar;
            if (Camera.main != null)
            {
                m_Cam = Camera.main.transform;
            }
            else
            {
                Debug.LogWarning(
                    "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
                // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
            }

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

        if (!m_Jump)
        {
            m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
        }
        {
            //inChat = !inChat;
            //Cursor.visible = inChat;
            //Cursor.lockState = inChat ? CursorLockMode.None : CursorLockMode.Locked;
            //GameManager.Instance().chatField.Select();
        }
    }
    private void FixedUpdate()
    {
        // read inputs
        float h = CrossPlatformInputManager.GetAxis("Horizontal");
        float v = CrossPlatformInputManager.GetAxis("Vertical");
        bool crouch = Input.GetKey(KeyCode.C);

        // calculate move direction to pass to character
        if (m_Cam != null)
        {
            // calculate camera relative direction to move:
            m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
            m_Move = v * m_CamForward + h * m_Cam.right;
        }
        else
        {
            // we use world-relative directions in the case of no main camera
            m_Move = v * Vector3.forward + h * Vector3.right;
        }
#if !MOBILE_INPUT
        // walk speed multiplier
        if (Input.GetKey(KeyCode.LeftShift)) m_Move *= 0.5f;
#endif

        // pass all parameters to the character control script
        m_Character.Move(m_Move, crouch, m_Jump);
        m_Jump = false;
    }
    public void Damage(int dmg)
    {
        health -= dmg;
        hpBar.fillAmount = maxHealth / health;

        if (health <= 0)
            Destroy(gameObject);
        print(health);
    }

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
