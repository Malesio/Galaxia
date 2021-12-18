using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SimulationManager : MonoBehaviour
{
    public bool Simulating = true;

    [Range(30, 80)]
    public int UpdatesPerSecond = 50;

    public int MaxStarsAllowed = 1;

    public GameObject EarthLikePlanetPrefab;
    public GameObject RockyPlanetPrefab;
    public GameObject VolcanicPlanetPrefab;
    public GameObject SandPlanetPrefab;

    private List<CelestialBody> bodies;
    private bool hasStar;

    public float PhysicsDeltaTime
    {
        get => 1.0f / UpdatesPerSecond;
    }

    public void Initialise()
    {
        var earth = Instantiate(EarthLikePlanetPrefab);
        earth.transform.Translate(0, 0, -1250);
        earth.transform.Rotate(-23, 0, 0);
        earth.transform.parent = transform;
        
        var rocky = Instantiate(RockyPlanetPrefab);
        rocky.transform.Translate(0, 0, -500);
        rocky.transform.parent = transform;
        
        var sand = Instantiate(SandPlanetPrefab);
        sand.transform.Translate(0, 0, -935);
        sand.transform.Rotate(-177, 0, 0);
        sand.transform.parent = transform;
        
        var volcanic = Instantiate(VolcanicPlanetPrefab);
        volcanic.transform.Translate(0, 0, -1970);
        volcanic.transform.Rotate(-25, 0, 0);
        volcanic.transform.parent = transform;

        bodies = FindObjectsOfType<CelestialBody>().ToList();
        hasStar = bodies.Where(b => b.CompareTag("Star")).Any();
    }

    private void Awake()
    {
        Time.fixedDeltaTime = PhysicsDeltaTime;
    }

    void Start()
    {
        Initialise();
    }

    public void SetSimulating(bool simulate)
    {
        Simulating = simulate;
    }

    void FixedUpdate()
    {
        if (Simulating)
        {
            // Update velocities and positions separately.
            foreach (var body in bodies)
            {
                body.ApplyGravitationalForces(bodies.Where(b => b != body), PhysicsDeltaTime);
            }

            foreach (var body in bodies)
            {
                body.Move(PhysicsDeltaTime);
                body.Rotate(PhysicsDeltaTime);

                // If the planet crashed into another planet or the sun,
                // destroy it.
                if (body.Crashed)
                {
                    Destroy(body.gameObject);
                }
            }

            // Remove references to crashed planets in our list of bodies.
            bodies.RemoveAll(body => body.Crashed);
        }
    }
}
