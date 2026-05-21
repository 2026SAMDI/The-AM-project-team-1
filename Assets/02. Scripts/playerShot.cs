using UnityEngine;
using UnityEngine.InputSystem;

public class playerShot : MonoBehaviour
{
    [SerializeField]private float shotRange = 50f;
    [SerializeField]private Camera playerFPSCam;
    [SerializeField]private LayerMask target; //없앨 타겟 조사
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)// 마우스를 눌렀을때
        {
            shot(); // 총 발사 신호 주기
        }
    }

    public void shot()
    {
        Ray ray = playerFPSCam.ScreenPointToRay(Mouse.current.position.ReadValue()); // 플레이어 캠에서 레이저 발사
        RaycastHit hit;

        if (Physics.Raycast(ray,out hit, shotRange,target)) // 타겟 레이어가 맞았다면
        {
            Debug.DrawRay(ray.origin,ray.direction*shotRange,Color.orange,2f);
            Destroy(hit.collider.gameObject); // 타겟으로 지정된 레이어의 오브젝트 삭제
        }
    }
}
