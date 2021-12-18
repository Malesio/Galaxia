using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class CelestialBody : MonoBehaviour
{
    /// <summary>
    /// The gravitational acceleration of the planet.
    /// </summary>
    [Range(1, 500)]
    public float Gravity = 9.81f;

    /// <summary>
    /// The initial velocity at which the planet should move in space.
    /// </summary>
    /// <remarks>An incorrect velocity such that the angular speed of the planet
    /// around its star is too low will result in it crashing into its star,
    /// leading to its destruction.</remarks>
    public Vector3 InitialVelocity;

    /// <summary>
    /// The angular speed at which the planet rotates around itself.
    /// </summary>
    public float AngularSpeed;

    /// <summary>
    /// The mass of the planet.
    /// </summary>
    /// <remarks>
    /// This value should not be set manually. It is computed from the physical 
    /// radius and gravity of the planet.</remarks>
    public float Mass { get; private set; }

    /// <summary>
    /// The real-time velocity vector of the planet.
    /// </summary>
    public Vector3 Velocity { get; private set; }

    public Rigidbody PhysicalBody { get; private set; }

    public bool Crashed { get; private set; }

    private bool isStar;

    /// <summary>
    /// Accelerates the planet, taking into account all gravitational forces exerted by
    /// other celestial bodies.
    /// </summary>
    /// <param name="otherBodies">A list of all the other celestial bodies in the system, <b>not</b> including this planet</param>
    /// <param name="deltaTime">The time delta for physics updates</param>
    public void ApplyGravitationalForces(IEnumerable<CelestialBody> otherBodies, float deltaTime)
    {
        if (isStar)
        {
            return;
        }

        foreach (var other in otherBodies)
        {
            Vector3 deltaPosition = other.PhysicalBody.position - PhysicalBody.position;

            float sqrDistance = deltaPosition.sqrMagnitude;
            if (sqrDistance == 0.0f)
                continue;

            Vector3 deltaDirection = deltaPosition.normalized;

            Vector3 acceleration = deltaDirection * SpaceConstants.UniversalGravitationalConstant * other.Mass / sqrDistance;

            Velocity += acceleration * deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!gameObject.CompareTag("Star"))
        {
            Crashed = true;
        }
    }

    /// <summary>
    /// Updates the physical position of the planet.
    /// </summary>
    /// <param name="deltaTime"></param>
    public void Move(float deltaTime)
    {
        if (!isStar)
        {
            PhysicalBody.MovePosition(PhysicalBody.position + Velocity * deltaTime);
        }
    }

    public void Rotate(float deltaTime)
    {
        PhysicalBody.MoveRotation(PhysicalBody.rotation * Quaternion.AngleAxis(AngularSpeed * deltaTime, Vector3.up));
    }

    private void Initialise()
    {
        isStar = CompareTag("Star");

        PhysicalBody = GetComponent<Rigidbody>();
        int radius = GetComponent<Planet>().Properties.Radius;

        Mass = (radius * radius * Gravity) / SpaceConstants.UniversalGravitationalConstant;
        PhysicalBody.mass = Mass;
        Velocity = InitialVelocity;
    }

    private void Awake()
    {
        Initialise();
    }
}
