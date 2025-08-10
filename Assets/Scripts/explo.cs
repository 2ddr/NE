using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explo : MonoBehaviour
{
    [SerializeField]
    Transform EffectsGroup;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void exploDestroy()
    {
        Destroy(gameObject, 0.0f);
    }
}
