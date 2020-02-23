using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentDetector : MonoBehaviour
{
    public Transform myTransform;
    private MovementController movementController;
    [Header("Ground")]
    public float groundDetectorHeight;
    public float groundDetectorDistance;
    //public LayerMask maskGround;
    public bool onGround;
    public Vector3 groundNormal;

    [Header("Wall")]
    public float rayWallsStepsDist = 0.4f;
    public bool[] onWalls;
    public bool[] onSteps;    

    [Header("Indicators")]
    public MeshRenderer[] mRUpIndicators;
    public MeshRenderer[] mRDownIndicators;        
    public MeshRenderer floorRenderer;
    

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckOnGround();
        //CheckNearWall();  
        CheckWalls();
    }

    public void Update()
    {
        
    }

   

    void Start()
    {
        onWalls = new bool[mRUpIndicators.Length];
        onSteps = new bool[mRDownIndicators.Length];
        movementController = myTransform.GetComponent<MovementController>();
    }

    private void CheckOnGround()
    {
        RaycastHit hit;
        Ray ray = new Ray(myTransform.position + Vector3.up * groundDetectorHeight, Vector3.down);
        onGround = Physics.Raycast(ray, out hit, groundDetectorDistance);
        if (onGround)
        {
            groundNormal = hit.normal;
            return;
        }

        for (int i = -1; i < 2; i += 2)
        {
            for (int j = -1; j < 2; j += 2)
            {
                onGround = Physics.Raycast(myTransform.position + new Vector3(0.2f * i, 0f, 0.2f * j) + Vector3.up * groundDetectorHeight, Vector3.down, groundDetectorDistance);
                if (onGround) return;
            }
        }

    }

    private void CheckWalls()
    {
        for (int i = 0;i<mRUpIndicators.Length;++i) 
        {
            Vector3 direction = Quaternion.Euler(0, i * 360/mRUpIndicators.Length, 0) * myTransform.forward;
            Ray rayWall = new Ray(myTransform.position + 1.7f * Vector3.up, direction);
            Ray rayStep = new Ray(myTransform.position + (movementController.stepOffset + 0.05f) * Vector3.up, direction);
            if (Physics.Raycast(rayWall, movementController.radious + 0.3f))
            {
                mRUpIndicators[i].material.SetColor("_Color", Color.red);                
                onWalls[i] = true;
            }
            else 
            {
                mRUpIndicators[i].material.SetColor("_Color", Color.green);
                onWalls[i] = false;
            }

            if (Physics.Raycast(rayStep, movementController.radious + 0.3f))
            {
                mRDownIndicators[i].material.SetColor("_Color", Color.red);
                onSteps[i] = true;
            }
            else
            {
                mRDownIndicators[i].material.SetColor("_Color", Color.green);
                onSteps[i] = false;
            }
        }
    }

   

    private void OnDrawGizmos()
    {
        if (onGround)
        {
            Gizmos.color = Color.red;            
        }
        else
        {
            Gizmos.color = Color.white;
        }
        Gizmos.DrawLine(myTransform.position + Vector3.up * groundDetectorHeight, myTransform.position + Vector3.up * groundDetectorHeight + Vector3.down * groundDetectorDistance);
                
    }
}
