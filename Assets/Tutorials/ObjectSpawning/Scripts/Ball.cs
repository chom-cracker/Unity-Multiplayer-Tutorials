using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using UnityEngine;

namespace DapperDino.UMT.ObjectSpawning
{
    public class Ball : NetworkBehaviour
    {
        [SerializeField] private Renderer ballRenderer;

        private NetworkVariableColor ballColour = new NetworkVariableColor();

        public override void NetworkStart()
        {
            // Make sure we are the server
            if (!IsServer) { return; }

            // Generate a random colour for this ball
            ballColour.Value = Random.ColorHSV();
        }

        private void OnEnable()
        {
            // Start listening for the ball colour updated
            ballColour.OnValueChanged += OnBallColourChanged;
        }

        private void OnDisable()
        {
            // Stop listening for the ball colour updated
            ballColour.OnValueChanged -= OnBallColourChanged;
        }

        private void Update()
        {
            // Make sure this is belongs to us
            if (!IsOwner) { return; }

            // Check to see if we just hit the space key
            if (!Input.GetKeyDown(KeyCode.Space)) { return; }

            // Send a message to the server to execute this method
            DestroyBallServerRpc();
        }

        private void OnBallColourChanged(Color oldBallColour, Color newBallColour)
        {
            // Only clients need to update the renderer
            if (!IsClient) { return; }

            // Update the colour of the player's mesh renderer
            ballRenderer.material.SetColor("_BaseColor", newBallColour);
        }

        [ServerRpc]
        private void DestroyBallServerRpc()
        {
            // By destroying a NetworkObject on the server,
            // the object will then be destroyed on all clients
            Destroy(gameObject);
        }
    }
}
