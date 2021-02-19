using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    const int boardHeight = 20;
    const int boardWidth = 10;
    GameObject[,]Grid =new GameObject[boardWidth,boardHeight];
    [SerializeField] Shape[] Shapes;
    public static bool isPaused=false;
    int score=0;
    int level=1;
    int line=0;
    bool gameOver = false;
    GameObject scoreText;
    GameObject linesText;
    GameObject levelText;
    GameObject nextShapeBox;
    AudioManager audioManager;
    public GameOverMenu GameOverScreen;
    public static float fallTime = 0.6f;
    public static float inputTime = 0.08f;
    Shape nextShape;
    Shape currentShape;
    void Start()
    {
        scoreText = GameObject.Find("ScoreText");
        linesText = GameObject.Find("LinesText");
        levelText = GameObject.Find("LevelText");
        nextShapeBox = GameObject.Find("NextShapeBox");
        audioManager = FindObjectOfType<AudioManager>();
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
        if (!currentShape.enabled && gameOver==false)
        {
            GameOver();
            if (!gameOver) 
            {
                spawnShape();
            }
            CheckGrid();
        }
    }
    public void SetGrid(int X, int Y, GameObject shapeBlock)                // Fill grid with tetromino blocks.
    {
        if (Y < 20)
            Grid[X, Y] = shapeBlock;
    }
    public GameObject GetGrid(int X, int Y)
    {
        return Grid[X, Y];
    }
    void spawnShape()                                 //Spawn new shape
    {
        audioManager.Play("FallSound");
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
    void GameOver()                                              //GameOver
    {
        for (int i = 0; i < boardWidth; i++) 
        {
            if (Grid[i, boardHeight-1] != null) 
            {
                gameOver = true;
                audioManager.Play("GameOverSound");
                GameOverScreen.Setup(score);
            }
        }
    }
    void CheckGrid()                           //Checking line is full 
    {
        bool isLineFilled;
        int firstLineIndex = -1;
        int filledLineCount = 0;
        for (int j = 0; j<boardHeight; j++)
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
                if (firstLineIndex == -1) 
                {
                    firstLineIndex = j;
                }
                filledLineCount++;
            }
        }
        if (filledLineCount > 0)
        {
            DeleteLine(firstLineIndex, filledLineCount);
        }
    }
    void DeleteLine(int firstLine,int count)
    {
        for (int j = firstLine; j < firstLine + count; j++)
        {
            for (int i = 0; i < boardWidth; i++)
            {
                Destroy(Grid[i, j]);
            }
        }
        audioManager.Play("LineClearSound");
        EditGrid(firstLine, count);
        Scoring(count);
        RemoveParentObjects();
    }
    void EditGrid(int lineNumber,int count)                            //Edit Grid when lines deleted.
    {
        for (int j = lineNumber+count; j<boardHeight; j++)
        {
            for (int i = 0; i < boardWidth; i++)
            {
                if (Grid[i, j] != null)
                {
                    Vector3 pos = Grid[i, j].transform.position;
                    Grid[i, j].transform.position = new Vector3(pos.x, pos.y - 1.0f*count, pos.z);
                    Grid[i, j - count] = Grid[i, j];
                    Grid[i, j] = null;
                }
            }
        }
    }
    void Scoring(int count)                                 
    {
        if (count > 1)
            score += (100 + (count * 10)) * count;
        else 
        {
            score += 100;
        }
        for (int i = 0; i < count; i++)
        {
            line++;
        }
        if (line % 10 == 0) 
        {
            level++;
            fallTime -= 0.03f;
        }
        scoreText.GetComponent<UnityEngine.UI.Text>().text = "  Score: " + score;
        linesText.GetComponent<UnityEngine.UI.Text>().text = "  Lines: " + line;
        levelText.GetComponent<UnityEngine.UI.Text>().text = "  Level: " + level;
    }
    void RemoveParentObjects()                                        // If a tetromino block has no child then it's empty and remove
    {
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
