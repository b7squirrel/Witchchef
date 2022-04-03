using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoulAttackBox : MonoBehaviour
{
    private GoulFighter goulFighter;

    private void Awake()
    {
        goulFighter = GetComponentInParent<GoulFighter>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("AttackBoxPlayer"))
        {
            goulFighter.isParried = true;
        }
        else
        {
            if(collision.CompareTag("HurtBoxPlayer") && !goulFighter.isParried)
            {
                PlayerHealthController.instance.isDead = true;
            }
        }
    }
}
