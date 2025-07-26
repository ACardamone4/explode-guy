using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Renamer : MonoBehaviour
{
    [SerializeField] private string _name;

    public string Name { get => _name; set => _name = value; }

    void Update()
    {
        this.gameObject.name = _name;
    }
}
