[RepeatOnTrigger]
public class MoveTowardsPlayer : ModAI {
    public override bool? AttackTrigger()
    {
        throw new System.NotImplementedException();
    }

    public override void Movement()
    {
        aiController.MoveToPosition(player.transform.position);
    }

    public override void OnLostPlayer()
    {
        throw new System.NotImplementedException();
    }

    public override void OnTriggered()
    {
        aiController.MoveToPosition(player.transform.position);
    }
}
