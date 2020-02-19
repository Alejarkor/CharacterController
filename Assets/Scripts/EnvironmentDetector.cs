using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentDetector : MonoBehaviour
{
    public Transform myTransform;

    [Header("Ground")]
    public float groundDetectorHeight;
    public float groundDetectorDistance;
    //public LayerMask maskGround;
    public bool onGround;

    [Header("Wall")]
    public float wallDetectorHeight;
    public float wallDetectorDistance;
    //public LayerMask maskWall;
    public bool nearWall;

   
    // Update is called once per frame
    void FixedUpdate()
    {
        CheckOnGround();
        CheckNearWall();       
    }

    private void CheckOnGround()
    {
        onGround = Physics.Raycast(myTransform.position + Vector3.up * groundDetectorHeight, Vector3.down, groundDetectorDistance);
        if (onGround) return;

        for (int i = -1; i < 2; i+=2)
        { 
            for (int j= -1; j < 2; j+=2)
            {
                onGround = Physics.Raycast(myTransform.position + new Vector3(0.25f*i, 0f, 0.25f*j) + Vector3.up * groundDetectorHeight, Vector3.down, groundDetectorDistance);
                if (onGround) return;
            }
        }
        
    }

    private void CheckNearWall()
    {
        nearWall = Physics.Raycast(myTransform.position + Vector3.up * wallDetectorHeight, myTransform.forward, wallDetectorDistance);
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

        if (nearWall)
        {
            Gizmos.color = Color.red;
        }
        else
        {
            Gizmos.color = Color.white;
        }
        Gizmos.DrawLine(myTransform.position + Vector3.up * wallDetectorHeight, myTransform.position + Vector3.up * wallDetectorHeight + myTransform.forward * wallDetectorDistance);
    }
}
