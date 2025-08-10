using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using Toggle = UnityEngine.UI.Toggle;

public class builder : MonoBehaviour
{
	public Transform fogOfWarGroup;
	public GameObject tankMask;

	public GameObject tank;
	public GameObject tankGroup;
	public Transform effectGroup;

	public GameObject activeBase;

	public Image PreviewImgChassis;
	public Image PreviewImgGun;
	
	[SerializeField] database db;
	public builderSlot equipmentSlot;
	public GameObject grGuns;
	public GameObject grChassis;

	public int dbChassisN;
	public int dbGun1N;
	
	public GameObject selectorObjGroup;

	public playerController plController;

	public tankMoveHelper moveHelper;
    // Start is called before the first frame update
    void Start()
    {
	    /*
	    int k = 0;
	    while(grGuns.transform.childCount>0 && k<10){
		    Debug.Log("before:"+grGuns.transform.childCount);
			Destroy(grGuns.transform.GetChild(0).gameObject);
			Debug.Log("after:"+grGuns.transform.childCount);
			k++;
	    }
*/
	    DelChild(grChassis);
	    DelChild(grGuns);
	    
	    
	    int n = db.GetGunLength();
	    builderSlot obj;
	    
	    for (int i = 0; i < db.GetChassisLength(); i++)
	    {
		    obj= Instantiate(equipmentSlot,grChassis.transform).GetComponent<builderSlot>();
		    obj.image.sprite = db.GetChassis(i).gameImage;
		    obj.TankPreviewImg = PreviewImgChassis;
		    obj.label.text = db.GetChassis(i).name;
		    obj.SetEquip(G.EqType.CHASSIS,i);
		    
		    obj.toggle.group = grChassis.GetComponent<ToggleGroup>();
		    obj.toggle.isOn = (i==0);
		    //obj.Builder = GetComponent<builder>();
	    }
	    
	    for (int i = 0; i < db.GetGunLength(); i++)
	    {
		     obj= Instantiate(equipmentSlot,grGuns.transform).GetComponent<builderSlot>();
		     obj.image.sprite = db.GetGun(i).gameImage;
		     obj.TankPreviewImg = PreviewImgGun;
		     obj.label.text = db.GetGun(i).name;
		     obj.SetEquip(G.EqType.GUN1,i);
		     
		     obj.toggle.group = grGuns.GetComponent<ToggleGroup>();
		     obj.toggle.isOn = (i==0);
		     //obj.Builder = GetComponent<builder>();
	    }
	    
	    
	    

    }

    public void btnBuildTank()
    {

	    buildTank(plController.baseList[plController.activeBase].transform.position, plController.mySide, dbChassisN, dbGun1N);
    }
    
    public void buildTank(Vector3 basePosition, int mySide, int ChassisN, int Gun1N)
    {
		//Debug.Log("basePosition: "+ basePosition+ " / mySide: " + mySide + " / ChassisN: " + ChassisN + " / Gun1N: " + Gun1N);
		
		
		tank obj = Instantiate(tank,tankGroup.transform).GetComponent<tank>();
	   // obj.transform.position = new Vector2(activeBase.transform.position.x,activeBase.transform.position.y);
	    obj.transform.position = new Vector2(basePosition.x,basePosition.y);

		if (mySide == G.PLAYER_SIDE)        
			{
				obj.fogMask = Instantiate(tankMask, fogOfWarGroup);
				obj.fogMask.transform.position = new Vector2(basePosition.x, basePosition.y);
				// obj.fogMask.gameObject.SetActive(true);
			}

		obj.effectsGroup = effectGroup;
		obj.selectorObjGroup = selectorObjGroup;
		obj.SetEquipment(db.GetChassis(ChassisN), db.GetGun(Gun1N));
		obj.SetMySide(mySide);
		obj.moveHelper=moveHelper;

		//Debug.Log("BUILDED");

	}
/*
    int[] SetEquipment(int _chassis, int _gun)
    {
	    int[] A = new int[2];
	    A[0] = _chassis;
	    
	    return ({_chassis,_gun});
    }
*/
    void DelChild(GameObject parent)
    {
	    for (int i = 0; i < parent.transform.childCount; i++)
	    {
		    Destroy(parent.transform.GetChild(i).gameObject);
	    }
	    
    }

    public void SelectedEquipmentNumber(G.EqType type, int N)
    {
	    switch (type)
	    {
		    case G.EqType.CHASSIS: dbChassisN = N; break;
		    case G.EqType.GUN1: dbGun1N = N; break;
	    }
    }
}
