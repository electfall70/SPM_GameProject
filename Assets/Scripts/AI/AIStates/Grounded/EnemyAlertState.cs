//Author: Rickard Lindgren
//SecondaryAuthor: Pol Lozano Llorens
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyState/AlertState")]
public class EnemyAlertState : EnemyState
{
    // Attributes
    [SerializeField] private float chaseDistance;
    [SerializeField] private float patrolDistance;
    [SerializeField] private float alertTime;

    private float alertTimer;

    // Methods
    public override void Enter()
    {
        base.Enter();
        AIController.Agent.SetDestination(AIController.Player.transform.position);
        alertTimer = alertTime;
    }

    public override void HandleUpdate()
    {
        base.HandleUpdate();
        alertTimer -= Time.deltaTime;
    }
    public override void EvaluateTransitions()
    {
        base.EvaluateTransitions();
        if (CanSeePlayer() && Vector3.Distance(AIController.transform.position, AIController.Player.transform.position) < chaseDistance)
            stateMachine.Transition<EnemyChaseState>();
        //Kan ocks� vara baserad p� avst�nd fr�n startpunkten? 
        //kanske ha en ReturnState d�r den typ sprintar tillbaka till sin rutt och d�r g�r den lite lugnare igen
        if (alertTimer < 0)
            stateMachine.Transition<EnemyPatrolState>();
        if (AIController.HealthComponent.IsStunned)
            stateMachine.Transition<EnemyStunState>();
    }
}
