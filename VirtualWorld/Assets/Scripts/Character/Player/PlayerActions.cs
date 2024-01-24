using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;
using StarterAssets;

public class PlayerActions : MonoBehaviour
{
    [Tooltip("Delay before queued action is used, to prevent awkward behaviour after character lands.")]
    [SerializeField] float queuedActionExecuteDelay = 0.5f;
    StarterAssetsInputs inputs;
    ThirdPersonController thirdPersonController;
    List<IPlayerAction> actions = new List<IPlayerAction>();

    IPlayerAction queuedAction;
    IPlayerAction queuedDelayedAction;

    void Start()
    {
        inputs = GetComponentInParent<StarterAssetsInputs>();
        thirdPersonController = GetComponentInParent<ThirdPersonController>();
        FindAllActions();
    }

    private void OnEnable()
    {
        queuedAction = null;
        queuedDelayedAction = null;
    }

    void Update()
    {
        if (inputs.action1)
        {
            queuedAction = actions[0];
        }

        if (queuedAction != null)
        {
            if (CanExecute(queuedAction))
            {
                Execute(queuedAction);
            }
            else // Can't execute for some reason, make the queued action start with a delay to make it look less awkward after landing for example
            {
                queuedDelayedAction = queuedAction;
                queuedAction = null;
            }
        }
        else if (queuedDelayedAction != null)
        {
            if (CanExecute(queuedDelayedAction))
            {
                StartCoroutine(DelayExecute(queuedDelayedAction, queuedActionExecuteDelay));
                queuedDelayedAction = null;
            }
        }
    }

    bool CanExecute(IPlayerAction action)
    {
        if (!action.RequireGrounded)
        {
            return true;
        }
        else if (thirdPersonController.Grounded || !action.RequireGrounded)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    IEnumerator DelayExecute(IPlayerAction action, float delay)
    {
        yield return new WaitForSeconds(delay);
        Execute(action);

    }

    void FindAllActions()
    {
        var actionScripts = FindObjectsOfType<MonoBehaviour>().OfType<IPlayerAction>();
        foreach (IPlayerAction actionScript in actionScripts)
        {
            actions.Add(actionScript);
        }
    }

    void Execute(IPlayerAction action)
    {
        inputs.ClearExecuteInputs();
        action.Execute();
    }

    // ---------- Action Key mapping On hold until solid input system ---------

    //string actionStringFormat = "action";
    //Dictionary<string, IPlayerAction> actionKeybinds = new Dictionary<string, IPlayerAction>();

    //void MapActionsToKeys()
    //{
    //    for (int i = 0; i < actions.Count; i++)
    //    {
    //        actionKeybinds.Add($"{actionStringFormat}{i + 1}", actions[i]);
    //    }
    //}

    //void ListenToInput()
    //{
    //    foreach (KeyValuePair<string, IPlayerAction> action in actionKeybinds)
    //    {

    //    }
    //}
}
