using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class NetworkConnector : MonoBehaviour
{
    [SerializeField] private NetworkManager networkManager;
    [SerializeField] private bool startAsServer = false;

    void Start()
    {
        if (Application.isBatchMode)
        { //Headless build
            Debug.Log("=== Server Build ===");
        }
        else if(startAsServer)
        {
            Debug.Log("=== Starting as Server ===");
            networkManager.StartServer();
        }
        else
        {
            Debug.Log("=== Client Build ===");
            networkManager.StartClient();
        }
    }

    public void TryConnectToServer()
    {
        networkManager.StartClient();
    }

}
