using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class tankMoveHelper : MonoBehaviour
{
    // Start is called before the first frame update
    private float SQ=G.SQ;

    private int width;
    private int height;
    private bool[,] aBlocked;

    public Tilemap tilemap;


    void Start()
    {
        width = tilemap.cellBounds.max.x;
        height = tilemap.cellBounds.max.y;
        aBlocked=new bool[height,width];

        //Debug.Log("aBlocked W/H:" + width + " /" + height);
        
        for (int h = 0; h < height; h++)
        for (int w = 0; w < width; w++)
        {
            aBlocked[h,w]=false;
        }

        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);
        //Debug.Log("aBlocked W/H:" + width + " /" + height);

        
        for (int x = 0; x < bounds.size.x; x++) {
            for (int y = 0; y < bounds.size.y; y++) {
                TileBase tile = allTiles[x + y * bounds.size.x];
                if (tile != null) {
                    SetBlocked(x*SQ,y*SQ);
                   // Debug.Log("x:" + x + " y:" + y + " tile:" + tile.name+" <<<<<<<<<<<<<<");
                } else {
                   // Debug.Log("x:" + x + " y:" + y + " tile: (null)");
                    
                }
            }
        }        

        /*
        width = tilemap.cellBounds.max.x;
        width = tilemap.cellBounds.max.y;

        TileBase[] tileArray = tilemap.GetTilesBlock(tilemap.cellBounds);

        aBlocked=new bool[height,width];
/*
        for (int index = 0; index < tileArray.Length; index++)
        {
            print(tileArray[index]);
            SetBlocked(tileArray[index].GetTileData(new Vector3()))
        }


        for (int h = 0; h < height; h++)
        for (int w = 0; w < width; w++)
        {
            if (tileArray[h*width+width]!=null)
            {
                aBlocked[h,w]=true; // all cells not blocked
                Debug.Log("tile: "+w+","+h);
            }
            else
                aBlocked[h,w]=false;
        }
*/

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        for (int h = 0; h < height; h++)
        for (int w = 0; w < width; w++)
        {
            if (aBlocked[h,w]) Debug.DrawLine(new Vector3(w*SQ,h*SQ),new Vector3(w*SQ+SQ,h*SQ+SQ),Color.red);
        }
    }

    public void SetBlocked(float x, float y)
    {
        if (y>=height*SQ || x>=width*SQ) Debug.LogError("aBlocked out of range! ");
        aBlocked[Mathf.FloorToInt(y/SQ), Mathf.FloorToInt(x/SQ)]=true;
    }
    public void SetBlocked(Vector2 pos)
    {
        if (pos.y>=height*SQ || pos.x>=width*SQ) Debug.LogError("aBlocked out of range! ");
        aBlocked[Mathf.FloorToInt(pos.y/SQ), Mathf.FloorToInt(pos.x/SQ)]=true;
    }

    public void SetEmpty(float x, float y)
    {
        if (y>=height*SQ || x>=width*SQ) Debug.LogError("aBlocked out of range! ");
        aBlocked[Mathf.FloorToInt(y/SQ), Mathf.FloorToInt(x/SQ)]=false;
    }
    public void SetEmpty(Vector2 pos)
    {
        if (pos.y>=height*SQ || pos.x>=width*SQ) Debug.LogError("aBlocked out of range! ");
        aBlocked[Mathf.FloorToInt(pos.y/SQ), Mathf.FloorToInt(pos.x/SQ)]=false;
    } 

    public bool SqIsEmpty(float x, float y)
    {
        if (y>=height*SQ || x>=width*SQ) Debug.LogError("aBlocked out of range! ");
       return !aBlocked[Mathf.FloorToInt(y/SQ), Mathf.FloorToInt(x/SQ)];
    }
    public bool SqIsEmpty(Vector2 pos)
    {
        if (pos.y>=height*SQ || pos.x>=width*SQ) Debug.LogError("aBlocked out of range! ");

        return !aBlocked[Mathf.FloorToInt(pos.y/SQ), Mathf.FloorToInt(pos.x/SQ)];
    }
}
/*
public class sqCell
{
    public bool blocked=false;
    public bool 
}*/