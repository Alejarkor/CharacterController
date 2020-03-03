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
    public float wallFitterDistance;
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
        if (state==characterState.climbing) return;
        alignMent = Vector3.Dot(myTransform.forward, relaiveInput.relativeHorizontalInput);
        float sign = Vector3.Dot(myTransform.right, relaiveInput.relativeHorizontalInput);

        if (alignMent <= 1f)
        {
            //Debug.Log("Alignement");
            myTransform.Rotate(Vector3.up, rotationAmount * Time.deltaTime * Mathf.Sign(sign) * (Mathf.Abs(alignMent-1f)/2f));
        }
    }
    void FixedUpdate()
    {
        Alignement(); //

        OnGroundBehavior();

        AirborneBehaviour();

        ClimbingBehavior();

        Jump();

        AbleToStartClimbing();

    }

    public void ClimbingBehavior() 
    {
        if (state != characterState.climbing) return;
        if (busy) return;
        AbleToStopClimbing();
        
        
        Vector3 fitterRayOrigin = transform.position + Vector3.up * 0.85f - 0.5f * transform.forward + relaiveInput.relativeVerticalInput * speed * InputReader.weightJoy1 * Time.fixedDeltaTime;

        Ray fitterRay = new Ray(fitterRayOrigin, transform.forward);

        RaycastHit hit;

        Vector3 newPosition = transform.position;



        if (Physics.Raycast(fitterRay, out hit, wallFitterDistance))
        {
            newPosition = hit.point - Vector3.up * 0.85f + 0.5f * Vector3.ProjectOnPlane(environmentDetector.downWallRayHit.normal,Vector3.up);
        }
        else 
        {
            SwitchState(characterState.airborne);
            //targetRigidbody.velocity = transform.forward * speed * InputReader.weightJoy1;
        }
        //float val = Mathf.Clamp(newPosition.y - transform.position.y, -stepOffset - 0.1f, stepOffset + 0.1f);
        float lerpValue = 0.5f;
        transform.position = Vector3.Lerp(transform.position, newPosition, lerpValue * lerpValue);
        
        
    }

    public void AbleToStartClimbing() 
    {
        if (busy) return;
        if (InputReader.xButton)         {
           
            if (environmentDetector.onWalls[0] && environmentDetector.onSteps[0])

            {
                Debug.DrawLine(environmentDetector.upWallRayHit.point, environmentDetector.upWallRayHit.point + environmentDetector.upWallRayHit.normal);
                SwitchState(characterState.climbing);
                StartCoroutine(StartClimbing());
            }
        }
    }

    public void AbleToStopClimbing() 
    {
        if (environmentDetector.onGround)
        {
            SwitchState(characterState.onGround); 
        }

        if (InputReader.bButton) 
        {
            SwitchState(characterState.airborne);            
        }
    }

    IEnumerator StartClimbing() 
    {
        busy = true;
        Vector3 climbForwardDirection = Vector3.ProjectOnPlane( -(environmentDetector.upWallRayHit.normal + environmentDetector.downWallRayHit.normal) * 0.5f, Vector3.up);
        Vector3 climbStartPosition = environmentDetector.upWallRayHit.point + (environmentDetector.downWallRayHit.point - environmentDetector.upWallRayHit.point).normalized * 1.7f + 0.5f * environmentDetector.downWallRayHit.normal;
        float transitionTime = 0.5f;
        float progress = 0f;
        float smoothnes = 0.05f;
        float step = 0.05f / transitionTime;

        while (progress<=1) 
        {
            progress += step;
            myTransform.forward = Vector3.Lerp(myTransform.forward, climbForwardDirection, progress);
            myTransform.position = Vector3.Lerp(myTransform.position, climbStartPosition, progress);
            yield return new WaitForSeconds(smoothnes);
        }
        busy = false;
    }

    IEnumerator UpStopClimbing(Vector3 postion)
    {
        busy = true;
        yield return null;

        busy = false;
    }

    IEnumerator DownStopClimbing()
    {
        busy = true;
        yield return null;

        busy = false;
    }

    public void OnGroundBehavior() 
    {
        if (state != characterState.onGround) return;
        if (busy) return;

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
                targetRigidbody.constraints = RigidbodyConstraints.None;
                break;



            //airborne

            case 2:                

                myCollider.enabled = true;
                targetRigidbody.isKinematic = false;
                targetRigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
                StartCoroutine(TurnBusy());
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

    IEnumerator TurnBusy() 
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
