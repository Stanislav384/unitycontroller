using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player; // Ссылка на игрока
    public Vector3 offset = new Vector3(0, 3, -5); // Смещение камеры
    public float sensitivity = 3f; // Чувствительность мыши

    private float currentX = 0f; // Текущий угол по оси X
    private float currentY = 0f; // Текущий угол по оси Y
    public float minY = -20f; // Минимальный угол камеры по Y
    public float maxY = 60f;  // Максимальный угол камеры по Y

    void Update()
    {
        // Получение ввода от мыши
        currentX += Input.GetAxis("Mouse X") * sensitivity;
        currentY -= Input.GetAxis("Mouse Y") * sensitivity;

        // Ограничение углов по оси Y
        currentY = Mathf.Clamp(currentY, minY, maxY);
    }

    void LateUpdate()
    {
        // Вычисление позиции камеры
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        Vector3 desiredPosition = player.position + rotation * offset;

        // Установка позиции и направления камеры
        transform.position = desiredPosition;
        transform.LookAt(player.position + Vector3.up * 1.5f); // Смотрим чуть выше центра персонажа
    }
}
