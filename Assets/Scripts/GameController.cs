using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    readonly float G = 10000f;
    public Ship ship { get; private set; }
    [SerializeField] GameObject celestialPrefab;
    float storedScale = 0f;
    [SerializeField] int celestialMax;
    [SerializeField] int xMax;
    [SerializeField] int yMax;
    [SerializeField] int zMax;
    [SerializeField] List<GameObject> celestialObjs;
    [SerializeField] List<GameObject> minableObjs;

    [Header("Exit Values")]
    [SerializeField] GameObject exit;
    bool exitPlaced;

    [Header("Monster Event Object")]
    [SerializeField] GameObject blackHoleMonster;

    [Header("Resource Value")]
    [SerializeField] float resources;

    [Header("Canvas Objects")]
    [SerializeField] GameObject instructionCanvas;
    public bool instructions { get; private set; }
    [SerializeField] GameObject gameOverCanvas;
    public bool gameOver = false;

    Coroutine sceneChangeRoutine;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        ship = FindObjectOfType<Ship>();
        GenerateGalaxy();
    }

    private void Start()
    {
        gameOver = false;
        blackHoleMonster.transform.localScale = Vector3.zero;
        blackHoleMonster.SetActive(false);

        int toggleInstruction = PlayerPrefs.GetInt("instructionCanvas", 0);
        instructionCanvas.SetActive(toggleInstruction == 0);

        FadeController.instance.StartFade(0f, 1f);
    }

    private void Update()
    {
        gameOverCanvas.SetActive(gameOver);
        instructions = instructionCanvas.activeSelf;
        Cursor.visible = gameOver;

        if (resources >= 6000 && !exitPlaced)
        {
            float exitX = xMax / 2f;
            float exitY = yMax / 2f;
            float exitZ = zMax / 2f;
            Vector3 randPos = new Vector3(Random.Range(-exitX, exitX), Random.Range(-exitY, exitY), Random.Range(-exitZ, exitZ));

            print("Added exit point");
            GameObject exitPoint = Instantiate(exit, randPos, Quaternion.identity);
            exitPlaced = true;
        }

        if (Time.timeSinceLevelLoad >= 500f)
        {
            blackHoleMonster.SetActive(true);
            blackHoleMonster.transform.localScale += Vector3.one * Time.deltaTime * 100f;
            if (blackHoleMonster.transform.localScale.x >= 2500f)
            {
                GameOver();
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Gravity();
    }

    void GenerateGalaxy()
    {
        for (int i = 0; i < celestialMax; i++)
        {
            Vector3 randPos = new Vector3(Random.Range(-xMax, xMax), Random.Range(-yMax, yMax), Random.Range(500f, zMax));
            float randScale = Random.Range(1f, 75f);
            bool placed = true;
            foreach (GameObject celestial in celestialObjs)
            {
                float checkDist = Vector3.Distance(randPos, celestial.transform.position);
                float checkOrigin = Vector3.Distance(randPos, Vector3.zero);
                if ((checkDist <= 10f && checkOrigin <= 1000) && (randScale <= storedScale + 10f && randScale >= storedScale - 10f))
                {
                    i--;
                    placed = false;
                    print("Repeated position");
                    break;
                }
            }
            if (placed)
            {
                print("Added celestial body");
                GameObject tempCelestial = Instantiate(celestialPrefab, randPos, Quaternion.identity);
                tempCelestial.name += $"({i})";
                tempCelestial.transform.localScale = Vector3.one * randScale * 500f;
                tempCelestial.GetComponent<Rigidbody>().mass = randScale;
                int mineCheck = Random.Range(0, 100);
                if (mineCheck % 2 == 0 && minableObjs.Count < celestialMax / 5f)
                {
                    tempCelestial.GetComponent<CelestialBody>().minable = true;
                    Material tempMat = tempCelestial.GetComponent<MeshRenderer>().materials[0];
                    tempMat.color = Color.yellow;
                    tempCelestial.GetComponent<MeshRenderer>().materials[0] = tempMat;
                    minableObjs.Add(tempCelestial);
                }
                celestialObjs.Add(tempCelestial);
            }
        }
    }

    void SetInitialVelocity()
    {
        foreach (GameObject a in celestialObjs)
        {
            float m2 = a.GetComponent<Rigidbody>().mass;
            float r = Vector3.Distance(a.transform.position, ship.transform.position);

            ship.transform.LookAt(a.transform);

            //Circular Orbit = ((G * M) / r)^0.5, where G = gravitational constant, M is the mass of the central object and r is the distance between the two objects
            //We ignore the mass of the orbiting object when the orbiting object's mass is negligible, like the mass of the earth vs. mass of the sun
            a.GetComponent<Rigidbody>().velocity += a.transform.right * Mathf.Sqrt((G * m2) / r);


            //foreach (GameObject b in celestialObjs)
            //{
            //    if (!a.Equals(b))
            //    {
            //        float m2 = b.GetComponent<Rigidbody>().mass;
            //        float r = Vector3.Distance(a.transform.position, b.transform.position);

            //        a.transform.LookAt(b.transform);

            //        //Circular Orbit = ((G * M) / r)^0.5, where G = gravitational constant, M is the mass of the central object and r is the distance between the two objects
            //        //We ignore the mass of the orbiting object when the orbiting object's mass is negligible, like the mass of the earth vs. mass of the sun
            //        a.GetComponent<Rigidbody>().velocity += a.transform.right * Mathf.Sqrt((G * m2) / r);
            //    }
            //}
        }
    }

    void Gravity()
    {
        foreach (GameObject a in celestialObjs)
        {
            float m1 = a.GetComponent<Rigidbody>().mass;
            float m2 = ship.GetComponent<Rigidbody>().mass;
            float r = Vector3.Distance(a.transform.position, ship.transform.position);

            ship.GetComponent<Rigidbody>().AddForce((a.transform.position - ship.transform.position).normalized * (G * (m1 * m2) / (r * r)));


            //foreach (GameObject b in celestialObjs)
            //{
            //    if (!a.Equals(b))
            //    {
            //        float m1 = a.GetComponent<Rigidbody>().mass;
            //        float m2 = b.GetComponent<Rigidbody>().mass;
            //        float r = Vector3.Distance(a.transform.position, b.transform.position);

            //        a.GetComponent<Rigidbody>().AddForce((b.transform.position - a.transform.position).normalized * (G * (m1 * m2) / (r * r)));
            //    }
            //}
        }
    }

    public void UpdateResources()
    {
        resources++;
    }

    public void GameOver()
    {
        Debug.Log("Game over");
        gameOver = true;
    }

    public void StartGame()
    {
        PlayerPrefs.SetInt("instructionCanvas", 1);
        instructionCanvas.SetActive(false);
    }

    public void MainMenu()
    {
        ChangeScene("MainMenu");
    }

    public void ChangeScene(string sceneToLoad)
    {
        if (sceneChangeRoutine == null)
            sceneChangeRoutine = StartCoroutine(SceneChange(sceneToLoad));
    }

    IEnumerator SceneChange(string sceneToLoad)
    {
        FadeController.instance.StartFade(1f, 1f);

        while (FadeController.instance.isFading)
            yield return null;

        SceneManager.LoadScene(sceneToLoad);

        sceneChangeRoutine = null;
    }
}
