using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class boomb : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerController>().Damage(10);
        }
    }
}
