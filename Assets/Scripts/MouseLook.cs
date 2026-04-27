using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float sens = 100f;
    public Transform playerBody;

    public bool isCurserLocked = true;
    float xRotation = 0f;

    public StateManager stateManager;

    public bool isInMenu = false;

    // Start is called before the first frame update
    void Start()
    {
        SetState();
    }

    // Update is called once per frame
    void Update()
    {
        if (stateManager != null && stateManager.currentState == "Fight" && stateManager.isPaused == false)
        {
            float mouseX = Input.GetAxis("Mouse X") * sens * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * sens * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerBody.Rotate(Vector3.up * mouseX);
        }
    }

    void SetState()
    {
        if (isCurserLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        } else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void MenuLock(bool asdf)
    {
        if (asdf)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        } else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
