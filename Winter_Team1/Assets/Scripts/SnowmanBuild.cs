using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SnowmanBuild : MonoBehaviour
{
    private Controls inputActions;
    
    public GameObject cursor;
    public GameObject[] partPrefabs;
    private GameObject previewFab;
    public Text displayText;
    [Space]
    public int maxParts = 6;
    [SerializeField] private int currentParts = 0;
    private int selectedPart = -1;
    [Space] [Space] [Tooltip("The Z position of the placed part")] public float offset = -0.2f;
    [Space] [Tooltip("In Degrees per second")] public float rotationDegrees = 45;

    private int rotateDir = 0;
    
    private float storedRotation = 0;
    private bool flip = false;

    // Update is called once per frame

    void Update()
    {
        storedRotation += (rotationDegrees * rotateDir) * Time.deltaTime; //I hate new input system. The fact that I need to jury rig a continuous action is stupid.
        
        
        if (previewFab != null)
        {
            previewFab.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(cursor.transform.position).x,
                Camera.main.ScreenToWorldPoint(cursor.transform.position).y, offset);
            
            previewFab.transform.rotation =  Quaternion.Euler(0, 0, storedRotation);
            previewFab.GetComponent<SpriteRenderer>().flipX = flip;
        }

        displayText.text = "Parts: " + currentParts + " / " + maxParts;
    }

    public void PlacePart(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GameObject[] bodySegments = GameObject.FindGameObjectsWithTag("Body"); //Get all the placed body segments
            for (int i = 0; i < bodySegments.Length; i++) //For each one...
            {
                Vector3 clickPos = Camera.main.ScreenToWorldPoint(cursor.transform.position); //Get the mouse position
                clickPos.z = offset;
                if (previewFab.GetComponent<ObjectPlaceCheck>().valid && currentParts < maxParts) //if clicking within the bounds of a body segment...
                {
                    GameObject newPart = Instantiate(partPrefabs[selectedPart], gameObject.transform); //Instantiate it
                    newPart.transform.position = clickPos; //Move it
                    newPart.transform.Rotate(0, 0, storedRotation); //Rotate it
                    newPart.GetComponent<SpriteRenderer>().flipX = flip; //Flip it
                    //Bop it. Uh, I mean...
                    currentParts++; //Add one to the current parts
                    break; //Stop checking
                }
            }
        }


    }

    public void RemovePart(InputAction.CallbackContext context)
    {

        if (context.performed)
        {
           
            GameObject[] placedParts = GameObject.FindGameObjectsWithTag("Part"); //Get all the placed part segments
            
            for (int i = 0; i < placedParts.Length; i++) //For each one...
            {
                Vector3 clickPos = Camera.main.ScreenToWorldPoint(cursor.transform.position); //Get the mouse position
                clickPos.z = offset;
                if (math.distance(clickPos.x, placedParts[i].transform.position.x) < placedParts[i].transform.localScale.x /2 &&
                    math.distance(clickPos.y, placedParts[i].transform.position.y) < placedParts[i].transform.localScale.y /2) //if clicking within the bounds of a body segment...
                {
                    Destroy(placedParts[i]); //Remove it
                    currentParts--; //Subtract one from the current parts
                    break; //Stop checking
                }
            }
        }
        
    }
    public void RotatePartClockwise(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            rotateDir = 1;
        }

        if (context.canceled)
        {
            rotateDir = 0;
        }
        
    }

    public void RotatePartCounterclockwise(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            rotateDir = -1;
        }
        
        if (context.canceled)
        {
            rotateDir = 0;
        }
    }

    public void flipPart(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            flip = !flip;
        }
    }

    public void ButtonInput(int i)
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        selectedPart = i;
        Destroy(previewFab);
        previewFab = Instantiate(partPrefabs[i], mousePos, new Quaternion(0, 0, storedRotation, 0));
        previewFab.tag = "Preview";
        previewFab.AddComponent<ObjectPlaceCheck>();
    }
}
