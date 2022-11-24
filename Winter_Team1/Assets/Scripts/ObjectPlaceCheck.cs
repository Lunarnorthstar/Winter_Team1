using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlaceCheck : MonoBehaviour
{
    public bool valid = false;

    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log(other.tag);
        if (other.tag == "Body")
        {
            valid = true;
            Debug.Log(valid);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Body")
        {
            valid = false;
        }
    }
}
