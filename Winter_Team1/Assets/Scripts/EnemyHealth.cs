using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private float parts = 0;
    private float currentParts = 0;

    public GameObject healthUI;
    
    
    // Start is called before the first frame update
    void Start()
    {
        parts = GameObject.FindGameObjectsWithTag("Enemy").Length;
        currentParts = parts;
    }

    // Update is called once per frame
    void Update()
    {
        currentParts = GameObject.FindGameObjectsWithTag("Enemy").Length;
        healthUI.GetComponent<RectTransform>().localScale = new Vector3(currentParts / parts, 1, 1);
    }
}
