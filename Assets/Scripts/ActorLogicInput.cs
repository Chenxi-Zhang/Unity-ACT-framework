using System;
using System.Collections.Generic;
using UnityEngine;

public class ActorLogicInput : MonoBehaviour
{
    public Actor actor;

    Dictionary<InputType, Action> inputActions = new ();
    Dictionary<InputType, Action> inputThisFrame = new ();

    public void RegisterInputAction(InputType inputType, Action action)
    {
        if (inputActions.ContainsKey(inputType))
        {
            throw new Exception($"Input action {inputType} already registered.");
        }
        inputActions[inputType] = action;
    }

    public void UnregisterInputAction(InputType inputType)
    {
        inputActions.Remove(inputType);
    }

    public void InputMove(Vector3 direction, float distance)
    {
        actor.movement.UpdateTurn(direction);
        if (distance > 0.1f)
        {
            TryAddInput(InputType.Move);
        }
        else
        {
            TryAddInput(InputType.MoveCancel);
        }
    }

    private void TryAddInput(InputType inputType)
    {
        if (inputActions.TryGetValue(inputType, out var action))
        {
            inputThisFrame[inputType] = action;
        }
    }

    void LateUpdate()
    {
        if (inputThisFrame.Count == 0)
            return;
        InputType execType = InputType.None;
        Action execAction = null;
        foreach (var (key, value) in inputThisFrame)
        {
            if (execType == InputType.None || key > execType)
            {
                execType = key;
                execAction = value;
            }
        }
        inputThisFrame.Clear();
        execAction?.Invoke();
    }
}

public enum InputType {
    None,
    Move,
    MoveCancel,
}