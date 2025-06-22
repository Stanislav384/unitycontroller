using System.Collections;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [Header("Параметры падения")]
    [Tooltip("Задержка в секундах, прежде чем платформа начнет исчезать после того, как игрок войдет в зону активации.")]
    public float fallDelay = 1.0f; 
    [Tooltip("Задержка в секундах, прежде чем платформа вернется на исходное место после исчезновения.")]
    public float respawnDelay = 3.0f; 

    private Vector3 initialPosition;        // Начальная позиция платформы в мире
    private Rigidbody2D rb;                 // Ссылка на компонент Rigidbody2D основной платформы
    private Collider2D platformCollider;    // Ссылка на коллайдер основной платформы
    private SpriteRenderer spriteRenderer;  // Ссылка на спрайт-рендер основной платформы
    private bool isFalling = false;         // Флаг, чтобы избежать повторного запуска корутины



    void Awake()
    {
        // Запоминаем начальную позицию основной платформы
        initialPosition = transform.position; 

        // Получаем ссылки на компоненты основной платформы
        rb = GetComponent<Rigidbody2D>();
        platformCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Проверяем, что Rigidbody 2D установлен в Kinematic
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

    }

    // Этот метод вызывается, когда другой триггер-коллайдер входит в ТРИГГЕР-КОЛЛАЙДЕР ЗОНЫ АКТИВАЦИИ
    // Этот метод будет находиться в FallingPlatform.cs, но он будет реагировать на OnTriggerEnter2D
    // на дочернем объекте FallTriggerZone благодаря тому, что FallTriggerZone является его потомком.
    // Главное, чтобы скрипт был на родительском объекте, а триггер на дочернем.
    void OnTriggerEnter2D(Collider2D other)
    {
        // Проверяем, что вошел игрок И что это коллайдер именно FallTriggerZone,
        // а не какой-либо другой триггер на этой же платформе (если они будут).
        // Слой FallPlatformTrigger очень помогает здесь.
        if (other.CompareTag("Player") && other.gameObject.layer == LayerMask.NameToLayer("FallPlatformTrigger") && !isFalling)
        {
            isFalling = true; // Устанавливаем флаг, чтобы не запускать корутину повторно
            StartCoroutine(FallAndRespawn()); // Запускаем корутину падения и возвращения
        }
    }

    // Корутина для управления последовательностью падения и возвращения платформы
    IEnumerator FallAndRespawn()
    {
        // 1. Ждем fallDelay секунд перед исчезновением
        yield return new WaitForSeconds(fallDelay);

        // Отключаем коллайдер основной платформы и спрайт-рендер, чтобы она стала невидимой и не взаимодействовала
        if (platformCollider != null) platformCollider.enabled = false;
        if (spriteRenderer != null) spriteRenderer.enabled = false;

        // Также отключим коллайдер зоны триггера, чтобы он не срабатывал, пока платформа "отсутствует"
        // Мы можем найти его, так как он является дочерним объектом
        Collider2D triggerCollider = transform.Find("FallTriggerZone")?.GetComponent<Collider2D>();
        if (triggerCollider != null) triggerCollider.enabled = false;

        // 3. Ждем respawnDelay секунд перед возвращением платформы
        yield return new WaitForSeconds(respawnDelay);

        // 4. Возвращение платформы
        // Включаем коллайдер основной платформы и спрайт-рендер
        if (platformCollider != null) platformCollider.enabled = true;
        if (spriteRenderer != null) spriteRenderer.enabled = true;

        // Включаем коллайдер зоны триггера обратно
        if (triggerCollider != null) triggerCollider.enabled = true;

        // Возвращаем платформу на начальную позицию
        transform.position = initialPosition;
        // Сбрасываем любую линейную скорость, если она вдруг появилась (на всякий случай)
        if (rb != null) rb.linearVelocity = Vector2.zero;

        

        isFalling = false; // Сбрасываем флаг, чтобы платформа снова могла падать
    }
}
