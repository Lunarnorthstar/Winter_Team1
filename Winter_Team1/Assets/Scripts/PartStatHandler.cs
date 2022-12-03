using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class PartStatHandler : MonoBehaviour
{
    public float durability;

    public float attack;

    public float speed;

    public GameObject particlePrefab;

    public int particleCount = 7;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (durability <= 0)
        {
            Instantiate(particlePrefab, gameObject.transform.position, Quaternion.identity);
            
            
            
            Destroy(gameObject);
        }
    }
}
