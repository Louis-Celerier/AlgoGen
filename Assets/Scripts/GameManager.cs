using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class GameManager : MonoBehaviour
{
    public GameObject game;
    public int individus;

    private GameObject cam;
    private GameObject backG;
    private GameObject racket;
    private GameObject ball;

    private List<GameObject> games = new List<GameObject>();
    private List<GameObject> balls = new List<GameObject>();
    private List<GameObject> rackets = new List<GameObject>();
    private List<GameObject> walls = new List<GameObject>();

    private int nbGamesMin = 5;
    private int gen;
    private const float gameSizeX = 22.7775f;
    private const float gameSizeY = 10f;

    // Start is called before the first frame update
    void Start()
    {
        gen = 1;

        racket = game.GetComponent<Transform>().GetChild(0).gameObject;
        ball = game.GetComponent<Transform>().GetChild(1).gameObject;

        int tailleCam = individus;
        while(Mathf.Sqrt(tailleCam) % 1 != 0)
        {
            tailleCam++;
        }
        
        tailleCam = (int)Mathf.Sqrt(tailleCam);

        cam = GameObject.FindWithTag("MainCamera");
        cam.GetComponent<Camera>().orthographicSize = tailleCam * 5;

        Vector3 center = new Vector3((tailleCam - 1) * (gameSizeX / 2), (tailleCam - 2) * -(gameSizeY / 2), cam.GetComponent<Transform>().position.z);
        cam.GetComponent<Transform>().position = center;

        backG = GameObject.FindWithTag("Background");

        center = new Vector3((tailleCam - 1) * (gameSizeX / 2), (tailleCam - 2) * -(gameSizeY / 2), backG.GetComponent<Transform>().position.z);
        backG.GetComponent<Transform>().position = center;

        center = new Vector3(backG.GetComponent<Transform>().localScale.x * tailleCam, backG.GetComponent<Transform>().localScale.y * tailleCam, backG.GetComponent<Transform>().localScale.z);
        backG.GetComponent<Transform>().localScale = center;

        Transform pos;
        int j = 0;
        int k = 0;
        for (int i = 0; i < individus; i++)
        {
            games.Add(Instantiate(game));

            Color c = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));

            rackets.Add(games[i].GetComponent<Transform>().GetChild(0).gameObject);
            balls.Add(games[i].GetComponent<Transform>().GetChild(1).gameObject);
            walls.Add(games[i].GetComponent<Transform>().GetChild(2).gameObject);

            rackets[i].GetComponent<RacketAI>().moveSpeed = Random.Range(20f, 30f);

            rackets[i].GetComponent<SpriteRenderer>().color = c;
            balls[i].GetComponent<SpriteRenderer>().color = c;

            pos = games[i].GetComponent<Transform>();
            pos.position = new Vector3(gameSizeX * j, -gameSizeY * k, pos.position.z);
            
            if (j < tailleCam-1) j++;
            else
            {
                j = 0;
                k++;
            }

            games[i].GetComponent<Transform>().position = pos.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadPlus)) Time.timeScale += 0.2f;
        if (Input.GetKeyDown(KeyCode.KeypadMinus)) Time.timeScale -= 0.2f;

        if (rackets.Count == 0)
        {
            for(int i = 0; i < games.Count; i++)
            {
                Destroy(games[i]);
                games.RemoveAt(i);
            }
            Start();
        }

        Cleaner();
        if (rackets.Count <= nbGamesMin)
        {
            gen++;
            Duplicate();
            Debug.Log("Generation : " + gen);
        }
    }

    private void Duplicate()
    {
        for(int i = 0; i < games.Count; i++)
        {
            if (games[i].GetComponent<Transform>().childCount == 1)
            {
                int rdm1 = Random.Range(0, rackets.Count);
                while (rackets[rdm1] == null) rdm1 = Random.Range(0, rackets.Count);
                int rdm2 = Random.Range(0, rackets.Count);
                while (rdm2 == rdm1 || rackets[rdm2] == null) rdm2 = Random.Range(0, rackets.Count);

                float newSpeed = (rackets[rdm1].GetComponent<RacketAI>().moveSpeed + rackets[rdm2].GetComponent<RacketAI>().moveSpeed) / 2 + Random.Range(-3f, 3f);

                rackets.Add(Instantiate(racket, games[i].GetComponent<Transform>(), false));
                balls.Add(Instantiate(ball, games[i].GetComponent<Transform>(), false));

                Color c = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));

                rackets[rackets.Count - 1].GetComponent<RacketAI>().moveSpeed = newSpeed;

                rackets[rackets.Count - 1].GetComponent<SpriteRenderer>().color = c;
                balls[rackets.Count - 1].GetComponent<SpriteRenderer>().color = c;

                rackets[rackets.Count - 1].transform.parent = games[i].transform;
                balls[rackets.Count - 1].transform.parent = games[i].transform;

                rackets[rackets.Count - 1].GetComponent<Transform>().localPosition = new Vector3(-10, 0, 0);
                balls[rackets.Count - 1].GetComponent<Transform>().localPosition = Vector3.zero;

                rackets[rackets.Count - 1].GetComponent<RacketAI>().ball = balls[rackets.Count - 1].GetComponent<Transform>();
                balls[rackets.Count - 1].GetComponent<Ball>().Racket = rackets[rackets.Count - 1];
            }
        }
    }

    public void Cleaner()
    {
        for(int i = 0; i < rackets.Count; i++)
        {
            if (rackets[i] == null)
            {
                rackets.RemoveAt(i);
                balls.RemoveAt(i);
            }
        }
    }
}