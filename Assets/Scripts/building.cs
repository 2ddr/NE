using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using Random = Unity.Mathematics.Random;

public class building : MonoBehaviour
{
    public Sprite color_red;
    public Sprite color_blue;
    public Sprite color_none;
    
    public int mySide=G.NOBODY_SIDE;

    public G.EqResources equipmentProduce;

    public bool isBase;

    public playerController ownerController;

    public float counter;

    int[] aAaims = new int[8];

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < aAaims.Length; i++)
        {
            aAaims[i] = 0;
        }

        SetOwner(mySide);
        //SetOwner(G.OWNER_NONE);
        transform.position = new Vector2(Mathf.Floor(transform.position.x / G.SQ) * G.SQ + G.SQ*0.5f,Mathf.Floor(transform.position.y / G.SQ) * G.SQ + G.SQ*0.5f);

        counter = 0.0f;
        equipmentProduce = isBase ? G.EqResources.UNIVERSAL : GetRandomEq();
    }

    // Update is called once per frame
    void Update()
    {
        //if (aimsCount>1.0f) aimsCount-= 0.25f * Time.deltaTime;

        if (mySide < 0) return;

        counter -= Time.deltaTime;

        if (counter<0)
        {
            counter = 5;
            TimeToAdd();
        }
    }

    public void TimeToAdd()
    {
        //UnityEngine.Debug.Log("mySide " + mySide + " / ownerController " + ownerController);
       // if (isBase) return;

        if (mySide>=0)
        if (ownerController!=null)
            ownerController.AddResource(equipmentProduce,1);

        
    }

    public void SetOwner(int own)
    {
        if (own < 0) return;


        mySide = own;
        tag = "Factory";
        GameObject[] arr=GameObject.FindGameObjectsWithTag("PlayerCtrl");

        foreach (GameObject pc in arr)
        {
            if (pc.GetComponent<playerController>().mySide == own)
                ownerController = pc.GetComponent<playerController>();
        }

       // if (ownerController == null) UnityEngine.Debug.Log("ownerController is NULL");

        RecolorAll(G.GetSideColor(mySide));

        
    }

    public void CaptureMe(int side)
    {

        if (isBase)
        {
            if (ownerController != null)
            {
                if (ownerController.baseList.IndexOf(this) == ownerController.activeBase)
                    //if (ownerController.baseList.[ownerController.activeBase] == this)
                    if (ownerController.baseList.Count > 1)
                        ownerController.activeBase = 0;
                    else
                        ownerController.activeBase = -1;

                ownerController.baseList.Remove(this);
            }

            GameObject[] arr = GameObject.FindGameObjectsWithTag("PlayerCtrl");

            foreach (GameObject pc in arr)
            {
                if (pc.GetComponent<playerController>().mySide == side)
                {
                    ownerController = pc.GetComponent<playerController>();
                    ownerController.baseList.Add(this);
                }

            }
        }



        SetOwner(side);

        


    }

    void RecolorAll(Color col)
    {
        GetComponent<SpriteRenderer>().color = col;
        
    }
    
    G.EqResources GetRandomEq()
    {
        int type = UnityEngine.Random.Range(0, 5);
        G.EqResources ret=G.EqResources.UNIVERSAL;
        switch (type)
        {
            case 0: ret = G.EqResources.CHASSIS; break;
            case 1: ret = G.EqResources.BALISTIC; break;
            case 2: ret = G.EqResources.JET; break;
            case 3: ret = G.EqResources.HIGHTECH; break;
            case 4: ret = G.EqResources.ELECTRONIC; break;
        }
        /*, , , , 
        switch (type)
        {
            case 0: ret = G.EqResources.GUN;break;
            case 1: ret = G.EqResources.ROTOR;break;
            case 2: ret = G.EqResources.PLASMA;break;
            case 3: ret = G.EqResources.GAUSS;break;
            case 4: ret = G.EqResources.LEGS;break;
            case 5: ret = G.EqResources.SIXWHEELS;break;
            case 6: ret = G.EqResources.TRACKS;break;
            case 7: ret = G.EqResources.WHEEL8;break;
            case 8: ret = G.EqResources.ANTIGRAV;break;
        }
        */
        return ret;
    }

    public void AddAimer(int side)
    {
        if (side < aAaims.Length)
            aAaims[side] += 1;
        else
           UnityEngine.Debug.Log("!!! side >= aAaims.Length ");

    }

    public int GetAimerCount(int side)
    {
        return aAaims[side]+1;
    }
    public void RemoveAimer(int side)
    {
        aAaims[side]-=1;
        aAaims[side] = aAaims[side] < 0 ? 0 : aAaims[side];
    }


}
