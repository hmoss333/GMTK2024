using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ShipController : MonoBehaviour
{
    [SerializeField] float velocity = 1f;
    [SerializeField] float rotSpeed = 2.5f;
    [SerializeField] float fuel = 100f;
    float fuelUsageRate = 0.35f;
    Rigidbody rb;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        fuel -= fuelUsageRate * velocity * Time.deltaTime;
        if (fuel <= 0) { fuel = 0; GameOver(); }

        Vector2 playerInput = InputController.instance.inputMaster.Player.Move.ReadValue<Vector2>();
        if (playerInput.x > 0) { transform.Rotate(0, rotSpeed * Time.deltaTime, 0, Space.World); }
        else if (playerInput.x < 0) { transform.Rotate(0, -rotSpeed * Time.deltaTime, 0, Space.World); }
        if (playerInput.y > 0) { transform.Rotate(rotSpeed * Time.deltaTime, 0, 0, Space.World); }
        else if (playerInput.y < 0) { transform.Rotate(-rotSpeed * Time.deltaTime, 0, 0, Space.World); }
        rb.velocity = transform.forward * velocity;

        //if (InputController.instance.inputMaster.Player.Move.triggered)
        //{
        //    if (playerInput.y > 0) { velocity++; }
        //    else if (playerInput.y < 0) { velocity--; }
        //}
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
