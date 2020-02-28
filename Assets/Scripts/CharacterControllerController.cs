using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerController : MonoBehaviour
{
    public CharacterController charController;

    public Transform myTransform;
    //public Rigidbody targetRigidbody;
    public RelativeInput relaiveInput;
    public EnvironmentDetector environmentDetector;
    public TranformPhysix tansformPhysix;

    private float alignMent = 0;
    public int rotationAmount = 1080;
    public float speed = 5;
    public int jumpPower = 2;
    public LayerMask mask;
    // Start is called before the first frame update
    void Update()
    {
        alignMent = Vector3.Dot(myTransform.forward, relaiveInput.relativeHorizontalInput);
        float sign = Vector3.Dot(myTransform.right, relaiveInput.relativeHorizontalInput);

        if (alignMent <= 1f)
        {
            Debug.Log("Alignement");
            myTransform.Rotate(Vector3.up, rotationAmount * Time.deltaTime * sign);
        }

        if (true)
        {
            

            Vector3 newPosition = myTransform.position + myTransform.forward * InputReader.weightJoy1 * speed * Time.fixedDeltaTime;
            RaycastHit hitPoint;
            Ray ray = new Ray(newPosition + Vector3.up, Vector3.down);
            if (Physics.Raycast(ray, out hitPoint, 1.5f, mask))
            {
                newPosition = hitPoint.point;

            }

            //transform.position = Vector3.Lerp(transform.position, newPosition, 0.5f);
            //targetRigidbody.velocity = (hitPoint.point - transform.position).normalized * speed * InputReader.weightJoy1;
            charController.Move((hitPoint.point - transform.position).normalized * speed * InputReader.weightJoy1);
        }



        //Jump();
    }

}
