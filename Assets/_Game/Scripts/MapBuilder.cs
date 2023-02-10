using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public enum direction
{
    top,
    down,
    left,
    right,
    idle
}
public class MapBuilder : GOSingleton<MapBuilder>
{
    // Start is called before the first frame update
    int[][] map ;
    GameObject[][] l_MapBricks;
    int row, col;
    int brickCount=0;
    int xStart, yStart;
    bool isTouch = false;
    [SerializeField] PlayerController playerController;
    [SerializeField] GameObject brickYellowPrefab;
    [SerializeField] GameObject brickBlackPrefab;
    [SerializeField] GameObject startPoint;
    [SerializeField] GameObject endPoint;
    [SerializeField] LevelManager levelManager;
    [SerializeField] UIManager uiManager;
    Vector3 pointA, pointB;

    public int XStart { get => xStart; set => xStart = value; }
    public int YStart { get => yStart; set => yStart = value; }
    public GameObject[][] L_MapBricks { get => l_MapBricks; set => l_MapBricks = value; }
    public int[][] Map { get => map; set => map = value; }

    void Start()
    {
        
    }
    private void Update()
    {
        if (playerController.L_Target.Count == 0)
        {
            GetPoint();
        }
    }
    public void OnInit()
    {

        ReadFile("Map/Map"+levelManager.CurrentLevel.ToString());
        BuildMap();
        playerController.OnInit();
    }
    GameObject BuildBrick(int posx,int posz,GameObject brick)
    {
        return Instantiate(brick,new Vector3(posx,0,posz),brick.transform.rotation);
    }
    public void BuildMap()
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
    public void ReadFile(string fileName)
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
    
    public void DeleteMap()
    {
        for(int i = 0; i < row; i++)
        {
            for(int j=0;j < col; j++)
            {
                if (map[i][j]>0)
                    Destroy(l_MapBricks[i][j].gameObject);
            }
        }
    }
    float CalCos(float x1, float y1, float x2, float y2)
    {
        return (x1 * x2 + y1 * y2) / (Mathf.Sqrt(x1 * x1 + y1 * y1) * Mathf.Sqrt(x2 * x2 + y2 * y2));
    }
    direction FindDirection(Vector3 pointA, Vector3 pointB)
    {
        Vector3 VectorAB = pointB - pointA;
        float angle = Mathf.Acos(CalCos(1, 0, VectorAB.x, VectorAB.y));
        if (angle <= Math.PI / 4)
        {
            return direction.right;
        }
        else if (angle <= 3 * Math.PI / 4)
        {
            if (VectorAB.y > 0)
            {
                return direction.top;
            }
            else
            {
                return direction.down;
            }
        }
        else
        {
            return direction.left;
        }

    }

    void GetPoint()
    {
        if (Input.GetMouseButtonDown(0))
        {
            pointA = Input.mousePosition;
            //Debug.Log(pointA.x.ToString()+" "+pointA.y.ToString()+" "+pointA.z.ToString());
            isTouch = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            isTouch = false;
            playerController.CurrentDirection = FindDirection(pointA, pointB);
            playerController.SetTarget();
        }
        if (isTouch)
        {
            pointB = Input.mousePosition;
            //Debug.Log(pointB.x.ToString() + " " + pointB.y.ToString() + " " + pointB.z.ToString());

        }
    }
    public void Win()
    {
        Debug.Log("You Win");
        Debug.Log("Score: " + playerController.BrickOwner.ToString());
        playerController.ClearBrick();
        DeleteMap();
        UIManager.GetInstance().EndGameMenu(true);
        UIManager.GetInstance().UpLevelButton.GetComponent<Button>().enabled = true;

    }
    public void UpLevel()
    {
        levelManager.CurrentLevel += 1;
        OnInit();
    }
    public void Lose()
    {
        Debug.Log("Game Over");
        UIManager.GetInstance().EndGameMenu(true);
        UIManager.GetInstance().UpLevelButton.GetComponent<Button>().enabled = false; 
    }
}
