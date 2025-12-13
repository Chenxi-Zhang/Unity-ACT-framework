
using UnityEngine;

public class InteractIndicator : MonoBehaviour
{
    public GameObject prefab;

    public Vector3 screenPos => Camera.main.WorldToScreenPoint(transform.position);

    private Canvas canvas => HudManager.Instance.hudCanvas;
    private RectTransform canvasRect;
    private DisplayState currentState;

    GameObject go;
    InteractUI interactUI;

    void Start()
    {
        go = Instantiate(prefab);
        interactUI = go.GetComponent<InteractUI>();
        go.transform.SetParent(canvas.transform, false);
        go.transform.localPosition = Vector3.zero;
        go.transform.localRotation = Quaternion.identity;
        go.transform.localScale = Vector3.one;
        canvasRect = canvas.GetComponent<RectTransform>();
        DoSetDisplayState(DisplayState.Hide);
    }

    void OnDestroy()
    {
        Destroy(go);
        go = null;
        interactUI = null;
    }

    void UpdateUIPosition()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            screenPos,
            canvas.worldCamera,
            out Vector2 localPoint
        );
        interactUI.transform.localPosition = new(localPoint.x, localPoint.y, screenPos.z);
    }

    private void DoSetDisplayState(DisplayState state)
    {
        currentState = state;
        interactUI.SetDisplayState(state);
    }

    public void UpdateDisplayState(DisplayState state)
    {
        if (state != currentState)
        {
            DoSetDisplayState(state);
        }
        if (state != DisplayState.Hide)
        {
            UpdateUIPosition();
        }
    }
}