using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotationTrap : MonoBehaviour
{
    [SerializeField] private float maxRotationSpeed = 50f;
    [SerializeField] private float changeInterval = 2f;

    private Vector3 rotationAxis;
    private float rotationSpeed;
    private float timer;

    void Start()
    {
        SetRandomRotation();
    }

    void Update()
    {
        transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime);

        timer += Time.deltaTime;

        if (timer >= changeInterval)
        {
            SetRandomRotation();
            timer = 0f;
        }
    }

    private void SetRandomRotation()
    {
        rotationAxis = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;

        rotationSpeed = Random.Range(-maxRotationSpeed, maxRotationSpeed);
    }
}
