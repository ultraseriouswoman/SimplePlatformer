using System.Diagnostics.CodeAnalysis;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float _playerSpeed;
    public float _walkSpeed = 2f;
    public float _sprintSpeed = 4f;
    public float _jumpForce = 100f;
    public float _mouseSensivity = 25f;
    
    public Canvas _gameoverCanvas;
    public Canvas _winCanvas;
    public GameObject _finisher;

    private bool _isGrounded;
    private bool _isDead;
    private bool _isFinished;
    
    private Rigidbody _rb;
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        RotationLogic();

        JumpLogic();

        MovementLogic();

        Dead();

        Win();
    }

    private void MovementLogic()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical   = Input.GetAxis("Vertical");

        Vector3 movement = new(moveHorizontal, 0.0f, moveVertical);

        if (Input.GetAxis("Sprint") > 0)
        {
            SprintLogic(movement);
        }
        else if (Input.GetAxis("Sprint") == 0)
        {
            WalkLogic(movement);
        }
    }

    private void JumpLogic()
    {
        if (Input.GetAxis("Jump") > 0)
        {
            if (_isGrounded)
            {
                _rb.AddForce(Vector3.up * _jumpForce);
            }
        }
    }

    private void WalkLogic(Vector3 movement)
    {
        _playerSpeed = _walkSpeed;
        transform.Translate(movement * _playerSpeed * Time.fixedDeltaTime);
    }
    private void SprintLogic(Vector3 movement)
    {
        _playerSpeed = _sprintSpeed;
        transform.Translate(movement * _playerSpeed * Time.fixedDeltaTime);
    }

    private void RotationLogic()
    {
        Plane playerplane = new(Vector3.up, transform.position); // Определяем плоскость куба в зависимости от позиции куба в пространстве
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Создаем луч, который будет зависеть от расположения мыши в пространстве
        float hitdist; // Переменная, нужная для определения местонахождения курсора в пространстве

        if (playerplane.Raycast(ray, out hitdist)) // Плоскость пересекается с лучом? Раз да, идем дальше
        {
            Vector3 targetpoint = ray.GetPoint(hitdist); // Получаем точку нахождения курсора в пространстве
            Quaternion targetrotation = Quaternion.LookRotation(targetpoint - transform.position); // Получаем, на сколько градусов нужно повернуть куб по отношению к курсору

            transform.rotation = Quaternion.Slerp(transform.rotation, targetrotation, _mouseSensivity * Time.fixedDeltaTime); // Разворачиваем куб к курсору
        }
    }  
    
    private void Dead()
    {
        if (_isDead)
        {
            _gameoverCanvas.gameObject.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }

    private void Win()
    {
        if (_isFinished)
        {
            _winCanvas.gameObject.SetActive(true);
            this.gameObject.SetActive(false);
        } 
    }

    private void OnCollisionEnter(Collision collision)
    {
        IsGroundedUpdate(collision, true);

        IsFinished(collision, true);
    }

    private void OnCollisionExit(Collision collision)
    {
        IsGroundedUpdate(collision, false);

        IsFinished(collision, false);
    }

    private void IsGroundedUpdate(Collision collision, bool value)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            _isGrounded = value; 
        }

        if (collision.gameObject.CompareTag("DeadZone"))
        {
            _isDead = value;
        }
    }

    private void IsFinished(Collision collision, bool value)
    {
        if (collision.gameObject.CompareTag("Finish"))
        {
            _isFinished = value;
        }
    }
}
