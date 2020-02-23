using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranformPhysix : MonoBehaviour
{
    public Vector3 velocity;

    public Vector3 angularVelocity;

    private Rigidbody myRig;
    private Vector3 _lastPosition;

    private Vector3 _lastRotation;
    
    // Start is called before the first frame update
    void Start()
    {
        myRig = GetComponent<Rigidbody>();
        _lastPosition = transform.position;
        _lastRotation = transform.rotation.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        velocity = (transform.position - _lastPosition) / Time.deltaTime;
        _lastPosition = transform.position;
        angularVelocity = transform.rotation.eulerAngles - _lastRotation;
        _lastRotation = transform.rotation.eulerAngles;
    }

    //private void OnCollisionEnter(Collision other)
    //{
    //    Rigidbody otherRig = other.gameObject.GetComponent<Rigidbody>();
    //    if (otherRig)
    //    {
    //        Vector3 force = myRig.mass * velocity.magnitude * velocity.normalized;
    //        other.rigidbody.AddForceAtPosition(force,other.contacts[0].point,ForceMode.Impulse);
    //    }
    //}
}
