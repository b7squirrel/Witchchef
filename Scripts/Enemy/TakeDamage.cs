using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamage : MonoBehaviour
{
    public Roll.rollType myRollType;
    public int currentHP;
    public int maxHP;

    public bool isStunned;
    public bool knockBack;
    public bool isRolling;

    public GameObject dieEffect;
    public GameObject dieBones;
    public Transform dieEffectPoint;

    [Header("Rolling")]
    public bool isCaptured;

    [Header("White Flash")]
    public Material whiteMat;
    private Material initialMat;
    public GameObject mSprite;  // 자신의 스프라이트를 끌어다 넣기
    private SpriteRenderer theSR;
    public float blinkingDuration;

    
    private void Start()
    {
        currentHP = maxHP;
        theSR = mSprite.GetComponent<SpriteRenderer>();
        initialMat = theSR.material;

    }
    private void Update()
    {
        if (isCaptured)
        {
            if(Inventory.instance.numberOfRolls < 3)
            GetRolled();
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("AttackBoxPlayer"))
        {
            if(!isStunned)
            {
                AudioManager.instance.Play("pan_hit_05");
                isStunned = true;
                knockBack = true;
            }
        }
        if (collision.CompareTag("ProjectileDeflected"))
        {
            AudioManager.instance.Play("Goul_Die_01");
            GameManager.instance.StartCameraShake(4, .5f);
            GameManager.instance.TimeStop(.2f);
            Die();
        }

        if (collision.CompareTag("Explosion"))
        {
            AudioManager.instance.Play("Goul_Die_01");
            Die();
        }

        if (collision.CompareTag("RollFlavored"))
        {
            Die();
        }

        if (collision.CompareTag("Rolling"))
        {
            isStunned = true;
        }
    }

    public void GetRolled()
    {
        AudioManager.instance.Stop("Energy_01");
        AudioManager.instance.Play("GetRolled_01");

        isStunned = false;
        isCaptured = false;

        RollSO _rollSo = RecipeRoll.instance.GetRollSo(myRollType);
        Instantiate(_rollSo.rollPrefab[0], dieEffectPoint.position, transform.rotation);
        Die();
    }
    public void Die()
    {
        Instantiate(dieBones, dieEffectPoint.position, transform.rotation);
        Instantiate(dieEffect, transform.position, transform.rotation);
        AudioManager.instance.Play("Goul_Die_01");
        AudioManager.instance.Stop("Energy_01");
        currentHP = maxHP;
        isStunned = false;
        isCaptured = false;
        Destroy(transform.parent.gameObject);
    }
    IEnumerator WhiteFlash()
    {
        theSR.material = whiteMat;
        yield return new WaitForSecondsRealtime(blinkingDuration);
        theSR.material = initialMat;
    }
}
