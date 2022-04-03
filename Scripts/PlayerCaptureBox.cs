using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCaptureBox : MonoBehaviour
{
    [HideInInspector] public BoxCollider2D boxCol;

    private void Awake()
    {
        boxCol = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ProjectileEnemy"))
        {
            if (PlayerPanAttack.instance.CaptureTimer > 0)
            {
                collision.GetComponent<EnemyProjectile>().isCaptured = true;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (boxCol == null)
            return;

        if (PlayerPanAttack.instance.CaptureTimer > 0)
        {
            Color color = new Color(1, 0, 0, .3f);
            Gizmos.color = color;
            Gizmos.DrawCube(boxCol.bounds.center, boxCol.bounds.size);
        }
    }
}
