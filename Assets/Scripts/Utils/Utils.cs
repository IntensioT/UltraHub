using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    private static float gameTime;
    private static float startTime;

    public static void StartTimer()
    {
        startTime = Time.time;
    }

    public static float GetGameTime()
    {
        UpdateTimer();
        return gameTime;
    }

    public static Vector3 GetRandomSpawnPoint()
    {
        return new Vector3(Random.Range(-4.5f, 4.5f), 4, Random.Range(-4.5f, 4.5f));
    }

    public static void SetRenderLayerInChildren(Transform transform, int layerNumber)
    {
        foreach (Transform trans in transform.GetComponentsInChildren<Transform>(true))
        {
            if (trans.CompareTag("IgnoreLayerChange"))
                continue;

            trans.gameObject.layer = layerNumber;
        }
    }
    private static void UpdateTimer()
    {
        gameTime = Time.time - startTime;
    }
}
