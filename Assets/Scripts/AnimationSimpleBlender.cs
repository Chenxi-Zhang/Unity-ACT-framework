
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

[ExecuteInEditMode]
public class AnimationSimpleBlender : MonoBehaviour
{
    public Animator animator;

    Dictionary<AnimationClip, BlendState> assetDict = new();
    List<BlendState> orderedAsset = new();

    private PlayableGraph _graph;
    private AnimationMixerPlayable animationMixerPlayable;

    public PlayableGraph Graph
    {
        get
        {
            if (_graph.IsValid())
            {
                return _graph;
            }
            _graph = PlayableGraph.Create();
            animationMixerPlayable = AnimationMixerPlayable.Create(_graph, 0);
            var playableOutput = AnimationPlayableOutput.Create(_graph, "Animation", animator);
            playableOutput.SetSourcePlayable(animationMixerPlayable);
            return _graph;
        }
    }

    static float DEFAULT_TARGET_WEIGHT = 1f;
    static float DEFAULT_BLEND_TIME = 0.1f;

    // Track blend states of animations
    private class BlendState
    {
        private Playable playable;
        private Clamper weightClamper;
        private float currentWeight;
        private float time;

        private bool isOut;

        public bool ToRemove => isOut && currentWeight <= 0;
        public float CurrentWeight => currentWeight;
        public Playable Playable => playable;
        public bool IsOut => isOut;

        public BlendState(Playable playable)
        {
            this.playable = playable;
        }

        float GetBlendTime(float blendTime, float expectFrom, float expectTo, float realFrom)
        {
            return Mathf.Max(0, blendTime * (expectTo - realFrom) / (expectTo - expectFrom));
        }

        public void SetBlendIn(float blendTime)
        {
            blendTime = GetBlendTime(blendTime, 0, DEFAULT_TARGET_WEIGHT, currentWeight);
            var startWeight = Mathf.Max(0.001f, currentWeight);
            weightClamper = new(0, blendTime, startWeight, DEFAULT_TARGET_WEIGHT);
            isOut = false;
        }

        public void SetBlendOut(float blendTime)
        {
            blendTime = GetBlendTime(blendTime, DEFAULT_TARGET_WEIGHT, 0, currentWeight);
            weightClamper = new(0, blendTime, currentWeight, 0);
            isOut = true;
        }

        public void SetTime(float graphTime)
        {
            time = graphTime;
            currentWeight = weightClamper.Clamp(graphTime);
            if (!isOut)
            {
                playable.SetTime(graphTime);
            }
        }

        public void Update(float deltaTime)
        {
            SetTime(time + deltaTime);
        }

        public void DestroyPlayable()
        {
            if (playable.IsValid())
            {
                playable.Destroy();
            }
        }
    }

    public void SetTime(AnimationClip clip, float time)
    {
        if (assetDict.TryGetValue(clip, out var state))
        {
            state.SetTime(time);
        }
    }

    public void RemoveClip(AnimationClip clip, float blendOutTime = -1)
    {
        if (!assetDict.TryGetValue(clip, out var state))
        {
            return;
        }
        // Use default blend time if not specified
        if (blendOutTime < 0)
            blendOutTime = DEFAULT_BLEND_TIME;
        // Mark for removal with blend-out
        state.SetBlendOut(blendOutTime);
        state.SetTime(0);
        // Don't immediately destroy or remove from dictionary - we'll do that after blend completes
    }

    public void AddClip(AnimationClip clip, float blendInTime = -1)
    {
        if (blendInTime < 0)
            blendInTime = DEFAULT_BLEND_TIME;
        if (!assetDict.TryGetValue(clip, out var state))
        {
            var playable = AnimationClipPlayable.Create(Graph, clip);
            playable.SetApplyPlayableIK(true);
            state = new BlendState(playable);
            assetDict.Add(clip, state);
        }
        state.SetBlendIn(blendInTime);
        state.SetTime(0);
        BuildMixerPlayable();
    }

    void BuildMixerPlayable()
    {
        for (int i = 0; i < animationMixerPlayable.GetInputCount(); i++)
        {
            Graph.Disconnect(animationMixerPlayable, i);
        }
        DeleteClips();
        // 顺序不重要，但是需要每次遍历顺序一致
        orderedAsset.Clear();
        orderedAsset.AddRange(assetDict.Values);
        animationMixerPlayable.SetInputCount(assetDict.Count);
        var weightSum = assetDict.Values.Sum(state => state.CurrentWeight);
        for (int i = 0; i < orderedAsset.Count; i++)
        {
            var state = orderedAsset[i];
            animationMixerPlayable.ConnectInput(i, state.Playable, 0, state.CurrentWeight / weightSum);
        }
    }

    readonly List<AnimationClip> clipsToRemove = new();
    void DeleteClips()
    {
        // Don't delete element during looping
        foreach (var (clip, state) in assetDict)
        {
            if (state.ToRemove)
            {
                clipsToRemove.Add(clip);
            }
        }
        if (clipsToRemove.Count > 0)
        {
            foreach (var clip in clipsToRemove)
            {
                var state = assetDict[clip];
                assetDict.Remove(clip);
                state.DestroyPlayable();
            }
            clipsToRemove.Clear();
        }
    }

    public void DoUpdate(float deltaTime)
    {
        foreach (var state in assetDict.Values)
        {
            // Update blend-out states
            if (state.IsOut)
            {
                state.Update(deltaTime);
            }
        }
        UpdateMixerWeight();
        Graph.Evaluate();
    }

    void UpdateMixerWeight()
    {
        float weightSum = assetDict.Values.Sum(state => state.CurrentWeight);
        if (weightSum < 0.001f)
            return;
        for (int i = 0; i < orderedAsset.Count; i++)
        {
            var state = orderedAsset[i];
            var weight = state.CurrentWeight;
            animationMixerPlayable.SetInputWeight(i, weight / weightSum);
        }
    }

    void OnDestroy()
    {
        if (_graph.IsValid())
        {
            _graph.Destroy();
        }
    }

}