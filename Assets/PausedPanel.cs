using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausedPanel : MonoBehaviour
{
    public GameObject Paused;
    public CarController CarController;
    public Rigidbody Rigidbody;

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Pause()
    {
        Paused.SetActive(true);
        CarController.motorPower = 0;
        CarController.brakePower = 0;
        Rigidbody.velocity = Vector3.zero;
    }
    public void Continue()
    {
        Paused.SetActive(false);
        CarController.motorPower = 300000;
        CarController.brakePower = 500000;
    }
}
