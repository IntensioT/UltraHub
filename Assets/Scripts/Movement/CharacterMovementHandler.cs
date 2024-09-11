using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.SceneManagement;

public class CharacterMovementHandler : NetworkBehaviour
{
    [Header("Animation")]
    public Animator characterAnimator;

    bool isRespawnRequested = false;

    float walkSpeed = 0;

    //Other components
    NetworkCharacterController networkCharacterController;
    HPHandler hpHandler;
    NetworkInGameMessages networkInGameMessages;
    NetworkPlayer networkPlayer;


    [SerializeField] private float doubleJumpMultiplier = 200.75f;
    [SerializeField] private float doubleJumpCD = 5f;

    public float DoubleJumpCDFactor => (DoubleJumpCD.RemainingTime(Runner) ?? 0f) / doubleJumpCD;

    [Networked] private TickTimer DoubleJumpCD { get; set; }

    private void Awake()
    {
        networkCharacterController = GetComponent<NetworkCharacterController>();
        hpHandler = GetComponent<HPHandler>();
        networkInGameMessages = GetComponent<NetworkInGameMessages>();
        networkPlayer = GetComponent<NetworkPlayer>();

    }

    // Start is called before the first frame update
    void Start()
    {
    }

    public override void FixedUpdateNetwork()
    {
        if (SceneManager.GetActiveScene().name == "Ready")
            return;


        if (Object.HasStateAuthority)
        {
            if (isRespawnRequested)
            {
                Respawn();
                return;
            }

            //Don't update the clients position when they are dead
            if (hpHandler.isDead)
                return;
        }

        //Get the input from the network
        if (GetInput(out NetworkInputData networkInputData))
        {
            //Rotate the transform according to the client aim vector
            transform.forward = networkInputData.aimForwardVector;

            //Cancel out rotation on X axis as we don't want our character to tilt
            Quaternion rotation = transform.rotation;
            rotation.eulerAngles = new Vector3(0, rotation.eulerAngles.y, rotation.eulerAngles.z);
            transform.rotation = rotation;

            //Move
            Vector3 moveDirection = transform.forward * networkInputData.movementInput.y + transform.right * networkInputData.movementInput.x;
            moveDirection.Normalize();

            networkCharacterController.Move(moveDirection);


            //Jump
            if (networkInputData.isJumpPressed)
            {
                networkCharacterController.Jump();
            }
            if (networkInputData.isJumpPressed && !networkCharacterController.Grounded
            && DoubleJumpCD.ExpiredOrNotRunning(Runner)
            )
            {
                Debug.Log("Double jump pressed: " + doubleJumpCD);
                networkCharacterController.Jump(true, overrideImpulse: networkCharacterController.jumpImpulse * doubleJumpMultiplier);
                DoubleJumpCD = TickTimer.CreateFromSeconds(Runner, doubleJumpCD);
            }

            Vector2 walkVector = new Vector2(networkCharacterController.Velocity.x, networkCharacterController.Velocity.z);
            walkVector.Normalize();

            walkSpeed = Mathf.Lerp(walkSpeed, Mathf.Clamp01(walkVector.magnitude), Runner.DeltaTime * 5);

            characterAnimator.SetFloat("walkSpeed", walkSpeed);


            //Check if we've fallen off the world.
            CheckFallRespawn();
        }

    }

    void CheckFallRespawn()
    {
        if (transform.position.y < -12)
        {
            if (Object.HasStateAuthority)
            {
                Debug.Log($"{Time.time} Respawn due to fall outside of map at position {transform.position}");

                networkInGameMessages.SendInGameRPCMessage(networkPlayer.nickName.ToString(), "fell off the world");

                Respawn();
            }

        }
    }

    public void RequestRespawn()
    {
        isRespawnRequested = true;
    }

    void Respawn()
    {
        SetCharacterControllerEnabled(true);
        networkCharacterController.Teleport(Utils.GetRandomSpawnPoint());

        hpHandler.OnRespawned();

        isRespawnRequested = false;
    }

    void CheckFinishEndGame()
    {
        if (transform.position.y < -12)
        {
            if (Object.HasStateAuthority)
            {
                Debug.Log($"{Time.time} Respawn due to fall outside of map at position {transform.position}");

                networkInGameMessages.SendInGameRPCMessage(networkPlayer.nickName.ToString(), "fell off the world");

                Respawn();
            }

        }
    }

    public void SetCharacterControllerEnabled(bool isEnabled)
    {
        networkCharacterController.enabled = isEnabled;
    }

    public void ResetCooldowns()
    {
        DoubleJumpCD = TickTimer.None;
    }
    public override void Spawned()
    {
        if (Object.HasStateAuthority)
        {
            networkCharacterController.Teleport(Utils.GetRandomSpawnPoint());
            this.ResetCooldowns();
        }
    }
}
