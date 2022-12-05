using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text ScoreTextHighscore;
    public GameObject GameOverText;

    private bool m_Started = false;
    private int m_Points;

    private bool m_GameOver = false;

    private bool m_IsNewHighscore = false;


    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }

        SetHighscoreText();
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";

        if (!m_IsNewHighscore && m_Points > GameManager.Instance.Highscore)
        {
            Debug.Log("New Highscore");
            NewHighscore();
        }
        else if (m_Points > GameManager.Instance.Highscore)
        {
            UpdateHighscorePoints();
        }

    }

    private void NewHighscore()
    {
        m_IsNewHighscore = true;
        GameManager.Instance.PlayerNameHighscore = GameManager.Instance.PlayerName;
        UpdateHighscorePoints();
    }

    private void UpdateHighscorePoints()
    {
        GameManager.Instance.Highscore = m_Points;
        SetHighscoreText();
    }

    private void SetHighscoreText()
    {
        ScoreTextHighscore.text = $"Best Score : {GameManager.Instance.PlayerNameHighscore} : {GameManager.Instance.Highscore}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        GameManager.Instance.UpdateHallOfFame(GameManager.Instance.PlayerName, m_Points);
        GameManager.Instance.Save();
    }

    public void ExitToMainMenu() {
        SceneManager.LoadScene(0);
    }
}
