using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class SnowmanFinish : MonoBehaviour
{
    public GameObject PM;

    public GameObject cursor;
    public GameObject UI;
    public GameObject viewCamera;
    public Vector3 cameraPos;
    public float cameraSize;

    public float speed = 300;
    

    public void ButtonInput()
    {
        Destroy(PM.GetComponent<SnowmanBuild>().previewFab);
        Destroy(PM.GetComponent<SnowmanBuild>());
        Destroy(PM.GetComponent<GamepadCursor>());
        
        Destroy(cursor);
        UI.SetActive(false);
        viewCamera.transform.position = cameraPos;
        viewCamera.GetComponent<Camera>().orthographicSize = cameraSize;

        PM.GetComponent<PlayerMovement>().speed = speed;
        PM.GetComponent<Rigidbody2D>().gravityScale = 1;
    }
}
