
using UnityEngine;

[CreateAssetMenu(fileName = "SfxData", menuName = "Configs/Sfx", order = 1001)]
public class SfxData : ScriptableObject
{
    public GameObject sfxPrefab;
    public float destroyDelay = 5f;
    [Tooltip("If true, the SFX will be attached to the binding object. If false, it keeps world position.")]
    public bool isAttaching;
    public BindingInfo bindingInfo;

    public SfxRuntimeData CreateSfxFor(Actor actor)
    {
        var go = CreateSfxGo(actor);
        return new SfxRuntimeData
        {
            sfxData = this,
            sfxObj = go
        };
    }

    private GameObject CreateSfxGo(Actor actor)
    {
        var bindingResult = bindingInfo.GetBindingTransform(actor.transform);
        var bindingTrans = bindingResult.target;
        var spawnPosition = bindingResult.worldPosition;
        var spawnRotation = bindingResult.worldRotation;
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            return Instantiate(sfxPrefab, spawnPosition, spawnRotation, bindingTrans);
        }
#endif
        if (isAttaching)
        {
            return Instantiate(sfxPrefab, spawnPosition, spawnRotation, bindingTrans);
        }
        else
        {
            return Instantiate(sfxPrefab, spawnPosition, spawnRotation);
        }
    }

}

public class SfxRuntimeData
{
    public SfxData sfxData;
    public GameObject sfxObj;

    public void Destroy()
    {
        if (sfxObj == null)
            return;
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            GameObject.DestroyImmediate(sfxObj);
            return;
        }
#endif
        if (sfxObj.TryGetComponent<BaseSfxContainer>(out var sfxContainer))
        {
            sfxContainer.Destroy(sfxData.destroyDelay);
        }
        else
        {
            GameObject.Destroy(sfxObj, sfxData.destroyDelay);
        }
    }
}
