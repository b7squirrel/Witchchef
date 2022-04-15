using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanMove : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            PanManager.instance.FlipRoll();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            //PanManager.instance.SwitchRolls();
        }
    }
}
