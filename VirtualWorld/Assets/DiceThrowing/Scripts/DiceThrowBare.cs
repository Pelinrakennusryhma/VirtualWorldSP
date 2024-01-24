using FMODUnity;
using Scenes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DiceMinigame
{
    public class DiceThrowBare : MonoBehaviour
    {
        // how far in front of the character does the dice scene load when throwing dice in the world
        [SerializeField] float forwardOffset = 4f;
        [SerializeField] CamMover camMover;
        [SerializeField] GameObject environments;
        [SerializeField] GameObject eventSystem;
        void Start()
        {
            PrepareScene();
        }

        void PrepareScene()
        {
            if (SceneLoader.Instance == null)
            {
                environments.SetActive(true);
                eventSystem.SetActive(true);
                return;
            }

            object sceneData = SceneLoader.Instance.sceneLoadParams.sceneData;

            // whether we're playing dicethrow on arcade or out in the world determines if the surroundings are shown
            if ((string)sceneData == "ShowEnvironment")
            {
                environments.SetActive(true);
                eventSystem.SetActive(true);

            } else
            {
                Vector3 offset = SceneLoader.Instance.sceneLoadParams.origo;
                Quaternion rotation = SceneLoader.Instance.sceneLoadParams.rotation;

                transform.position = transform.position + offset;
                transform.rotation = rotation;

                transform.position = transform.position + (transform.forward * forwardOffset);

                camMover.Init();
                camMover.GetComponent<StudioListener>().enabled = false;

                environments.SetActive(false);
                eventSystem.SetActive(false);
            }
        }
    }
}

