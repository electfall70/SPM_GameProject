//Author: Rickard Lindgren
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public static Checkpoint currentCheckPoint;
    [SerializeField] private string UItext;
    [SerializeField] private List<int> scenesOnThisCheckpoint = new List<int>();

    public List<int> ScenesOnCheckpoint { get => scenesOnThisCheckpoint; }

    /*DEN BORDE INTE AKTIVERAS S� H�R, SPELAREN BORDE G� FRAM OCH INTERAGERA*/
    private void OnTriggerEnter/*STAY!?*/(Collider other)
    {
        if (other.CompareTag("Player") && this!=currentCheckPoint)
        {
            Debug.Log("player");
            ActivateCheckpoint(other.gameObject);
        }
    }

    private void ActivateCheckpoint(GameObject player)
    {
        EventHandler<UITriggerEvent>.FireEvent(new UITriggerEvent(UItext));
        Debug.Log("ActivateCheckpoint");
        currentCheckPoint = this;
        player.GetComponent<HealthComponent>().ResetHealth();
    }
    
}

