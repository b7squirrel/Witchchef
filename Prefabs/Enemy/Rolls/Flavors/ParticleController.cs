using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ϴ� numberOfRoll�� 1�� �صξ���. ���߿� Merge�� �����ϸ� �� �� ���� �޾Ƽ� �׿� �´� ��ƼŬ�� ����ϵ��� ����.
/// </summary>
public class ParticleController : MonoBehaviour
{
    private ParticleSystem ps;
    public float sizeMultiplier;
    public float lifeTimeMultiplier;
    public float shapeRadius;
    public float shapeAngle;
    public bool emission;
    
    public bool isFollowing;
    Transform _rollToFollow;

    private void Start()
    {
        ps = GetComponent<ParticleSystem>();

        var psMain = ps.main;
        var psShape = ps.shape;
        var psEmission = ps.emission;
        psEmission.enabled = true;
        
        psMain.startLifetimeMultiplier = 0;
        psMain.startSizeMultiplier = .1f;

        psShape.radius = .01f;
        psShape.angle = 15;
    }

    private void Update()
    {
        if (isFollowing)
        {
            FollowSlot();
        }
    }

    public void SetSlotToFollow(PanSlot _slot)
    {
        _rollToFollow = _slot.GetComponent<PanSlot>().GetRoll();
        isFollowing = true;
    }

    /// <summary>
    /// ���� ���̳� ������ �ε��ļ� �ı��Ǿ� ������ ���� �� �̻� Follow�� �������� ���ϵ��� ������ �ı�
    /// </summary>
    void FollowSlot()
    {
        if (_rollToFollow != null)
        {
            transform.position = _rollToFollow.position;
        }
        else
        {
            DestroyParticle();
        }
    }

    public void DestroyParticle()
    {
        isFollowing = false;
        Destroy(gameObject);
    }
}
