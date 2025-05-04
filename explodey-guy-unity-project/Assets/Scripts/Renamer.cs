using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Renamer : MonoBehaviour
{
    [SerializeField] private string _name;
    void Awake()
    {
        this.gameObject.name = _name;
    }
}
