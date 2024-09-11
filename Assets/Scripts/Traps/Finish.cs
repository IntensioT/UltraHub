using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : NetworkBehaviour
{
    NetworkRunner networkRunnerInParent;
    NetworkRunner networkRunner;

    void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        networkRunnerInParent = other.GetComponentInParent<NetworkRunner>();
        networkRunner = other.GetComponent<NetworkRunner>();
        Debug.Log("Finish collision: Network Runner this : " + this.Runner);
        // Завершить текущую сессию
        OnWinnerEndGame(Runner);

        // Включить курсор
        EnableCursor();

        // Перезагрузить сцену
        SceneManager.LoadScene("MainMenu"); // Замените на имя вашей сцены выбора сессии
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
