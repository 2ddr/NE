using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class G : MonoBehaviour
{
    public const float SQ = 24.0f;
    public const int CAPTURE_BASES = 0;
    public const int CAPTURE_FACTORIES = 1;
    public const int DESTROY_ENEMIES = 2;
    /*
    public const int OWNER_NONE = 0;
    public const int OWNER_PLAYER = 1;
    public const int OWNER_ENEMY = 2;

    public const int NONE = 0;
    public const int PLAYER = 1;
    public const int ENEMY = 2;
    */
    public const int CHASSIS = 0;
    public const int GUN1 = 1;

    public const int NOBODY_SIDE = -1;
    public const int PLAYER_SIDE = 0;

    /*
    public enum WarSides {
        NONE,PLAYER,ENEMY
    };
*/
    public enum EqType
    {
        CHASSIS, GUN1
    };
    /*
    public enum EqResources {
        GUN,ROTOR,PLASMA,GAUSS,
        LEGS,SIXWHEELS,TRACKS,WHEEL8,ANTIGRAV
    };
    */
    public enum EqResources
    {
        UNIVERSAL, CHASSIS, BALISTIC, JET, HIGHTECH, ELECTRONIC
    };
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public static Color GetSideColor(int mySide)
    {
        //int mySide = side;
        if (mySide == PLAYER_SIDE)
        {
            return Color.red;
        }

        if (mySide == NOBODY_SIDE)
        {
            return Color.white;
        }

        return Color.cyan;
    }

    public static int GetSideNumber()
    {

        return Random.Range(1, 9999);
    }

}


