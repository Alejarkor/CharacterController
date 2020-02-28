using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelativeInput : MonoBehaviour
{
    public Transform camera;


    public Transform target;
    public float relativeAngle;
    public Vector3 relativeHorizontalInput;
    public Vector3 relativeVerticalInput;
    /// <summary>
    /// Absolute value angle between target forward and projected camera forward.
    /// </summary>
    private float angleABS;

    private float frontSector;
    private float rightSector;
    private Vector3 camProjected;
    private Vector3 targetProjected;


    [Header("Indicators")]
    public Transform arrowHorizontal;
    public Transform arrowVertical;
    private MeshRenderer mrArrowHorizontal;
    private MeshRenderer mrArrowVertical;

    public void Start()
    {
        mrArrowHorizontal = arrowHorizontal.GetComponent<MeshRenderer>();
        mrArrowVertical = arrowVertical.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        camProjected = Vector3.ProjectOnPlane(camera.forward, Vector3.up).normalized;
        
        
        targetProjected = Vector3.ProjectOnPlane(target.forward, Vector3.up).normalized; 
        
        
        float angleABS = Vector3.Angle(target.forward, camProjected);

        frontSector = Vector3.Dot(target.forward, camProjected);
        rightSector = Vector3.Dot(target.right, camProjected);

        if (rightSector>=0)
        {
            relativeAngle = angleABS;
        }
        else
        {
            relativeAngle = 360 - angleABS;
        }

        relativeVerticalInput = !float.IsNaN(InputReader.joy1Angle)? Quaternion.AngleAxis(InputReader.joy1Angle, -target.forward) * Vector3.up:relativeVerticalInput;
        
        relativeHorizontalInput = !float.IsNaN(InputReader.joy1Angle)? Quaternion.Euler(0, InputReader.joy1Angle, 0) * camProjected : relativeHorizontalInput;
        //relativeVerticalInput = !float.IsNaN(InputReader.joy1Angle)? Quaternion.Euler(0, InputReader.joy1Angle, 0) * camProjected : relativeHorizontalInput;

        DebugInputArrow();
    }

    public void DebugInputArrow() 
    {
        arrowHorizontal.forward = relativeHorizontalInput.normalized;
        arrowHorizontal.localScale = Vector3.one * InputReader.weightJoy1*2f;
        mrArrowHorizontal.material.SetColor("_Color", Color.Lerp(Color.cyan, Color.red, InputReader.weightJoy1));

        arrowVertical.forward = relativeVerticalInput;
        arrowVertical.localScale = Vector3.one*InputReader.weightJoy1 * 2f;
        mrArrowVertical.material.SetColor("_Color", Color.Lerp(Color.cyan, Color.red, InputReader.weightJoy1));
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        
        Gizmos.DrawLine(target.position, target.position + camProjected * 2f);
        
        Gizmos.color = Color.green;
        
        Gizmos.DrawLine(target.position, target.position + relativeHorizontalInput * 2f );
    }
}
