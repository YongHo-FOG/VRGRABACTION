using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayGrabber : MonoBehaviour
{
    public float maxDistance = 10f;
    public LayerMask grabbableLayer;
    public LineRenderer lineRenderer;

    private Transform grabbedObject;
    private Rigidbody grabbedRb;
    private bool isGrabbing;

    public float pullForce = 10f;
    public float pushForce = 10f; // 밀기 힘 추가
    public Vector3 holdOffset = new Vector3(0, 0, 1.2f); // 손 앞

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        // 레이 시각화
        lineRenderer.SetPosition(0, ray.origin);
        lineRenderer.SetPosition(1, ray.origin + ray.direction * maxDistance);

        // Space 키로 잡기
        if (Input.GetKeyDown(KeyCode.Space) && !isGrabbing)
        {
            if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, grabbableLayer))
            {
                var rb = hit.collider.attachedRigidbody;
                if (rb != null)
                {
                    grabbedObject = rb.transform;
                    grabbedRb = rb;
                    grabbedRb.useGravity = false;
                    isGrabbing = true;
                }
            }
        }

        // Space 키를 떼면 끌기 중단 (release)
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Pull();
        }

        // P 키로 밀기
        if (Input.GetKeyDown(KeyCode.P) && isGrabbing && grabbedObject != null)
        {
            Push();
        }

        // 잡고 있으면 물체를 손 앞으로 끌어당김
        if (isGrabbing && grabbedObject != null)
        {
            Vector3 targetPos = transform.TransformPoint(holdOffset);
            Vector3 direction = targetPos - grabbedObject.position;
            grabbedRb.velocity = direction * pullForce;
        }
    }

    void Pull()
    {
        if (grabbedRb != null)
        {
            grabbedRb.useGravity = true;
            grabbedRb.velocity = Vector3.zero;
        }
        grabbedObject = null;
        grabbedRb = null;
        isGrabbing = false;
    }

    void Push()
    {
        if (grabbedRb != null)
        {
            Vector3 pushDir = (grabbedObject.position - transform.position).normalized;
            grabbedRb.useGravity = true;
            grabbedRb.velocity = Vector3.zero;
            grabbedRb.AddForce(pushDir * pushForce, ForceMode.Impulse);
        }
        grabbedObject = null;
        grabbedRb = null;
        isGrabbing = false;
    }
}
