using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flightController : MonoBehaviour
{
    [Header("Physical Properties")]
    [SerializeField] private float mass = 0.3f;

    [Header("Components")]
    [SerializeReference] private gps gps;

    private void FixedUpdate()
    {
        if (gps != null)
        {
            Debug.Log(gps.WriteSentence());
        }
    }

    public float GetMass()
    {
        return mass;
    }
}
