using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public static Transform currentCheckPoint;


    /*DEN BORDE INTE AKTIVERAS S� H�R, SPELAREN BORDE G� FRAM OCH TRYCKA P� EN KNAPP*/
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            currentCheckPoint = this.transform;
            //call CheckPointEvent. enemies respawns and player resets
        }
    }
}
