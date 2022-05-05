using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoulAttackBox : MonoBehaviour
{
    [SerializeField] EnemyHealth _enemyHealth; // �ڽ��� EnemyHealth �������
    [SerializeField] float parriedBufferTime;
    float parriedBufferTimeCounter;
    bool isHittingPlayer;
    bool isHittingAttackBox;

    private void Start()
    {
        parriedBufferTimeCounter = parriedBufferTime;
    }

    private void Update()
    {
        if (isHittingPlayer)
        {
            parriedBufferTimeCounter -= Time.deltaTime;
            if (parriedBufferTimeCounter <= 0)
            {
                PlayerHealthController.instance.isDead = true;
                isHittingAttackBox = false;
                isHittingPlayer = false;
            }
            else
            {
                if (isHittingAttackBox)
                {
                    parriedBufferTimeCounter = parriedBufferTime;
                    _enemyHealth.SetParriedState(true);
                    isHittingAttackBox = false;
                    isHittingPlayer = false;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("AttackBoxPlayer"))
        {
            isHittingAttackBox = true;
        }
        else
        {
            if(collision.CompareTag("HurtBoxPlayer"))  // �и��� ���� ���ÿ� hurt box�� ������ �����ϵ���
            {
                isHittingPlayer = true;
            }
        }
    }
}
