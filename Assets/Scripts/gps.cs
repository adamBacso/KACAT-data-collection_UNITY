using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;

public class gps : MonoBehaviour
{
    [SerializeField] private string gpsIdentifier = "$GPGGA";
    private string gpsSentence;
    private const char checksumEnd = '*';

    private Vector3 velocity;
    private Vector3 position;


    [SerializeField] private Vector3 startingPosition;
    private TimeSpan elapsedTime;
    private DateTime startingTime;
    [SerializeField] private float timeTick;

    private float latitudeChange = 111319.9f;

    private void Start()
    {
        Initiate(4729.33f, 1903.05f, 153f);
        timeTick = Time.fixedDeltaTime;
    }
     
    public void UpdateGPS(Vector3 acceleration)
    {
        velocity += acceleration * timeTick;

        position.x += velocity.x * timeTick / latitudeChange;
        position.y += velocity.y * timeTick / (latitudeChange * Mathf.Cos(Mathf.Deg2Rad * position.x));
        position.z += velocity.z * timeTick;
    }
    
    void Initiate(float lon, float lat, float alt)
    {
        startingTime = DateTime.Now;

        position.x = lat;
        position.y = lon;
        position.z = alt;

        velocity.x = 0;
        velocity.y = 0;
        velocity.z = 0;

        startingPosition.x = position.x;
        startingPosition.y = position.y;
        startingPosition.z = position.z;
    }

    public string WriteSentence()
    {
        string latHemisphere;
        string lonHemisphere;
        float time = DateTime.Now.Hour*10000 + DateTime.Now.Minute*100 + DateTime.Now.Second + DateTime.Now.Millisecond/100;
        
        if (position.x > 0)
            latHemisphere = "N";
        else
            latHemisphere = "S";

        if (position.y > 0)
            lonHemisphere = "E";
        else
            lonHemisphere = "W";

        int fix = 1;
        int satellites = 8;
        float hdop = 0.9f;
        string altitudeUnit = "M";
        float hGeoid = 46.9f;
        string hGeoidUnit = "M";
        float timeSince = Time.fixedDeltaTime;
        string checksumEnd = "*";

        string[] gpsData =
        {
            gpsIdentifier,
            time.ToString(),
            position.x.ToString(),
            latHemisphere,
            position.y.ToString(),
            lonHemisphere,
            fix.ToString(),
            satellites.ToString(),
            hdop.ToString(),
            position.z.ToString(),
            altitudeUnit,
            hGeoid.ToString(),
            hGeoidUnit,
            timeSince.ToString(),
            checksumEnd
        };

        string sentence = string.Join(',',gpsData);
        sentence += CalculateChecksum(sentence).ToString();
        return sentence;
    }

    byte CalculateChecksum(string sentence)
    {
        byte checksum = 0;
        for (int i = 1; i < sentence.Length; i++)
        {
            checksum ^= Encoding.UTF8.GetBytes(sentence)[0];
        }
        return checksum;
    }
}
