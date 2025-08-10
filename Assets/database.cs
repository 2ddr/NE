using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class database : MonoBehaviour
{
    
   [SerializeField] List<chassis> aChassis = new List<chassis>();
   [SerializeField] List<gun> aGuns = new List<gun>();   
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public gun GetGun(int N)
    {
        return aGuns[N];
    }
    public chassis GetChassis(int N)
    {
        return aChassis[N];
    }

    public int GetGunLength()
    {
        return aGuns.Count;
    }
    
    public int GetChassisLength()
    {
        return aChassis.Count;
    }
    
    
}

[System.Serializable]
public class chassis
{
    public string name;
    public float cost;
    public Sprite gameImage;
    public GameObject prefab;
    
    public float[] speed =  {1, 1, 1, 1};
    //public float range;
}

[System.Serializable]
public class gun
{
    public string name;
    public float cost;
    public Sprite gameImage;
    public GameObject prefab;
    public GameObject bullet;
        
    public float hitpower;
    public float range;
    public float rate; //fire per minute

}