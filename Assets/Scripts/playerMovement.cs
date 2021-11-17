using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class playerMovement : MonoBehaviour
{
    private CharacterController cc;
    private int maxScore;
    [SerializeField] private float movementSpeed=5f;
    private bool isgameOver;
    [SerializeField] private float g=-9.8f;
    [SerializeField] private float jumpHeight = 3f;
    [SerializeField] private Transform playerCam;
    private float pitch;
    private int prevScore;
    private float prevTime;
    private int currScore;
    [SerializeField] private int scoreMultipler;
    [SerializeField] private TextMeshProUGUI score;

    
    [SerializeField] private Canvas defaultUI;
    [SerializeField] private Canvas endgameUI;
    [SerializeField] private Canvas pregameUI;
    [SerializeField] private TextMeshProUGUI pregameTimer;
    [SerializeField] private TextMeshProUGUI endgameScoreCard;
    
    public static bool inPregame;
    private float pregameTime = 4f;
    
    public Transform groundCheck;
    public float groundDistance = 0.2f;
    public LayerMask groundMask;
    Vector3 velocity;
    bool isGrounded;
    
    // Start is called before the first frame update
    void Start()
    {
        
        maxScore = PlayerPrefs.GetInt("HighScore", 0);
        pregameTime = 4f;
        inPregame = true;
        Cursor.lockState = CursorLockMode.Locked;
        cc = GetComponent<CharacterController>();
        velocity=Vector3.zero;
        prevTime = Time.time;
        score.text = "0";
        currScore = 0;
        pregameUI.enabled = true;
        defaultUI.enabled = false;
        endgameUI.enabled = false;
        isgameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        look();
        if (inPregame)
        {
            pregameTime -= Time.deltaTime;
            pregameTimer.text = ((int) pregameTime).ToString();
            if (pregameTime <= 0f)
            {
                inPregame = false;
                pregameUI.enabled = false;
                defaultUI.enabled = true;
                enemyMovement.doNotMove = false;
            }
            return;
        }
        
        if (isgameOver) return;
        gravity();
        
        move();
        if (enemyMovement.doNotMove) return;
        shoot();
    }

    void gravity()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0)
            velocity.y=-2f;
        
        velocity.y += g * Time.deltaTime;
        cc.Move(velocity * Time.deltaTime);
        
    }
    
    void look()
    {
        transform.Rotate(0f,Input.GetAxis("Mouse X"),0f);
        pitch -= Input.GetAxis("Mouse Y");
        pitch = Mathf.Clamp(pitch, -60, 60);
        playerCam.transform.localRotation = Quaternion.Euler(pitch, 0, 0);

    }

    void move()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector3 movement = transform.forward * y + transform.right * x;
        movement = Vector3.ClampMagnitude(movement, 1f);
        cc.Move(movement * movementSpeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * g);
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            velocity.y = 0f;
        }

    }

    void shoot()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            RaycastHit hit;
            if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit))
            {
                enemyMovement enemy = hit.transform.GetComponent<enemyMovement>();
                if (enemy != null)
                {
                    enemy.onDeath();
                    updateScore();
                }

               

            }
            
        }
        
    }

    void updateScore()
    {
        float div = Time.time - prevTime;
        if (div <= 0f) return;
        currScore +=(int) (scoreMultipler * 1f / (div)) ;
        prevTime = Time.time;
        score.text = currScore.ToString();
    }
    
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.gameObject.CompareTag("Enemy"))
            prepareGameOver();
    }


   public void prepareGameOver()
    {
        isgameOver = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
        enemyMovement.doNotMove = true;
        
        
        defaultUI.enabled = false;
        endgameUI.enabled = true;
        endgameScoreCard.text = currScore.ToString();
        if (currScore > maxScore)
        {
            maxScore = currScore;
            PlayerPrefs.SetInt("HighScore",maxScore);
        }
    }


    public void QuitGame()
    {
        SceneManager.LoadScene("Scenes/menuScene");
    }

    public void ReStartGame()
    {
        
        Cursor.lockState = CursorLockMode.Locked;
        isgameOver = false;
        Time.timeScale = 1f;
        Scene scene = SceneManager.GetActiveScene(); 
        SceneManager.LoadScene(scene.name);
        
    }
    
}
