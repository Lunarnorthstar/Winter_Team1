using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlaceCheck : MonoBehaviour
{
    public bool valid = false;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!gameObject.CompareTag("PreviewGlove")) //If you're not a glove
        {
            if (other.CompareTag("Body")) //If you're within proper distance of a body segment...
            {
                valid = true; //Then you're valid.
                //Debug.Log(valid);
            }
        }
        else if (gameObject.CompareTag("PreviewGlove")) //If you're a glove...
        {
            if (other.CompareTag("Arm")) //if you're within proper distance of an arm segment...
            {
                valid = true; //Then you're valid.
            }
        }
        
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!gameObject.CompareTag("PreviewGlove"))
        {
            if (other.CompareTag("Body"))
            {
                valid = false;
                //Debug.Log(valid);
            }
        }
        else if (gameObject.CompareTag("PreviewGlove"))
        {
            if (other.CompareTag("Arm"))
            {
                valid = false;
            }
        }
    }
}
