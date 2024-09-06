using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fans : MonoBehaviour
{
    [SerializeField]
    public float pushForce = 10f;
    public Transform fanDirection;

    void OnTriggerStay(Collider other)
    {
        Debug.Log(other.attachedRigidbody + "Collider trigger");
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Debug.Log(rb.name + "- Rigid body");
            // Применяем силу в направлении вентилятора
            Vector3 forceDirection = fanDirection.forward;
            rb.AddForce(forceDirection * pushForce, ForceMode.Force);
        }
    }
}
