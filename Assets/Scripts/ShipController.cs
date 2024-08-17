using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ShipController : MonoBehaviour
{
    readonly float G = 1000f;
    [SerializeField] float velocity = 1f;
    [SerializeField] float rotSpeed = 2.5f;
    [SerializeField] float fuel = 100f;
    float fuelUsageRate = 0.35f;
    Rigidbody rb;


    [SerializeField] List<GameObject> celestials;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        celestials = GameObject.FindGameObjectsWithTag("Celestial").ToList();
    }

    private void Update()
    {
        fuel -= fuelUsageRate * velocity * Time.deltaTime;
        if (fuel <= 0) { GameOver(); }

        Vector2 playerInput = InputController.instance.inputMaster.Player.Move.ReadValue<Vector2>();
        if (playerInput.x > 0) { transform.Rotate(0, rotSpeed * Time.deltaTime, 0, Space.World); }
        else if (playerInput.x < 0) { transform.Rotate(0, -rotSpeed * Time.deltaTime, 0, Space.World); }
        rb.velocity = transform.forward * velocity;

        if (InputController.instance.inputMaster.Player.Move.triggered)
        {
            if (playerInput.y > 0) { velocity++; }
            else if (playerInput.y < 0) { velocity--; }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Gravity();
    }

    void SetInitialVelocity()
    {
        foreach (GameObject a in celestials)
        {
            foreach (GameObject b in celestials)
            {
                if (!a.Equals(b))
                {
                    float m2 = b.GetComponent<Rigidbody>().mass;
                    float r = Vector3.Distance(a.transform.position, b.transform.position);

                    a.transform.LookAt(b.transform);

                        //Circular Orbit = ((G * M) / r)^0.5, where G = gravitational constant, M is the mass of the central object and r is the distance between the two objects
                        //We ignore the mass of the orbiting object when the orbiting object's mass is negligible, like the mass of the earth vs. mass of the sun
                        a.GetComponent<Rigidbody>().velocity += a.transform.right * Mathf.Sqrt((G * m2) / r);
                }
            }
        }
    }

    void Gravity()
    {
        foreach (GameObject a in celestials)
        {
            foreach (GameObject b in celestials)
            {
                if (!a.Equals(b))
                {
                    float m1 = a.GetComponent<Rigidbody>().mass;
                    float m2 = b.GetComponent<Rigidbody>().mass;
                    float r = Vector3.Distance(a.transform.position, b.transform.position);

                    a.GetComponent<Rigidbody>().AddForce((b.transform.position - a.transform.position).normalized * (G * (m1 * m2) / (r * r)));
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameOver();
    }

    private void GameOver()
    {
        Debug.Log("GameOver");
        //Play explosion effect
        //Display gameover text
    }
}
