using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Vector3 = UnityEngine.Vector3;

public class PlayerController : MonoBehaviour
{
    
    [SerializeField] MapBuilder _mapBuilder;
    [SerializeField] int brickOwner = 0;
    [SerializeField] private float speed=2f;
    [SerializeField] private List<GameObject> l_Bricks= new List<GameObject>(); 
    [SerializeField] private GameObject brickYellowPrefab;
    [SerializeField] private GameObject brickWhitePrefab;
    Vector3 offset = new Vector3(0,-0.1f,0);
    [SerializeField] float brickHeight = 0.15f;
    Vector3 pointA,pointB;
    bool isTouch=false;
    direction currentDirection=direction.idle;
    float duration = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnInit()
    {
        transform.position = new Vector3(_mapBuilder.XStart, 0.3f, _mapBuilder.YStart);
        //transform.rotation = _mapBuilder.L_MapBricks[_mapBuilder.XStart][_mapBuilder.YStart].transform.rotation;
        
    }
    // Update is called once per frame
    void Update()
    {
        GetPoint();
        
    }
    float CalCos(float x1,float y1,float x2,float y2)
    {
        return (x1 * x2 + y1 * y2) / (Mathf.Sqrt(x1 * x1 + y1 * y1)* Mathf.Sqrt(x2 * x2 + y2 * y2));
    }
    direction FindDirection(Vector3 pointA,Vector3 pointB)
    {
        Vector3 VectorAB=pointB - pointA;
        float angle = Mathf.Acos(CalCos(1,0,VectorAB.x,VectorAB.y));
        if(angle<=Math.PI/4)
        {
            return direction.right;
        }
        else if (angle <= 3*Math.PI/4)
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
            Move();

        }
        if (isTouch)
        {
            pointB = Input.mousePosition;
            //Debug.Log(pointB.x.ToString() + " " + pointB.y.ToString() + " " + pointB.z.ToString());
            
        }
 
       
    }
    void Move()
    {
        currentDirection = FindDirection(pointA, pointB);
        if (currentDirection == direction.top)
        {
            //transform.Translate(Vector3.right*speed*Time.deltaTime);
            int numBrick = _mapBuilder.CountBrick((int)transform.position.x,(int)transform.position.z,direction.top);
            Vector3 target = new Vector3(transform.position.x -  numBrick, transform.position.y,transform.position.z);
            //transform.localPosition = Vector3.Lerp(transform.position,target , 3);
            //Debug.Log(numBrick);
            for(int i = 0; i < numBrick; i++)
            {
                
                transform.localPosition = Vector3.Lerp(transform.position, new Vector3(transform.position.x - 1, transform.position.y, transform.position.z), 1);
                int x = (int)transform.position.x;
                int y = (int)transform.position.z;
                switch (_mapBuilder.Map[x][y])
                {
                    case 1:
                        AddBrick(x,y);
                        break;
                    case 2:
                        if (brickOwner <= 0)
                        {
                            Lose();
                            return;
                        }
                        RemoveBrick(x,y);
                        break;
                    case 4:
                        Win();
                        break;
                }
            }
        }
        else if (currentDirection == direction.down)
        {
            int numBrick = _mapBuilder.CountBrick((int)transform.position.x, (int)transform.position.z, direction.down);
            Vector3 target = new Vector3(transform.position.x + numBrick, transform.position.y, transform.position.z);
            //transform.localPosition = Vector3.Lerp(transform.position, target, 3);
            //transform.Translate(Vector3.left*speed * Time.deltaTime);
            for (int i = 0; i < numBrick; i++)
            {
                
                transform.localPosition = Vector3.Lerp(transform.position, new Vector3(transform.position.x + 1, transform.position.y, transform.position.z), 3);
                int x = (int)transform.position.x;
                int y = (int)transform.position.z;
                switch(_mapBuilder.Map[x][y])
                {
                    case 1:
                        AddBrick(x,y);
                        break;
                    case 2:
                        if (brickOwner <= 0)
                        {
                            Lose();
                            return;
                        }
                        RemoveBrick(x,y);
                        break;
                    case 4:
                        Win();
                        break;
                }
            }
        }
        else if (currentDirection == direction.right)
        {
            int numBrick = _mapBuilder.CountBrick((int)transform.position.x, (int)transform.position.z, direction.right);
            Vector3 target = new Vector3(transform.position.x, transform.position.y, transform.position.z + numBrick);
            //transform.localPosition = Vector3.Lerp(transform.position, target, 3);
            //transform.Translate(Vector3.forward * speed * Time.deltaTime);
            for (int i = 0; i < numBrick; i++)
            {
                
                transform.localPosition = Vector3.Lerp(transform.position, new Vector3(transform.position.x , transform.position.y, transform.position.z+1), 3);
                int x = (int)transform.position.x;
                int y = (int)transform.position.z;
                switch (_mapBuilder.Map[x][y])
                {
                    case 1:
                        AddBrick(x,y);
                        break;
                    case 2:
                        if (brickOwner <= 0)
                        {
                            Lose();
                            return;
                        }
                        RemoveBrick(x,y);
                        break;
                    case 4:
                        Win();
                        break;
                }
            }
        }
        else if (currentDirection == direction.left)
        {
            //transform.Translate(Vector3.back * speed * Time.deltaTime);
            int numBrick = _mapBuilder.CountBrick((int)transform.position.x, (int)transform.position.z, direction.left);
            Vector3 target = new Vector3(transform.position.x , transform.position.y, transform.position.z - numBrick);
            //transform.localPosition = Vector3.Lerp(transform.position, target, 3);
            for (int i = 0; i < numBrick; i++)
            {
                
                transform.localPosition = Vector3.Lerp(transform.position, new Vector3(transform.position.x, transform.position.y, transform.position.z - 1), 3);
                int x = (int)transform.position.x;
                int y = (int)transform.position.z;
                switch (_mapBuilder.Map[x][y])
                {
                    case 1:
                        AddBrick(x,y);
                        break;
                    case 2:                  
                        if (brickOwner <= 0)
                        {
                            Lose();
                            return;
                        }
                        RemoveBrick(x,y);
                        break;
                    case 4:
                        Win();
                        break;
                }
            }
        }
       
    }
    void ClearBrick()
    {
        foreach(GameObject brick in l_Bricks)
        {
            Destroy(brick);
        }
        brickOwner = 0;
        transform.position = new Vector3(transform.position.x,0.3f,transform.position.z);
        l_Bricks.Clear();
    }
    void Lose()
    {
        Debug.Log("Game Over");
    }
    void Win()
    {
        Debug.Log("You Win");
        Debug.Log("Score: " + brickOwner.ToString());
        ClearBrick();
    }    
    public void AddBrick(int i,int j)
    {
        _mapBuilder.Map[i][j] = 5;
        Destroy(_mapBuilder.L_MapBricks[i][j].gameObject);
        Instantiate(brickWhitePrefab,new Vector3(i,0,j),brickWhitePrefab.transform.rotation);
        brickOwner++;
        transform.position += new Vector3(0, brickHeight, 0);
        l_Bricks.Add(Instantiate(brickYellowPrefab, transform.position +offset - (brickOwner + 1) * brickHeight * Vector3.up, brickYellowPrefab.transform.rotation, transform));
        
    }
    
    public void RemoveBrick(int i,int j)
    {
        _mapBuilder.Map[i][j] = 5;
        Destroy(_mapBuilder.L_MapBricks[i][j].gameObject);
        Instantiate(brickWhitePrefab, new Vector3(i, 0, j), brickWhitePrefab.transform.rotation);
        Destroy(l_Bricks[brickOwner - 1].gameObject);
        l_Bricks.RemoveAt(brickOwner-1);
        transform.position -= new Vector3(0, brickHeight, 0);
        brickOwner--;
    }

}
