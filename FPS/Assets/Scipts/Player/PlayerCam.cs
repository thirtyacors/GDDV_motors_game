using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    [SerializeField] private float lookSensitivity;
    [SerializeField] private float smoothing;

    private GameObject player;
    private Vector2 smoothedVelocity;
    private Vector2 courrentLookingPos;
    
    private void Start() 
    {   
        player = transform.parent.gameObject;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        RotateCamera();
        CheckForShooting();
    }

    private void RotateCamera()
    {
        Vector2 inputValues = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        inputValues = Vector2.Scale(inputValues, new Vector2(lookSensitivity * smoothing, lookSensitivity * smoothing));

        smoothedVelocity.x = Mathf.Lerp(smoothedVelocity.x, inputValues.x, 1f / smoothing);
        smoothedVelocity.y = Mathf.Lerp(smoothedVelocity.y, inputValues.y, 1f / smoothing);

        courrentLookingPos += smoothedVelocity;

        transform.localRotation = Quaternion.AngleAxis(-courrentLookingPos.y, Vector3.right);
        player.transform.localRotation = Quaternion.AngleAxis(courrentLookingPos.x, player.transform.up);

    }
    private void CheckForShooting()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit whatIHit;
            if(Physics.Raycast(transform.position, transform.forward, out whatIHit,  Mathf.Infinity))
            {
                IDamageable damageable = whatIHit.collider.GetComponent<IDamageable>();
                if(damageable != null)
                {
                    damageable.DealDamage(10);
                }
            }
        }
    }
}
