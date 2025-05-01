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
    private InputAction side;
    private InputAction menu;
    private InputAction level;
    public bool Up;
    public bool Down;
    public bool Side;

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
    [SerializeField] private GameObject _smokeTrail;

    [SerializeField] private bool paused;

    // public GameManager GM;

    // public static PlayerMovement instance;
    // Start is called before the first frame update
    [SerializeField] private CheckpointManager _checkpointManager;

    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private SpriteRenderer _backpackSprite;
    [SerializeField] private SpriteRenderer _arrowSprite;

    [SerializeField] private GameObject _arrowSideUp;
    [SerializeField] private GameObject _arrowUp;
    [SerializeField] private GameObject _arrowSide;
    [SerializeField] private GameObject _arrowSideDown;
    [SerializeField] private GameObject _arrowDown;
    private bool _pauseExplodeAnimationActive;

    public ParticleSystem PauseExplodeParticle;

    public AudioManager audioManager;
    public GameObject audioManagerObject;

    private int bombPower;
    private bool bombing;

    [SerializeField] private bool gameRestarting = false;
    private void Awake()
    {
        audioManagerObject = GameObject.Find("Audio Manager");
        if (audioManagerObject != null)
        {
            audioManager = audioManagerObject.GetComponent<AudioManager>();
        }

        _lastDirection = 1;

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
        side = MPI.currentActionMap.FindAction("Side");
        menu = MPI.currentActionMap.FindAction("Menu");
        level = MPI.currentActionMap.FindAction("Level");

        //release = MPI.currentActionMap.FindAction("Release");

        MPI.currentActionMap.Enable();
        move.started += Handle_MoveStarted;
        move.canceled += Handle_MoveCanceled;
        restart.performed += Handle_RestartPerformed;
        menu.performed += BackToMenu;
        level.performed += ToLevel;
        quit.performed += Handle_QuitPerformed;
        attack.performed += Handle_Attack;
        up.started += Handle_Up;
        up.canceled += Handle_UpStop;
        side.started += Handle_Side;
        side.canceled += Handle_SideStop;
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
        side.started -= Handle_Side;
        side.canceled -= Handle_SideStop;
    }

    private void Handle_Up(InputAction.CallbackContext obj)
    {
        Up = true;
        Down = false;

    }
    
    private void BackToMenu(InputAction.CallbackContext obj)
    {
        if (gameRestarting == false)
        {
            gameRestarting = true;
            SceneManager.LoadScene("Menu");
        }
        else
        {
            return;
        }
    }
    
    private void ToLevel(InputAction.CallbackContext obj)
    {

        if (gameRestarting == false)
        {
            gameRestarting = true;
            SceneManager.LoadScene("FUSELevel");
        }
        else
        {
            return;
        }

    }

    private void Handle_UpStop(InputAction.CallbackContext obj)
    {
        Up = false;
    }

    private void Handle_Side(InputAction.CallbackContext obj)
    {
        Side = true;

    }

    private void Handle_SideStop(InputAction.CallbackContext obj)
    {
        Side = false;
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
        if (_canExplode == true && _dying == false && paused == false && _cutscene == false && _bombed == false)
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
        audioManager.Explosion();
        GameObject AttackInstance = Instantiate(_explosion, _self.position, _self.rotation);
        _explosionBox.SetActive(true);
        PlayerRB.freezeRotation = false;
        PlayerShouldBeMoving = true;
        _canAttack = false;
        _attacking = true;
        _showFuse = false;
        _canMove = false;
        _hasGrounded = false;
            if (Up == true && Side == true)
            {
                PlayerRB.velocity = new Vector2(_lastDirection * _explosionPower, _explosionPower);
            }
            else if (Up == true && Side == false)
            {
                PlayerRB.velocity = new Vector2(PlayerRB.velocity.x, _explosionPower);
            }
            else if (Down == true && Side == true)
            {
                PlayerRB.velocity = new Vector2(_lastDirection * _explosionPower, -_explosionPower * .95f);
            }
            else if (Down == true && Side == false)
            {
                PlayerRB.velocity = new Vector2(PlayerRB.velocity.x, -_explosionPower * .95f);
            }
            else
            {
                PlayerRB.velocity = new Vector2(_lastDirection * _explosionPower * 1.5f, 0);
            }
            PlayerRB.AddTorque(_rotationAmount/* * moveDirection*/);
        this._collider.sharedMaterial = _bounceMaterial;
        this.PlayerRB.sharedMaterial = _baseMaterial;
        // StartCoroutine(attackDuration());
    }

    void StopAttack()
    {
        _stopAttackTimer = _stopAttackTimerMax;
        if (_holdingMove == true)
        {
            _moving = true;
        } else
        {
            _moving = false;
        }
        //PlayerRB.velocity = new Vector2(0, 0);
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
        if (_attacking)
        {
            _attacking = false;
            PlayerRB.velocity = new Vector2(0, 0);
        }
        _canMove = true;
        //TryAttack();
        _animator.SetBool("Walking", false);
        _animator.SetBool("ExplodeStop", true);
        audioManager.PauseExplosion();
        bombing = false;
    }

    public void ExplodeStopAnimStop()
    {
        _animator.SetBool("ExplodeStop", false);
    }

    public void NoSpeed()
    {
        PlayerSpeed = 0;
        BaseGravity = 0;
        PauseExplodeParticle.Play();
    }

    public void NormalSpeed()
    {
        PlayerSpeed = 20;
        BaseGravity = 10;
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

        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Bouncy" || collision.gameObject.tag == "Ground2")
        {
            PlayerRB.rotation = (0);

            audioManager.Land();
        }

        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Ground2")
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
            //} else
            //{
            //    PlayerRB.velocity = new Vector2(PlayerRB.velocity.x, 50);
            //}
        }
        if (collision.gameObject.tag == "Bomb")
        {
            Attack();
            Vector2 Direction = (transform.position - collision.gameObject.transform.position).normalized;
            if (bombing)
            {
                if (bombPower < 3)
                {
                    bombPower += 1;
                }
                PlayerRB.velocity = new Vector2(Direction.x * _explosionPower * _bombBoost * bombPower * -1, _explosionPower * _bombBoost * bombPower * -1);
            } else
            {
                bombPower = 0;
                PlayerRB.velocity = new Vector2(Direction.x * _explosionPower * _bombBoost * -1, _explosionPower * _bombBoost * -1);
            }

                //PlayerRB.velocity = new Vector2(_explosionPower * _currentDirection * _bombBoost, _explosionPower * _bombBoost * -1);
                _bombParticles.SetActive(true);
            _bombed = true;
            bombing = true;
            Invoke("CanStopBomb", .5f);
        }
    }

    public void CanStopBomb()
    {
        _bombed = false;
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
        if (Up == true && _attacking == false && Side == true) // Side Up
        {
            _arrowUp.SetActive(false);
            _arrowDown.SetActive(false);
            _arrowSideUp.SetActive(true);
            _arrowSideDown.SetActive(false);
            _arrowSide.SetActive(false);
        } 
        if (Up == true && _attacking == false && Side == false) // Up
        {
            _arrowUp.SetActive(true);
            _arrowSideUp.SetActive(false);
            _arrowSideDown.SetActive(false);
            _arrowSide.SetActive(false);
            _arrowDown.SetActive(false);
        } 
        else if (Down == true && _attacking == false && Side == true) // SideDown
        {
            _arrowUp.SetActive(false);
            _arrowDown.SetActive(false);
            _arrowSideUp.SetActive(false);
            _arrowSideDown.SetActive(true);
            _arrowSide.SetActive(false);
        } 
        else if (Down == true && _attacking == false && Side == false) // down
        {
            _arrowUp.SetActive(false);
            _arrowDown.SetActive(true);
            _arrowSideUp.SetActive(false);
            _arrowSideDown.SetActive(false);
            _arrowSide.SetActive(false);
        } 
        else if (Down == false && Up == false && _attacking == false) // Side
        {
            _arrowUp.SetActive(false);
            _arrowDown.SetActive(false);
            _arrowSideUp.SetActive(false);
            _arrowSideDown.SetActive(false);
            _arrowSide.SetActive(true);
        } 

        if (_attacking == true || _showFuse == false)
        {
            _arrowUp.SetActive(false);
            _arrowDown.SetActive(false);
            _arrowSideUp.SetActive(false);
            _arrowSideDown.SetActive(false);
            _arrowSide.SetActive(false);
        }


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
            _smokeTrail.SetActive(false);
        } else
        {
            _smokeTrail.SetActive(true);
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

        if (collision.gameObject.tag == "Killbox" && _dying != true)
        {
            _dying = true;
            _bombParticles.SetActive(false);
            _bouncyParticles.SetActive(false);
            _fuseParticles.SetActive(false);
            _animator.SetBool("Die", true);
            audioManager.Death();
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
        if (collision.gameObject.tag == "Disappear")
        {
            _cutscene = true;
            _spriteRenderer.enabled = false;
            _holdingMove = false;
            _moving = false;
        }
        if (collision.gameObject.tag == "Reappear")
        {
            _cutscene = false;
            _spriteRenderer.enabled = true;
        }
        //if (collision.gameObject.tag == "Disable")
        //{
        //    this.gameObject.SetActive(false);
        //}
       
        if (collision.gameObject.tag == "GiveTNT")
        {
            _canExplode = true;
        }
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Bouncy" || collision.gameObject.tag == "Ground2")
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
            if (_interactIcon != null)
            {
                _interactIcon.SetActive(false);
            }
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