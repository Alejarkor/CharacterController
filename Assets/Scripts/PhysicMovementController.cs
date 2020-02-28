using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicMovementController : MonoBehaviour
{
    public Transform myTransform;
    public Rigidbody targetRigidbody;    
    public RelativeInput relaiveInput;
    public EnvironmentDetector environmentDetector;

    private float alignMent = 0;
    public int rotationAmount = 1080;
    public int displacementPower = 5;
    public int jumpPower = 2;
    public LayerMask mask;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    //private void FixedUpdate()
    //{
    //    Vector3 newVelocity = myTransform.forward * InputReader.weightJoy1 * speed;
    //    targetRigidbody.velocity = new Vector3(newVelocity.x, environmentDetector.onGround? 0f : targetRigidbody.velocity.y, newVelocity.z);
    //}

    // Update is called once per frame
    void Update()
    {
        alignMent = Vector3.Dot(myTransform.forward, relaiveInput.relativeHorizontalInput);
        float sign = Vector3.Dot(myTransform.right, relaiveInput.relativeHorizontalInput);

        if (alignMent <= 0.99f)
        {
            Debug.Log("Alignement");
            myTransform.Rotate(Vector3.up, rotationAmount * Time.deltaTime * sign);
        }

        if (environmentDetector.onGround)
        {
            
            /*Vector3 newPosition = myTransform.position + myTransform.forward * InputReader.weightJoy1 * speed * Time.deltaTime;
            RaycastHit hitPoint;
            Ray ray = new Ray(newPosition + Vector3.up, Vector3.down);
            if (Physics.Raycast(ray, out hitPoint, 5f, mask))
            {
                newPosition = hitPoint.point;
                //targetRigidbody.velocity = (hitPoint.point - transform.position) * speed;
            }*/
            
            //transform.position = Vector3.Lerp(transform.position, newPosition,0.5f);
            targetRigidbody.velocity = transform.forward * InputReader.weightJoy1 * displacementPower ;
        }
        

        Jump();
    }

    private void Jump()
    {
        if (environmentDetector.onGround)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                targetRigidbody.velocity = new Vector3(targetRigidbody.velocity.x, jumpPower, targetRigidbody.velocity.z);
            }
        }
    }
}
