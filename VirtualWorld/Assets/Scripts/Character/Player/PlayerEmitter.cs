using StarterAssets;
using UI;
using FishNet;
using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine.InputSystem;
using Scenes;
using FishNet.Object;
using FishNet.Component.Animating;
using System.Collections.Generic;

namespace Characters
{
    public class PlayerEmitter : NetworkBehaviour
    {
        [SerializeField] StarterAssetsInputs inputs;
        [SerializeField] PlayerInput playerInput;

        [SerializeField] ThirdPersonController controller;

        [SerializeField] CinemachineVirtualCamera _cinemachineVirtualCamera;
        [SerializeField] List<GameObject> ownedObjects;

        private void Awake()
        {
            foreach (GameObject gameObject in ownedObjects)
            {
                gameObject.SetActive(false);
            }
        }
        public override void OnStartClient()
        {
            base.OnStartClient();

            // if not our own character notify the minigame loading system about a new gameobject being instantiated
            if (!IsOwner)
            {
                SceneLoader.Instance.NewMainSceneObjectAdded(gameObject);
                return;
            }

            // owned character is made priority for camera
            _cinemachineVirtualCamera.Priority = 100;

            UIManager.Instance.SetPlayerCharacter(gameObject);
            CharacterManagerNonNetworked.Instance?.SetOwnedCharacter(gameObject);
            foreach (GameObject gameObject in ownedObjects)
            {
                gameObject.SetActive(true);
            }

            controller.shouldAnimate = true;

            EnableNetworkedControls();
        }

        void EnableNetworkedControls()
        {
            if (IsClient && IsOwner)
            {
                inputs.enabled = true;
                playerInput.enabled = true;
            }
        }
    }
}
