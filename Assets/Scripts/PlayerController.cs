using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector3 startPos = new Vector3(0, 100, 0);

    public Transform offset;

    private float limX = 200f;
    private float limY = 200f;
    private float limLowY = 0f;
    private float limZ = 200f;

    private  float speed = 20f;
    public float turnSpeed = 20f;
    private float horizontalInput;
    private float verticalInput;

    public AudioClip shootClip;

    private AudioSource cameraAudioSource;
    private AudioSource playerAudioSource;

    public GameObject projectilePrefab;
    public GameObject recoletable;
    public GameObject obstacle;

    private float spawnRate = 5f;
    private float spawnMargin = 5f;

    private Vector3 randomPos;

    private int score;

    public bool gameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        // posicion inicial
        transform.position = startPos;

        //marcador a 0
        score = 0;

        //spawn de recolectables
        for (float coinInstances = 10f; coinInstances >=0; coinInstances -= 1f)
        {
            randomPos = RandomPosition();
            Instantiate(recoletable, randomPos, recoletable.transform.rotation);
        }

        //inicio corrutina
        StartCoroutine("SpawnObstacle");

        cameraAudioSource = GameObject.Find("Main Camera").GetComponent<AudioSource>();
        playerAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //movimiento constante
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
        //controles
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime * horizontalInput);
        transform.Rotate(Vector3.right, turnSpeed * Time.deltaTime * -verticalInput);

        //limites
        if (transform.position.x <= -limX)
        {
            transform.position = new Vector3(-limX, transform.position.y, transform.position.z);
        }
        if (transform.position.x >= limX)
        {
            transform.position = new Vector3(limX, transform.position.y, transform.position.z);
        }
        if (transform.position.y <= limLowY) 
        {
            transform.position = new Vector3(transform.position.x, limLowY, transform.position.z);
        }
        if (transform.position.y >= limY)
        {
            transform.position = new Vector3(transform.position.x, limY, transform.position.z);
        }
        if (transform.position.z <= -limZ)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -limZ);
        }
        if (transform.position.z >= limZ)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, limZ);
        }

        


        if (Input.GetKeyDown(KeyCode.RightControl))
        {
            //Disparar
            Instantiate(projectilePrefab, offset.position, projectilePrefab.transform.rotation = transform.rotation);

            playerAudioSource.PlayOneShot(shootClip, 1);
        }
    }

    public void RotateGameObject(KeyCode key, Vector3 rotation)
    {
        if (Input.GetKeyDown(key))
        {
            transform.rotation *= Quaternion.Euler(rotation);
        }
    }


    // Genera una posicion aleatoria 
    public Vector3 RandomPosition()
    {
        return new Vector3(Random.Range(-limX + spawnMargin, limX - spawnMargin), Random.Range(limLowY + spawnMargin, limY - spawnMargin), Random.Range(-limZ + spawnMargin, limZ - spawnMargin));
    }

    private IEnumerator SpawnObstacle()
    {
        while (!gameOver)
        {
            yield return new WaitForSeconds(spawnRate);
            randomPos = RandomPosition();
            Instantiate(obstacle, randomPos, recoletable.transform.rotation);

        }

    }
    private void OnCollisionEnter(Collision otherCollider)
    {
        if (!gameOver)
        {
            if (otherCollider.gameObject.CompareTag("Coin"))
            {

            }
            else if (otherCollider.gameObject.CompareTag("Obstacle"))
            {

                gameOver = true;
            }
        }

    }

    //recojer monedas
    private void OnTriggerEnter(Collider otherTrigger)
    {
        if (otherTrigger.gameObject.CompareTag("Coin"))
        {
            Destroy(otherTrigger.gameObject);
            score = score + 1;
        }

    }
    //game over
    private void OnCollisionEnter(Collider otherCollider)
    {
        if (otherCollider.gameObject.CompareTag("Obstacle"))
        {
            Destroy(otherCollider.gameObject);
            Destroy(gameObject);
            Debug.Log($"Puntuacion final:{score}");
        }

    }
}
