using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;


public class PlayerControls : MonoBehaviour
{
    [SerializeField] private GameObject pauseFirstButton;

    public PlayerInput MPI;
    private InputAction move;
    private InputAction restart;
    private InputAction quit;
    private InputAction attack;
    private InputAction up;
    private InputAction down;
    public bool Up;
    public bool Down;

    //private float coyoteTime = 0.2f;
    //private float coyoteTimeCounter;
    public float PlayerSpeed;
    public bool PlayerShouldBeMoving;
    private bool playerJump;
    public Rigidbody2D PlayerRB;
    [SerializeField] private float moveDirection;
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
    [SerializeField] private bool _cutscene;
    [SerializeField] private bool _canExplode;
    [SerializeField] private bool _showFuse;
    [SerializeField] private bool _dying;
    [SerializeField] private float _explosionPower;
    [SerializeField] private float _rotationAmount;
    //[SerializeField] private float _shakyCamDuration;

    [SerializeField] private GameObject _explosion;
    [SerializeField] private GameObject _bouncyExplosion;
    [SerializeField] private GameObject _deathTransition;
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private Transform _self;

    [SerializeField] private bool _grounded;
    [SerializeField] private bool _moving;
    [SerializeField] private bool _hasGrounded;
    [SerializeField] private bool _bouncing;
    [SerializeField] private float _bouncepadBoost;
    [SerializeField] private bool _bombed;
    [SerializeField] private float _bombBoost;
    [SerializeField] private float _slowDownSpeed;
    [SerializeField] private float _stopAttackTimerMax;
    [SerializeField] private float _stopAttackTimer;

    [SerializeField] private GameObject _bouncyParticles;
    [SerializeField] private GameObject _bombParticles;
    [SerializeField] private GameObject _fuseParticles;
    [SerializeField] private GameObject _explosionBox;
    [SerializeField] private GameObject _interactIcon;
    //[SerializeField] private GameObject _shakyCam;
    //[SerializeField] private GameObject _basicCam;
    [SerializeField] private Animator _animator;

    [SerializeField] private float _velocityX;
    [SerializeField] private float _velocityY;
    [SerializeField] private float _currentDirection;
    [SerializeField] private Collider2D _collider;
    [SerializeField] private PhysicsMaterial2D _bounceMaterial;
    [SerializeField] private PhysicsMaterial2D _baseMaterial;
    [SerializeField] private ParticleSystem _dust;

    [SerializeField] private bool paused;

    // public GameManager GM;

    // public static PlayerMovement instance;
    // Start is called before the first frame update
    [SerializeField] private CheckpointManager _checkpointManager;

    private void Awake()
    {
        if (_canExplode == true)
        {
            _animator.SetBool("TNT", true);
        }
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
        up = MPI.currentActionMap.FindAction("Up");
        down = MPI.currentActionMap.FindAction("Down");
        //release = MPI.currentActionMap.FindAction("Release");

        MPI.currentActionMap.Enable();
        move.started += Handle_MoveStarted;
        move.canceled += Handle_MoveCanceled;
        restart.performed += Handle_RestartPerformed;
        quit.performed += Handle_QuitPerformed;
        attack.performed += Handle_Attack;
        up.started += Handle_Up;
        up.canceled += Handle_UpStop;
        down.started += Handle_Down;
        down.canceled += Handle_DownStop;
        //if (_animator == null)
        //{
        //    _animator.enabled = true;
        //}

    }

    public void OnDisable()
    {
        MPI.currentActionMap.Disable();
        move.started -= Handle_MoveStarted;
        move.canceled -= Handle_MoveCanceled;
        restart.performed -= Handle_RestartPerformed;
        quit.performed -= Handle_QuitPerformed;
        attack.performed -= Handle_Attack;
        //_animator.enabled = false;
        up.started -= Handle_Up;
        up.canceled -= Handle_UpStop;
        down.started -= Handle_Down;
        down.canceled -= Handle_DownStop;
    }

    private void Handle_Up(InputAction.CallbackContext obj)
    {
        Up = true;
        Down = false;
    }

    private void Handle_UpStop(InputAction.CallbackContext obj)
    {
        Up = false;
    }

    private void Handle_Down(InputAction.CallbackContext obj)
    {
        Down = true;
        Up = false;
    }

    private void Handle_DownStop(InputAction.CallbackContext obj)
    {
        Down = false;
    }

    private void Handle_Attack(InputAction.CallbackContext obj)
    {
        if (_canExplode == true && _dying == false && paused == false)
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
    }

