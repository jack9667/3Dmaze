using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MazeCell : MonoBehaviour {

    //public bool isInList = false;
    public bool isInMaze = false;

    public int x, y;
    public int flag = 0;
    public List<bool> wallList = new List<bool>() ; //fals为没有物体

    public void setCell(Vector2 vec)
    {
        this.x = (int)vec.x;
        this.y = (int)vec.y;
        this.isInMaze = true;
    }

}
