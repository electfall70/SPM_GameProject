using UnityEngine;

public abstract class EnemyState : State
{
    [SerializeField] protected float moveSpeed;
    

    private AIController aiController;
    public AIController AIController => aiController = aiController != null ? aiController : (AIController)owner;

    public override void Enter()
    {
        AIController.Agent.speed = moveSpeed;
        //Debug.Log("ENTER: " + stateMachine.CurrentState + "   " + AIController.transform.position);
    }
    public override void HandleUpdate()
    {
        AIController.Animator.SetFloat("Speed", AIController.Agent.velocity.magnitude); 
    }

    public override void EvaluateTransitions()
    {
        //VI kanske kan l�gga stun h�r men d� m�ste stunState vara b�da f�r b�da fienderna?
        //if (AIController.isStunned) stateMachine.Transition<StunState>();
        if(AIController.HealthComponent.dead) { stateMachine.Transition<EnemyDeadState>(); }
    }

    protected bool CanSeePlayer()
    {
        //TODO Fix better line of sight
        UnityEngine.Debug.Assert(aiController);
        UnityEngine.Debug.Assert(aiController.player);
        return !Physics.Linecast(aiController.transform.position, aiController.player.transform.position, aiController.visionMask);
    }
}
