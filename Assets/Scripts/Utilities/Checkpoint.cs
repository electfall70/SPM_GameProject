//Author: Rickard Lindgren
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public static Checkpoint currentCheckPoint;
    [SerializeField] private string UItext;
    [SerializeField] private List<int> scenesOnThisCheckpoint = new List<int>();
    [SerializeField] private AudioData checkpointAudio;
    [SerializeField] private AudioSource audioSource;

    private bool playedMusic;
    public List<int> ScenesOnCheckpoint { get => scenesOnThisCheckpoint; }

    private void OnEnable() => EventHandler<ReloadEvent>.RegisterListener(Reload);
    private void OnDisable() => EventHandler<ReloadEvent>.UnregisterListener(Reload);

    /*DEN BORDE INTE AKTIVERAS S� H�R, SPELAREN BORDE G� FRAM OCH INTERAGERA*/
    private void OnTriggerEnter/*STAY!?*/(Collider other)
    {
        if (other.CompareTag("Player") && this!=currentCheckPoint)
        {
            //Debug.Log("player");
            ActivateCheckpoint(other.gameObject);
        }
    }

    private void ActivateCheckpoint(GameObject player)
    {
        if(playedMusic == false && checkpointAudio != null)
        {
            EventHandler<SoundEvent>.FireEvent(new SoundEvent(checkpointAudio, audioSource));
            playedMusic = true;
        }
        EventHandler<CheckPointEvent>.FireEvent(new CheckPointEvent(UItext));
        //Debug.Log("ActivateCheckpoint");
        currentCheckPoint = this;
        player.GetComponent<HealthComponent>().ResetHealth();
    }

    private void Reload(ReloadEvent eve)
    {
        //EventHandler<CheckPointEvent>.FireEvent(new CheckPointEvent(UItext));
    }
    
}

