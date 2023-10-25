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

    [SerializeField] private Vector3 startVelocity = new Vector3(6, 50, 3);
    private Vector3 velocity;
    [SerializeField] private float drag = 0.98f;
    private Vector3 position;
    [SerializeField] private Vector3 startingPosition;

    private float latitudeChange = 111319.9f;

    private TimeSpan elapsedTime;
    private DateTime startingTime;
    private float timeTick;

    private void Start()
    {
        Initiate(4729.33f, 1903.05f, 153f);
        timeTick = Time.fixedDeltaTime;
        velocity = startVelocity;
    }

    private void FixedUpdate()
    {
        if (position.y < 0)
            velocity = startVelocity;
        UpdateGPS();
    }

    private void UpdateGPS()
    {
        velocity.y += Physics.gravity.y * timeTick;
        velocity *= drag;
        position.x += velocity.x * timeTick / latitudeChange;
        position.y += velocity.y * timeTick;
        position.z += velocity.z * timeTick / (latitudeChange * Mathf.Cos(Mathf.Deg2Rad * position.x));
    }
    
    void Initiate(float lon, float lat, float alt)
    {
        startingTime = DateTime.Now;

        position.x = lat;
        position.z = lon;
        position.y = alt;

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
            position.z.ToString(),
            lonHemisphere,
            fix.ToString(),
            satellites.ToString(),
            hdop.ToString(),
            position.y.ToString(),
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

    string CalculateChecksum(string sentence)
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
