using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackBox : MonoBehaviour
{
    private BoxCollider2D boxCol;
    private Color parryColor;
    private bool isParrying;

    private PlayerAttack playerAttack;


    private void Awake()
    {
        boxCol = GetComponent<BoxCollider2D>();
        parryColor = new Color(1, 0, 1, 0.5f);

        playerAttack = GetComponentInParent<PlayerAttack>();
    }

    private void Update()
    {
        if (playerAttack.parryTimer > 0f)
        {
            playerAttack.parryTimer -= Time.deltaTime;
        }
        else
        {
            playerAttack.parryTimer = 0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(playerAttack.parryTimer > 0f)
        {
            if(collision.CompareTag("ProjectileEnemy"))
            {
                var clone = collision.GetComponent<EnemyProjectile>();
                clone.GetComponent<EnemyProjectile>().contactPoint = new Vector2(collision.transform.position.x, collision.transform.position.y);
                clone.GetComponent<EnemyProjectile>().isParried = true;
            }
        }
        else
        {
            return;
        }
    }

    private void OnDrawGizmos()
    {
        if (boxCol == null)
            return;
        Gizmos.color = parryColor;
        Gizmos.DrawCube(boxCol.bounds.center, boxCol.bounds.size);
    }

}
