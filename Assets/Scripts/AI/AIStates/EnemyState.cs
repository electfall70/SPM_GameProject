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
        //l�gg in en HitState som g�r typ knockback?
        if(AIController.HealthComponent.CurrentHealth <= 0) { stateMachine.Transition<EnemyDeadState>(); }
    }

    protected bool CanSeePlayer()
    {
        //TODO Fix better line of sight
        return !Physics.Linecast(aiController.transform.position, aiController.Player.transform.position, aiController.VisionMask);
    }

    protected float DistanceToPlayer()
    {
        return Vector3.Distance(AIController.transform.position, aiController.Player.transform.position);
    }

}
