using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public Roll.rollType myRollType;
    public void GetRolled()
    {
        RollSO _rollSo = RecipeRoll.instance.GetRollSo(myRollType);
        Instantiate(_rollSo.rollPrefab[0], transform.position, transform.rotation);
        Die();
    }

    void Die()
    {
        transform.parent.gameObject.SetActive(false);
    }
}
