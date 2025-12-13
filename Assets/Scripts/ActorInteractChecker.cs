
using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class ActorInteractChecker : MonoBehaviour
{

    public List<InteractObject> interactObjs = new();
    public InteractObject focusedObj;

    static int actorLayer = -1;
    static int ActorLayer
    {
        get
        {
            if (actorLayer == -1)
            {
                actorLayer = LayerMask.NameToLayer("InteractActor");
            }
            return actorLayer;
        }
    }

    void Awake()
    {
        var rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
        var coll = GetComponent<Collider>();
        coll.isTrigger = true;
        gameObject.layer = ActorLayer;
    }

    void OnTriggerEnter(Collider other)
    {
        var interactObj = other.GetComponent<InteractObject>();
        if (interactObj)
        {
            AddInteractObject(interactObj);
        }
    }

    void OnTriggerExit(Collider other)
    {
        var interactObj = other.GetComponent<InteractObject>();
        if (interactObj)
        {
            RemoveInteracObject(interactObj);
        }
    }

    void RemoveInteracObject(InteractObject interactiveObject)
    {
        interactObjs.Remove(interactiveObject);
        interactiveObject.Hide();
        interactObjs.RemoveAll(i => i == null);
    }

    void AddInteractObject(InteractObject interactiveObject)
    {
        interactObjs.Add(interactiveObject);
        interactObjs.RemoveAll(i => i == null);
    }

    public void SetFocusedObject(InteractObject mainObj)
    {
        if (mainObj != focusedObj)
        {
            focusedObj = mainObj;
        }
    }
}