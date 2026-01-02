#if NODECANVAS
using NodeCanvas.Framework;
using ParadoxNotion.Design;

[Name("Play Action Timeline")]
[Category("ActorAI")]
[Description("播放一个Action，并等待其完成")]
public class PlayActionTimeline : BaseActorActionTask
{
    [RequiredField]
    public BBParameter<ActionTimelineAsset> action;

    protected override string info
    {
        get
        {
            if (action.isNone || action.value == null)
                return "Play Action [None]";
            return $"Play '{action.value.name}'";
        }
    }

    protected override void OnExecute()
    {
        if (action.isNone || action.value == null)
        {
            EndAction(false);
            return;
        }
        actor.logicInput.InputForceAction(InputType.ForceActionAI, action.value, OnActionDone);
    }

    private void OnActionDone()
    {
        EndAction(true);
    }

}
#endif