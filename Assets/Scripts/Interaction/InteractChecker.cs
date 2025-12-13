
using TMPro;
using UnityEngine;

public class InteractChecker : MonoBehaviour
{

    public BoxCollider boxCollider;

    public bool IsInteractable(Actor actor)
    {
        return boxCollider && boxCollider.bounds.Contains(actor.transform.position);
    }

}