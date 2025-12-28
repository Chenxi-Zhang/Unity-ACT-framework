
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New StrafeMoveAnimation", menuName = "Tools/ActionTimline/StrafeMoveAnimation")]
public class StrafeMoveAnimation : ScriptableObject
{
    public ActionTimelineAsset idle;
    public ActionTimelineAsset forward;
    public ActionTimelineAsset backward;
    public ActionTimelineAsset left;
    public ActionTimelineAsset right;
    public float forwardToRightAngle = 60;
    public float rightToBackwardAngle = 120;
    public float backwardToLeftAngle = 240;
    public float leftToForwardAngle = 300;

    public Vector2 TurnFromRight(Vector2 input)
    {
        // 逆时针旋转90度的变换矩阵
        // [ 0 -1 ]
        // [ 1  0 ]
        return new Vector2(-input.y, input.x);
    }

    public Vector2 TurnFromBack(Vector2 input)
    {
        // 逆时针旋转180度的变换矩阵
        // [-1  0 ]
        // [ 0 -1 ]
        return new Vector2(-input.x, -input.y);
    }

    public Vector2 TurnFromLeft(Vector2 input)
    {
        // 逆时针旋转270度（或顺时针旋转90度）的变换矩阵
        // [ 0  1 ]
        // [-1  0 ]
        return new Vector2(input.y, -input.x);
    }

    public ActionTimelineAsset GetDirAnim(Vector2 inputDir, out Vector2 faceDir)
    {
        if (inputDir.sqrMagnitude < 0.01f)
        {
            faceDir = Vector2.zero;
            return idle;
        }
        var direction = -Mathf.Atan2(inputDir.y, inputDir.x) * Mathf.Rad2Deg + 90f;
        // 标准化角度到 [0, 360) 范围
        direction %= 360f;
        if (direction < 0)
            direction += 360f;

        // 根据配置的分界角度判断区间并返回对应动画
        if ((0 <= direction && direction < forwardToRightAngle) || (leftToForwardAngle <= direction && direction <= 360f))
        {
            // Forward区间：左前到前右
            faceDir = inputDir; // forward基准方向为0度
            return forward;
        }
        else if (direction >= forwardToRightAngle && direction < rightToBackwardAngle)
        {
            // Right区间：前右到右后
            faceDir = TurnFromRight(inputDir);
            return right;
        }
        else if (direction >= rightToBackwardAngle && direction < backwardToLeftAngle)
        {
            // Backward区间：右后到后左
            faceDir = TurnFromBack(inputDir);
            return backward;
        }
        else
        {
            // Left区间：后左到左前
            faceDir = TurnFromLeft(inputDir);
            return left;
        }
    }

    public bool IsPlaying(Actor actor)
    {
        var playingAction = actor.actionPlayableDirector.PlayingAction;
        return playingAction == idle ||
            playingAction == forward ||
            playingAction == backward ||
            playingAction == left ||
            playingAction == right;
    }
}