using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public Transform myTransform;
    public Rigidbody targetRigidbody;    
    public RelativeInput relaiveInput;
    public EnvironmentDetector environmentDetector;
    public TranformPhysix tansformPhysix;
    
    private float alignMent = 0;
    public int rotationAmount = 1080;
    public int speed = 5;
    public int jumpPower = 2;
    public LayerMask mask;
    private Coroutine jumping;

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
    void FixedUpdate()
    {
        alignMent = Vector3.Dot(myTransform.forward, relaiveInput.relativeInput);
        float sign = Vector3.Dot(myTransform.right, relaiveInput.relativeInput);

        if (alignMent <= 1f)
        {
            Debug.Log("Alignement");
            myTransform.Rotate(Vector3.up, rotationAmount * Time.deltaTime * sign);
        }

        Vector3 newVelocity = transform.forward * speed * InputReader.weightJoy1 * Time.fixedDeltaTime + Vector3.up * targetRigidbody.velocity.y;
                

        if (environmentDetector.onGround)
        {
            //newVelocity.y = 0;
        }
                

        targetRigidbody.velocity = newVelocity;

        Jump();
    }

    private void Jump()
    {
        if (environmentDetector.onGround)
        {
            if (InputReader.aButton && jumping==null)
            {
                jumping = StartCoroutine(SuperJump());
            }
        }
    }

    IEnumerator SuperJump() 
    {
        int milis = 0;
        while (InputReader.aButton && milis<50) 
        {
            milis += (int)(Time.fixedDeltaTime * 50);
            yield return null;
        }
        targetRigidbody.velocity = new Vector3(targetRigidbody.velocity.x, jumpPower * (float)milis/50f, targetRigidbody.velocity.z);
        Debug.Log("JUMP!! with milis: " + milis);
        jumping = null;
    }
}