    void Attack()
    {
        //_basicCam.SetActive(false);
        //_shakyCam.SetActive(true);
        //StopAllCoroutines();
        //StartCoroutine(StopShakyCam());
        GameObject AttackInstance = Instantiate(_explosion, _self.position, _self.rotation);
        _explosionBox.SetActive(true);
        PlayerRB.freezeRotation = false;
        PlayerShouldBeMoving = true;
        _canAttack = false;
        _attacking = true;
        _showFuse = false;
        _canMove = false;
        _hasGrounded = false;
        //_animator.SetBool("Attack", true);
        //GameObject AttackInstance = Instantiate(_explosion, _self.position, _self.rotation);
        //if (moveDirection != 0)
        //{
            if (Up == true)
            {
                PlayerRB.velocity = new Vector2(_lastDirection * _explosionPower, _explosionPower);
            }
            else if (Down == true)
            {
                PlayerRB.velocity = new Vector2(_lastDirection * _explosionPower, -_explosionPower * .95f);
            } 
            else
            {
                PlayerRB.velocity = new Vector2(_lastDirection * _explosionPower * 1.5f, 0);
            }
            PlayerRB.AddTorque(_rotationAmount * moveDirection);
        //} else if (moveDirection == 0)
        /*{
            if (Up == true)
            {
                PlayerRB.velocity = new Vector2(_lastDirection * _explosionPower, _explosionPower);
            }
            else if (Down == true)
            {
                PlayerRB.velocity = new Vector2(_lastDirection * _explosionPower, -_explosionPower);
            }
            else
            {
                PlayerRB.velocity = new Vector2(_lastDirection * _explosionPower * 1.5f, 0);
            }
            PlayerRB.AddTorque(_rotationAmount * _lastDirection);
        }*/
        this._collider.sharedMaterial = _bounceMaterial;
        this.PlayerRB.sharedMaterial = _baseMaterial;
        // StartCoroutine(attackDuration());
    }

    //public IEnumerator StopShakyCam()
    //{
    //    yield return new WaitForSeconds(_shakyCamDuration);
    //    _basicCam.SetActive(true);
    //    _shakyCam.SetActive(false);
        
    //}

    void StopAttack()
    {
        //StopAllCoroutines();
        //StartCoroutine(StopShakyCam());
        _stopAttackTimer = _stopAttackTimerMax;
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
        _explosionBox.SetActive(false);
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
        if (_cutscene == false)
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

        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Bouncy")
        {
            PlayerRB.rotation = (0);
        }

        if (collision.gameObject.tag == "Ground")
        {
            if (_attacking && _bouncing == false)
            {
                PlayerRB.velocity = new Vector2(PlayerRB.velocity.x / _slowDownSpeed, PlayerRB.velocity.y / _slowDownSpeed);
            }
        }

        if (collision.gameObject.tag == "Killbox")
        {
            _dying = true;
            _bombParticles.SetActive(false);
            _bouncyParticles.SetActive(false);
            _fuseParticles.SetActive(false);
            _animator.SetBool("Die", true);
        }
        if (collision.gameObject.tag == "Bouncy")
        {
            if (_attacking == true)
            {
                print("bouncy");
                _bouncing = true;
                PlayerRB.velocity = new Vector2(_explosionPower * _currentDirection * _bouncepadBoost, _explosionPower * _bouncepadBoost);
                _bouncyParticles.SetActive(true);
            }
        }
        if (collision.gameObject.tag == "Bomb")
        {
            Attack();
            _bombed = true;
            PlayerRB.velocity = new Vector2(_explosionPower * _currentDirection * _bombBoost, _explosionPower * _bombBoost * -1);
            _bombParticles.SetActive(true);
        }
    }

    public void DeathTransition()
    {
        _deathTransition.SetActive(true);
    }

    public void Die()
    {
        RestartGame();
    }

    public void FixedUpdate()
    {

        if (_grounded == true && _attacking == true && _bouncing == false)
        {
            _stopAttackTimer -= Time.deltaTime;
            if (_stopAttackTimer <= 0)
            {
                _stopAttackTimer = _stopAttackTimerMax;
                StopAttack();
            }
        }

        if (_attacking == false)
        {
            //print("Not attacking");
            PlayerRB.rotation = (0);
        }

        if (PlayerShouldBeMoving == true)
        {
            if (_canMove == true)
            {
                //print("PlayerRB Should Be Moving");
                //Makes the player able to move, and turns on the moving animation   
                PlayerRB.velocity = new Vector2(PlayerSpeed * moveDirection, PlayerRB.velocity.y);
                //Animator.SetBool("IsMoving", true);
            }

        }
        
        if (_cutscene == true || _dying == true)
        {
            PlayerRB.velocity = new Vector2(0, 0);
            PlayerRB.rotation = (0);
            PlayerRB.freezeRotation = true;
        }

    }


    public void EquipTNTEndCutscene()
    {
        _animator.SetBool("TNT", true);
        _animator.SetBool("EquipTNTCutscene", false);
        _cutscene = false;
    }
    public void UnequipTNTEndCutscene()
    {
        _showFuse = false;
        _animator.SetBool("TNT", false);
        _animator.SetBool("UnequipTNTCutscene", false);
        _cutscene = false;
    }
    
