using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    private ParticleSystem ps;
    public float sizeMultiplier;
    public float lifeTimeMultiplier;
    public float shapeRadius;
    public float shapeAngle;
    public bool emission;

    // Cooking System에서 아웃풋을 생성할 때 Flavor 갯수를 넘겨받는다
    // Pan Attack에서 Hit Roll 때 Flavor 갯수를 넘겨받는다
    // 한 곳에서 깔끔하게 갯수를 넘겨 받도록 수정해야 함
    [Header("Passed by PanAttack / CookingSystem")]
    public int numberOfRolls;

    private void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        var psMain = ps.main;
        var psShape = ps.shape;
        var psEmission = ps.emission;
        psEmission.enabled = true;

        if (numberOfRolls == 0)
        {
            psEmission.enabled = false;
        }
        if (numberOfRolls == 1)
        {
            psMain.startLifetimeMultiplier = 0;
            psMain.startSizeMultiplier = -.3f;

            psShape.radius = .6f;
            psShape.angle = 2;
        }
        if (numberOfRolls == 2)
        {
            psMain.startLifetimeMultiplier = .7f;
            psMain.startSizeMultiplier = .3f;

            psShape.radius = .8f;
            psShape.angle = 35;
        }
        if (numberOfRolls == 3)
        {
            psMain.startLifetimeMultiplier = 1;
            psMain.startSizeMultiplier = .6f;

            psShape.radius = 1;
            psShape.angle = 35;
        }
        //psMain.startLifetimeMultiplier = lifeTimeMultiplier;
        //psMain.startSizeMultiplier = sizeMultiplier;

        //psShape.radius = shapeRadius;
        //psShape.angle = shapeAngle;
    }

    public void DestroyParticle()
    {
        Destroy(gameObject);
    }
}
