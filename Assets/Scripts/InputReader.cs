using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class InputReader : MonoBehaviour
{
    
    public static bool xButton;
    public static bool yButton;
    public static bool aButton;
    public static bool bButton;
    
    public static bool statButton;
    public static bool optionButton;

    public static bool l1Button;
    public static bool r1Button;
    
    public static Vector2 joy1;
    public static float joy1Angle;
    public static float weightJoy1;
    
    public static Vector2 joy2;
    public static float weightJoy2;

    public static float l2Axis;
    public static float r2Axis;
    
    
    
    // Update is called once per frame
    void Update()
    {
        joy1 = new Vector2(Input.GetAxis("Joy1_x"), Input.GetAxis("Joy1_y"));

        UpdateJoyAngle(joy1, ref joy1Angle);

        weightJoy1 = Mathf.Sqrt((joy1.x * joy1.x) + (joy1.y * joy1.y));

        aButton = Input.GetButton("A_button");
    }

    private void UpdateJoyAngle(Vector2 joyValue, ref float joyAngle)
    {
        if (joyValue.x == 0f && joyValue.y == 0f)
        {
            joyAngle = float.NaN;
            return ;
        }

        float angle = Mathf.Rad2Deg * Mathf.Atan(Mathf.Abs(joyValue.y / joyValue.x));

        if (Mathf.Sign(joyValue.x)==1 && Mathf.Sign(joyValue.y)==-1)
        {
            joyAngle = 90f + angle;
        }
        else if (Mathf.Sign(joyValue.x)==-1 && Mathf.Sign(joyValue.y)==-1)
        {
            joyAngle = 270f - angle;
        }
        else if (Mathf.Sign(joy1.x)==-1 && Mathf.Sign(joyValue.y)==1)
        {
            joyAngle = 270f + angle;
        }
        else if (Mathf.Sign(joyValue.x)==+1 && Mathf.Sign(joyValue.y)== +1)
        {
            joyAngle = 90f - angle;
        }
    }
}
