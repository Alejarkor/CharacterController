using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelativeInput : MonoBehaviour
{
    public Transform camera;


    public Transform target;
    public float relativeAngle;
    public Vector3 relativeInput;
    /// <summary>
    /// Absolute value angle between target forward and projected camera forward.
    /// </summary>
    private float angleABS;

    private float frontSector;
    private float rightSector;
    private Vector3 camProjected;
    private Vector3 targetProjected;


    [Header("Indicators")]
    public Transform arrow;
    private MeshRenderer mrArrow;

    public void Start()
    {
        mrArrow = arrow.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        camProjected = Vector3.ProjectOnPlane(camera.forward, Vector3.up).normalized;
        
        //TODO: Probably target's transform never rotate so it could be used target.forward insted of targetProjected
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

        relativeInput = !float.IsNaN(InputReader.joy1Angle)? Quaternion.Euler(0, InputReader.joy1Angle, 0) * camProjected : relativeInput;

        DebugInputArrow();
    }

    public void DebugInputArrow() 
    {
        arrow.forward = relativeInput.normalized;
        arrow.localScale = Vector3.one * InputReader.weightJoy1*2f;
        mrArrow.material.SetColor("_Color", Color.Lerp(Color.cyan, Color.red, InputReader.weightJoy1));
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        
        Gizmos.DrawLine(target.position, target.position + camProjected * 2f);
        
        Gizmos.color = Color.green;
        
        Gizmos.DrawLine(target.position, target.position + relativeInput * 2f );
    }
}
