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
    [Space] public float rotationDegrees = 45;
    private float storedRotation = 0;

    // Update is called once per frame

    void Update()
    {

        if (previewFab != null)
        {
            previewFab.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(cursor.transform.position).x,
                Camera.main.ScreenToWorldPoint(cursor.transform.position).y, offset);
            
            previewFab.transform.rotation =  Quaternion.Euler(0, 0, storedRotation);
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
                    Instantiate(partPrefabs[selectedPart], clickPos, Quaternion.Euler(0, 0, storedRotation)); //Instantiate it
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
            storedRotation += rotationDegrees;
        }
        
    }

    public void RotatePartCounterclockwise(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            
            storedRotation -= rotationDegrees;

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
