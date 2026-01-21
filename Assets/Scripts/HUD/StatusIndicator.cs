
using UnityEngine;

public class StatusIndicator : MonoBehaviour
{
    public GameObject prefab;

    public Vector3 screenPos => Camera.main.WorldToScreenPoint(transform.position);

    private Canvas canvas => HudManager.Instance.hudCanvas;
    private RectTransform canvasRect;

    GameObject go;
    StatusBar statusBar;

    public void DoStart()
    {
        go = Instantiate(prefab);
        statusBar = go.GetComponent<StatusBar>();
        go.transform.SetParent(canvas.transform, false);
        go.transform.localPosition = Vector3.zero;
        go.transform.localRotation = Quaternion.identity;
        go.transform.localScale = Vector3.one;
        canvasRect = canvas.GetComponent<RectTransform>();
    }

    void OnDestroy()
    {
        Destroy(go);
        go = null;
        statusBar = null;
    }

    public void UpdateUIPosition()
    {
        var screenPos = this.screenPos;
        if (screenPos.z < 0)
        {
            go.SetActive(false);
            return;
        }
        else
        {
            go.SetActive(true);
        }
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            screenPos,
            canvas.worldCamera,
            out Vector2 localPoint
        );
        statusBar.transform.localPosition = new(localPoint.x, localPoint.y, screenPos.z);
    }

    public void SetHpRatio(float ratio)
    {
        statusBar.SetHpRatio(ratio);
    }

    public void UpdateStatus(Actor actor)
    {
        statusBar.UpdateStatus(actor);
    }

}