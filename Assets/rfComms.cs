using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class rfComms : MonoBehaviour
{
    [Header("Components")]
    [SerializeReference] gps gps;
    [SerializeField] private List<int> gpsUseful = new List<int> { 0, 1, 2, 4, 9, 13 };
    

    private string sentence = "";


    private void FixedUpdate()
    {
        sentence = "";
        if (gps != null)
        {
            GPS();
        }
    }

    private void GPS()
    {
        string gpsSentence = "";
        string incoming =  gps.WriteSentence();
        string[] dataPoints = incoming.Split(',');

        foreach (int index in gpsUseful)
        {
            gpsSentence += dataPoints[index] + ",";
        }

        sentence += gpsSentence + "*" + CalculateChecksum(gpsSentence);
    }

    public string GetCommsSentence()
    {
        return sentence;
    }

    private string CalculateChecksum(string sentence)
    {
        byte checksum = 0;
        byte[] charBytes = Encoding.UTF8.GetBytes(sentence);
        for (int i = 1; i < sentence.Length; i++)
        {
            checksum ^= charBytes[i];
        }
        return checksum.ToString("X2");
    }
}
