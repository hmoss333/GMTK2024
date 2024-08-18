using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System;


public class MainMenuController : MonoBehaviour
{
    public static MainMenuController instance;

    [SerializeField] MainMenuElement[] menuElements;
    public MainMenuElement currentElement { get; private set; }
    [SerializeField] float camSpeed;
    private int index;
    [SerializeField] AudioSource audioSource;
    [SerializeField] float maxAudioVolume;
    string sceneToLoad;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        Cursor.visible = false;
    }

    private void Start()
    {
        index = 0;
        currentElement = menuElements[index];

        //FadeController.instance.StartFade(0f, 1f);
    }

    private void Update()
    {
        Vector2 inputVal = InputController.instance.inputMaster.Player.Move.ReadValue<Vector2>();
        bool inputCheck = InputController.instance.inputMaster.Player.Move.WasPressedThisFrame();
        if (Mathf.RoundToInt(inputVal.x) < 0 && inputCheck)
        {
            index--;
            if (index < 0)
                index = menuElements.Length - 1;
        }
        else if (Mathf.RoundToInt(inputVal.x) > 0 && inputCheck)
        {
            index++;
            if (index > menuElements.Length - 1)
                index = 0;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //audioSource.Stop();
            //audioSource.clip = currentElement.clip;
            //audioSource.Play();
            currentElement.onClick();
        }
    }

    private void FixedUpdate()
    {
        currentElement = menuElements[index];

        Camera.main.transform.rotation = Quaternion.Slerp(Camera.main.transform.rotation, currentElement.pos.rotation, camSpeed * Time.deltaTime);
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, currentElement.pos.position, camSpeed * Time.deltaTime);
    }

    public void NewGameButton()
    {
        SceneManager.LoadScene("Flight");
    }

    public void QuitGameButton()
    {
        StartCoroutine(QuitGame());
    }

    IEnumerator QuitGame()
    {
        //FadeController.instance.StartFade(1.0f, 1f);

        //while (FadeController.instance.isFading)
        //    yield return null;

        yield return null;

        Application.Quit();
    }

    IEnumerator FadeAudio(AudioSource audioSource, float aValue, float aTime)
    {
        float delta = audioSource.volume;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            delta = Mathf.Lerp(delta, aValue, t);
            audioSource.volume = delta;
            yield return null;
        }
    }
}

[System.Serializable]
public class MainMenuElement
{
    public enum MenuOption { title, continueGame, newGame, settings, exit }
    public MenuOption menuOption;
    public Transform pos;
    public AudioClip clip;
    public UnityEvent trigger;

    public void onClick()
    {
        trigger.Invoke();
    }
}