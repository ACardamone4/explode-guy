using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class PlayerControls : MonoBehaviour
{
    public PlayerInput MPI;
    private InputAction move;
    private InputAction restart;
    private InputAction quit;
    private InputAction attack;

    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;
    public float PlayerSpeed;
    public bool PlayerShouldBeMoving;
    private bool playerJump;
    public Rigidbody2D PlayerRB;
    private float moveDirection;
    public float JumpForce;
    private InputAction jump;
    public bool PerformLaunch;
    [SerializeField] private bool _colliding;
    [SerializeField] private float inputHorizontal;
    public Vector2 GoUp;
    public Vector2 Walk;
    public bool InAir;
    [SerializeField] private bool _canMove;

    //public CinemachineVirtualCamera PlayerCam;

    [SerializeField] private float BaseGravity;
    //private bool releasing;
    //[SerializeField] private bool noVertical;


    [SerializeField] private float _attackWindup;
    [SerializeField] private float _attackDuration;
    [SerializeField] private float _timeAttackCooldown;
    [SerializeField] private float _lastDirection;
    [SerializeField] private bool _canAttack;
    [SerializeField] private bool _attacking;
    [SerializeField] private bool _holdingMove;
    [SerializeField] private float _explosionPower;
    [SerializeField] private float _rotationAmount;

    [SerializeField] private GameObject _explosion;
    [SerializeField] private GameObject _bouncyExplosion;
    [SerializeField] private Transform _self;

    [SerializeField] private bool _grounded;
    [SerializeField] private bool _moving;
    [SerializeField] private bool _hasGrounded;
    [SerializeField] private bool _bouncing;
    [SerializeField] private float _bouncepadBoost;
    [SerializeField] private bool _bombed;
    [SerializeField] private float _bombBoost;

    [SerializeField] private GameObject _bouncyParticles;
    [SerializeField] private GameObject _bombParticles;
    [SerializeField] private Animator _animator;

    [SerializeField] private float _velocityX;
    [SerializeField] private float _velocityY;
    [SerializeField] private float _currentDirection;
    [SerializeField] private Collider2D _collider;
    [SerializeField] private PhysicsMaterial2D _bounceMaterial;
    [SerializeField] private PhysicsMaterial2D _baseMaterial;

    // public GameManager GM;

    // public static PlayerMovement instance;
    // Start is called before the first frame update
    [SerializeField] private CheckpointManager _checkpointManager;

    private void Awake()
    {
        
        _checkpointManager = FindObjectOfType<CheckpointManager>();
        transform.position = _checkpointManager.LastCheckPointPos;
        _canMove = true;
        PlayerRB.gravityScale = BaseGravity;
        _moving = false;

        playerJump = false;

        //IsColliding = false;
        _colliding = false; //Makes it so the player can't infinitely jump
        //Grabs the player's control and body
        PlayerRB = GetComponent<Rigidbody2D>();
        MPI = GetComponent<PlayerInput>();

        //Grabs all the player's inputs
        move = MPI.currentActionMap.FindAction("Move");
        restart = MPI.currentActionMap.FindAction("Restart");
        quit = MPI.currentActionMap.FindAction("Quit");
        attack = MPI.currentActionMap.FindAction("Attack");
        //release = MPI.currentActionMap.FindAction("Release");

        MPI.currentActionMap.Enable();
        move.started += Handle_MoveStarted;
        move.canceled += Handle_MoveCanceled;
        restart.performed += Handle_RestartPerformed;
        quit.performed += Handle_QuitPerformed;
        attack.performed += Handle_Attack;
        //if (_animator == null)
        //{
        //    _animator.enabled = true;
        //}
        
    }

    /*void Start()
    {
        _checkpointManager = FindObjectOfType<CheckpointManager>();
        transform.position = _checkpointManager.LastCheckPointPos;
        _canMove = true;
        PlayerRB.gravityScale = BaseGravity;
        _moving = false;
        

        //GM = FindObjectOfType<GameManager>();
        //Sets the player's position to the last checkpoint they touch
        //transform.position = GM.LastCheckPointPos;

        playerJump = false;

        //IsColliding = false;
        _colliding = false; //Makes it so the player can't infinitely jump
        //Grabs the player's control and body
        PlayerRB = GetComponent<Rigidbody2D>();
        MPI = GetComponent<PlayerInput>();

        //Grabs all the player's inputs
        move = MPI.currentActionMap.FindAction("Move");
        restart = MPI.currentActionMap.FindAction("Restart");
        quit = MPI.currentActionMap.FindAction("Quit");
        attack = MPI.currentActionMap.FindAction("Attack");
        //release = MPI.currentActionMap.FindAction("Release");

        MPI.currentActionMap.Enable();
        move.started += Handle_MoveStarted;
        move.canceled += Handle_MoveCanceled;
        restart.performed += Handle_RestartPerformed;
        quit.performed += Handle_QuitPerformed;
        attack.performed += Handle_Attack;
    }
    //public void OnDestroy()
    //{
    //    restart.performed -= Handle_RestartPerformed;
    //    quit.performed -= Handle_QuitPerformed;
    //    attack.performed -= Handle_Attack;
    //    move.started -= Handle_MoveStarted;
    //    move.canceled -= Handle_MoveCanceled;
        
    //}*/

    public void OnDisable()
    {
        MPI.currentActionMap.Disable();
        move.started -= Handle_MoveStarted;
        move.canceled -= Handle_MoveCanceled;
        restart.performed -= Handle_RestartPerformed;
        quit.performed -= Handle_QuitPerformed;
        attack.performed -= Handle_Attack;
        //_animator.enabled = false;
    }

    private void Handle_Attack(InputAction.CallbackContext obj)
    {
        TryAttack();
        if (_canAttack == true)
        {
            Attack();
        }
        else if (_canAttack == false)
        {
            StopAttack();
        }
    }

    void Attack()
    {
        PlayerRB.freezeRotation = false;
        PlayerShouldBeMoving = true;
        _canAttack = false;
        _attacking = true;
        _canMove = false;
        _hasGrounded = false;
        //_animator.SetBool("Attack", true);
        //GameObject AttackInstance = Instantiate(_explosion, _self.position, _self.rotation);
        if (moveDirection != 0)
        {
            PlayerRB.velocity = new Vector2(moveDirection * _explosionPower, _explosionPower);
            PlayerRB.AddTorque(_rotationAmount * moveDirection);
        } else if (moveDirection == 0)
        {
            PlayerRB.velocity = new Vector2(_explosionPower * _lastDirection, _explosionPower);
            PlayerRB.AddTorque(_rotationAmount * _lastDirection);
        }
        this._collider.sharedMaterial = _bounceMaterial;
        this.PlayerRB.sharedMaterial = _baseMaterial;
        // StartCoroutine(attackDuration());
    }

    void StopAttack()
    {
        if (_holdingMove == true)
        {
            _moving = true;
        } else
        {
            _moving = false;
        }
        this._collider.sharedMaterial = _baseMaterial;
        this.PlayerRB.sharedMaterial = _baseMaterial;
        //this._collider.isTrigger = false;
        PlayerRB.rotation = (0);
        PlayerRB.freezeRotation = true;
        _bouncing = false;
        _bombed = false;
        _bouncyParticles.SetActive(false);
        _bombParticles.SetActive(false);
        _attacking = false;
        _canMove = true;
        //TryAttack();
        _animator.SetBool("Walking", false);
    }

    void TryAttack()
    {
        if (_attacking == false && _hasGrounded == true)
        {
            _canAttack = true;
            //Attack();
        }
        else if (_attacking == false && _grounded == true)
        {
            _canAttack = true;
            //Attack();
        }
    }

    private void Handle_ToMenuPerformed(InputAction.CallbackContext context)
    {
        SceneManager.LoadScene("IntroCutscene");
    }

    private void Handle_MoveStarted(InputAction.CallbackContext obj)
    {
        _holdingMove = true;
        if (_canMove == true)
        {
            //Can only be active if dash isn't occuring
            //Turns on the movement command
            PlayerShouldBeMoving = true;
            _moving = true;
        }
    }
    private void Handle_MoveCanceled(InputAction.CallbackContext obj)
    {//Can only be active if dash isn't occuring
        _holdingMove = false;
        if (_canMove == true)
        {
            _moving = false;
            //Turns off the movement command
            PlayerShouldBeMoving = false;
            //Turns off the movement animation
            //Animator.SetBool("IsMoving", false);

            //print("Handled move Canceled");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        /*if (_attacking == true)
        {
            if (_bouncing == true)
            {
                GameObject BouncyAttackInstance = Instantiate(_bouncyExplosion, _self.position, _self.rotation);
            }
            else
            {
                GameObject AttackInstance = Instantiate(_explosion, _self.position, _self.rotation);
            }
        }*/

        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Bouncy")
        {

            /*if (_attacking == false)
            {
                _hasGrounded = true;
            }*/
            PlayerRB.rotation = (0);
        }

        if (collision.gameObject.tag == "Killbox")
        {
            RestartGame();
        }
        if (collision.gameObject.tag == "Bouncy")
        {
            if (_attacking == true && _bouncing == false)
            {
                _bouncing = true;
                PlayerRB.velocity = new Vector2(_explosionPower * _currentDirection * _bouncepadBoost, _explosionPower * _bouncepadBoost);
                _bouncyParticles.SetActive(true);
            }
        }
        if (collision.gameObject.tag == "Bomb")
        {
            Attack();
            _bombed = true;
            PlayerRB.velocity = new Vector2(_explosionPower * _currentDirection * _bombBoost, _explosionPower * _bombBoost);
            _bombParticles.SetActive(true);
        }
    }

    public void FixedUpdate()
    {
        if (coyoteTimeCounter > 0f)
        {
            //Checks if the player is colliding with something, and if so turns off InAir and makes sure the double jump doesn't occur
            //DoubleJump = false;
            InAir = false;

            //Animator.SetBool("InAir", false);

            //Animator.SetBool("OnGround", true);

        }

        if (_attacking == false)
        {
            print("Not attacking");
            PlayerRB.rotation = (0);
        }


        if (InAir == true) //Makes animations show the player is moving and in the air
        {
            // Animator.SetBool("InAir", true);
            //Animator.SetBool("IsMoving", true);
            //Animator.SetBool("OnGround", false);
            //Animator.SetBool("Dash", false);
        }

        if (PlayerShouldBeMoving == true)
        {
            if (_canMove == true)
            {
                print("PlayerRB Should Be Moving");
                //Makes the player able to move, and turns on the moving animation   
                PlayerRB.velocity = new Vector2(PlayerSpeed * moveDirection, PlayerRB.velocity.y);
                //Animator.SetBool("IsMoving", true);
            }

        }
        

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Bouncy")//Checks if the player is touching the ground
        {
                PlayerRB.gravityScale = BaseGravity;
                _grounded = true;
                //GroundSaver = true;
                print("Touch Grass");

                //IsColliding = true;

                //Turns on ground animations
                //Animator.SetBool("OnGround", true);
                //Animator.SetBool("InAir", false);
                //Animator.SetBool("DoubleJump", false);
                //Animator.SetBool("Dash", false);
                InAir = false;
                // CanDoubleJump = false;
                //JumpIndicator.gameObject.SetActive(false);
                _colliding = true;

            }
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Bouncy")
        {
            
            if (_attacking == false)
            {
                _hasGrounded = true;
                PlayerRB.rotation = (0);
            }
        }
        if (collision.gameObject.tag == "Stop")
        {
            StopAttack();
            PlayerRB.rotation = (0);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Bouncy")
        {//Makes the game realize the player is not touching the ground
            //print("Dont Touch Grass");
            InAir = true;
            // CoyoteTime = true;
            // IsColliding = false;
            _colliding = false;
            //CanDoubleJump = true;
            print("Left Grass");
            _grounded = false;
            if (_attacking == false)
            {
                _hasGrounded = true;
            }
            
        }
    }

    // Update is called once per frame
    public void Update()
    {
        PlayerRB.gravityScale = BaseGravity;

        //if (_grounded = true && _moving == true)
        //{
        //    _animator.SetBool("Walking", true);
        //} 

        //if (_grounded = true && PlayerRB.velocity.y != 0)
        //{
        //    _animator.SetBool("Walking", true);
        //} 

        if (_moving == true && _velocityY > -2)
        {
            if (_bouncing == false)
            {
                _animator.SetBool("Walking", true);
            }
        }

        if (_velocityY >= 0)
        {
            //_animator.SetBool("Jumping", true);
        }

        if (_velocityY < -2)
        {
            if (_bouncing == false)
            {
                _animator.SetBool("Falling", true);
            }
            
        }

        if (_velocityY >= -2)
        {

            _animator.SetBool("Falling", false);
            //_animator.SetBool("Jumping", false);
        }

        _velocityX = PlayerRB.velocity.x;
        _velocityY = PlayerRB.velocity.y;

        //if (PlayerRB.velocity.y <= 0)
        //{
        //    _animator.SetBool("Falling", true);
        //} 

        if (PlayerRB.velocity.y < -2 || _moving == false)
        {
            _animator.SetBool("Walking", false);
        }

        if (_colliding == true)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        //if (Walking == true)
        //{
        if (PlayerShouldBeMoving == true)
        {
            //Checks what direction the player should be moving(Horizontally)
            moveDirection = move.ReadValue<float>();
        }
        else if (PlayerShouldBeMoving == false)
        {
            PlayerRB.velocity = new Vector2(0, PlayerRB.velocity.y);
        }
        if (moveDirection > 0)
        {
            //gameObject.transform.localScale = new Vector2(1, 1);
            _lastDirection = 1;
        }
        if (moveDirection < 0)
        {
            //gameObject.transform.localScale = new Vector2(-1, 1);
            _lastDirection = -1;
        }

        if (PlayerRB.velocity.x > 0)
        {
            gameObject.transform.localScale = new Vector2(1, 1);
            _currentDirection = 1;
        }
        else if (PlayerRB.velocity.x < 0)
        {
            gameObject.transform.localScale = new Vector2(-1, 1);
            _currentDirection = -1;
        }
        //}

        //if (inputHorizontal < 0)
        //{
        //    //Makes the player look left
        //    gameObject.transform.localScale = new Vector2(-1, 1);
        //}
        if (inputHorizontal == 0 && InAir == false)
        {//Makes the player go to idle animation
            //Animator.SetBool("IsMoving", false);
        }
    }
    private void Handle_QuitPerformed(InputAction.CallbackContext onj)
    {//Quits the game when quit is pressed
        //print("Handled quit Performed");
        //QuitGame();
        Application.Quit();
        print("Quit");
    }

    private void Handle_RestartPerformed(InputAction.CallbackContext obj)
    {//Sets the game back to the previous checkpoint
         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }



}