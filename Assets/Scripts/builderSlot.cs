using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using Toggle = UnityEngine.UI.Toggle;

public class builderSlot : MonoBehaviour
{
    public UnityEngine.UI.Image TankPreviewImg;
    
    public Toggle toggle;
    public Image image;
    public Text label;
    
    private G.EqType equipmentType;
    private int equipmentTypeNumber = -1;
    
    UnityEngine.UI.Image btn;
    UnityEngine.UI.Toggle Tog;

    public builder Builder;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
       // throw new System.NotImplementedException();
       Builder = GameObject.FindGameObjectWithTag("Builder").GetComponent<builder>();
    }

    public void TaskOnClick()
    {
        //Debug.Log (btn.sprite);
        if (toggle.isOn)
        {
            TankPreviewImg.sprite = image.sprite;
            Builder.SelectedEquipmentNumber(equipmentType,equipmentTypeNumber);
            
        }
    }

    public void SetEquip(G.EqType eqType, int eqTypeN)
    {
        equipmentType = eqType;
        equipmentTypeNumber = eqTypeN;

    }
}
