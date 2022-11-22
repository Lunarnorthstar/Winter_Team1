using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class SnowmanBuild : MonoBehaviour
{
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
        if (Input.GetMouseButtonDown(0)) //Mouse 0 is left click
        {
            PlacePart();
        }
        if (Input.GetMouseButtonDown(1)) //Mouse 1 is right click
        {
            RemovePart();
        }

        if (previewFab != null)
        {
            previewFab.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
                Camera.main.ScreenToWorldPoint(Input.mousePosition).y, offset);
            
            previewFab.transform.rotation = new Quaternion(0, 0, storedRotation, 0);
        }

        
        

        displayText.text = "Parts: " + currentParts + " / " + maxParts;
    }

    public void PlacePart()
    {
        GameObject[] bodySegments = GameObject.FindGameObjectsWithTag("Body"); //Get all the placed body segments
        for (int i = 0; i < bodySegments.Length; i++) //For each one...
        {
            Vector3 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); //Get the mouse position
            clickPos.z = offset;
            if (math.distance(clickPos.x, bodySegments[i].transform.position.x) < bodySegments[i].transform.localScale.x /2 &&
                math.distance(clickPos.y, bodySegments[i].transform.position.y) < bodySegments[i].transform.localScale.y /2 && currentParts < maxParts) //if clicking within the bounds of a body segment...
            {
                Instantiate(partPrefabs[selectedPart], clickPos, Quaternion.identity); //Instantiate it
                currentParts++; //Add one to the current parts
                break; //Stop checking
            }
        }
    }

    public void RemovePart()
    {
        GameObject[] placedParts = GameObject.FindGameObjectsWithTag("Part"); //Get all the placed part segments
        for (int i = 0; i < placedParts.Length; i++) //For each one...
        {
            Vector3 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); //Get the mouse position
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

    public void RotatePart(int dir)
    {
        storedRotation += rotationDegrees * dir;
    }

    public void ButtonInput(int i)
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        selectedPart = i;
        Destroy(previewFab);
        previewFab = Instantiate(partPrefabs[i], mousePos, new Quaternion(0, 0, storedRotation, 0));
        previewFab.tag = "Preview";
    }
}
