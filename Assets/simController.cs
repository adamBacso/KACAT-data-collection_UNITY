using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simController : MonoBehaviour
{
    [SerializeReference] gps gpsModule;
    [SerializeField] Vector3 acceleration = new Vector3(1,2,4);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (gpsModule != null)
        {
            gpsModule.UpdateGPS(acceleration);
        }
    }
}
