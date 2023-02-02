using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBuilder : MonoBehaviour
{
    // Start is called before the first frame update
    int[][] map ;
    int row, col;
    int brickCount=0;
    [SerializeField] GameObject brickYellowPrefab;
    [SerializeField] GameObject brickBlackPrefab;
    [SerializeField] GameObject startPoint;
    [SerializeField] GameObject endPoint;
    void Start()
    {
        ReadFile("Map/Map1");
        //Debug.Log(CountBrick(0, 0, "right"));
        //Debug.Log(CountBrick(1, 3, "left"));
        BuildMap();

    }
    void BuildBrick(int posx,int posz,GameObject brick)
    {
        Instantiate(brick,new Vector3(posx,0,posz),brick.transform.rotation);
    }
    void BuildMap()
    {
        Debug.Log(row.ToString() + " " + col.ToString());
        for(int i = 0; i < row; i++)
        {
            for(int j = 0; j < col; j++)
            {
                if(map[i][j] == 1)
                {
                    BuildBrick(i, j, brickYellowPrefab);
                }
                if (map[i][j] == 2)
                {
                    BuildBrick(i,j,brickBlackPrefab);
                }
                if (map[i][j] == 3)
                {
                    BuildBrick(i, j, startPoint);
                }
                if (map[i][j] == 4)
                {
                    BuildBrick(i, j, endPoint);
                }
            }
        }
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
    // Update is called once per frame
    int CountBrick(int x,int y,string huong) {
        int res = 0;
        int i,j;
        switch (huong) {
            case "below":
                j = y;
                for(i = x; i < row; i++)
                {
                    if (map[i][j] == 1)
                    {
                        res += 1;
                    }
                }
                break;
            case "top":
                j = y;
                for (i = x; i >= 0; i--)
                {
                    if (map[i][j] == 1)
                    {
                        res += 1;
                    }
                }
                break;
            case "left":
                i = x;
                for (j = y; j >= 0; j--)
                {
                    if (map[i][j] == 1)
                    {
                        res += 1;
                    }
                }
                break;
            case "right":
                i = x;
                for (j = y; j < col; j++)
                {
                    if (map[i][j] == 1)
                    {
                        res += 1;
                    }
                }
                break;
        }
        return res;
    }
    void Update()
    {
        
    }
}
