using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float speed = 4f; // 탄알 이동 속력
    private Rigidbody bulletRigidbody; // 이동에 사용할 리지드 바디 컴포넌트
    void Start()
    {
        // 게임 오브젝트에서 Rigidbody 컴포넌트를 찾아 bulletRigidbody에 할당
        bulletRigidbody = GetComponent<Rigidbody>();
        // 리지드바디의 속도 = 앞쪽 방향 * 이동속력
        bulletRigidbody.linearVelocity = transform.forward * speed;

        // 3초 뒤에 자시의 게임 오브젝트 파괴
        Destroy(gameObject, 3f);
    }

    // 트리거 충돌 시 자동으로 실행되는 메소드
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerController playerController = other.GetComponent<PlayerController>();
        if (playerController == null) return;

        if (playerController.IsInvincible)
            Destroy(gameObject); // 무적: 탄알만 사라짐
        else
            playerController.Die();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
