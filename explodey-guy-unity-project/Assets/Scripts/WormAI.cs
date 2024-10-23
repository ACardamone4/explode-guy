using System.Collections;
using UnityEngine;

public class WormAI : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    private float baseMoveSpeed;
    [SerializeField] private float _delayTime;
    [SerializeField] private float _deathTime;
    [SerializeField] private GameObject _target;
    [SerializeField] private bool _playerIsTarget;

    private void Awake()
    {
        baseMoveSpeed = _moveSpeed;
        if (_playerIsTarget)
        {
            _target = GameObject.FindGameObjectWithTag("Player");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_target != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, _target.transform.position, _moveSpeed * Time.deltaTime);
            if (_target.transform.position.x > transform.position.x)
            {
                transform.localScale = new Vector2(-1, 1);
            } else
            {
                transform.localScale = new Vector2(1, 1);
            }
        }
        else if (_playerIsTarget)
        {
            _target = GameObject.FindGameObjectWithTag("Player");
        }
        else if (this.gameObject.activeInHierarchy == false)
        {
            return;
        } 
        else
        {
            StartCoroutine(Die());
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator Die()
    {
        yield return new WaitForSeconds(_deathTime);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == _target)
        {
            _moveSpeed = 0;
        }
        if (collision.gameObject.tag == "EnemyKillBox")
        {
            if (this.gameObject.activeInHierarchy == false)
            {
                return;
            }
            else
            {
                StartCoroutine(Die());
            }
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == _target)
        {
            if (this.gameObject.activeInHierarchy == false)
            {
                return;
            } else 
            {
            StartCoroutine(NormalMove());
            }
        }
    }

    private IEnumerator NormalMove()
    {
        yield return new WaitForSeconds(_delayTime);
        _moveSpeed = baseMoveSpeed;
    }
}
