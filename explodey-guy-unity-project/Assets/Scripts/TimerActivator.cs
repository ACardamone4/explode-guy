using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class TimerActivator : MonoBehaviour
{
    [SerializeField] private bool _activate;
    [SerializeField] private bool _reset;
    [SerializeField] private Timer _timer;
    private GameObject _timerObject;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _timerObject = GameObject.Find("Timer Manager");
            if (_timerObject != null)
            {
                _timer = _timerObject.GetComponent<Timer>();

                if (_reset)
                {
                    _timer.Reset();
                }

                if (_activate)
                {
                    _timer.Active = true;
                }
                else
                {
                    _timer.Active = false;
                }
            }
        }
    }
}
