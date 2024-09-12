using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hummer : MonoBehaviour
{

    [SerializeField] public GameObject hummerFoundation;
    [SerializeField] public GameObject hummerInstance;
    [SerializeField] protected float rotationSpeed = 100f;

    // Update is called once per frame
    void Update()
    {
        hummerInstance.transform.RotateAround(hummerFoundation.transform.position, Vector3.up, rotationSpeed * Time.deltaTime);        
    }
}
