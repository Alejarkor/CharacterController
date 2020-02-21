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
    public Vector3 groundNormal;

    [Header("Wall")]
    public float wallDetectorHeight;
    public float wallDetectorDistance;
    //public LayerMask maskWall;
    public bool nearWall;

    public bool OnFrontWall;
    public bool OnBackWall;
    public bool OnRightWall;
    public bool OnLeftWall;

    [Header("Indicators")] 
    public GameObject floor;
    public GameObject front;
    public GameObject back;
    public GameObject right;
    public GameObject left;

    private MeshRenderer floorRenderer;
    private MeshRenderer frontRenderer;
    private MeshRenderer backRenderer;
    private MeshRenderer rightRenderer;
    private MeshRenderer leftRenderer;

    private Material floorMat;
    private Material frontMat;
    private Material backMat;
    private Material rightMat;
    private Material leftMat;

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckOnGround();
        //CheckNearWall();  
        CheckWalls();
    }

    public void Update()
    {
        UpdateIndicators();
    }

    public void UpdateIndicators()
    {
        if (onGround)
        {
            //floorMat;
            floorRenderer.material.SetColor("_Color", Color.red);
        }
        else
        {
            floorRenderer.material.SetColor("_Color", Color.green);
        }
        
        if (OnFrontWall)
        {
            //floorMat;
            frontRenderer.material.SetColor("_Color", Color.red);
        }
        else
        {
            frontRenderer.material.SetColor("_Color", Color.green);
        }
        
        if (OnBackWall)
        {
            //floorMat;
            backRenderer.material.SetColor("_Color", Color.red);
        }
        else
        {
            backRenderer.material.SetColor("_Color", Color.green);
        }
        
        if (OnRightWall)
        {
            //floorMat;
            rightRenderer.material.SetColor("_Color", Color.red);
        }
        else
        {
            rightRenderer.material.SetColor("_Color", Color.green);
        }
        
        if (OnLeftWall)
        {
            //floorMat;
            leftRenderer.material.SetColor("_Color", Color.red);
        }
        else
        {
            leftRenderer.material.SetColor("_Color", Color.green);
        }
        
    }

    void Start()
    {
        floorRenderer = floor.GetComponent<MeshRenderer>();
        frontRenderer = front.GetComponent<MeshRenderer>();
        backRenderer = back.GetComponent<MeshRenderer>();
        rightRenderer = right.GetComponent<MeshRenderer>();
        leftRenderer = left.GetComponent<MeshRenderer>();
        
        floorMat = floorRenderer.material;
        frontMat = frontRenderer.material;
        backMat = backRenderer.material;
        rightMat = rightRenderer.material;
        leftMat = leftRenderer.material;
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

        for (int i = -1; i < 2; i+=2)
        { 
            for (int j= -1; j < 2; j+=2)
            {
                onGround = Physics.Raycast(myTransform.position + new Vector3(0.25f*i, 0f, 0.25f*j) + Vector3.up * groundDetectorHeight, Vector3.down, groundDetectorDistance);
                if (onGround) return;
            }
        }
        
    }

    private void CheckWalls()
    {
        OnFrontWall =  Physics.Raycast(myTransform.position + Vector3.up * wallDetectorHeight, myTransform.forward, wallDetectorDistance);
        OnBackWall =  Physics.Raycast(myTransform.position + Vector3.up * wallDetectorHeight, -myTransform.forward, wallDetectorDistance);
        OnRightWall =  Physics.Raycast(myTransform.position + Vector3.up * wallDetectorHeight, myTransform.right, wallDetectorDistance);
        OnLeftWall =  Physics.Raycast(myTransform.position + Vector3.up * wallDetectorHeight, -myTransform.right, wallDetectorDistance);
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
