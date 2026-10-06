using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject gameoverText; // 게임 오버 시 활성화할 텍스트 게임 오브젝트
    public Text timeText; // 생존 시간을 표시할 텍스트 컴포넌트
    public Text recordText; // 최고 기록을 표시할 텍스트 컴포넌트

    private float survivalTime; // 생존 시간
    private bool isGameOver; // 게임 오버 상태
    void Start()
    {
        Time.timeScale = 1f; // 재시작 시 멈춘 시간을 되돌림
        survivalTime = 0f;
        isGameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        // ESC: 게임 종료 (에디터에서는 플레이 모드 종료)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
            return;
        }

        // R: 언제든 다시 시작 (게임 중에도, 게임 오버 후에도)
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
            return;
        }

        if (!isGameOver)
        {
            survivalTime += Time.deltaTime;
            timeText.text = "Time: " + (int)survivalTime;
        }
    }
    public void EndGame()
    {
        // 현재 상태를 게임 오버 상태로 변경
        isGameOver = true;
        // 게임 오버 텍스트 활성화
        gameoverText.SetActive(true);
        Time.timeScale = 0f; // 총알, 스폰, 패턴 모두 정지

        // BestTime 키로 저장된 이전까지의 최고 기록 가져오기
        float BestTime = PlayerPrefs.GetFloat("BestTime");

        // 현재 생존 시간이 이전까지의 최고 기록보다 큰 경우
        if (survivalTime > BestTime)
        {
            // 현재 생존 시간을 최고 기록으로 저장
            BestTime = survivalTime;
            // 변경된 최고 기록을 BestTime 키로 저장
            PlayerPrefs.SetFloat("BestTime", BestTime);
            PlayerPrefs.Save();
        }
        // 최고 기록을 recordText 텍스트 컴포넌트를 이용해 표시
        recordText.text = "Best Time: " + (int)BestTime;
    }

    private void RestartGame()
    {
        Time.timeScale = 1f; // LoadScene 전에 반드시 복구
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
