using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CelestialBody : MonoBehaviour
{
    public float resources;
    Vector3 startScale;

    //Test
    public LineRenderer Line;


    private void Start()
    {
        resources = transform.localScale.x;
        startScale = transform.localScale;


        //Test
        // set the color of the line
        Line.startColor = Color.red;
        Line.endColor = Color.red;

        // set width of the renderer
        Line.startWidth = 0.3f;
        Line.endWidth = 0.3f;
        Line.positionCount = 0;
    }

    private void FixedUpdate()
    {
        transform.localScale = new Vector3(resources, resources, resources);
        if (transform.localScale.x <= 0.0f)
            this.gameObject.SetActive(false);


        float shipDist = Vector3.Distance(transform.position, GameController.instance.ship.transform.position);
        if (shipDist <= 300f)
        {
            //Test
            if (Line.positionCount == 0)
            {
                Line.positionCount = 1;
                Line.SetPosition(0, transform.position);
            }
            else
            {
                Line.positionCount = 2;
                Line.SetPosition(1, GameController.instance.ship.transform.position);
            }

            print($"Draining from {name}");
            resources -= 35;
            GameController.instance.UpdateResources();
        }
        else
        {
            Line.positionCount = 0;
        }
    }
}
