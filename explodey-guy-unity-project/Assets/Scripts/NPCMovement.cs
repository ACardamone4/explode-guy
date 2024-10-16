using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NPCMovement : MonoBehaviour
{
    [SerializeField] private bool _forward;
    [SerializeField] private bool _backward;
    [SerializeField] private bool _grounded;
    [SerializeField] private bool _falling;
    [SerializeField] private bool _moving;
    [SerializeField] private Rigidbody2D _rigidBody;
    [SerializeField] private GameObject _explosion;
    [SerializeField] private Transform _self;
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _jumpPower;
    [SerializeField] private float _moveDirection;
    [SerializeField] private float _explosionPower;
    [SerializeField] private Collider2D _collider;
    [SerializeField] private PhysicsMaterial2D _bounceMaterial;
    [SerializeField] private PhysicsMaterial2D _baseMaterial;
    [SerializeField] private float _rotationAmount;
    [SerializeField] private Animator _animator;
    [SerializeField] private string _color;
    [SerializeField] private string _equipment;

    public void Awake()
    {
        

        print(_color + "_Action_" + _equipment);
    }

    public void Update()
    {
        if (_forward)
        {
            _rigidBody.velocity = new Vector2(_movementSpeed, _rigidBody.velocity.y);
            gameObject.transform.localScale = new Vector2(1, 1);
            _moveDirection = 1;
            _moving = true;
            _animator.Play(_color + "_NPC_Walking_" + _equipment);
        }
        if (_backward)
        {
            _rigidBody.velocity = new Vector2(_movementSpeed * -1, _rigidBody.velocity.y);
            gameObject.transform.localScale = new Vector2(-1, 1);
            _moveDirection = -1;
            _moving = true;
            _animator.Play(_color + "_NPC_Walking_" + _equipment);
        }

        if (_rigidBody.velocity.y < -1)
        {
            _animator.Play(_color + "_NPC_Fall_" + _equipment);
        } 
        else if (_rigidBody.velocity.y >= 0 && _moving == false)
        {

            _animator.Play(_color + "_NPC_Idle_" + _equipment);
        }
    }

    public void Stop()
    {
        _rigidBody.velocity = new Vector2(0, 0);
        _forward = false;
        _backward = false;
        _moving = false;
        _animator.Play(_color + "_NPC_Idle_" + _equipment);
    }

    public void Jump()
    {
        _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, _jumpPower);
        _animator.Play(_color + "_NPC_Fall_" + _equipment);
    }
    public void Explode()
    {
        GameObject AttackInstance = Instantiate(_explosion, _self.position, _self.rotation);
        _rigidBody.freezeRotation = false;
        _rigidBody.velocity = new Vector2(_moveDirection * _explosionPower, _explosionPower);
        _rigidBody.AddTorque(_rotationAmount * _moveDirection);
        this._collider.sharedMaterial = _bounceMaterial;
        this._rigidBody.sharedMaterial = _baseMaterial;
    }

    public void StopExplode()
    {
        this._collider.sharedMaterial = _baseMaterial;
        this._rigidBody.sharedMaterial = _baseMaterial;
        //this._collider.isTrigger = false;
        _rigidBody.rotation = (0);
        _rigidBody.freezeRotation = true;
        Stop();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Killbox"))
        {
            GameObject AttackInstance = Instantiate(_explosion, _self.position, _self.rotation);
            new WaitForSeconds(.2f);
            Destroy(gameObject);
        }
        if (collision.CompareTag("Stop"))
        {
            Stop();
        }
        if (collision.CompareTag("Forward"))
        {
            _forward = true;
            _backward = false;
        }
        if (collision.CompareTag("Backward"))
        {
            _forward = false;
            _backward = true;
        }
        if (collision.CompareTag("Explode"))
        {
            Explode();
        }
        if (collision.CompareTag("StopExplode"))
        {
            StopExplode();
        }

    }
}
