using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

public class ApplicationController : MonoBehaviour
{
    [SerializeField] private ClientSingleton clientSingletonPrefab;
    [SerializeField] private HostSingleton hostSingletonPrefab;

    [SerializeField] private NetworkObject playerPrefab;

    private async void Start() 
    {
        DontDestroyOnLoad(this.gameObject);

        await LaunchInMode(SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.Null);
    }

    private async Task LaunchInMode(bool isDedicatedServer)
    {
        if(isDedicatedServer)
        {

        }
        else
        {
            HostSingleton hostSingleton = Instantiate(hostSingletonPrefab);
            hostSingleton.CreateHost(playerPrefab);

            ClientSingleton clientSingleton = Instantiate(clientSingletonPrefab);
            bool isAuthenticated = await clientSingleton.CreateClient();

            if(isAuthenticated)
            {
                clientSingleton.clientGameManager.GoToMainMenu();
            }
        }
    }
}
