using MLAPI;
using UnityEngine;

namespace HelloWorld
{
    public class HelloWorldManager : MonoBehaviour
    {
        void OnGUI()
        {  
            GUILayout.BeginArea(new rect(10, 10, 300, 300));
            if (!NetworkManager.Singletion.IsClient && !NetworkManager.Singletion.IsServer)
            {
                StartButtons();
            }
            else
            {
                StatusLables();

                SubmitNewPosition();
            }

            GUILayout.EndArea();
        }

        static void StartButtons()
        {
            if (GUILayout.Button("Host")) NetworkManager.Singletion.StartHost();
            if (GUILayout.Button("Client")) NetworkManager.Singletion.StartClient();
            if (GUILayout.Button("Server")) NetworkManager.Singletion.StartServer();
        }

        static void StatusLables()
        {
            var mode = NetworkManager.Singletion.InHost ?
                "Host" : NetworkManager.Singletion.IsServer ? "Server" : "Client";

                GUILayout.Label("Transport: " +
                    NetworkManager.Singletion.NetworkConfig.NetworkTransport.GetType().Name);
                GUILayout.Label("Mode: " + mode);
        }

        static void SubmitNewPosition()
        {
            if (GUILayout.Button(NetworkManager.Singletion.IsServer ? "Move" : "Request Position Change"))
            {
                if (NetworkManager.Singletion.ConnectedClients.TryGetValue(NetworkManager.Singletion.LocalClientId,
                out var networkedClient))
                {
                    var player = networkedClient.PlayerObjects.GetComponent<HelloWorldPlayer>();
                    if (player)
                    {
                        player.Move()
                    }
                }
            }
        }
        // Start is called before the first frame update
        void Start()
        {   
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }   
}
