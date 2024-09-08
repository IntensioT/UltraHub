using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class Fans : NetworkBehaviour
{
    [SerializeField]
    public float pushForce = 100f;
    public Transform fanDirection;

    NetworkCharacterController networkCharacterController;

    private void Awake()
    {
        networkCharacterController = GetComponentInParent<NetworkCharacterController>();
    }
    void OnTriggerStay(Collider other)
    {
        Debug.Log(other.attachedRigidbody + "Collider trigger");


        networkCharacterController = other.GetComponentInParent<NetworkCharacterController>();
        Debug.Log(networkCharacterController);

        

        if (networkCharacterController != null)
        {
            Debug.Log(networkCharacterController.name + " - NetworkCharacterController");
            // Применяем силу в направлении вентилятора
            Vector3 forceDirection = fanDirection.forward;
            Debug.DrawLine(transform.up, forceDirection, Color.red);
            Vector3 moveDirection = forceDirection;

            // Нормализуем направление движения, если это необходимо
            moveDirection.Normalize();

            // Передаем направление движения в networkCharacterController
            networkCharacterController.Move(moveDirection, pushForce);
        }
    }
}
