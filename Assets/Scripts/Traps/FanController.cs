using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class FanController : NetworkBehaviour
{
    public Fans leftFan;
    public Fans rightFan;
    public float switchInterval = 2f;

    private bool isLeftFanActive = true;
    private float timer = 0f;

    void Update()
    {
        if (Object == null) return;
        if (Object.HasStateAuthority)
        {
            timer += Time.deltaTime;
            if (timer >= switchInterval)
            {
                timer = 0f;
                isLeftFanActive = !isLeftFanActive;
                RpcSwitchFan(isLeftFanActive);
            }
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    void RpcSwitchFan(bool leftFanActive)
    {
        leftFan.enabled = leftFanActive;
        leftFan.SetAnimationActive(leftFanActive);
        leftFan.GetComponent<Collider> ().enabled = leftFanActive;
        
        rightFan.enabled = !leftFanActive;
        rightFan.SetAnimationActive(!leftFanActive);
        rightFan.GetComponent<Collider> ().enabled = !leftFanActive;
    }
}
