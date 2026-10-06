using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Rigidbody playerRigidbody; // 이동에 사용할 리지드바디 컴포넌트
    public float speed = 8f; // 이동 속력

    [Header("Shield")]
    public Color shieldColor = Color.cyan; // 무적 상태일 때의 색
    public bool IsInvincible { get; private set; } // 무적 상태 여부

    private Renderer playerRenderer; // 색을 바꾸기 위한 랜더러
    private Color originalColor; // 원래 색
    private Coroutine shieldCoroutine; // 실행 중인 쉴드 코루틴
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerRenderer = GetComponent<Renderer>();
        originalColor = playerRenderer.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        // 수평축과 수직축의 입력값을 감지하여 저장
        float xInput = Input.GetAxis("Horizontal");
        float zInput = Input.GetAxis("Vertical");

        // 실제 이동속도를 입력값과 이동속력을 사용해 결정
        float xSpeed = xInput * speed;
        float zSpeed = zInput * speed;

        // Vector3 속도를 (xSpeed, 0, zSpeed)로 생성
        Vector3 newVelocity = new Vector3(xSpeed, 0f, zSpeed);

        // 리지드바디의 속도에 newVelocity 할당
        playerRigidbody.linearVelocity = newVelocity;
    }

    // 쉴드 아이템을 먹었을 때 호출
    public void ActivateShield(float duration, Color color)
    {
        // 이미 쉴드가 활성화되어 있다면 기존 타이머 중지하고 새로 시작
        if (shieldCoroutine != null)
        {
            StopCoroutine(shieldCoroutine);
        }
        // 쉴드 코루틴 시작
        shieldCoroutine = StartCoroutine(ShieldCoroutine(duration, color));
    }

    private IEnumerator ShieldCoroutine(float duration, Color shieldColor)
    {
        // 무적 상태로 전환
        IsInvincible = true;
        // 색을 무적 상태 색으로 변경
        playerRenderer.material.color = shieldColor;
        float remaining = duration;
        while (remaining > 0f)
        {
            remaining -= Time.deltaTime;

            // 마지막 1.5초는 깜빡여서 곧 끝난다는걸 알려줌
            if(remaining <= 1.5f)
            {
                bool on = Mathf.FloorToInt(remaining * 8f) % 2 == 0;
                // 깜빡임 효과
                playerRenderer.material.color = on ? shieldColor : originalColor;
            }
            yield return null;
        }

        IsInvincible = false;
        playerRenderer.material.color = originalColor;
        shieldCoroutine = null; // 코루틴 종료 후 null로 초기화
    }
    public void Die()
    {
        // 무적 상태면 죽지않음
        if (IsInvincible) return;
        // 자신의 게임 오브젝트를 비활성화
        gameObject.SetActive(false);

        // 씬에 존재하는 GameManager 오브젝트를 찾아서 가져오기
        GameManager gameManager = FindAnyObjectByType<GameManager>();

        // GameManager의 EndGame() 메서드 호출
        gameManager.EndGame();
    }
}
