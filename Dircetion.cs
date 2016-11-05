using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Dircetion : MonoBehaviour {

    public string[] dircectionWall = new string[]{"North",  //0,1
                                                  "South",  //0,-1
                                                  "West",   //-1,0
                                                  "East"};  //1,0

    public Vector2[] directionCell=new Vector2[]{new Vector2(0,1),      //0
                                                 new Vector2(0,-1),     //1
                                                 new Vector2(-1,0),     //2
                                                 new Vector2(1,0)};     //3
    





}
