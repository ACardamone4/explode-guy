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
    [SerializeField] private float _explosionPower;
    [SerializeField] private float _rotationAmount;

    [SerializeField] private GameObject _explosion;
    [SerializeField] private Transform _self;

    [SerializeField] private bool _grounded;
    [SerializeField] private bool _moving;
    [SerializeField] private bool _hasGrounded;
    [SerializeField] private bool _bouncing;
    [SerializeField] private float _bouncepadBoost;

    //[SerializeField] private Animator _animator;

    [SerializeField] private float _velocityX;
    [SerializeField] private float _velocityY;
    [SerializeField] private Collider2D _collider;
    [SerializeField] private PhysicsMaterial2D _bounceMaterial;
    [SerializeField] private PhysicsMaterial2D _baseMaterial;


    // public GameManager GM;

    // public static PlayerMovement instance;
    // Start is called before the first frame update
    [SerializeField] private CheckpointManager _checkpointManager;

    void Start()
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

        ////jump = MPI.currentActionMap.FindAction("Jump");

        ////jump.started += Handle_JumpAction;
        ////jump.canceled += Handle_JumpActionCanceled;
        move.started += Handle_MoveStarted;
        move.canceled += Handle_MoveCanceled;
        restart.performed += Handle_RestartPerformed;
        quit.performed += Handle_QuitPerformed;
        attack.performed += Handle_Attack;
        //release.performed += Handle_Release;
        //release.canceled += Handle_ReleaseCanceled;
        //SC = FindObjectOfType <SpiritController>();
    }

    public void OnDestroy()
    {
        //Remove control when OnDestroy activates
        move.started -= Handle_MoveStarted;
        move.canceled -= Handle_MoveCanceled;
        restart.performed -= Handle_RestartPerformed;
        quit.performed -= Handle_QuitPerformed;
        ////jump.started -= Handle_JumpAction;
        ////jump.canceled -= Handle_JumpActionCanceled;

        attack.performed -= Handle_Attack;
        //release.performed -= Handle_Release;
        //release.canceled -= Handle_ReleaseCanceled;
    }

    private void Handle_JumpActionCanceled(InputAction.CallbackContext context)
    {

            if (PlayerRB.velocity.y > 0f)
            {
                PlayerRB.velocity = new Vector2(PlayerRB.velocity.x, PlayerRB.velocity.y * 0.5f);
            }
        
    }

    private void Handle_Attack(InputAction.CallbackContext obj)
    {
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
        PlayerShouldBeMoving = true;
        _canAttack = false;
        _attacking = true;
        _canMove = false;
        _hasGrounded = false;
        //_animator.SetBool("Attack", true);
        GameObject AttackInstance = Instantiate(_explosion, _self.position, _self.rotation);
        if (moveDirection != 0)
        {
            PlayerRB.velocity = new Vector2(moveDirection * _explosionPower, _explosionPower);
        } else if (moveDirection == 0)
        {
            PlayerRB.velocity = new Vector2(_explosionPower * _lastDirection, _explosionPower);
        }
        PlayerRB.AddTorque(_rotationAmount * moveDirection);
        this._collider.sharedMaterial = _bounceMaterial;
        this.PlayerRB.sharedMaterial = _baseMaterial;
        // StartCoroutine(attackDuration());
    }

    void StopAttack()
    {
        this._collider.sharedMaterial = _baseMaterial;
        this.PlayerRB.sharedMaterial = _baseMaterial;
        //this._collider.isTrigger = false;
        PlayerRB.rotation = (0);
        _bouncing = false;
        _attacking = false;
        _canMove = true;
        TryAttack();
        //_animator.SetBool("Attack", false);
    }

    void TryAttack()
    {
        if (_attacking == false && _hasGrounded == true)
        {
            _canAttack = true;
            Attack();
        }
        else if (_attacking == false && _grounded == true)
        {
            _canAttack = true;
            Attack();
        }
    }

    //private IEnumerator attackDuration()
    //{
    //    yield return new WaitForSeconds(_attackWindup);
    //    GameObject AttackInstance = Instantiate(_explosion, _self.position, _self.rotation);
    //    PlayerRB.velocity = new Vector2(moveDirection * _explosionPower, _explosionPower);
    //    PlayerRB.AddTorque(_rotationAmount * moveDirection);
    //    this._collider.sharedMaterial = _bounceMaterial;
    //    this.PlayerRB.sharedMaterial = _baseMaterial;
    //    //this._collider.isTrigger = true;
    //    yield return new WaitForSeconds(_attackDuration);
    //    StartCoroutine(attackCooldown());
    //}

    //private IEnumerator attackCooldown()
    //{
    //    this._collider.sharedMaterial = _baseMaterial;
    //    this.PlayerRB.sharedMaterial = _baseMaterial;
    //    //this._collider.isTrigger = false;
    //    PlayerRB.rotation = (0);
    //    _attacking = false;
    //    _canMove = true;
    //    //_animator.SetBool("Attack", false);
    //    yield return new WaitForSeconds(_timeAttackCooldown);
    //    _canAttack = true;
    //}

    private void Handle_ToMenuPerformed(InputAction.CallbackContext context)
    {
        SceneManager.LoadScene("IntroCutscene");
    }
    private void Handle_JumpAction(InputAction.CallbackContext obj)
    {
        if (_canMove == true)
        {
            //Checks if the player is touching the ground
            if (coyoteTimeCounter > 0f)
            /// if (IsColliding == true)
            {
                //Makes the player jump command activate
                playerJump = true;
                //Allows the PerformLaunch Command to be allowed.
                PerformLaunch = true;
                //Makes it so the double jump doesn't activate
                //DoubleJump = false;
            }
            else
            {

                //Makes is so the player can't jump, but they are able to double jump
                playerJump = false;
                PerformLaunch = true;
                //if (CanDoubleJump == true)
                //{
                //    DoubleJump = true;
                //    //Animator.SetBool("DoubleJump", true);
                //}



            }
            if (playerJump == true)
            {
                PlayerShouldBeMoving = true;
            }
        }
    }
    private void Handle_MoveStarted(InputAction.CallbackContext obj)
    {
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
        if (_attacking == true)
        {
            GameObject AttackInstance = Instantiate(_explosion, _self.position, _self.rotation);
        }

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
                PlayerRB.velocity = new Vector2(_explosionPower * _lastDirection * _bouncepadBoost, _explosionPower * _bouncepadBoost);
            }
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
        if (PerformLaunch == true && playerJump == true)
        {
            //CoyoteTimer = 0;
            //IsColliding = false;

            if (transform.parent != null)
            {
                PlayerRB.velocity = new Vector2(transform.parent.GetComponent<Rigidbody2D>().velocity.x, JumpForce + transform.parent.GetComponent<Rigidbody2D>().velocity.y);
                coyoteTimeCounter = 0;
            }
            else
            {
                PlayerRB.velocity = new Vector2(0, JumpForce);
                coyoteTimeCounter = 0;
            }
            //print("PlayerRB should be jumpin");


            //Launches the player upwards

            //Turns off the player jump
            playerJump = false;
            PerformLaunch = false;
            //Turns on the jumping animation
            //Animator.SetBool("IsMoving", true);
            //Animator.SetBool("OnGround", false);
            //Animator.SetBool("InAir", true);
            //Animator.SetBool("DoubleJump", false);
            //Animator.SetBool("Dash", false);

            InAir = true;
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
            }
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
            PlayerRB.rotation = (0);
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

        if (_velocityY == 0 && _moving == true)
        {
            //_animator.SetBool("Walking", true);

        }

        if (_velocityY >= 0)
        {
            //_animator.SetBool("Jumping", true);
        }

        if (_velocityY <= 0)
        {
            //_animator.SetBool("Falling", true);
        }

        if (_velocityY == 0)
        {
            //_animator.SetBool("Jumping", false);
            //_animator.SetBool("Falling", false);
        }

        _velocityX = PlayerRB.velocity.x;
        _velocityY = PlayerRB.velocity.y;

        //if (PlayerRB.velocity.y <= 0)
        //{
        //    _animator.SetBool("Falling", true);
        //} 

        if (PlayerRB.velocity.y != 0 || _moving == false)
        {
            //_animator.SetBool("Walking", false);
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
            gameObject.transform.localScale = new Vector2(1, 1);
            _lastDirection = 1;
        }
        if (moveDirection < 0)
        {
            gameObject.transform.localScale = new Vector2(-1, 1);
            _lastDirection = -1;
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