using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    private const int boardHeight = 20;
    private const int boardWidth = 10;
    private GameObject[,]Grid =new GameObject[boardWidth,boardHeight];
    [SerializeField] Shape[] Shapes;
    private int score=0;
    private int level=1;
    private int line=0;
    private GameObject scoreText;
    private GameObject linesText;
    private GameObject levelText;
    private GameObject nextShapeBox;
    public static float fallTime = 0.6f;
    Shape nextShape;
    Shape currentShape;
    public void SetGrid(int X,int Y,GameObject shapeBlock) 
    {
        Grid[X, Y] = shapeBlock;
    }
    public GameObject GetGrid(int X,int Y) 
    {
        return Grid[X, Y];
    }
    void Start()
    {
        scoreText = GameObject.Find("ScoreText");
        linesText = GameObject.Find("LinesText");
        levelText = GameObject.Find("LevelText");
        nextShapeBox = GameObject.Find("NextShapeBox");

        currentShape = Shapes[Random.Range(0, Shapes.Length)];
        Vector3 pos = new Vector3(transform.position.x, 10, 1);
        currentShape = Instantiate(currentShape, currentShape.getDefaultPos(), Quaternion.identity);  // No rotation,
        nextShape = spawnNextShape();
        Vector3 shapeBoxPos = nextShapeBox.transform.position;
        nextShape = Instantiate(nextShape, new Vector3(shapeBoxPos.x, shapeBoxPos.y, shapeBoxPos.z), Quaternion.identity);
        nextShape.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!currentShape.enabled)
        {
            spawnShape();
            CheckGrid();
        }
    }
    void spawnShape() 
    {
        currentShape = nextShape;
        currentShape.transform.position = currentShape.getDefaultPos();
        currentShape.enabled = true;
        nextShape = spawnNextShape();
        Vector3 shapeBoxPos = nextShapeBox.transform.position;
        nextShape = Instantiate(nextShape, new Vector3(shapeBoxPos.x, shapeBoxPos.y, shapeBoxPos.z), Quaternion.identity);
        nextShape.enabled = false;
    }
    Shape spawnNextShape() 
    {
        Shape shape = Shapes[Random.Range(0, Shapes.Length)];
        return shape;
    }
    void CheckGrid()
    {
        bool isLineFilled;
        for (int j = boardHeight-1; j>=0; j--)
        {
            isLineFilled = false;
            for (int i = 0; i < boardWidth; i++)
            {
                if (Grid[i,j] != null)
                {
                    isLineFilled = true;
                }
                else
                {
                    isLineFilled = false;
                    break;
                }
            }
            if (isLineFilled == true)
            {
                DeleteLine(j);
                Scoring();
            }
        }
    }
    void DeleteLine(int j) 
    {
        for (int i = 0; i < boardWidth; i++) 
        {
            Destroy(Grid[i, j]);
        }
        RemoveParentObjects();
        EditGrid(j);
    }
    void EditGrid(int lineNumber)
    {
        for (int j = lineNumber + 1; j < boardHeight; j++)
        {
            for (int i = 0; i < boardWidth; i++)
            {
                if (Grid[i, j] != null)
                {
                    Vector3 pos = Grid[i, j].transform.position;
                    Grid[i, j].transform.position = new Vector3(pos.x, pos.y - 1.0f, pos.z);
                    Grid[i, j - 1] = Grid[i, j];
                    Grid[i, j] = null;
                }
            }
        }
    }
    void Scoring() 
    {
        score += 100;
        line++;
        if (line % 10 == 0) 
        {
            level++;
        }
        scoreText.GetComponent<UnityEngine.UI.Text>().text = "  Score:    " + score;
        linesText.GetComponent<UnityEngine.UI.Text>().text = "  Lines:    " + line;
        levelText.GetComponent<UnityEngine.UI.Text>().text = "  Level:    " + level;
    }
    void RemoveParentObjects() 
    {
        Debug.Log("Burdayim-ParentObject");
        GameObject[] Tetrominoes=GameObject.FindGameObjectsWithTag("Tetromino");
        for (int i = 0; i < Tetrominoes.Length; i++) 
        {
            if (Tetrominoes[i].transform.childCount == 0) 
            {
                Destroy(Tetrominoes[i]);
            }
        }
    }
}
