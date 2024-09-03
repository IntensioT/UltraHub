using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class Spikes : NetworkBehaviour
{
    [Header("Configuration")]
    public float activationDelay = 1f;
    public float rechargeTime = 5.3f;
    public byte damage = 5;
    public string trapName = "Spike trap";

    public Material defaultMaterial;
    public Material activatedMaterial;
    public Material damageMaterial;
    public Material deactivatedMaterial;


    TickTimer activationTickTimer = TickTimer.None;
    TickTimer damageTickTimer = TickTimer.None;
    TickTimer rechargeTickTimer = TickTimer.None;

    [Networked]
    private spikeState currentState { get; set; }

    [Networked]
    private materialState currentMat { get; set; }
    public enum materialState
    {
        defaultMaterial,
        activatedMaterial,
        damageMaterial,
        deactivatedMaterial
    }

    private Renderer trapRenderer;
    ChangeDetector changeDetector;

    public enum spikeState
    {
        isActivated,
        isRecharging,
        isWaiting
    }


    void Start()
    {
        trapRenderer = GetComponent<Renderer>();
        if (trapRenderer == null)
        {
            Debug.LogError("TrapBlock: Renderer component is missing.");
        }
    }

    void Update()
    {
        if (Object == null || !Object.HasStateAuthority)
            return;

        if (currentState == spikeState.isActivated)
        {
            if (activationTickTimer.Expired(Runner))
            {
                ActivateTrap();
            }
        }

        if (currentState == spikeState.isRecharging)
        {
            if (damageTickTimer.Expired(Runner))
                currentMat = materialState.deactivatedMaterial;

            if (rechargeTickTimer.Expired(Runner))
            {
                RechargeTrap();
            }
        }

    }

    public override void Render()
    {
        foreach (var change in changeDetector.DetectChanges(this, out var previousBuffer, out var currentBuffer))
        {
            switch (change)
            {
                case nameof(currentState):
                    var enumReader = GetPropertyReader<spikeState>(nameof(currentState));
                    var (previousEnum, currentEnum) = enumReader.Read(previousBuffer, currentBuffer);
                    Debug.Log("current buffer: " + currentBuffer);
                    currentState = currentEnum;
                    break;
                case nameof(currentMat):
                    var materialReader = GetPropertyReader<materialState>(nameof(currentMat));
                    var (previousMaterial, currentMaterial) = materialReader.Read(previousBuffer, currentBuffer);
                    this.currentMat = currentMaterial;
                    if (trapRenderer != null)
                    {
                        switch (this.currentMat)
                        {
                            case 0:
                                trapRenderer.material = defaultMaterial;
                                break;
                            case (materialState)1:
                                trapRenderer.material = activatedMaterial;
                                break;
                            case (materialState)2:
                                trapRenderer.material = damageMaterial;
                                break;
                            case (materialState)3:
                                trapRenderer.material = deactivatedMaterial;
                                break;
                        }
                    }
                    break;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (currentState != spikeState.isActivated && currentState != spikeState.isRecharging && other.CompareTag("Player"))
        {
            currentState = spikeState.isActivated;
            currentMat = materialState.activatedMaterial;
            activationTickTimer = TickTimer.CreateFromSeconds(Runner, 1);
        }
    }

    void ActivateTrap()
    {
        currentState = spikeState.isRecharging;
        currentMat = materialState.damageMaterial;

        Collider[] colliders = Physics.OverlapBox(transform.position, transform.localScale / 2, Quaternion.identity);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                HPHandler hPHandler = collider.GetComponent<HPHandler>();
                if (hPHandler != null)
                {
                    hPHandler.OnTakeDamage(trapName, damage);
                }
            }
        }
        damageTickTimer = TickTimer.CreateFromSeconds(Runner, 0.3f);
        rechargeTickTimer = TickTimer.CreateFromSeconds(Runner, 5.3f);
    }

    void RechargeTrap()
    {
        currentState = spikeState.isWaiting;
        currentMat = materialState.defaultMaterial;
    }


    public override void Spawned()
    {
        changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);
        currentState = spikeState.isWaiting;
    }
}
