using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SnowmanBuild : MonoBehaviour
{
    private Controls inputActions;
    
    public GameObject cursor; //The cursor.
    public GameObject[] partPrefabs; //An array of all the prefabs of parts
    public GameObject previewFab; //The gameobject used to display the preview of the part you're placing
    [Space]
    public GameObject displayWeight; //The text that displays the part count
    public GameObject displayHealth;
    public GameObject displaySpeed;
    public GameObject displayAttack;
    [Space]
    public float maxParts = 6; //The maximum number of parts that can be placed
    [SerializeField] private float currentParts = 0; //The number of currently placed parts
    private int selectedPart = -1; //Which part you are ready to place
    [Space] [Space] [Tooltip("What will be the Z position of the placed part")] public float offset = -0.2f;
    [Space] [Tooltip("Rotation speed in Degrees per second")] public float rotationDegrees = 45;

    private int rotateDir = 0; //The direction to rotate. 0 means no rotation
    
    private float storedRotation = 0; //The total rotation of the object
    private bool flip = false; //Whether or not the object should be flipped on the X axis

    void Update()
    {
        storedRotation += (rotationDegrees * rotateDir) * Time.deltaTime; //I hate new input system. The fact that I need to jury rig a continuous action is stupid.
        //On topic, this will constantly rotate the part in the direction specified by the variable. More on that later.
        
        
        if (previewFab != null) //If the preview object exists...
        {
            previewFab.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(cursor.transform.position).x, Camera.main.ScreenToWorldPoint(cursor.transform.position).y, offset); 
            //Set the preview object's position to the cursor position. And before you ask, the cursor is a UI element, so childing won't work.
            
            previewFab.transform.rotation =  Quaternion.Euler(0, 0, storedRotation); //Set the preview object's rotation to the rotation of the part when it will be placed.
            previewFab.GetComponent<SpriteRenderer>().flipX = flip; //If it should be flipped, flip it.
        }

        float partRatio =  currentParts/maxParts;


        float totalHealth = 0;
        float totalSpeed = 125;
        float totalAttack = 0;
        GameObject[] allParts = FindGameObjectsWithTags("Part", "Arm", "Glove");
        foreach (var part in allParts)
        {
            totalHealth += part.GetComponent<PartStatHandler>().durability;
            totalSpeed += part.GetComponent<PartStatHandler>().speed;
            totalAttack += part.GetComponent<PartStatHandler>().attack;
        }

        totalSpeed /= 500;
        totalHealth /= 100;
        totalAttack /= 100;
        
        
        displayWeight.GetComponent<RectTransform>().localScale = new Vector3(partRatio, 1, 1);
        displayHealth.GetComponent<RectTransform>().localScale = new Vector3(totalHealth, 1, 1);
        displaySpeed.GetComponent<RectTransform>().localScale = new Vector3(totalSpeed, 1, 1);
        displayAttack.GetComponent<RectTransform>().localScale = new Vector3(totalAttack, 1, 1);
        
    }

    public void PlacePart(InputAction.CallbackContext context)
    {
        //This is a rough one, hang in there.
        if (context.performed) //If the input is "performed" - this means it will only be activated when the button reaches the pressed state and not when first pressed or released, instead of all three.
        {
            GameObject[] bodySegments = GameObject.FindGameObjectsWithTag("Body"); //Get all the placed body segments
            
            Vector3 clickPos = Camera.main.ScreenToWorldPoint(cursor.transform.position); //Get the mouse position
            clickPos.z = offset;
            for (int i = 0; i < bodySegments.Length; i++) //For each one...
            {
                if (previewFab.GetComponent<ObjectPlaceCheck>().valid && currentParts < maxParts) //if clicking within the bounds of a body segment...
                {
                    GameObject newPart = null; //Create the variable for storing the created part
                    if (previewFab.tag == "PreviewGlove") //Gloves have special logic. If it's a glove...
                    {
                        GameObject[] arms = GameObject.FindGameObjectsWithTag("Arm"); //Find all the objects tagged as arm.
                        GameObject parent = arms[0]; //Initialize the variable that stores the arm the glove will be attached to. Start at 1 to avoid nullreference shenanigans.
                        
                        foreach (GameObject part in arms) //I have a newfound love for Foreach statements. They read very well, practically self-explanatory.
                        {
                            if (math.distance(clickPos, part.transform.position) < math.distance(clickPos, parent.transform.position)) //If it's closer to the place object than the previous closest...
                            {
                                parent = part; //Set that one as the "closest"
                            }
                        }
                        newPart = Instantiate(partPrefabs[selectedPart], parent.transform); //Instantiate it.
                    }
                    else //And if you're not placing a glove you can just skip all of that and-
                    {
                        newPart = Instantiate(partPrefabs[selectedPart], gameObject.transform); //Instantiate it.
                    }
                    newPart.transform.position = clickPos; //Move it
                    newPart.transform.Rotate(0, 0, storedRotation); //Rotate it
                    newPart.GetComponent<SpriteRenderer>().flipX = flip; //Flip it
                    //Bop it. Uh, I mean...

                    currentParts++; //Add one to the current parts
                    break; //Stop checking
                }
                //If not clicking inside a body segment, do nothing.
            }
        }


    }

    public void RemovePart(InputAction.CallbackContext context)
    {

        if (context.performed) //Same function as above. One call per button press.
        {

            GameObject[] placedParts = FindGameObjectsWithTags("Part", "Arm", "Glove"); //Get all the placed part segments using this neat custom function I copy-pasted off Stack Overflow.
        
            Vector3 clickPos = Camera.main.ScreenToWorldPoint(cursor.transform.position); //Get the mouse position.
            clickPos.z = offset;
            for (int i = 0; i < placedParts.Length; i++) //For each one...
            {
                if (math.distance(clickPos.x, placedParts[i].transform.position.x) < placedParts[i].transform.localScale.x /2 && math.distance(clickPos.y, placedParts[i].transform.position.y) < placedParts[i].transform.localScale.y /2) 
                    //if clicking within the bounds of a part...
                {
                    Destroy(placedParts[i]); //Remove it
                    currentParts--; //Subtract one from the current parts
                    break; //Stop checking
                }
                //If not clicking on a part, do nothing.
            }
        }
        
    }
    public void RotatePartClockwise(InputAction.CallbackContext context) //Extremely, incredibly stupid that I need to make two functions for this. The default input system has custom Axes.
    {
        if (context.performed) //Same as above functions. One call.
        {
            rotateDir = 1; //Tell the game to start rotating in that direction.
        }

        if (context.canceled) //When it's released...
        {
            rotateDir = 0; //Tell the game to stop rotating.
        }
        //The absurdity here is that this is even necessary. There is no new input system support for a continuous function on holding down a button.
    }

    public void RotatePartCounterclockwise(InputAction.CallbackContext context) //This is the same as above, but in the other direction.
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

    public void flipPart(InputAction.CallbackContext context) //This flips the part.
    {
        if (context.performed) //When the button is pressed...
        {
            flip = !flip; //Flip the part by inverting the bool that tells you whether or not to flip the part.
        }
    }

    public void ButtonInput(int i) //This gets input from the buttons to swap the selected part.
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); //Get the mouse position.
        mousePos.z = 0;
        
        selectedPart = i; //Set the button's input value to the selected prefab. Buttons will pass in a number that is the position in the array of part prefabs to be set to.
        Destroy(previewFab); //Destroy the current preview object.
        previewFab = Instantiate(partPrefabs[i], mousePos, new Quaternion(0, 0, storedRotation, 0)); //Instantiate a new one.
        previewFab.tag = String.Concat("Preview" + previewFab.tag); //Change its tag. This is so you don't delete it.
        previewFab.AddComponent<ObjectPlaceCheck>(); //Give it a special component.
    }


    public void CleanUp()
    {
        GameObject[] parts = FindGameObjectsWithTags("Glove", "Arm", "Part");

        foreach (GameObject part in parts)
        {
            Destroy(part.GetComponent<Rigidbody2D>());
        }
    }
    
    
    
    GameObject[] FindGameObjectsWithTags(params string[] tags) //This is a custom function.
    {
        var all = new List<GameObject>(); //Make an empty list of gameobjects
         
        foreach(string tag in tags) //For each tag input into the function...
        {
            var temp = GameObject.FindGameObjectsWithTag(tag).ToList(); //Find all the gameobjects with that tag and add them to a list
            all = all.Concat(temp).ToList(); //Compile the list of an individual tag into the list of all tags
        }

        return all.ToArray(); //Convert it into an array and pass it back.
    }
}
