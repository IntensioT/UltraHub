using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Finish : NetworkBehaviour
{

    [SerializeField] GameObject playerEndGameMenu;
    void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Debug.Log("Finish collision: Network Runner this : " + this.Runner);

        other.GetComponent<NetworkInGameMessages>().SendEndGameRPCMessage(other.GetComponent<NetworkPlayer>().nickName.ToString());

        EnableCursor();

        // Перезагрузить сцену
        // SceneManager.LoadScene("MainMenu");
    }

    private void EnableCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }


    public async void OnWinnerEndGame(NetworkRunner runner)
    {
        Debug.Log("Winner end game");

        // Shut down the current runner
        await runner.Shutdown(shutdownReason: ShutdownReason.HostMigration);
    }

}
