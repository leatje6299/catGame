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

    public void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(mousePosition.action.ReadValue<Vector2>());
        RaycastHit hit;
        Debug.DrawLine(ray.origin, ray.direction, Color.green);
        if (Physics.Raycast(ray, out hit, 100))
        {
            if (hit.collider != null)
            {
                print(hit.collider.tag);
            }
        }
    }
    

}
