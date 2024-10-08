using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System.Runtime.InteropServices;

public class WeaponHandler : NetworkBehaviour
{
    [Header("Prefabs")]
    public GrenadeHandler grenadePrefab;
    public RocketHandler rocketPrefab;

    [Header("Effects")]
    public ParticleSystem fireParticleSystem;
    public ParticleSystem fireParticleSystemRemotePlayer;

    [Header("Aim")]
    public Transform aimPoint;

    [Header("Collision")]
    public LayerMask collisionLayers;


    [Networked]
    public bool isFiring { get; set; }

    ChangeDetector changeDetector;



    float lastTimeFired = 0;
    float maxHitDistance = 200;



    //Timing
    TickTimer grenadeFireDelay = TickTimer.None;
    TickTimer rocketFireDelay = TickTimer.None;

    //Other components
    HPHandler hpHandler;
    NetworkPlayer networkPlayer;
    NetworkObject networkObject;

    private void Awake()
    {
        hpHandler = GetComponent<HPHandler>();
        networkPlayer = GetBehaviour<NetworkPlayer>();
        networkObject = GetComponent<NetworkObject>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public override void FixedUpdateNetwork()
    {
        if (hpHandler.isDead)
            return;

        //Get the input from the network
        if (GetInput(out NetworkInputData networkInputData))
        {
            if (networkInputData.isFireButtonPressed)
                Fire(networkInputData.aimForwardVector, networkInputData.cameraPosition);

            if (networkInputData.isGrenadeFireButtonPressed)
                FireGrenade(networkInputData.aimForwardVector);

            if (networkInputData.isRocketLauncherFireButtonPressed)
                FireRocket(networkInputData.aimForwardVector, networkInputData.cameraPosition);
        }
    }

    public override void Render()
    {
        foreach (var change in changeDetector.DetectChanges(this, out var previousBuffer, out var currentBuffer))
        {
            switch (change)
            {
                case nameof(isFiring):
                    var boolReader = GetPropertyReader<bool>(nameof(isFiring));
                    var (previousBool, currentBool) = boolReader.Read(previousBuffer, currentBuffer);
                    OnFireChanged(previousBool, currentBool);
                    break;
            }
        }
    }

    void Fire(Vector3 aimForwardVector, Vector3 cameraPosition)
    {
        //Limit fire rate
        if (Time.time - lastTimeFired < 0.15f)
            return;

        StartCoroutine(FireEffectCO());

        HPHandler hitHpHandler = CalculateFireDirection(aimForwardVector, cameraPosition, out Vector3 fireDirection);

        if (hitHpHandler != null && Object.HasStateAuthority)
            hitHpHandler.OnTakeDamage(networkPlayer.nickName.ToString(), 1);

        lastTimeFired = Time.time;
    }

    HPHandler CalculateFireDirection(Vector3 aimForwardVector, Vector3 cameraPosition, out Vector3 fireDirection)
    {
        fireDirection = aimForwardVector;
        float hitDistance = maxHitDistance;

        LagCompensatedHit hitinfo;

        //Get a raycast from third peerson camera 
        if (networkPlayer.isThirdPersonCamera)
        {
            Runner.LagCompensation.Raycast(cameraPosition, fireDirection, hitDistance, Object.InputAuthority, out hitinfo, collisionLayers, HitOptions.IgnoreInputAuthority | HitOptions.IncludePhysX);

            //check against other players 
            if (hitinfo.Hitbox != null)
            {
                Debug.DrawRay(cameraPosition, fireDirection * hitDistance, new Color(0.4f, 0, 0), 1f);

                fireDirection = (hitinfo.Point - aimPoint.position).normalized;
                hitDistance = hitinfo.Distance;
            }
            //Check against physX colliders if didn't hit player 
            else if (hitinfo.Collider != null)
            {
                fireDirection = (hitinfo.Point - aimPoint.position).normalized;
                hitDistance = hitinfo.Distance;

                Debug.DrawRay(cameraPosition, fireDirection * hitDistance, new Color(0, 0.4f, 0), 1f);

            }
            else
            {
                fireDirection = ((cameraPosition + fireDirection * hitDistance) - aimPoint.position).normalized;

                Debug.DrawRay(cameraPosition, fireDirection * hitDistance, new Color(0, 0, 0.4f), 1f);
            }
        }

        //reset hit distance
        hitDistance = maxHitDistance;

        Runner.LagCompensation.Raycast(aimPoint.position, fireDirection, maxHitDistance, Object.InputAuthority, out hitinfo, collisionLayers, HitOptions.IgnoreInputAuthority | HitOptions.IncludePhysX);

        //Check against other players 
        if (hitinfo.Hitbox != null)
        {
            hitDistance = hitinfo.Distance;
            HPHandler hitHpHandler = null;

            if (Object.HasStateAuthority)
            {
                hitHpHandler = hitinfo.Hitbox.transform.root.GetComponent<HPHandler>();
                Debug.DrawRay(aimPoint.position, fireDirection * hitDistance, Color.red, 1f);

                return hitHpHandler;
            }

        }
        else if (hitinfo.Collider != null)
        {
            hitDistance = hitinfo.Distance;

            // Draw ray for collider
            Debug.DrawRay(aimPoint.position, fireDirection * hitDistance, Color.green, 1f);
        }
        else
        {
            // Draw ray for no hit
            Debug.DrawRay(aimPoint.position, fireDirection * maxHitDistance, Color.blue, 1f);
        }

        return null;
    }

//     void FireGrenade(Vector3 aimForwardVector)
// {
//     // Check that we have not recently fired a grenade.
//     if (grenadeFireDelay.ExpiredOrNotRunning(Runner))
//     {
//         Runner.Spawn(grenadePrefab, aimPoint.position + aimForwardVector * 1.5f, Quaternion.LookRotation(aimForwardVector), Object.InputAuthority, (runner, spawnedGrenade) =>
//         {
//             if (spawnedGrenade == null)
//             {
//                 Debug.LogError("spawnedGrenade is null.");
//                 return;
//             }

//             var grenadeHandler = spawnedGrenade.GetComponent<GrenadeHandler>();
//             if (grenadeHandler == null)
//             {
//                 Debug.LogError("GrenadeHandler component is missing on spawnedGrenade.");
//                 return;
//             }

//             grenadeHandler.Throw(aimForwardVector * 15, Object.InputAuthority, networkPlayer.nickName.ToString());
//         });

//         // Start a new timer to avoid grenade spamming
//         grenadeFireDelay = TickTimer.CreateFromSeconds(Runner, 1.0f);
//     }
// }


    void FireGrenade(Vector3 aimForwardVector)
    {
        //Check that we have not recently fired a grenade. 
        if (grenadeFireDelay.ExpiredOrNotRunning(Runner))
        {
            Runner.Spawn(grenadePrefab, aimPoint.position + aimForwardVector * 1.5f, Quaternion.LookRotation(aimForwardVector), Object.InputAuthority, (runner, spawnedGrenade) =>
            {
                spawnedGrenade.GetComponent<GrenadeHandler>().Throw(aimForwardVector * 15, Object.InputAuthority, networkPlayer.nickName.ToString());
            });

            //Start a new timer to avoid grenade spamming
            grenadeFireDelay = TickTimer.CreateFromSeconds(Runner, 1.0f);
        }
    }

    void FireRocket(Vector3 aimForwardVector, Vector3 cameraPosition)
    {
        //Check that we have not recently fired a grenade. 
        if (rocketFireDelay.ExpiredOrNotRunning(Runner))
        {
            CalculateFireDirection(aimForwardVector, cameraPosition, out Vector3 fireDirection);

            Runner.Spawn(rocketPrefab, aimPoint.position + fireDirection * 1.5f, Quaternion.LookRotation(fireDirection), Object.InputAuthority, (runner, spawnedRocket) =>
            {
                spawnedRocket.GetComponent<RocketHandler>().Fire(Object.InputAuthority, networkObject, networkPlayer.nickName.ToString());
            });

            //Start a new timer to avoid grenade spamming
            rocketFireDelay = TickTimer.CreateFromSeconds(Runner, 3.0f);
        }
    }

    IEnumerator FireEffectCO()
    {
        isFiring = true;

        if (networkPlayer.isThirdPersonCamera)
            fireParticleSystemRemotePlayer.Play();
        else fireParticleSystem.Play();

        yield return new WaitForSeconds(0.09f);

        isFiring = false;
    }


    void OnFireChanged(bool previous, bool current)
    {
        if (current && !previous)
        {
            OnFireRemote();
        }

    }

    void OnFireRemote()
    {
        if (!Object.HasInputAuthority)
            fireParticleSystemRemotePlayer.Play();
    }

    public override void Spawned()
    {
        changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);
    }
}
