using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanMove : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            PanManager.instance.FlipRoll();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            PanManager.instance.SwitchRolls();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (PanManager.instance.CountRollNumber() == 0)
                return;
            PanManager.instance.ClearRoll();
            EffectsClearRoll();
        }
    }
    void EffectsClearRoll()
    {
        AudioManager.instance.Play("fire_explosion_01");
        AudioManager.instance.Play("pan_hit_03");
        GameManager.instance.StartCameraShake(8, .8f);
        GameManager.instance.TimeStop(.1f);
    }
}
