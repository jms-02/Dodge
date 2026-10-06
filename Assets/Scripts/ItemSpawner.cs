using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject shieldItemPrefab; // 생성할 쉴드 아이템 프리팹
    public float spawnIntervalMin = 8f; // 최소 생성 주기
    public float spawnIntervalMax = 15f; // 최대 생성 주기
    public float fieldHalfSize = 7f; // 생성 범위
    public float spawnHeight = 1f; // 생성 높이

    private float timer;
    private float nextSpawnTime;
    private GameObject currentItem; // 현재 생성된 아이템
    void Start()
    {
        //nextSpawnTime = Random.Range(spawnIntervalMin, spawnIntervalMax);
        // 첫 번째 쉴드 아이템은 게임 시작 후 4초 뒤 생성
        nextSpawnTime = 4f;
    }

    // Update is called once per frame
    void Update()
    {
        // 필드에 아이템이 남아 있으면 새로 만들지 않고 대기
        if(currentItem != null) return;
        
        timer += Time.deltaTime;

        if (timer >= nextSpawnTime)
        {            
            timer = 0f;
            nextSpawnTime = Random.Range(spawnIntervalMin, spawnIntervalMax);

            Vector3 pos = new Vector3(
                Random.Range(-fieldHalfSize, fieldHalfSize),
                spawnHeight,
                Random.Range(-fieldHalfSize, fieldHalfSize));

            currentItem = Instantiate(shieldItemPrefab, pos, Quaternion.identity);
        }
    }
}
