using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AsteroidController : MonoBehaviour
{
    public float orbitRadius;

    [SerializeField] float minSpeed = 1f;
    [SerializeField] float maxSpeed = 1f;
    [SerializeField] float avoidanceDistance = 1f;

    private Transform player;
    private Rigidbody body;
    private Transform parent;
    private float speed;

    private void Start()
    {
        body = GetComponent<Rigidbody>();
        parent = transform.parent;
        player = Camera.main.transform;
        speed = Random.Range(minSpeed, maxSpeed);
    }

    private void FixedUpdate()
    {
        var avoidDir = (transform.position - player.position).Flat();
        if (avoidDir.magnitude < avoidanceDistance)
        {
            body.AddForce(avoidDir.normalized * speed, ForceMode.Acceleration);
        }

        Ray ray = new Ray(parent.position, transform.position - parent.position);
        var beltPoint = ray.GetPoint(orbitRadius);
        var flowDir = Vector3.Cross((parent.position - transform.position), parent.up).normalized * speed;
        var asteroidDir = (beltPoint + flowDir - transform.position).normalized;

        //Debug.DrawRay(transform.position, asteroidDir, Color.red, 0.1f);

        body.AddForce(asteroidDir * speed, ForceMode.Acceleration);
    }
}
