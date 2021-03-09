using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BirdController : MonoBehaviour
{
    public delegate void PlayerDelegate();
    public static event PlayerDelegate OnPlayerDied;
    public static event PlayerDelegate OnPlayerScored;
    public float jumpForce;
    public float fallingSpeed;
    public float movingSpeed;
    public Vector3 startPos;
    public AudioSource scoreSound;
    public AudioSource dieSound;
    public AudioSource gameMusic;
    Rigidbody2D birdrb;
    Quaternion goingDown;
    Quaternion goingUp;
    GameplayManager game;

    void Start()
    {
        birdrb = GetComponent<Rigidbody2D>();
        goingDown = Quaternion.Euler(0, 0, -80);
        goingUp = Quaternion.Euler(0, 0, 35);
        game = GameplayManager.Instance;
        birdrb.simulated = false;
        gameMusic.Play();
    }

    void OnEnable() 
    {
        GameplayManager.OnGameStarted += OnGameStarted;
        GameplayManager.OnGameOverConfirmed += OnGameOverConfirmed;
    }

    void OnDisable()
    {
        GameplayManager.OnGameStarted -= OnGameStarted;
        GameplayManager.OnGameOverConfirmed -= OnGameOverConfirmed;
    }

    void OnGameStarted()
    {
        birdrb.velocity = Vector3.zero;
        birdrb.simulated = true;
    }
    void OnGameOverConfirmed() 
    {
        transform.localPosition = startPos;
        transform.rotation = Quaternion.identity;
    }

    void Update()
    {
        if (game.GameOver) 
            return;

        transform.position += Vector3.right * movingSpeed * Time.deltaTime;

        if (Input.GetMouseButtonDown(0))
        {
            transform.rotation = goingUp;
            birdrb.velocity = Vector3.zero;
            birdrb.AddForce(Vector2.up * jumpForce, ForceMode2D.Force);
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, goingDown, fallingSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ScoreZone") 
        {
            Debug.Log("Punkt?");
            OnPlayerScored(); //idzie info do Game Mgra
            scoreSound.Play();
        }

        if (collision.gameObject.tag == "DeadZone")
        {
            Debug.Log("Dead?");
            birdrb.simulated = false;
            OnPlayerDied(); //idzie info do Game Managera
            dieSound.Play();
        }
    }
}
