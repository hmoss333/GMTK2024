using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CelestialBody : MonoBehaviour
{
    public float resources;
    Vector3 startScale;
    [SerializeField] ParticleSystem drainEffect;


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
    }

    public void DrainResources()
    {
        print($"Draining from {name}");
        drainEffect.Emit(10);
        resources -= 15f;//35;
        GameController.instance.UpdateResources();
    }
}
