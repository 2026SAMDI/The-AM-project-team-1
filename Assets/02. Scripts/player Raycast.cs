using UnityEngine;

public class playerRaycast : MonoBehaviour
{
    [SerializeField]private float rayDistance = 5f;

    private void Update()
    {
        Ray ray = new Ray(transform.position,transform.forward);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, rayDistance))
        {
            Debug.Log($"Target Hit:{hit.collider.gameObject.name}");
        }
        Debug.DrawRay(ray.origin,ray.direction * rayDistance,Color.red);

    }
}
