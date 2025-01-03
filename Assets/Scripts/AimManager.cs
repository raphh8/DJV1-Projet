using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimManager : MonoBehaviour
{
    private float x, y;
    [SerializeField] Transform camFollowPos;
    [SerializeField] float mouseSense = 1;

    void Update()
    {
        x += Input.GetAxisRaw("Mouse X") * mouseSense;
        y -= Input.GetAxisRaw("Mouse Y") * mouseSense;
        y = Mathf.Clamp(y, -80, 80);
    }

    private void LateUpdate()
    {
        camFollowPos.localEulerAngles = new Vector3(y, camFollowPos.localEulerAngles.y, camFollowPos.localEulerAngles.z);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, x, transform.eulerAngles.z);
    }
}
