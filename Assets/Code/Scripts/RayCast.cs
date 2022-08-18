using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RayCast : MonoBehaviour
{
    public GameObject currentHitObj = null;
    [SerializeField] private InputActionReference mousePosition;

    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;    
    }

    private void DetectObject()
    {
        Ray ray = mainCamera.ScreenPointToRay(mousePosition.action.ReadValue<Vector2>());
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 200))
        {
            if(hit.collider != null)
            {
                print(hit.collider.tag);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Debug.DrawLine(transform.position, transform.forward);
    }
}
