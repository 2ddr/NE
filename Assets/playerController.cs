using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class playerController : MonoBehaviour
{
    public bool controlledByHuman;

    public List<building> baseList;//=new List<building>;
    public int activeBase=0;
    public int mySide;

    [SerializeField] 
    private builder Builder;

    [Range(0.5f, 5)] public float buildDuration=2999.0f;
    
    private float cpuBuildCounter;

    private int resUniversal=0;
    /*
    private int resGun=0;
    private int resRotor=0;
    private int resPlasma=0;
    private int resGauss=0;
    
    private int resLegs=0;
    private int resSixWheels=0;
    private int resTracks=0;
    private int resWheels8=0;
    private int resAirbag=0;
    private int resAntigrav=0;*/

    private int resChassis = 0;
    private int resBalistic = 0;
    private int resJet = 0;
    private int resHightech = 0;
    private int resElectronic = 0;

    public TMPro.TextMeshProUGUI tmResourcesShow;    
        
        
    
    // Start is called before the first frame update
    void Start()
    {
        
       //buildDuration = 2.0f;

        if (controlledByHuman)
        {
            mySide = G.PLAYER_SIDE;
            //buildDuration = 3.0f;
        }

        //cpuBuildCounter = 3.0f;
        
    }

    private void Awake()
    {
        RenewResourcesText();

        //baseList.fo
        foreach (building b in baseList)
        {
           b.SetOwner(mySide);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (buildDuration<0.6f) return;

        if (activeBase>=0)
        if (mySide == G.PLAYER_SIDE)
            humanUpdate();
        else
            cpuUpdate();
    }
    
    void humanUpdate()
    {

        cpuBuildCounter -= Time.deltaTime;

        if (cpuBuildCounter > 0) return;
        
        cpuBuildCounter = buildDuration;
        Builder.buildTank(baseList[activeBase].transform.position, mySide, Random.Range(0, 5), Random.Range(0, 4));

        

        //Debug.Log(cpuBuildCounter);
    }
    
    void cpuUpdate()
    {
        cpuBuildCounter -= Time.deltaTime;
        
        if (cpuBuildCounter>0) return;

        cpuBuildCounter = buildDuration;
        Builder.buildTank(baseList[activeBase].transform.position, mySide, Random.Range(0, 5), Random.Range(0, 4));

    }

    public void AddEqRes(G.EqResources res, int amount)
    {
       

        /*   
        switch (res)
        {
            case G.EqResources.GUN: resGun += amount;
                break;
            case G.EqResources.ROTOR: resRotor += amount;
                break;
            case G.EqResources.PLASMA: resPlasma += amount;
                break;
            case G.EqResources.GAUSS: resGauss += amount;
                break;
            case G.EqResources.LEGS: resLegs += amount;
                break;
            case G.EqResources.SIXWHEELS: resSixWheels += amount;
                break;
            case G.EqResources.TRACKS: resTracks += amount;
                break;
            case G.EqResources.WHEEL8: resWheels8 += amount;
                break;
            case G.EqResources.ANTIGRAV: resAntigrav += amount;
                break;
        }
        */
    }
    /*
    public void buildTank(Vector3 basePosition,G.WarSides mySide)
    {
        
        
        
        Builder.buildTank();
            
        tank obj = Instantiate(tank,tankGroup.transform).GetComponent<tank>();
        // obj.transform.position = new Vector2(activeBase.transform.position.x,activeBase.transform.position.y);
        obj.transform.position = new Vector2(basePosition.x,basePosition.y);
        obj.SetEquipment(db.GetChassis(dbChassisN), db.GetGun(dbGun1N));
        obj.selectorObjGroup = selectorObjGroup;
        obj.mySide = mySide;
	    
	    

    }*/

    public void AddResource(G.EqResources res, int amount)
    {

        switch (res)
        {
            case G.EqResources.UNIVERSAL:
                resUniversal += amount;
                break;
            case G.EqResources.CHASSIS:
                resChassis += amount;
                break;
            case G.EqResources.BALISTIC:
                resBalistic += amount;
                break;
            case G.EqResources.JET:
                resHightech += amount;
                break;
            case G.EqResources.HIGHTECH:
                resJet += amount;
                break;
            case G.EqResources.ELECTRONIC:
                resElectronic += amount;
                break;
        }

        /*
        switch (res)
        {
            case G.EqResources.GUN:         resGun += amount; break;
            case G.EqResources.ROTOR:       resRotor += amount; break;
            case G.EqResources.PLASMA:      resPlasma += amount; break;
            case G.EqResources.GAUSS:       resGauss += amount; break;
            case G.EqResources.LEGS:        resLegs += amount; break;
            case G.EqResources.SIXWHEELS:   resSixWheels += amount; break;
            case G.EqResources.TRACKS:      resTracks += amount; break;
            case G.EqResources.WHEEL8:      resWheels8 += amount; break;
            case G.EqResources.ANTIGRAV:    resAntigrav += amount; break;
        }
        */
    }

    public void RenewResourcesText()
    {
        if (tmResourcesShow != null)
        {
            tmResourcesShow.SetText(
               "UNIVERSAL: " + resUniversal + "\n" +
               "CHASSIS: " + resChassis + "\n" +
               "BALISTIC: " + resBalistic + "\n" +
               "JET: " + resJet + "\n" +
               "HIGHTECH: " + resHightech + "\n" +
               "ELECTRONIC: " + resElectronic + "\n"
               );
        }

    /*   tmResourcesShow.SetText(
           "GUN: " + resGun + "\n" +
           "ROTOR: " + resRotor + "\n" +
           "PLASMA: " + resPlasma + "\n" +
           "GAUSS: " + resGauss + "\n" +
           "LEGS: " + resLegs + "\n" +
           "SIXWHEELS: " + resSixWheels + "\n" +
           "TRACKS: " + resTracks + "\n" +
           "WHEEL8: " + resWheels8 + "\n" +
           "ANTIGRAV: " + resAntigrav + "\n"
           );/**/

    }
}
