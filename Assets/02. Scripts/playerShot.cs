using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShot : MonoBehaviour
{
    [SerializeField]private float shotRange = 50f;
    [SerializeField]private Camera playerFPSCam;
    [SerializeField]private LayerMask target; //없앨 타겟 조사
    [SerializeField]private SpawnTarget spawnTarget; // 다른 클래스 들고오기(정확도 모름)
    [SerializeField]private ScoreManagement scoreRaise; // 이것도(사실 맞는지는 모름)
    [SerializeField]private AccuracySystem accuracyManager;
    // InputSystem으로 제작
    public void OnShot(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        if (!StartCountdown.canClick) return;

        accuracyManager.addShot();
        Ray ray = playerFPSCam.ScreenPointToRay(Mouse.current.position.ReadValue());// 플레이어 캠에서 레이저 발사
        RaycastHit hit;

        if (Physics.Raycast(ray,out hit, shotRange,target)) // 타겟 레이어가 맞았다면
        {
            accuracyManager.addHit();
            Debug.DrawRay(ray.origin,ray.direction*shotRange,Color.orange,2f);
            Destroy(hit.collider.gameObject); // 타겟으로 지정된 레이어의 오브젝트 삭제
            spawnTarget.SpawnNewTarget(); // 새 타겟 생성
            scoreRaise.ScoreRaised(); // 점수 추가
        }
        else
        {
            scoreRaise.Scoredown(); // 점수 감소
        }
    }
}
