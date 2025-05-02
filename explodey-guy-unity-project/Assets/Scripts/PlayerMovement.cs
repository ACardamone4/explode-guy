using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
     private GameObject _self;
     private float _horizontal;
     private float _vertical;
    [SerializeField] private float _speed;
    private float _baseSpeed;
    [SerializeField] private float _explosionPower;
    private float _lastHorizontal;
    private bool _grounded;
    private bool _attacking;
    [SerializeField] private bool _canAttack;
    private bool _attackDelayActive;
    private bool _paused;
    private bool _timeStopWaiting;
    [SerializeField] private bool _dying;
    //[SerializeField] private PlayerAnimations _pAnims;
    private CheckpointManager _checkpointManager;
    private Rigidbody2D _rigidbody;
    private AudioManager audioManager;
    private GameObject audioManagerObject;
    [SerializeField] private GameObject _explosion;
    private GameObject _fuseParticles;
    private Collider2D _collider;
    [SerializeField] private PhysicsMaterial2D _bounceMaterial;
    [SerializeField] private PhysicsMaterial2D _baseMaterial;
    private GameObject _pauseMenu;
    private GameObject _pauseFirstButton;
    private Animator _pAnim;
    private GameObject _pauseExplodeParticleGameObject;
    private ParticleSystem _pauseExplodeParticle;
    private GameObject _bouncyParticles;
    private GameObject _bombParticles;
    private GameObject _deathTransition;
    private GameObject _tntBackpack;
     private GameObject _arrow;
    [SerializeField] private GameObject[] _arrowPoints;

    private void Awake()
    {
        _dying = false;
        _lastHorizontal = 1;
        _baseSpeed = _speed;
        _fuseParticles = GameObject.Find("Fuse");
        _self = GameObject.Find("Player");
        if (_self != null)
        {
            _rigidbody = _self.GetComponent<Rigidbody2D>();
            _collider = _self.GetComponent<CircleCollider2D>();
            _pAnim = _self.GetComponent<Animator>();
        }
        _pauseMenu = GameObject.Find("Pause Menu");
        _pauseFirstButton = GameObject.Find("PauseTopButton");
        _pauseMenu.SetActive(false);
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
        _rigidbody = GetComponent<Rigidbody2D>();
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
        _deathTransition = GameObject.Find("DeathTransition");
        if (_deathTransition != null)
        {
            _deathTransition.SetActive(false);
        }
        _arrow = GameObject.Find("Arrow");
        _tntBackpack = GameObject.Find("TNTBackpack");
    }

    private void FixedUpdate()
    {
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

        if (_canAttack && !_dying)
        {
            _arrow.SetActive(true);
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
        if (!_attackDelayActive)
        {
            _attackDelayActive = true;

            print("fuck");
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

            Invoke("AttackDelay", .3f);
        }
        
    }

    void AttackDelay()
    {
        _attackDelayActive = false;
    }

    void AttackAction()
    {
        HitStop(.08f);
        audioManager.Explosion();
        GameObject AttackInstance = Instantiate(_explosion, this.transform.position, this.transform.rotation);
        _rigidbody.freezeRotation = false;
        _fuseParticles.SetActive(false);
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
                _fuseParticles.SetActive(true);
            } 
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Killbox" && _dying != true)
        {
            _dying = true;
            _fuseParticles.SetActive(false);
            _pAnim.SetBool("Die", true);
            audioManager.Death();
        }
    }

    public void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void DeathTransition()
    {
        _deathTransition.SetActive(true);
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
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Bouncy")
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
            AttackAction();
            Invoke("AttackDelay", .3f);
            Vector2 Direction = (transform.position - collision.gameObject.transform.position).normalized;
            _rigidbody.velocity = new Vector2(Direction.x * _explosionPower * 2, Direction.y * _explosionPower * 2);

            //PlayerRB.velocity = new Vector2(_explosionPower * _currentDirection * _bombBoost, _explosionPower * _bombBoost * -1);
            _bombParticles.SetActive(true);
        }
    }

    public void Pause(InputAction.CallbackContext context)
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
        Time.timeScale = 1;
        _pauseMenu.SetActive(false);
        _fuseParticles.SetActive(false);
        _pAnim.SetBool("Die", true);
    }

    public void disableMovement()
    {
        _speed = 0;
    }
}
