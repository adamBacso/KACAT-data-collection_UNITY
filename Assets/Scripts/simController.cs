using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class simController : MonoBehaviour
{
    [SerializeReference] gps gpsModule;
    [SerializeReference] flightController kacat;
    [SerializeField] Vector3 acceleration = new Vector3(5,3,10);
    [Range(0.5f,1f)] float drag = 0.01f;
    Vector3 currAcceleration;
    
    // Start is called before the first frame update
    void Start()
    {
        currAcceleration = acceleration;
        
    }

    //private void FixedUpdate()
    //{

    //    if (gpsModule != null)
    //    {
    //        currAcceleration.y = currAcceleration.y+(Physics.gravity.y);
    //        currAcceleration -= currAcceleration*drag;
    //        Debug.Log(currAcceleration);
    //        gpsModule.UpdateGPS(currAcceleration);
    //    }
    //}
}
