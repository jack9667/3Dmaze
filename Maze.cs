using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class Maze : MonoBehaviour {

    public  int sizeX, sizeY;
    //public Vector2 mazeSize ;
    public MazeCell cellPrefab;
    public float stepDelay;
    public Wall mazeWall;

    private MazeCell[,] cellArray;
    private Wall[,] wallArray;
    private List<MazeCell> cellList;
    private Dircetion dircetion = new Dircetion();
    private static int flag = 0;
    private int cellCountM = 0;
    //private Dircetion dircetion;

	// Use this for initialization
	void Start () {
        

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //gameManager的主调用入口
    public void Generate()
    {
        WaitForSeconds delay = new WaitForSeconds(stepDelay);   //创建延时时间
        cellArray = new MazeCell[sizeX, sizeY];     //cell数组整体
        wallArray = new Wall[sizeX, sizeY];
        cellList=new List<MazeCell>();              //cell链表用于入栈谈栈
        DoFirstStep();
               
    }


    public void DoFirstStep()
    {
        //从0，0点开始创建一个cube并删除北边墙
        MazeCell bufCell = CreateCell(new Vector2(0,0));
        Wall  bufWall = CreateWall(bufCell);
        DestoryDirWall(bufWall, "South");

        //当list里面没有cube时候结束迷宫
        while (cellList.Count != 0)
        {
            if (bufCell.flag < 4)
            {
                bufCell = GetNextCell(bufCell);
            }
            else
            {
                cellList.RemoveAt(cellList.Count - 1);
                GetCellNeighbor(cellList[cellList.Count - 1]);
                bufCell=GetNextCell(cellList[cellList.Count - 1]);
            }
        }
        Debug.Log(cellCountM);
    }

    public MazeCell GetNextCell(MazeCell cell)  //递归创建当前cell的可前进的邻居，如果flag=4(即四面都不空)弹出栈，一直到遇到包含可走边的cell
    {
        if(cellList.Count==1&&cell.flag==4)     //递归终止
        {
            cellList.RemoveAt(cellList.Count - 1);
            return cell;
        }
        else if (cell.flag < 4)                      //递归返回可走边方向
        {
            return GiveMeNeighbor(cell);
        }
        else
        {
            cellList.RemoveAt(cellList.Count - 1);
            GetCellNeighbor(cellList[cellList.Count - 1]);
            return GetNextCell(cellList[cellList.Count - 1]);
        }
    }

    public MazeCell GiveMeNeighbor(MazeCell cell)   //随机获取cell的可走边，创建新cell入栈
    {

        int bufDir = GetRandomDirection(cell);
        Debug.Log(cell.name + "方向走：" + bufDir);

        DestoryDirWall(wallArray[cell.x,cell.y], dircetion.dircectionWall[bufDir]);

        int buffx = 0;
        int buffy = 0;
        buffx = cell.x + (int)dircetion.directionCell[bufDir].x;
        buffy = cell.y + (int)dircetion.directionCell[bufDir].y;

        MazeCell cell2 = CreateCell(new Vector2(buffx,buffy));
        if (cell2 != null)
        {
            Wall bufW = CreateWall(cell2);
            DestoryDirWall(bufW, dircetion.dircectionWall[GetDestoryWallDir(bufDir)]);          //在这里删除墙!!

            //GetCellNeighbor(cell2);

            return cell2;
        }
        else
            return cell;
    }

    public int GetDestoryWallDir(int x)
    {
        switch (x)
        {
            case 0:
                {
                    x += 1;
                    return x;
                    
                }
            case 1:
                {
                    x -= 1;
                    return x;
                }
            case 3:
                {
                    x -= 1;
                    return x;
                }
            case 2:
                {
                    x += 1;
                    return x;
                }
            default:
                return 0;
        }
    }

    public void GetCellNeighbor(MazeCell cell)      //获取cell四个边是否为空，存储信息
    {
        cell.flag = 0;  //cell含有几个墙
        for(int i=0;i<4;i++)
        {
            if (IsCellWallNull(cell, dircetion.directionCell[i]))  //有墙，即方向返回true
                cell.flag++;
            cell.wallList.Add(IsCellWallNull(cell, dircetion.directionCell[i]));
        }
    }

    public bool IsCellWallNull(MazeCell cell,Vector2 vec)   //vec方向没有物体返回true
    {

        int x=cell.x + (int)vec.x;
        int y=cell.y + (int)vec.y;

        if (x >= 0 && x < sizeX && y >= 0 && y < sizeY && cellArray[x, y] == null) //移动vec方向后，结果不是边界且vec方向为空，返回false
        {

             return false;
        }
        else
        {
             return true;
        }
    }

    public int GetRandomDirection(MazeCell cell)    //随机获取一个空的方向
    {
        int randomDir = Random.Range(0, 4);
        while (cell.wallList[randomDir] == true)
        {
            randomDir = Random.Range(0, 4);
        }
        return randomDir; 
    }

    public MazeCell CreateCell(Vector2 vec)         //在vec方向创建cell，入栈，并记录cell的墙数
    {
        if (cellArray[(int)vec.x, (int)vec.y] == null)
        {
            MazeCell newCell = Instantiate(cellPrefab) as MazeCell;
            cellArray[(int)vec.x, (int)vec.y] = newCell;              //把创建的cell填入到对应的cell数组中
            cellList.Add(newCell);                  //把cell入栈
            newCell.setCell(vec);
            newCell.isInMaze = true;

            newCell.name = "mazeCell" + vec.x + "," + vec.y;
            newCell.transform.parent = transform;
            newCell.transform.localPosition = new Vector3(vec.x - sizeX * 0.5f + 0.5f, 0f, vec.y - sizeY * 0.5f + 0.5f);

            GetCellNeighbor(newCell);
            cellCountM++;
            return newCell;
        }
        else return null;

    }

    public Wall CreateWall(MazeCell cell)
    {
            Wall newWall = Instantiate(mazeWall) as Wall;
            Debug.Log(cell.x +"," + cell.y);
            wallArray[cell.x, cell.y] = newWall;
            newWall.name = "mazeWall" + cell.x + "," + cell.y;
            newWall.transform.parent = cell.transform;
            newWall.transform.position = cell.transform.localPosition;
            return newWall;
    }

    public void DestoryDirWall(Wall wall,string dirWall)
    {
        if(wall!=null)
        Destroy(wall.transform.FindChild(dirWall).GetComponent<Wall>().gameObject);
    }
}
