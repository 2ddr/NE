using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameSpeedControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSpeedToX1()
    {
        Time.timeScale = 1;
    }

    public void SetSpeedToX2()
    {
        Time.timeScale = 2;
    }
    public void SetSpeedToX4()
    {
        Time.timeScale = 4;
    }


}
