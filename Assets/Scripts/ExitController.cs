using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitController : MonoBehaviour
{
    [SerializeField] LineRenderer line;
    Coroutine sceneChangeRoutine;

    private void Update()
    {
        line.positionCount = 2;
        line.SetPosition(0, transform.position);
        line.SetPosition(1, GameController.instance.ship.transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Spaceship")
        {
            if (sceneChangeRoutine == null)
                sceneChangeRoutine = StartCoroutine(ChangeScene());
        }
    }

    IEnumerator ChangeScene()
    {
        FadeController.instance.StartFade(0f, 2.5f);

        while (FadeController.instance.isFading)
            yield return null;

        GameController.instance.ChangeScene("Flight");

        sceneChangeRoutine = null;
    }
}
