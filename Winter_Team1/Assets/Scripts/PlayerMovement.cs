using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 inputVector;
    private GameObject[] arms;
    private bool grabbing = false;

    public float jumpImpulse = 0;
    public float speed = 300;
    // Start is called before the first frame update

    public void onMove(InputAction.CallbackContext ctx) => inputVector = ctx.ReadValue<Vector2>();
    // Update is called once per frame

    private void FixedUpdate()
    {
        transform.Translate((new Vector3(inputVector.x,0,0) * speed * Time.fixedDeltaTime));

        arms = GameObject.FindGameObjectsWithTag("Arm");


        if (grabbing)
        {
            foreach (GameObject arm in arms)
            {
                GameObject target = arm.GetComponent<HandScript>().target;

                if (target != null)
                {
                    if(math.distance(arm.transform.position, target.transform.position) > 0.2)
                    {
                        target.transform.parent.GetComponent<Transform>().Translate( arm.transform.position - target.transform.position);

                        target.GetComponent<PartStatHandler>().durability -= arm.GetComponent<PartStatHandler>().attack * Time.fixedDeltaTime;

                    }
                }
                
                

            }
        }
    }

    /*void Update()
    {
        transform.Translate((new Vector3(inputVector.x,0,0) * speed * Time.deltaTime));

        arms = GameObject.FindGameObjectsWithTag("Arm");


        if (grabbing)
        {
            foreach (GameObject arm in arms)
            {
                GameObject target = arm.GetComponent<HandScript>().target;
                if(math.distance(arm.transform.position, target.transform.position) > 0.2)
                {
                    target.transform.parent.GetComponent<Transform>().Translate( arm.transform.position - target.transform.position);

                    target.GetComponent<PartStatHandler>().durability -= arm.GetComponent<PartStatHandler>().attack * Time.deltaTime;

                }
                
                
                
            }
        }

    }*/

    public void Grab(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            grabbing = true;
        }

        if (context.canceled)
        {
            grabbing = false;
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpImpulse));
        }
    }
}
