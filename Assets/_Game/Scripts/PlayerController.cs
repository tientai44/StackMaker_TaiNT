using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Vector3 = UnityEngine.Vector3;

public class PlayerController : MonoBehaviour
{
    
    [SerializeField] MapBuilder _mapBuilder;
    [SerializeField] int brickOwner = 0;
    [SerializeField] private float speed=1f;
    [SerializeField] private List<GameObject> l_Bricks= new List<GameObject>(); 
    [SerializeField] private GameObject brickYellowPrefab;
    [SerializeField] private GameObject brickWhitePrefab;
    Vector3 offset = new Vector3(0,-0.1f,0);
    [SerializeField] float brickHeight = 0.15f;
    bool isTarget = false;
    direction currentDirection=direction.idle;
    float timer=1f;

    Queue<Vector3> l_Target= new Queue<Vector3>();
    Vector3 target;

    public Queue<Vector3> L_Target { get => l_Target; set => l_Target = value; }
    public direction CurrentDirection { get => currentDirection; set => currentDirection = value; }
    public int BrickOwner { get => brickOwner; set => brickOwner = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnInit()
    {
        transform.position = new Vector3(_mapBuilder.XStart, 0.3f, _mapBuilder.YStart);
        
    }
    // Update is called once per frame
    void Update()
    {
        
        if(l_Target.Count > 0)
        {
            if (!isTarget)
            {
                target = new Vector3(l_Target.Peek().x, transform.position.y , l_Target.Peek().z);
                isTarget = true;
            }
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            if (transform.position==target)
            {
                isTarget = false;
                l_Target.Dequeue();
                int x = (int)transform.position.x;
                int y = (int)transform.position.z;
                switch (_mapBuilder.Map[x][y])
                {
                    case 1:
                        AddBrick(x, y);
                        break;
                    case 2:
                        if (brickOwner <= 0)
                        {
                            MapBuilder.GetInstance().Lose();
                            return;
                        }
                        RemoveBrick(x, y);
                        break;
                    case 4:
                        _mapBuilder.Win();
                        break;
                    case 5:
                        break;
                }
            }
            return;
        }
        
    }
    
    public void SetTarget()
    {
        if (currentDirection == direction.top)
        {
            int numBrick = _mapBuilder.CountBrick((int)transform.position.x,(int)transform.position.z,direction.top);

            for(int i = 1; i <= numBrick; i++)
            {
                l_Target.Enqueue(new Vector3(transform.position.x - i, 0, transform.position.z));
              
            }
       
        }
        else if (currentDirection == direction.down)
        {
            int numBrick = _mapBuilder.CountBrick((int)transform.position.x, (int)transform.position.z, direction.down);

            for (int i = 1; i <= numBrick; i++)
            {
                l_Target.Enqueue(new Vector3(transform.position.x + i, 0, transform.position.z));

            }
        }
        else if (currentDirection == direction.right)
        {
            int numBrick = _mapBuilder.CountBrick((int)transform.position.x, (int)transform.position.z, direction.right);

            for (int i = 1; i <= numBrick; i++)
            {
                l_Target.Enqueue(new Vector3(transform.position.x , 0, transform.position.z+i));
            }
        }
        else if (currentDirection == direction.left)
        {
            int numBrick = _mapBuilder.CountBrick((int)transform.position.x, (int)transform.position.z, direction.left);

            for (int i = 1; i <= numBrick; i++)
            {
                l_Target.Enqueue(new Vector3(transform.position.x , 0, transform.position.z-i));

            }
        }
    }
    public void ClearBrick()
    {
        foreach(GameObject brick in l_Bricks)
        {
            Destroy(brick);
        }
        UIManager.GetInstance().PointText.text = "Score: " + BrickOwner.ToString();
        brickOwner = 0;
        transform.position = new Vector3(transform.position.x,0.3f,transform.position.z);
        l_Bricks.Clear();
    }
    
    
    public void AddBrick(int i,int j)
    {
        _mapBuilder.Map[i][j] = 5;
        Destroy(_mapBuilder.L_MapBricks[i][j].gameObject);
        _mapBuilder.L_MapBricks[i][j] = Instantiate(brickWhitePrefab,new Vector3(i,0,j),brickWhitePrefab.transform.rotation);
        brickOwner++;
        transform.position += new Vector3(0, brickHeight, 0);
        l_Bricks.Add(Instantiate(brickYellowPrefab, transform.position +offset - (brickOwner + 1) * brickHeight * Vector3.up, brickYellowPrefab.transform.rotation, transform));
        
    }
    
    public void RemoveBrick(int i,int j)
    {
        _mapBuilder.Map[i][j] = 5;
        Destroy(_mapBuilder.L_MapBricks[i][j].gameObject);
        _mapBuilder.L_MapBricks[i][j]= Instantiate(brickWhitePrefab, new Vector3(i, 0, j), brickWhitePrefab.transform.rotation);
        Destroy(l_Bricks[brickOwner - 1].gameObject);
        l_Bricks.RemoveAt(brickOwner-1);
        transform.position -= new Vector3(0, brickHeight, 0);
        brickOwner--;
    }

}
