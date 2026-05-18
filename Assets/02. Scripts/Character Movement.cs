using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField]private CharacterController cc_;
    [SerializeField]private float speed = 10f; // 플레이어 스피드
    [SerializeField]private float mouseSensitive = 100f;
    [SerializeField]private Transform PlayerCamera;

    private float xRotation = 0f; // 캐릭터 방향 조정
    private Vector2 moveInput;
  
    // 마우스 안보이게, 마우스 고정
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    // 캐릭터 움직인 구현 로직
    public void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }
    private void Update()
    {
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y; // 움직임 구현
        move.Normalize(); // 모든 이동 1로 고정
        cc_.Move(move * speed * Time.deltaTime);
    }   
    // 마우스 시점 변환 코드
    public void OnLook(InputAction.CallbackContext ctx)
    {
        Vector2 lookInput = ctx.ReadValue<Vector2>();
        float mouseX = lookInput.x * mouseSensitive * Time.deltaTime;
        float mouseY = lookInput.y * mouseSensitive * Time.deltaTime;

        // 마우스 좌우 회전
        transform.Rotate(Vector3.up * mouseX);
        // 마우스 상하 회전
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -60f,60f);
        PlayerCamera.localRotation = Quaternion.Euler(xRotation,0,0);
    }
}
