using Cinemachine;
using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{

    [SerializeField] private CinemachineVirtualCamera _cam;
    [SerializeField] private float _intensity;
    [SerializeField] private float _shakeDuration;
    [SerializeField] private float _shakeDurationMax;
    [SerializeField] private bool _shaking;


    public void Awake()
    {
        _cam = GetComponent<CinemachineVirtualCamera>();
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = _cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
    }

    public void Shake()
    {
        _shakeDuration = _shakeDurationMax;
        _shaking = true;
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = _cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = _intensity;
    }
    
    public void StopShake()
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = _cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
    }

    private void Update()
    {
        if (_shaking)
        {
            _shakeDuration -= Time.deltaTime;
            if (_shakeDuration <= 0)
            {
                _shakeDuration = _shakeDurationMax;
                _shaking = false;
                StopShake();
            }
        }
        
    }
}