    public void CanExplode()
    {
        _showFuse = true;
        _canExplode = true;
    }
    public void CannotExplode()
    {
        _canExplode = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Dialogue")
        {
            _interactIcon.SetActive(true);
        }

        if (collision.gameObject.tag == "Refresh")
        {
            _canAttack = true;
            _canExplode = true;
            _showFuse = true;
        }

        if (collision.gameObject.tag == "Killbox")
        {
            _dying = true;
            _bombParticles.SetActive(false);
            _bouncyParticles.SetActive(false);
            _fuseParticles.SetActive(false);
            _animator.SetBool("Die", true);
        }
        if (collision.gameObject.tag == "EquipTNTCutscene")
        {
            _animator.SetBool("EquipTNTCutscene", true);
            _cutscene = true;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "UnequipTNTCutscene")
        {
            StopAttack();
            _animator.SetBool("UnequipTNTCutscene", true);
            _cutscene = true;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "Cutscene")
        {
            _cutscene = true;
            _holdingMove = false;
            _moving = false;
        }
        if (collision.gameObject.tag == "GiveTNT")
        {
            _canExplode = true;
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
            PlayerRB.velocity = new Vector2 (0,0);
            PlayerRB.rotation = (0);
        }
        //if (collision.gameObject.tag == "Cutscene")
        //{
        //    _cutscene = false;
        //}
    }

    public void Grounded()
    {
            PlayerRB.gravityScale = BaseGravity;
            _grounded = true;
            //print("Touch Grass");
            InAir = false;
            _colliding = true;
            if (_attacking == false)
            {
                if (_canExplode == true)
                {
                    _showFuse = true;
                }
                _hasGrounded = true;
                PlayerRB.rotation = (0);
            }
    }

    public void NotGrounded()
    {
        //Makes the game realize the player is not touching the ground
            //print("Dont Touch Grass");
            InAir = true;
            // CoyoteTime = true;
            // IsColliding = false;
            _colliding = false;
            //CanDoubleJump = true;
            //print("Left Grass");
            _grounded = false;
            if (_attacking == false)
            {
                _hasGrounded = true;
            }

        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Cutscene")
        {
            _cutscene = false;
        }

        if (collision.tag == "Dialogue")
        {
            _interactIcon.SetActive(false);
        }
    }

    // Update is called once per frame
    public void Update()
    {

        if (_showFuse == true)
        {
            _fuseParticles.SetActive(true);
        } else
        {
            _fuseParticles.SetActive(false);
        }

        if (!paused)
        {
            Time.timeScale = 1;
            //Time.fixedDeltaTime = Time.deltaTime;
        } else if (paused)
        {
            Time.timeScale = 0;
            //Time.fixedDeltaTime = Time.deltaTime;
        }


        if (_canExplode == true)
        {
            _animator.SetBool("TNT", true);
        }

        PlayerRB.gravityScale = BaseGravity;

        if (_moving == true && _velocityY > -2)
        {
            if (_bouncing == false)
            {
                _animator.SetBool("Walking", true);
                _dust.Play();
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
            _dust.Stop();
        }


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
        
        if (_lastDirection > 0 && !_attacking)
        {
            gameObject.transform.localScale = new Vector2(1, 1);
            _currentDirection = 1;
        }
        else if (_lastDirection < 0 && !_attacking)
        {
            gameObject.transform.localScale = new Vector2(-1, 1);
            _currentDirection = -1;
        }

        if (_velocityX > 0 && _attacking)
        {
            gameObject.transform.localScale = new Vector2(1, 1);
            _currentDirection = 1;
        } else if (_velocityX < 0 && _attacking)
        {
            gameObject.transform.localScale = new Vector2(-1, 1);
            _currentDirection = -1;
        }
    }
    private void Handle_QuitPerformed(InputAction.CallbackContext onj)
    {
        Application.Quit();
        print("Quit");
    }

    private void Handle_RestartPerformed(InputAction.CallbackContext obj)
    {
        if (!paused)
        {
            Pause();
        } else
        {
            UnPause();
        }
    }

    public void Pause()
    {
        print("Pause");
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(pauseFirstButton);
        _pauseMenu.SetActive(true);
        paused = true;
    }

    public void UnPause()
    {
        Time.timeScale = 1;
       // Time.fixedDeltaTime = Time.deltaTime;
        paused = false;
        _pauseMenu.SetActive(false);
    }
    
    public void Respawn()
    {
        Time.timeScale = 1;
       // Time.fixedDeltaTime = Time.deltaTime;
        paused = false;
        _pauseMenu.SetActive(false);
        _dying = true;
        _bombParticles.SetActive(false);
        _bouncyParticles.SetActive(false);
        _fuseParticles.SetActive(false);
        _animator.SetBool("Die", true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void StartCutscene()
    {
        _cutscene = true;
    }

    public void StopCutscene()
    {
        _cutscene = false;
    }

}