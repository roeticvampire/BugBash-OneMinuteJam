using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class enemyMovement : MonoBehaviour
{

     public static float enemySpeed=2;
     public static int currentCount;
     public bool stopspeaking;
     public static bool doNotMove;
    [SerializeField] private Transform playerTarget;
    [SerializeField] private List<Transform> spawnlocations;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private ParticleSystem bloodparticles;
    [SerializeField] private float minDist = 1.8f;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip buzzingSound;
    
    // Start is called before the first frame update
    void Start()
    {
        stopspeaking = false;
        doNotMove = true;
        currentCount++;
        transform.position = spawnlocations[Random.Range(0, spawnlocations.Count)].position;
    }

    
    // Update is called once per frame
    void Update()
    {
        if (doNotMove)
        {
            if (playerMovement.inPregame==false && stopspeaking == false)
            {
                _audioSource.Stop();
                stopspeaking = true;
            }
            return;
        }
        Vector3 movement = playerTarget.position-transform.position;
        playerTarget.GetComponent<playerMovement>();
        if (movement.magnitude <= minDist)
        {
            playerTarget.GetComponent<playerMovement>().prepareGameOver();
            return;
        }
        
        
        Vector3 move=Vector3.zero;
        float x = Mathf.Abs(movement.x);
        float y = Mathf.Abs(movement.y);
        float z = Mathf.Abs(movement.z);
        if (x > y && x > z) move.x = movement.x;
        else if (y > z) move.y = movement.y;
        else move.z = movement.z;
        move = Vector3.ClampMagnitude(move, 1f);
        transform.position += move * Time.deltaTime* enemySpeed;



    }

    public void StopSounds()
    {
        _audioSource.Stop();
    }
    public void onDeath()
    {
        //play blast sound
        //emit blast particle effects
        bloodparticles.transform.position = transform.position;
        _audioSource.PlayOneShot(deathSound);
        
        bloodparticles.Play();
        transform.position = spawnlocations[Random.Range(0, spawnlocations.Count)].position;
        _audioSource.Play();
        
        
    }
}
