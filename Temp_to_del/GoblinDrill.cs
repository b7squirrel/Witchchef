using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinDrill : MonoBehaviour
{
    public Transform drillPoint;
    Vector2 _drillPoint;
    public Vector2 drillSize;
    public LayerMask groundLayer;
    public LayerMask enemyLayer;

    public GameObject debrisParticleEffect;

    [Header("Debug")]
    public GameObject debugDot;

    private void Update()
    {
        _drillPoint = drillPoint.position;

        for (int i = (int)-drillSize.x; i <= drillSize.x; i++)
        {
            for (int j = (int)-drillSize.y; j <= (int)drillSize.y; j++)
            {
                Vector3 _cellPosition =
                    new Vector3(_drillPoint.x + i, _drillPoint.y + j, 0);

                //Instantiate(debugDot, _cellPosition, Quaternion.identity);

                Collider2D _hitground = Physics2D.OverlapCircle(_cellPosition, .02f, groundLayer);
                if (_hitground != null)
                {
                    _hitground.GetComponent<Tiles>().RemoveTile(_cellPosition);
                    GenerateDebris(_cellPosition);
                }
            }
        }
    }

    
    private void GenerateDebris(Vector3 _DebrisPoint)
    {
        Instantiate(debrisParticleEffect, _DebrisPoint, Quaternion.identity);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, .5f);
        Gizmos.DrawCube(_drillPoint, drillSize);
    }
}
