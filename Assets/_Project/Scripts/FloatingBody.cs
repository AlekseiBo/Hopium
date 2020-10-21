using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FloatingBody : MonoBehaviour
{
    [SerializeField] float minVertical;
    [SerializeField] float maxVetical;
    [SerializeField] float horizontal;
    [SerializeField] float avoidanceDistance;
    [SerializeField] float deacceleration;


    private Transform player;
    private Rigidbody body;
    private Vector3 targetPoint;
    private float displacement;

    void Start()
    {
        player = Camera.main.transform;
        body = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        transform.rotation = Quaternion.Euler(0f, Random.Range(0, 359), 0f);
        displacement = Random.Range(minVertical, maxVetical);

        UpdateLevel();
    }

    private void FixedUpdate()
    {
        var upliftForce = (targetPoint.y - transform.position.y) * displacement;
        body.AddForce(new Vector3(0f, upliftForce, 0f), ForceMode.Acceleration);

        var moveDirection = (transform.position - player.position).Flat();
        if (moveDirection.magnitude < avoidanceDistance)
        {
            body.AddForce(moveDirection.normalized * horizontal, ForceMode.Acceleration);
        }
        else
        {
            body.velocity = new Vector3(body.velocity.x * deacceleration, body.velocity.y, body.velocity.z * deacceleration);
        }
    }

    public void UpdateLevel()
    {
        targetPoint = transform.position + Vector3.one * displacement;
    }
}
