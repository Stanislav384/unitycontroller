using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 6f; // Скорость ходьбы
    public float runSpeed = 12f; // Скорость бега
    public float rotationSpeed = 200f; // Скорость поворота
    public float jumpForce = 5f; // Сила прыжка
    public LayerMask groundLayer; // Слой земли для проверки

    private Rigidbody rb; 
    private bool isGrounded; 
    private float currentSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = runSpeed; // Бег
        }
        else
        {
            currentSpeed = moveSpeed; // Ходьба
        }

        // Управление движением и поворотом
        float horizontal = Input.GetAxis("Horizontal"); // Поворот (A/D)
        float vertical = Input.GetAxis("Vertical");     // Движение вперёд/назад (W/S)

        // Поворот персонажа
        if (horizontal != 0)
        {
            transform.Rotate(0, horizontal * rotationSpeed * Time.deltaTime, 0);
        }

        // Движение вперёд/назад
        if (vertical != 0)
        {
            Vector3 move = transform.forward * vertical * currentSpeed * Time.deltaTime;
            rb.MovePosition(rb.position + move);
        }

        // Прыжок (только если на земле)
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Debug.Log("Jump!");
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {
        Vector3 groundCheckPosition = transform.position + Vector3.down * 1f;

        isGrounded = Physics.CheckSphere(groundCheckPosition, 0.5f, groundLayer);

        Debug.DrawLine(transform.position, groundCheckPosition, isGrounded ? Color.green : Color.red);
        Debug.Log("Is Grounded: " + isGrounded);
    }
}
