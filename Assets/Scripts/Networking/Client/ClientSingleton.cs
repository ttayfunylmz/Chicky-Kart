using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ClientSingleton : MonoBehaviour
{
    private static ClientSingleton instance;

    public ClientGameManager clientGameManager { get; private set; }

    public static ClientSingleton Instance
    {
        get
        {
            if (instance != null) { return instance; }

            instance = FindObjectOfType<ClientSingleton>();

            if(instance == null)
            {
                Debug.LogError("No ClientSingleton is found in the scene!");
                return null;
            }

            return instance;
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public async Task<bool> CreateClient()
    {
        clientGameManager = new ClientGameManager();
        return await clientGameManager.InitializeAsync();
    }

    private void OnDestroy() 
    {
        clientGameManager?.Dispose();
    }
    
}
