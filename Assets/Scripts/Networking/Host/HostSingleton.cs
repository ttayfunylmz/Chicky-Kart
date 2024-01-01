using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

public class HostSingleton : MonoBehaviour
{
    private static HostSingleton instance;

    public HostGameManager hostGameManager { get; private set; }

    public static HostSingleton Instance
    {
        get
        {
            if (instance != null) { return instance; }

            instance = FindObjectOfType<HostSingleton>();

            if(instance == null)
            {
                Debug.LogError("No HostSingleton is found in the scene!");
                return null;
            }

            return instance;
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void CreateHost(NetworkObject playerPrefab)
    {
        hostGameManager = new HostGameManager(playerPrefab);
    }

    private void OnDestroy() 
    {
        hostGameManager?.Dispose();
    }
}
