using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class RoyalController : MonoBehaviour
{
    public Animator anim;
    public Transform targetTransform;
    public Rigidbody targetRigidbody;
    public Transform onGroundHelper;
    public LayerMask onGroundRayMask;
    public RelativeInput relaiveInput;
    public Vector3 velocity;

    private float alignMent = 0;

    public int rotationAmount = 1080;
    public int speed = 5;
    public int jumpPower = 2;
    public bool onGround = true;
    
    private void Awake()
    {
        //anim = GetComponent<Animator>();
        //myTransform = transform;
    }

    public void Update()
    {
        velocity = targetRigidbody.velocity;
        anim.SetFloat("weight", InputReader.weightJoy1);
        Quaternion rot = targetTransform.rotation * Quaternion.FromToRotation(Vector3.ProjectOnPlane(targetTransform.forward, Vector3.up) , relaiveInput.relativeInput);
        Vector3 rotationEuler = targetTransform.rotation.eulerAngles;
        rotationEuler.x = 0;
        rotationEuler.z = 0;
        //myTransform.rotation = Quaternion.Lerp(Quaternion.Euler(rotationEuler) , rot, 0.1f);

        alignMent = Vector3.Dot(targetTransform.forward, relaiveInput.relativeInput);
        float sign = Vector3.Dot(targetTransform.right, relaiveInput.relativeInput);
        
        if (alignMent<=0.99f)
        {
            Debug.Log("Alignement");
            targetTransform.Rotate(Vector3.up, rotationAmount*Time.deltaTime * sign );
        }

        /*if (anim.GetCurrentAnimatorStateInfo(0).IsName("Left Turn") || anim.GetCurrentAnimatorStateInfo(0).IsName("Right Turn") || anim.GetCurrentAnimatorStateInfo(0).IsName("Blend Tree"))
        {
            myTransform.Rotate(Vector3.up, 720f*Time.deltaTime * sign * Mathf.Clamp(InputReader.weightJoy1,0.5f,1f));
        }*/

        if (InputReader.weightJoy1>0)
        {
            targetRigidbody.position += targetTransform.forward * InputReader.weightJoy1 * speed * Time.deltaTime;
        }
        
        IsOnGround();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if (!onGround)
        {
            anim.SetFloat("verticalVelocity", targetRigidbody.velocity.y);
        }

    }

    private void OnDrawGizmos()
    {
        if (alignMent >= 0.99f)
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.red;
        }

        Gizmos.DrawSphere(targetRigidbody.position + Vector3.up *2f,0.25f);
    }

    private void IsOnGround()
    {
        if(Physics.Raycast(onGroundHelper.position, Vector3.down, 0.25f, onGroundRayMask))
        {
            onGround = true;
            anim.SetBool("onGround", true);
        }
        else
        {
            onGround = false;
            anim.SetBool("onGround", false);
        }
    }

    private void Jump()
    {
        if (onGround)
        {
            targetRigidbody.AddForce(Vector3.up * jumpPower,ForceMode.Impulse);
            anim.SetBool("jump", true);
        }
    }
}
