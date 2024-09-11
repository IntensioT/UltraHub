using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class Fans : NetworkBehaviour
{
    [SerializeField]
    public float pushForce = 100f;
    public Transform fanDirection;
    public Animator fanAnimator;

    NetworkCharacterController networkCharacterController;

    private void Awake()
    {
        networkCharacterController = GetComponentInParent<NetworkCharacterController>();
    }
    void OnTriggerStay(Collider other)
    {
        if (!isActiveAndEnabled) return;
        if (!other.CompareTag("Player")) return;

        Debug.Log(other.attachedRigidbody + "Collider trigger");


        networkCharacterController = other.GetComponentInParent<NetworkCharacterController>();

        if (networkCharacterController != null)
        {
            Debug.Log(networkCharacterController.name + " - NetworkCharacterController");
            // Применяем силу в направлении вентилятора
            Vector3 forceDirection = fanDirection.forward;
            Vector3 moveDirection = forceDirection;

            // Нормализуем направление движения, если это необходимо
            moveDirection.Normalize();

            // Передаем направление движения в networkCharacterController
            networkCharacterController.Move(moveDirection, pushForce);
        }
    }
    public void SetAnimationActive(bool isActive)
    {
        fanAnimator.enabled = isActive;
    }
}
