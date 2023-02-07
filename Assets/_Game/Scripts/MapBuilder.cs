using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum direction
{
    top,
    down,
    left,
    right,
    idle
}
public class MapBuilder : MonoBehaviour
{
    // Start is called before the first frame update
    int[][] map ;
    GameObject[][] l_MapBricks;
    int row, col;
    int brickCount=0;
    int xStart, yStart;
    [SerializeField] PlayerController playerController;
    [SerializeField] GameObject brickYellowPrefab;
    [SerializeField] GameObject brickBlackPrefab;
    [SerializeField] GameObject startPoint;
    [SerializeField] GameObject endPoint;

    public int XStart { get => xStart; set => xStart = value; }
    public int YStart { get => yStart; set => yStart = value; }
    public GameObject[][] L_MapBricks { get => l_MapBricks; set => l_MapBricks = value; }
    public int[][] Map { get => map; set => map = value; }

    void Start()
    {
        ReadFile("Map/Map1");
        
        //Debug.Log(CountBrick(0, 0, "right"));
        //Debug.Log(CountBrick(1, 3, "left"));
        BuildMap();
        playerController.OnInit();

    }
    GameObject BuildBrick(int posx,int posz,GameObject brick)
    {
        return Instantiate(brick,new Vector3(posx,0,posz),brick.transform.rotation);
    }
    void BuildMap()
    {
        SetMap();
        for (int i = 0; i < row; i++)
        {
            for(int j = 0; j < col; j++)
            {
                if(map[i][j] == 1)
                {
                    l_MapBricks[i][j]=BuildBrick(i, j, brickYellowPrefab);
                }
                if (map[i][j] == 2)
                {
                    l_MapBricks[i][j] = BuildBrick(i,j,brickBlackPrefab);
                }
                if (map[i][j] == 3)
                {
                    xStart = i;
                    yStart = j;
                    l_MapBricks[i][j] = BuildBrick(i, j, startPoint);
                }
                if (map[i][j] == 4)
                {
                    l_MapBricks[i][j] = BuildBrick(i, j, endPoint);
                }
            }
        }
    }
    void SetMap()
    {
        l_MapBricks = new GameObject[row][];
        for(int i = 0; i < row; i++)
        {
            l_MapBricks[i] = new GameObject[col];
;       }
    }
    void ReadFile(string fileName)
    {
        var textFile = Resources.Load<TextAsset>(fileName);
        string text = textFile.text;
        string[] arrListStr = text.Split('\n');
        row = arrListStr.Length;
        map = new int[arrListStr.Length][];

        for (int i = 0; i < arrListStr.Length; i++)
        {
            string[] temp = arrListStr[i].Split(',');
            col = temp.Length;
            map[i] = new int[col];
            for (int j = 0; j < col; j++)
            {
                map[i][j] = int.Parse(temp[j]);
            }
            //for (int j = 0; j < temp.Length; j++)
            //{
            //    Debug.Log(i + " " + j + " " + map[i][j]);

            //}
        }
       
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                //Debug.Log(i + " " + j + " " + map[i][j]);
                if (map[i][j] == 1)
                {
                    brickCount += 1;
                }
                
            }
        }
        //Debug.Log("So gach " + brickCount.ToString());
    }
    public int CountBrick(int x,int y,direction direct) {
        int res = 0;
        int i,j;
        switch (direct) {
            case direction.down:
                j = y;
                for(i = x+1; i < row; i++)
                {
                    if (map[i][j] >0)
                    {
                        res += 1;
                    }
                    else
                    {
                        break;
                    }
                    
                }
                break;
            case direction.top:
                j = y;
                for (i = x-1; i >= 0; i--)
                {
                    if (map[i][j] > 0)
                    {
                        res += 1;
                    }
                    else
                    {
                        break;
                    }
                }
                break;
            case direction.left:
                i = x;
                for (j = y-1; j >= 0; j--)
                {
                    if (map[i][j] >0)
                    {
                        res += 1;
                    }
                    else
                    {
                        break;
                    }
                }
                break;
            case direction.right:
                i = x;
                for (j = y+1; j < col; j++)
                {
                    if (map[i][j] > 0)
                    {
                        res += 1;
                    }
                    else
                    {
                        break;
                    }
                }
                break;
        }
        return res;
    }
    
}
