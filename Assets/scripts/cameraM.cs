using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraM : MonoBehaviour
{
    private const float Y_ANGLE_MIN = 10f;
    private const float Y_ANGLE_MAX = 80f;

    public Transform target;
    public float distance = 5f;
    private float curY = 0f;
    private float curX = 0f;
    public float sensY = 4f;
    public float sensX = 4f;

    public int distanceMin = 4;
    public int distanceMax = 10;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        curX += Input.GetAxis("Mouse X") * sensX;
        curY += Input.GetAxis("Mouse Y") * sensY;
        curY = Mathf.Clamp(curY, Y_ANGLE_MIN, Y_ANGLE_MAX);
    }
    private void LateUpdate()
    {
        Vector3 dir = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(curY, curX, 0);

        transform.position = target.position + rotation * dir;
        transform.LookAt(target.transform);

        if (Input.GetAxis("Mouse ScrollWheel") != 0)
            distance -= Mathf.Sign(Input.GetAxis("Mouse ScrollWheel"));
        distance = Mathf.Clamp(distance, distanceMin, distanceMax);
    }
}
