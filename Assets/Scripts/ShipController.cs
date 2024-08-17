using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ShipController : MonoBehaviour
{
    public float velocity = 1f;
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
        fuel -= Mathf.Abs(fuelUsageRate * velocity * Time.deltaTime);
        if (fuel <= 0) { fuel = 0; GameController.instance.GameOver(); }

        //Vector2 playerInput = InputController.instance.inputMaster.Player.Move.ReadValue<Vector2>();
        //if (playerInput.x > 0) { transform.Rotate(0, rotSpeed * Time.deltaTime, 0, Space.Self); }
        //else if (playerInput.x < 0) { transform.Rotate(0, -rotSpeed * Time.deltaTime, 0, Space.Self); }
        //if (playerInput.y > 0) { transform.Rotate(rotSpeed * Time.deltaTime, 0, 0, Space.Self); }
        //else if (playerInput.y < 0) { transform.Rotate(-rotSpeed * Time.deltaTime, 0, 0, Space.Self); }
        //rb.velocity = transform.forward * velocity;

        ////if (InputController.instance.inputMaster.Player.Move.triggered)
        ////{
        ////    if (playerInput.y > 0) { velocity++; }
        ////    else if (playerInput.y < 0) { velocity--; }
        ////}
    }  

    private void OnCollisionEnter(Collision collision)
    {
        //Play explosion effect
        GameController.instance.GameOver();
    }
}
