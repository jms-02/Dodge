using UnityEngine;

public class ShieldItem : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float shieldDuration = 5f; // 쉴드 지속 시간
    public float lifeTime = 10f; // 쉴드아이템이 존재하는 시간
    private Renderer itemRenderer;

    void Start()
    {
        itemRenderer = GetComponent<Renderer>();
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();

        if (player != null)
        {
            // 쉴드 아이템의 현재 색을 플레이어에게 전달
            Color itemColor = itemRenderer.material.color;
            player.ActivateShield(shieldDuration, itemColor);

            Destroy(gameObject);
        }
    }
}
