using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MovementController : MonoBehaviour
{
    public enum characterState 
    {
        onGround,
        climbing,
        airborne
    }

    [Header("Config parameters")]
    [Range(0,70)]
    public int slopeLimit = 45;
    private float dotSlopeLimit;
    [Range(0f, 1f)]
    public float stepOffset = 0.5f;
    [Range(0.25f, 1f)]
    public float radious;

    public float stepRayDist;
    public float wallRayDist;

    [Header("Character statics")]
    public characterState state = characterState.onGround;

    public Transform myTransform;
    public Rigidbody targetRigidbody;
    public Collider myCollider;

    public TranformPhysix transformPhysix;
    public RelativeInput relaiveInput;
    public EnvironmentDetector environmentDetector;
    public Transform groundFitter;
    
    private float alignMent = 0;
    public int rotationAmount = 1080;
    public float speed = 5f;
    public float jumpPower = 7.5f;
    public float groundFitterDistance;
    public float airBorneSensitivity;
    public LayerMask mask;
    private Coroutine jumping;
    private bool busy = false;
    

    // Start is called before the first frame update
    void Start()
    {
        stepRayDist = stepOffset / Mathf.Tan(Mathf.Deg2Rad * slopeLimit);
        wallRayDist = 1.7f/ Mathf.Tan(Mathf.Deg2Rad * slopeLimit);
        dotSlopeLimit = -1 * slopeLimit / 90f;
    }

    
    //private void FixedUpdate()
    //{
    //    Vector3 newVelocity = myTransform.forward * InputReader.weightJoy1 * speed;
    //    targetRigidbody.velocity = new Vector3(newVelocity.x, environmentDetector.onGround? 0f : targetRigidbody.velocity.y, newVelocity.z);
    //}

    // Update is called once per frame

    public void Alignement() 
    {
        alignMent = Vector3.Dot(myTransform.forward, relaiveInput.relativeInput);
        float sign = Vector3.Dot(myTransform.right, relaiveInput.relativeInput);

        if (alignMent <= 1f)
        {
            //Debug.Log("Alignement");
            myTransform.Rotate(Vector3.up, rotationAmount * Time.deltaTime * Mathf.Sign(sign) * (Mathf.Abs(alignMent-1f)/2f));
        }
    }
    void FixedUpdate()
    {
        Alignement();
        OnGroundDesplacement();
        AirborneBehaviour();
        Jump();
    }


    public void OnGroundDesplacement() 
    {
        if (state != characterState.onGround) return;

        float value = Vector3.Dot(transform.forward, environmentDetector.groundNormal);
        //Debug.Log("dot: " + Math.Round(value, 2));
        if (environmentDetector.onWalls[0] || environmentDetector.onWalls[1] || environmentDetector.onWalls[7]) return;
        //Vector3 fitterRayOrigin = transform.position + (value < 0 ? 1f + value : 1f) * transform.forward * speed * InputReader.weightJoy1 * Time.fixedDeltaTime + Vector3.up *(groundFitter.position.y - transform.position.y) ;
        Vector3 fitterRayOrigin = transform.position + (1 - Mathf.Abs(value)) * transform.forward * speed * InputReader.weightJoy1 * Time.fixedDeltaTime + Vector3.up * (groundFitter.position.y - transform.position.y);
        Ray fitterRay = new Ray(fitterRayOrigin, Vector3.down);

        RaycastHit hit;

        Vector3 newPosition = transform.position;

        if (Physics.Raycast(fitterRay, out hit, groundFitterDistance))
        {
            newPosition = hit.point;
        }
        else 
        {
            SwitchState(characterState.airborne);
            targetRigidbody.velocity = transform.forward * speed * InputReader.weightJoy1;
        }
        float val = Mathf.Clamp(newPosition.y - transform.position.y, -stepOffset - 0.1f, stepOffset + 0.1f);
        float lerpValue = 1 - Mathf.Abs(val / (stepOffset + 0.1f));
        transform.position = Vector3.Lerp(transform.position, newPosition, lerpValue * lerpValue);
    }

    private void Jump()
    {
        if (state == characterState.onGround) 
        {
            if (InputReader.aButton)
            {
                SwitchState(characterState.airborne);
                targetRigidbody.velocity = transformPhysix.velocity/2f +  jumpPower * Vector3.Lerp(Vector3.up, myTransform.forward ,InputReader.weightJoy1 * 0.3f).normalized;
            }           
        }        
    }

    public void AirborneBehaviour() 
    {
        if (state != characterState.airborne) return;

        if (!busy) 
        {
            targetRigidbody.velocity += myTransform.forward * InputReader.weightJoy1 * Time.fixedDeltaTime * airBorneSensitivity;

            if (environmentDetector.onGround) 
            {
                SwitchState(characterState.onGround);
            }
        }
    }

    public void SwitchState(characterState newState) 
    {
        if (newState == state) return;
        state = newState;
        switch ((int)state) 
        {
            //onGround
            case 0:
                myCollider.enabled = false;
                targetRigidbody.isKinematic = true;
                break;

            //climbing
            case 1:
                myCollider.enabled = false;
                targetRigidbody.isKinematic = true;
                break;

            //airborne
            case 2:                
                myCollider.enabled = true;
                targetRigidbody.isKinematic = false;
                StartCoroutine(DisableOnGround());
                break;
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

    IEnumerator DisableOnGround() 
    {
        busy = true;
        yield return new WaitForSeconds(0.2f);
        busy = false;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(groundFitter.position, groundFitter.position + Vector3.down * groundFitterDistance);
    }
}
