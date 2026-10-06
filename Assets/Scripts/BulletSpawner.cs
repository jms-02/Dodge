using System.Collections;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject bulletPrefab; // 생성할 탄알의 원본 프리팸
    public float spawnRateMin = 0.5f; // 최소 생성 주기
    public float spawnRateMax = 3f; // 최대 생성 주기

    [Header("Burst Mode")]
    public int burstCount = 3; // 한번에 연사할 총알 수
    public float burstShotInterval = 0.12f; // 연사 간격
    public float burstRateMin = 1.5f; // 연사 모드 최소 생성 주기
    public float burstRateMax = 3f; // 연사 모드 최대 생성 주기

    [Header("Wave Mode")]
    public int fanBulletCount = 7; // 웨이브 모드에서 한 번에 발사할 총알 수
    public float fanAngleStep = 15f; // 웨이브 모드에서 총알 사이의 각도
    public float waveRate = 2f; // 웨이브 모드 생성 주기

    private Transform target; // 발사할 대상
    private float spawnRate; // 생성주기
    private float timeAfterSpawn; // 최근 생성 시점에서 지난 시간
    void Start()
    {
        // 최근 생성 이후의 누적 시간을 0으로 초기화
        timeAfterSpawn = 0f;

        // 탕알 생성 간격을 spawnRateMin과 spawnRateMax 사이에서 랜덤 지정
        spawnRate = Random.Range(spawnRateMin, spawnRateMax);

        // PlayerController 컴포넌트를 가진 게임 오브젝트를 찾아 조준 대상으로 설정
        target = FindAnyObjectByType<PlayerController>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        // 플레이어가 없거나 죽었으면 발사하지 않음
        if (target == null || !target.gameObject.activeInHierarchy) return;

        // timeAfterSpawn 갱신
        timeAfterSpawn += Time.deltaTime;

        if (timeAfterSpawn < spawnRate) return;
        timeAfterSpawn = 0f;

        switch (PatternDirector.CurrentMode)
        {
            case FireMode.Burst:
                StartCoroutine(FireBurst());
                spawnRate = Random.Range(burstRateMin, burstRateMax);
                break;
            case FireMode.Wave:
                FireFan();
                spawnRate = waveRate;
                break;
            default:
                FireSingle();
                spawnRate = Random.Range(spawnRateMin, spawnRateMax);
                break;
        }
    }

    // 플레이어를 향한 수평 방향
    private Vector3 DirectionToTarget()
    {
        Vector3 dir = target.position - transform.position;
        dir.y = 0f; // 수평 방향만 고려

        if (dir.sqrMagnitude < 0.0001f)
        {
            return transform.forward; // 방향이 거의 없으면 현재 앞쪽 방향 반환
        }
        return dir.normalized; // 정규화된 방향 벡터 반환
    }

    private void SpawnBullet(Quaternion rotation)
    {
        Instantiate(bulletPrefab, transform.position, rotation);
    }

    // 일반: 플레이어를 향한 단발 발사
    private void FireSingle()
    {
        SpawnBullet(Quaternion.LookRotation(DirectionToTarget()));
    }

    // 버스트: 짧은 간격으로 여러발 발사
    private IEnumerator FireBurst()
    {
        for (int i = 0; i < burstCount; i++)
        {
            FireSingle(); // 매 발마다 플레이어를 다시 조준
            yield return new WaitForSeconds(burstShotInterval);
        }
    }

    private void FireFan()
    {
        Quaternion center = Quaternion.LookRotation(DirectionToTarget());
        float startAngle = -fanAngleStep * (fanBulletCount - 1) / 2f;

        for (int i = 0; i < fanBulletCount; i++)
        {
            float angle = startAngle + fanAngleStep * i;
            SpawnBullet(center * Quaternion.Euler(0f, angle, 0f));
        }
    }
        
}
