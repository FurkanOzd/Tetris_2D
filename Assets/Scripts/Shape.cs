using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape : MonoBehaviour
{
    private double boardWidth=10;
    private double boardHeight = 20;
    private float fallTime = 0f;
    GameManager gameManager;
    [SerializeField] Vector2 defaultPos;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    public Vector2 getDefaultPos() 
    {
        return defaultPos;
    }

	void Move()
	{
        fallTime += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.RotateAround(transform.position,Vector3.forward,90f);
            if (!CollisionDetect(transform.position, true)) 
            {
                transform.RotateAround(transform.position, Vector3.forward, -90f);
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) || fallTime>=GameManager.fallTime)
        {
            Vector3 newPos = new Vector3(0.0f,-1.0f,0.0f);
            if (CollisionDetect(newPos))
            {
                transform.position += newPos;
            }
            else
            {
                for (int i = 0; i < transform.childCount; i++) 
                {
                    Transform child = transform.GetChild(i);
                    int X = Mathf.RoundToInt(child.position.x-0.5f);
                    int Y = Mathf.RoundToInt(child.position.y-0.5f);
                    gameManager.SetGrid(X,Y,child.gameObject);
                }
                enabled = false;
            }
            fallTime = 0f;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Vector3 newPos = new Vector3(-1.0f,0.0f, 0.0f);
            if (CollisionDetect(newPos))
            {
                transform.position += newPos;
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Vector3 newPos = new Vector3(1.0f,0.0f, 0.0f);
            if (CollisionDetect(newPos))
            {
                transform.position += newPos;
            }
        }
    }
    bool CollisionDetect(Vector3 pos, bool rotate = false)
    {
        float X=0;
        float Y=0;
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (!rotate)
            {
                X = child.position.x + pos.x;
                Y = child.position.y + pos.y;
            }
            else
            {
                X = child.position.x;
                Y = child.position.y;
            }
            if (Y > 0 && Y < boardHeight && X > 0 && X < boardWidth)
            {
                if (gameManager.GetGrid(Mathf.RoundToInt(X - 0.5f), Mathf.RoundToInt(Y - 0.5f))!=null)
                {
                    return false;
                }
            }
            if (X < 0 || Y < 0 || X > boardWidth || Y > boardHeight)
            {
                return false;
            }
        }
        return true;
    }
}
