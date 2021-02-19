using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    GameObject finalScoreText;
    // Start is called before the first frame update
    public void Setup(int score) 
    {
        gameObject.SetActive(true);
        finalScoreText = GameObject.FindGameObjectWithTag("FinalPoint");
        finalScoreText.GetComponent<UnityEngine.UI.Text>().text = "SCORE : "+score.ToString();
    }
    public void RestartGame() 
    {
        SceneManager.LoadScene("Game");
    }
	public void Exit()
	{
        Application.Quit();
	}
}
