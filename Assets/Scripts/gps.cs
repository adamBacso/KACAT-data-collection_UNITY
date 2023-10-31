using System;
using UnityEngine;

public class gps : MonoBehaviour
{
    [SerializeField] private string gpsIdentifier = "$GPGGA";
    private const char checksumEnd = '*';

    [SerializeField] private Vector3 startVelocity;
    private Vector3 velocity;
    [SerializeField] private float drag = 0.98f;
    private Vector3 position;
    [SerializeField] private Vector3 startingPosition;

    private TimeSpan elapsedTime;
    private DateTime startingTime;
    private float timeTick;

    private void Start()
    {
        Initiate();
        timeTick = Time.fixedDeltaTime;
    }

    private void FixedUpdate()
    {
        UpdateGPS();
    }

    private void UpdateGPS()
    {
        if (position.y< 0)
            velocity = startVelocity;

        velocity.y += Physics.gravity.y * timeTick;
        velocity *= drag;
        position.x += velocity.x * timeTick;
        position.y += velocity.y * timeTick;
        position.z += velocity.z * timeTick;

    }

    void Initiate()
    {
        startingTime = DateTime.Now;

        position.x = 0;
        position.z = 0;
        position.y = 0;

        velocity = startVelocity;
    }

    float latitude;
    float longitude;
    public string WriteSentence()
    {
        float rEarth = 6371000f;

        longitude = (startingPosition.z * Mathf.Deg2Rad + position.z / (rEarth * Mathf.Cos(latitude*Mathf.Deg2Rad))) * Mathf.Rad2Deg;
        string lonHemisphere;

        latitude = (startingPosition.x * Mathf.Deg2Rad + position.x / rEarth) * Mathf.Rad2Deg;
        string latHemisphere;
        float time = (float)DateTime.Now.Hour*10000 + (float)DateTime.Now.Minute*100 + (float)DateTime.Now.Second + ((float)DateTime.Now.Millisecond)/100;

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
        float altitude = position.y;
        string altitudeUnit = "M";
        float hGeoid = 46.9f;
        string hGeoidUnit = "M";
        float timeSince = Time.fixedDeltaTime;
        string checksumEnd = "*";

        string[] gpsData =
        {
            gpsIdentifier,
            time.ToString(),
            latitude.ToString(),
            latHemisphere,
            longitude.ToString(),
            lonHemisphere,
            fix.ToString(),
            satellites.ToString(),
            hdop.ToString(),
            altitude.ToString(),
            altitudeUnit,
            hGeoid.ToString(),
            hGeoidUnit,
            timeSince.ToString(),
            checksumEnd
        };

        string sentence = string.Join(',', gpsData); ;
        return sentence;
    }


}
