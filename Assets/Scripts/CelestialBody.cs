using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CelestialBody : MonoBehaviour
{
    public float resources;
    public bool depleated = false;
    public bool minable = false;
    Vector3 startScale;
    [SerializeField] ParticleSystem drainEffect;
    [SerializeField] GameObject blackHolePrefab;
    [SerializeField] AudioClip destroyClip;
    [SerializeField] AudioSource destroySource;

    Coroutine destroyRoutine;


    private void Start()
    {
        resources = transform.localScale.x;
        startScale = transform.localScale;

        blackHolePrefab.SetActive(false);
        destroySource.loop = false;
        destroySource.clip = destroyClip;
    }

    private void FixedUpdate()
    {
        if (!depleated && minable)
        {
            transform.localScale = new Vector3(resources, resources, resources);
            GetComponent<Rigidbody>().mass = resources / 500f;
        }
        else if (depleated)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * 1000f, 2.5f * Time.deltaTime);
            if (transform.localScale.x <= 2000f)
            {
                this.gameObject.SetActive(false);
            }
        }
    }

    public void DrainResources()
    {
        if (!depleated && minable)
        {
            print($"Draining from {name}");
            drainEffect.Emit(10);
            resources -= 15f;//35;
            GameController.instance.UpdateResources();

            if (transform.localScale.x <= 5000.0f)
            {
                depleated = true;

                if (!destroySource.isPlaying)
                    destroySource.Play();
                blackHolePrefab.SetActive(true);
            }
        }
    }
}
