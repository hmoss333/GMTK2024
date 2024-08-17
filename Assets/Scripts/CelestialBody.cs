using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelestialBody : MonoBehaviour
{
    public float resources;
    [SerializeField] Vector3 startScale;


    private void Start()
    {
        resources = transform.localScale.x;
        startScale = transform.localScale;
    }

    private void FixedUpdate()
    {
        transform.localScale = new Vector3(resources, resources, resources);
        if (transform.localScale.x <= 0.0f)
            this.gameObject.SetActive(false);


        float shipDist = Vector3.Distance(transform.position, GameController.instance.ship.transform.position);
        if (shipDist <= 500f)
        {
            Debug.DrawLine(transform.position, GameController.instance.ship.transform.position, Color.red);
            print($"Draining from {name}");
            resources -= 35;
            GameController.instance.UpdateResources();
        }
    }
}
