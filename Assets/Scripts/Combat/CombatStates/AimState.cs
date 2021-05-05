//Author: Rickard Lindgren
using UnityEngine;

[CreateAssetMenu(menuName = "CombatState/Aiming")]
public class AimState : CombatState
{   
    public override void Enter()
    {
        Player.Animator.SetBool(isAimingBoolHash, true);
        Player.SetCrosshair(true);
    }

    public override void HandleUpdate()
    {
        base.HandleUpdate();
        //Maybe zoom in camera
    }

    public override void EvaluateTransitions()
    {
        if (!Player.AimInput)
            stateMachine.Transition<IdleState>();
        if (Player.AttackInput)    
            stateMachine.Transition<ShootState>();       
    }

    public override void Exit()
    {
        Player.Animator.SetBool(isAimingBoolHash, false);
        Player.SetCrosshair(false);
    }
}
