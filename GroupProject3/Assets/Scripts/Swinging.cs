using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swinging : MonoBehaviour
{
    [Header("Input")]
    public KeyCode swingKey = KeyCode.Mouse0;

    [Header("References")]
    public LineRenderer lr;
    public Transform hand, cam, player, orientation;
    public LayerMask whatIsGrappleable;

    [Header("Swinging")]
    private float maxSwingDistance = 25f;
    private Vector3 swingPoint;
    private SpringJoint joint;

    private Vector3 currentGrapplePosition;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(swingKey))
        {
            StartSwing();
        }
        if (Input.GetKeyUp(swingKey))
        {
            StopSwing();
        }
    }

    private void LateUpdate()
    {
        DrawRope();
    }

    void StartSwing()
    {
        RaycastHit hit;
        if(Physics.Raycast(orientation.position, orientation.forward, out hit, maxSwingDistance, whatIsGrappleable))
        {
            swingPoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = swingPoint;

            float distanceFromPoint = Vector3.Distance(player.position, swingPoint);

            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;

            joint.spring = 4.5f;
            joint.damper = 7f;
            joint.massScale = 4.5f;

            lr.positionCount = 2;
            currentGrapplePosition = hand.position;
        }
    }

    void StopSwing()
    {
        lr.positionCount = 0;
        Destroy(joint);
    }

    void DrawRope()
    {
        if (!joint)
        {
            return;
        }

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, swingPoint, Time.deltaTime * 8f);

        lr.SetPosition(0, hand.position);
        lr.SetPosition(1, swingPoint);
    }
}
