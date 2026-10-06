using UnityEngine;
using UnityEngine.UI;

// 총알 발사 모드 종류
public enum FireMode
{
    Normal, // 일반 모드
    Burst, // 연발 모드
    Wave // 웨이브 모드
}


public class PatternDirector : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // spawner가 현재 모드를 읽을 수 있도록 static
    public static FireMode CurrentMode { get; private set; } = FireMode.Normal;

    public float normalDuration = 10f; // 일반 모드 지속 시간
    public float burstDuration = 6f; // 연발 모드 지속 시간
    public float waveDuration = 6f; // 웨이브 모드 지속 시간
    public Text modeText; // 현재 모드 표시 UI 텍스트

    // 모드가 돌아가는 순서
    private FireMode[] sequence = { FireMode.Normal, FireMode.Burst, FireMode.Wave };
    private int index;
    private float timer;
    void Start()
    {
        index = 0;
        timer = 0f;
        SetMode(sequence[index]);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if(timer >= GetDuration(CurrentMode))
        {
            timer = 0f;
            index = (index + 1) % sequence.Length;
            SetMode(sequence[index]);
        }
    }

    private float GetDuration(FireMode mode)
    {
        switch (mode)
        {
            case FireMode.Burst: return burstDuration;
            case FireMode.Wave: return waveDuration;
            default: return normalDuration;
        }
    }

    private void SetMode(FireMode mode)
    {
        CurrentMode = mode;
        
        if (modeText != null)
        {
            switch (mode)
            {
                case FireMode.Burst: modeText.text = "버스트 모드"; break;
                case FireMode.Wave: modeText.text = "웨이브 모드"; break;
                default: modeText.text = "일반 모드"; break;
            }
        }
    }
}
