using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.SceneManagement;

public class NetworkInGameMessages : NetworkBehaviour
{
    InGameMessagesUIHander inGameMessagesUIHander;
    EndGameMenuUIHandler endGameMenuUIHandler;
    NetworkButtonUIHandler networkButtonUIHandler;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void SendInGameRPCMessage(string userNickName, string message)
    {
        RPC_InGameMessage($"<b>{userNickName}</b> {message}");
    }

    public void SendEndGameRPCMessage(string userNickName)
    {
        RPC_EndGameMessage(userNickName);
    }

    public async void OnWinnerEndGame()
    {
        Debug.Log("Winner end game");

        // Shut down the current runner
        await this.Runner.Shutdown(shutdownReason: ShutdownReason.HostMigration);

        SceneManager.LoadScene("MainMenu");
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    void RPC_InGameMessage(string message, RpcInfo info = default)
    {
        Debug.Log($"[RPC] InGameMessage {message}");

        if (inGameMessagesUIHander == null)
            inGameMessagesUIHander = NetworkPlayer.Local.localCameraHandler.GetComponentInChildren<InGameMessagesUIHander>();

        if (inGameMessagesUIHander != null)
            inGameMessagesUIHander.OnGameMessageReceived(message);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    void RPC_EndGameMessage(string message, RpcInfo info = default)
    {
        Debug.Log($"[RPC] EndGameMessage {message}");

        if (endGameMenuUIHandler == null)
            endGameMenuUIHandler = NetworkPlayer.Local.localCameraHandler.GetComponentInChildren<EndGameMenuUIHandler>();

        if (endGameMenuUIHandler != null)
            endGameMenuUIHandler.OnEndGameMessageReceived(message);

        if (networkButtonUIHandler == null)
        {
            networkButtonUIHandler = NetworkPlayer.Local.localCameraHandler.GetComponentInChildren<NetworkButtonUIHandler>();
        }

        if (networkButtonUIHandler != null)
        {
            networkButtonUIHandler.OnEndGameMessageReceived();
        }
    }


}
