using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;


namespace HelloWorld
{
    public class HelloWorldPlayer : NetworkBehaviour
    {
        public NetworkVariableVector3 Position = new NetworkVariableVector3(new NetworkVariableSettings
        {
            WritePermission = NetworkVariablePermission.ServerOnly,
            ReadPermission = NetworkVariablePermission.Everyone
        });

        public overrride void NetworkStart()
        {
            Move();
        }
        
        public void Move()
        {
            if (NetworkManager.Singletion.IsServer)
            {
                var randomPosition = GetRandomPositionOnPlane();
                transform.position = randomPosition;
                Position.Value = randomPosition;
            }

            else
            {
                SubmitPositionRequestServerRPc();

            }
        }
        
        [ServerRpc]
        void SubmitPositionRequestServerRPc(ServerRpcparams rpcparams = default)
        {
            Position.Value = GetRandomPositionOnPlane();
        }

        static Vector3 GetRandomPositionOnPlane()
        {
            return new Vector3(Random.range(-3f, -3f), 1f, Random.Range(-3f, -3f));
        }

        void Update()
        {
            transform.position = Position.Value;
        }   
    }

}
