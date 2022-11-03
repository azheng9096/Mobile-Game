using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeTime : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 5);
    }
}
