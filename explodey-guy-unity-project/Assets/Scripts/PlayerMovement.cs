using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    private GameObject _self;
    private float _horizontal;
    private float _backpackControl;
    [SerializeField] private float _backpackFloat;
    private float _vertical;
    [SerializeField] private float _speed;
    private float _baseSpeed;
    [SerializeField] private float _explosionPower;
    private float _lastHorizontal;
    private bool _grounded;
    private bool _attacking;
    private bool _canAttack;
    private bool _attackDelayActive;
    private bool _paused;
    private bool _timeStopWaiting;
    private bool _dying;
    private bool _hasSpawned;
    private CheckpointManager _checkpointManager;
    private Rigidbody2D _rigidbody;
    private AudioManager audioManager;
    private GameObject audioManagerObject;
    [SerializeField] private GameObject _explosion;
    private GameObject _fuseParticlesGameobject;
    private ParticleSystem _fuseParticles;
    private Collider2D _collider;
    [SerializeField] private PhysicsMaterial2D _bounceMaterial;
    [SerializeField] private PhysicsMaterial2D _baseMaterial;
    private GameObject _pauseMenu;
    private GameObject _pauseFirstButton;
    private Animator _pAnim;
    private Animator _backpackAnims;
    private GameObject _pauseExplodeParticleGameObject;
    private ParticleSystem _pauseExplodeParticle;
    private GameObject _fuseLightGameobject;
    private Light2D _fuseLight;
    private GameObject _bouncyParticles;
    private GameObject _bombParticles;
    private GameObject _deathTransition;
    private GameObject _tntBackpack;
    private GameObject _arrow;
    private Animator _arrowAnims;
    private int _backpackInt;
    [SerializeField] private GameObject[] _arrowPoints;
    [SerializeField] private string[] _backpack;
    private DataPersistenceManager _dataPersistanceManager;
    private GameObject _dataPersistanceManagerGameobject;
    private GameManager _gameManager;
    private GameObject _gameManagerGameobject;

    private void Awake()
    {
        _dataPersistanceManagerGameobject = GameObject.Find("DataPersistanceManager");
        if (_dataPersistanceManagerGameobject != null)
        {
            _dataPersistanceManager = _dataPersistanceManagerGameobject.GetComponent<DataPersistenceManager>();
        }
        _gameManagerGameobject = GameObject.Find("GameManager");
        if (_gameManagerGameobject != null)
        {
            _gameManager = _gameManagerGameobject.GetComponent<GameManager>();
        }
        _dying = false;
        _lastHorizontal = 1;
        _baseSpeed = _speed;
        _rigidbody = GetComponent<Rigidbody2D>();
        _self = GameObject.Find("Player");
        if (_self != null)
        {
            _rigidbody = _self.GetComponent<Rigidbody2D>();
            _collider = _self.GetComponent<CircleCollider2D>();
            _pAnim = _self.GetComponent<Animator>();
        }
        _fuseParticlesGameobject = GameObject.Find("Fuse");
        if (_fuseParticlesGameobject != null)
        {
            _fuseParticles = _fuseParticlesGameobject.GetComponent<ParticleSystem>();
        }
        _fuseLightGameobject = GameObject.Find("FuseLight");
        if (_fuseLightGameobject != null)
        {
            _fuseLight = _fuseLightGameobject.GetComponent<Light2D>();
        }
        _tntBackpack = GameObject.Find("TNTBackpack");
        if (_tntBackpack != null)
        {
            _backpackAnims = _tntBackpack.GetComponent<Animator>();
        }
        _pauseExplodeParticleGameObject = GameObject.Find("PauseExplodeParticles");
        if (_pauseExplodeParticleGameObject != null)
        {
            _pauseExplodeParticle = _pauseExplodeParticleGameObject.GetComponent<ParticleSystem>();
        }
        _bouncyParticles = GameObject.Find("Bouncy Effect");
        if (_bouncyParticles != null)
        {
            _bouncyParticles.SetActive(false);
        }
        _bombParticles = GameObject.Find("Bomb Effect");
        if (_bouncyParticles != null)
        {
            _bombParticles.SetActive(false);
        }
        _arrow = GameObject.Find("Arrow");
        if (_arrow != null)
        {
            _arrowAnims = _arrow.GetComponent<Animator>();
            _arrow.SetActive(false);
        }
        _tntBackpack = GameObject.Find("TNTBackpack");
        NoSpeed();
        Invoke("FindEverything", .3f);
        
    }

    public void FindEverything()
    {
        NormalSpeed();
        _pauseMenu = GameObject.Find("Pause Menu");
        if (_pauseMenu != null)
        {
            _pauseFirstButton = GameObject.Find("PauseTopButton");
            _pauseMenu.SetActive(false);
        }
        _checkpointManager = FindObjectOfType<CheckpointManager>();
        if (_checkpointManager != null)
        {
            transform.position = _checkpointManager.LastCheckPointPos;
        }
        audioManagerObject = GameObject.Find("Audio Manager");
        if (audioManagerObject != null)
        {
            audioManager = audioManagerObject.GetComponent<AudioManager>();
        }
        _deathTransition = GameObject.Find("DeathTransition");
        if (_deathTransition != null)
        {
            _deathTransition.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        if (_dataPersistanceManager != null && _hasSpawned == false)
        {
            _hasSpawned = true;
            _self.transform.position = new Vector2(_dataPersistanceManager.GameData.PlayerPosX, _dataPersistanceManager.GameData.PlayerPosY);

        }

        if (_grounded && !_attacking)
        {
            _rigidbody.velocity = new Vector2(0, 0);
            if (_horizontal == 0)
            {
                _pAnim.SetBool("Walking", false);
            } else
            {
                _pAnim.SetBool("Walking", true);
            }
        }

        if (!_attacking)
        {
            _rigidbody.velocity = new Vector2(_horizontal * _speed, _rigidbody.velocity.y);
            if (_horizontal > 0)
            {
                _horizontal = 1;
                _lastHorizontal = _horizontal;
                gameObject.transform.localScale = new Vector2(1, 1);
            }
            else if (_horizontal < 0)
            {
                _horizontal = -1;
                _lastHorizontal = _horizontal;
                gameObject.transform.localScale = new Vector2(-1, 1);
            }

            if (_vertical > 0)
            {
                _vertical = 1;
            } 
            else if (_vertical < 0)
            {
                _vertical = -1;
            }

            if (_rigidbody.velocity.y < 0)
            {
                _pAnim.SetBool("Falling", true);
            }
            else
            {
                _pAnim.SetBool("Falling", false);
            }
        } 
        else
        {
            _speed = 0;
        }

        if (_attacking)
        {
            _pAnim.SetBool("Walking", false);
            _arrow.SetActive(false);
        }

        if (_dying == true)
        {
            _rigidbody.velocity = new Vector2(0, 0);
            _rigidbody.rotation = (0);
            _rigidbody.freezeRotation = true;
            _arrow.SetActive(false);
            _tntBackpack.SetActive(false);
        }

        if (_canAttack && !_dying && !_attacking)
        {
            _arrow.SetActive(true);
            _fuseParticlesGameobject.SetActive(true);
            CheckParticleColor();
            _arrowAnims.Play("Arrow_" + _backpack[_backpackInt]);
            if (_horizontal == 0 && _vertical == 1)
            {
                _arrow.transform.position = _arrowPoints[0].transform.position;
                _arrow.transform.rotation = _arrowPoints[0].transform.rotation;
            }
            else if (_horizontal == 1 && _vertical == 1 || _horizontal == -1 && _vertical == 1)
            {
                _arrow.transform.position = _arrowPoints[1].transform.position;
                _arrow.transform.rotation = _arrowPoints[1].transform.rotation;
            }
            else if (_horizontal == 1 && _vertical == 0 || _horizontal == 0 && _vertical == 0 || _horizontal == -1 && _vertical == 0)
            {
                _arrow.transform.position = _arrowPoints[2].transform.position;
                _arrow.transform.rotation = _arrowPoints[2].transform.rotation;
            }
            else if (_horizontal == 1 && _vertical == -1 || _horizontal == -1 && _vertical == -1)
            {
                _arrow.transform.position = _arrowPoints[3].transform.position;
                _arrow.transform.rotation = _arrowPoints[3].transform.rotation;
            }
            else if (_horizontal == 0 && _vertical == -1)
            {
                _arrow.transform.position = _arrowPoints[4].transform.position;
                _arrow.transform.rotation = _arrowPoints[4].transform.rotation;
            }
        }
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!_attackDelayActive && !_paused)
            {
                _attackDelayActive = true;
                if (_attacking)
                {
                    StopAttack();
                }
                else if (!_attacking && _canAttack)
                {
                    _grounded = false;
                    _attacking = true;
                    _canAttack = false;
                    AttackAction();
                }

                Invoke("AttackDelay", .15f);
            }
        }
    }

    void AttackDelay()
    {
        _attackDelayActive = false;
    }

    void AttackAction()
    {
        _attacking = true;
        HitStop(.08f);
        audioManager.Explosion();
        GameObject AttackInstance = Instantiate(_explosion, this.transform.position, this.transform.rotation);
        _rigidbody.freezeRotation = false;
        _fuseParticlesGameobject.SetActive(false);
        if (_horizontal != 0 || _vertical != 0)
        {
            _rigidbody.velocity = new Vector2(_horizontal * _explosionPower, _vertical * _explosionPower);
        } 
        else if (_horizontal == 0 && _vertical == 0)
        {
            print("nothin");
            _rigidbody.velocity = new Vector2(_explosionPower * 1 * _lastHorizontal, _vertical * _explosionPower);
        }
            _rigidbody.AddTorque(9999);
        this._collider.sharedMaterial = _bounceMaterial;
    }

    public void HitStop(float duration)
    {
        if (!_timeStopWaiting)
        {
            Time.timeScale = 0;
        }
        StartCoroutine(Wait(duration));
    }

    IEnumerator Wait(float duration)
    {
        _timeStopWaiting = true;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1;
        _timeStopWaiting = false;
    }

    void StopAttack()
    {
        _pAnim.SetBool("ExplodeStop", true);
        _speed = _baseSpeed;
        this._collider.sharedMaterial = _baseMaterial;
        _bombParticles.SetActive(false);
        _bouncyParticles.SetActive(false);
        _rigidbody.rotation = (0);
        _rigidbody.freezeRotation = true;
        _attacking = false;
        _rigidbody.velocity = new Vector2(0, 0);
        audioManager.PauseExplosion();
    }

    public void Move(InputAction.CallbackContext context)
    {
        _horizontal = context.ReadValue<Vector2>().x;
        _vertical = context.ReadValue<Vector2>().y;
        if (_speed > 0)
        {
            _pAnim.SetBool("Walking", true);
        }
    }

    public void BackpackSwap(InputAction.CallbackContext context)
    {
        if (context.performed && _grounded && !_attacking)
        {
            _backpackControl = context.ReadValue<Vector2>().x;
            _backpackFloat += 1 * _backpackControl;
            if (_backpackFloat < 0)
            {
                _backpackFloat = 3;
            }
            else if (_backpackFloat > 3)
            {
                _backpackFloat = 0;
            }
            _backpackInt = (int)_backpackFloat;
            print(_backpackInt);
            print(_backpack[_backpackInt]);
            _backpackAnims.Play("TNT_" + _backpack[_backpackInt]);
            _arrowAnims.Play("Arrow_" + _backpack[_backpackInt]);
            CheckParticleColor();
        }
    }

   public void CheckParticleColor()
    {
        if (_backpackInt == 0)
        {
            _fuseParticles.startColor = new Color(1, 0, 0);
            _fuseLight.color = new Color(1, 0, 0);
        }
        else if (_backpackInt == 1)
        {
            _fuseParticles.startColor = new Color(1f, 0.07843138f, 0.5764706f);
            _fuseLight.color = new Color(1f, 0.07843138f, 0.5764706f);
        }
        else if (_backpackInt == 2)
        {
            _fuseParticles.startColor = new Color(0, 0, 1);
            _fuseLight.color = new Color(0, 0, 1);
        }
        else if (_backpackInt == 3)
        {
            _fuseParticles.startColor = new Color(0, 1, 0);
            _fuseLight.color = new Color(0, 1, 0);
        }
    }

    public void NoSpeed()
    {
        _speed = 0;
        _rigidbody.gravityScale = 0;
        _pauseExplodeParticle.Play();
    }

    public void NormalSpeed()
    {
        _speed = 20;
        _rigidbody.gravityScale = 10;
        _pAnim.SetBool("ExplodeStop", false);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Bouncy"))
        {
            if (!_attacking)
            {
                //_pAnims.SetGrounded(true);
                _grounded = true;
                _canAttack = true;
                _fuseParticlesGameobject.SetActive(true);
            } 
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Killbox" && _dying != true)
        {
            _dying = true;
            _fuseParticlesGameobject.SetActive(false);
            _pAnim.SetBool("Die", true);
            audioManager.Death();
        }

        if (collision.CompareTag("Checkpoint"))
        {
            _dataPersistanceManager.GameData.PlayerPosX = (collision.transform.position.x);
            _dataPersistanceManager.GameData.PlayerPosY = (collision.transform.position.y);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Bouncy"))
        {
            _grounded = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" && audioManagerObject != null || collision.gameObject.tag == "Bouncy" && audioManagerObject != null)
        {
            audioManager.Land();
        }
        if (collision.gameObject.tag == "Ground")
        {
            if (_attacking)
            {
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x / 1.1f, _rigidbody.velocity.y / 1.1f);
            }
        }

        if (collision.gameObject.tag == "Bouncy" && _attacking)
        {
            print("bouncy");
            _rigidbody.velocity = new Vector2(_explosionPower * _horizontal * 1.1f, _explosionPower * _vertical * 1.1f);
            _bouncyParticles.SetActive(true);
        }

        if (collision.gameObject.tag == "Bomb")
        {
            _attacking = true;
            _attackDelayActive = true;
            AttackAction();
            Invoke("AttackDelay", .3f);
            Vector2 Direction = (transform.position - collision.gameObject.transform.position).normalized;
            _rigidbody.velocity = new Vector2(Direction.x * _explosionPower * 2, Direction.y * _explosionPower * 2);
            _bombParticles.SetActive(true);
            _canAttack = true;
        }
    }

    public void Pause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!_paused)
            {
                Pause();
            }
            else
            {
                UnPause();
            }
        }
    }

    public void Pause()
    {
        print("Pause");
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_pauseFirstButton);
        _pauseMenu.SetActive(true);
        _paused = true;
        Time.timeScale = 0;
    }

    public void UnPause()
    {
        Time.timeScale = 1;
        _paused = false;
        _pauseMenu.SetActive(false);
    }

    public void Respawn()
    {
        _dying = true;
        Time.timeScale = 1;
        _pauseMenu.SetActive(false);
        _fuseParticlesGameobject.SetActive(false);
        _pAnim.SetBool("Die", true);
    }

    public void Die()
    {
        _dataPersistanceManager.Save();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void DeathTransition()
    {
        _deathTransition.SetActive(true);
    }

    public void disableMovement()
    {
        _speed = 0;
    }

    public void ResetData(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            print("Reset");
            _dataPersistanceManager.NewGame();
        }
    }
}
