using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class flightController : MonoBehaviour
{
    [SerializeReference] private rfComms comms;

    [Header("Physical Properties")]
    [SerializeField] private float mass = 0.3f;

    [Header("GPS")]
    //private float latitudeChange = 111319.9f;


    private string incoming;
     
    private void FixedUpdate()
    {
        AnalyseSentence();
    }

    private void AnalyseSentence()
    {
        if (comms != null)
        {
            incoming = comms.GetCommsSentence();
        }
        string[] dataByComponents = incoming.Split('$');

        foreach (string component in dataByComponents)
        {
            string[] dataPoints = component.Split(',');

            if (dataPoints[0] == "GPGGA")
                ProcessGPSData(dataPoints);
        }
    }

    private void ProcessGPSData(string[] dataPoints)
    {
        Vector3 coordPosition = new Vector3();
        coordPosition.x = float.Parse(dataPoints[2]); //latitude
        coordPosition.y = float.Parse(dataPoints[3]); //altitude
        coordPosition.z = float.Parse(dataPoints[4]); //longitude
    }

    public float GetMass()
    {
        return mass;
    }
}
