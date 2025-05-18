using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;

    void Start()
    {
        GameObject startPoint = GameObject.FindGameObjectWithTag("StartPoint");
        if (startPoint != null)
        {
            Instantiate(playerPrefab, startPoint.transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogError("StartPoint не найден. Убедись, что у объекта выставлен тег 'StartPoint'.");
        }
    }
}
