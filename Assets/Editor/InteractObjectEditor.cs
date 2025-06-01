using UnityEditor;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[CustomEditor(typeof(InteractObject))]
class InteractObjectEditor : Editor
{
    private TimelineAsset tempTimelineAsset;
    private PlayableDirector previewDirector;

    BasePlayableDirector targetDirector;
    BasePlayableDirector actorDirector;

    private void OnEnable()
    {
        InteractObject config = (InteractObject)target;
        Actor actor = SceneObjectTool.GetComponentInSceneOrPrefabStage<Actor>();
        if (!actor)
            return;
        var actorDirector = actor.actionPlayableDirector;
        var targetDirector = config.targetDirector;
        // Create a temporary timeline when the component is selected
        CreateTemporaryTimeline(targetDirector, actorDirector, actor);
        EditorApplication.update -= OnEditorUpdate;
        EditorApplication.update += OnEditorUpdate;
    }

    void OnDisable()
    {
        EditorApplication.update -= OnEditorUpdate;
    }

    private void CreateTemporaryTimeline(BasePlayableDirector targetDirector, BasePlayableDirector actorDirector, Actor actor)
    {
        if (Application.isPlaying)
            return;
        CleanupTemporaryTimeline();
        InteractObject obj = (InteractObject)target;
        // Make sure we have valid PlayableDirectors
        if (targetDirector == null || actorDirector == null)
            return;

        // Get original timelines from both directors
        TimelineAsset targetTimeline = obj.targetAction.TimelineAsset;
        TimelineAsset actorTimeline = obj.actorAction.TimelineAsset;

        if (targetTimeline == null || actorTimeline == null)
            return;

        // Create a temporary timeline asset
        tempTimelineAsset = ScriptableObject.CreateInstance<TimelineAsset>();
        tempTimelineAsset.name = "TempInteractionTimeline";

        // Create tracks to reference the original timelines
        var firstTrack = tempTimelineAsset.CreateTrack<ControlTrack>("Target director");
        var secondTrack = tempTimelineAsset.CreateTrack<ControlTrack>("Actor director");

        // Calculate the total duration based on both timelines
        var totalDuration = Mathf.Max(
            (float)targetTimeline.duration,
            (float)actorTimeline.duration
        );

        // Create clips that will control the original directors
        var firstClip = firstTrack.CreateClip<ControlPlayableAsset>();
        firstClip.start = 0;
        firstClip.duration = targetTimeline.duration;

        var secondClip = secondTrack.CreateClip<ControlPlayableAsset>();
        secondClip.start = 0;
        secondClip.duration = actorTimeline.duration;

        // Create a preview director in the editor
        GameObject previewObj = new GameObject("PreviewDirector");
        previewObj.hideFlags = HideFlags.HideAndDontSave;
        previewDirector = previewObj.AddComponent<PlayableDirector>();
        previewDirector.playableAsset = tempTimelineAsset;
        EditorApplication.delayCall -= TimelinePreviewDirector;
        EditorApplication.delayCall += TimelinePreviewDirector;
        this.targetDirector = targetDirector;
        this.actorDirector = actorDirector;
        obj.ApplyInteract(actor);
        targetDirector.PlayAction(obj.targetAction);
        targetDirector.playableDirector.Pause();
        actorDirector.PlayAction(obj.actorAction);
        actorDirector.playableDirector.Pause();
    }

    void TimelinePreviewDirector()
    {
        if (previewDirector == null)
            return;
        var window = TimelineEditor.GetOrCreateWindow();
        window.SetTimeline(previewDirector);
    }

    private void CleanupTemporaryTimeline()
    {
        if (previewDirector != null)
        {
            if (Application.isPlaying)
                Destroy(previewDirector.gameObject);
            else
                DestroyImmediate(previewDirector.gameObject);
            previewDirector = null;
        }
        if (tempTimelineAsset != null)
        {
            DestroyImmediate(tempTimelineAsset);
            tempTimelineAsset = null;
        }
    }

    // void OnDestroy()
    // {
    //     CleanupTemporaryTimeline();
    // }

    void OnEditorUpdate()
    {
        if (previewDirector == null)
            return;
        targetDirector.playableDirector.time = previewDirector.time;
        targetDirector.playableDirector.Evaluate();
        actorDirector.playableDirector.time = previewDirector.time;
        actorDirector.playableDirector.Evaluate();
    }

    public override void OnInspectorGUI()
    {
        InteractObject obj = (InteractObject)target;
        EditorGUI.BeginChangeCheck();
        // Draw default inspector
        base.OnInspectorGUI();
        if (EditorGUI.EndChangeCheck())
        {
            Actor actor = SceneObjectTool.GetComponentInSceneOrPrefabStage<Actor>();
            if (!actor)
                return;
            var actorDirector = actor.actionPlayableDirector;
            var targetDirector = obj.targetDirector;
            // Recreate the timeline when properties change
            CreateTemporaryTimeline(targetDirector, actorDirector, actor);
        }
    }

}