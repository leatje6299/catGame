using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class CharController : MonoBehaviour
{
    [SerializeField] private InputActionReference movementControl;
    [SerializeField] private InputActionReference jumpControl;
    [SerializeField] private InputActionReference mousePosition;
    [SerializeField] private FieldOfView fow;

    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float rotationSpeed = 4f;

    private CharacterController controller;
    private Vector3 playerVelocity;

    private bool groundedPlayer;
    private bool canJump;

    private Transform cameraMainTransform;
    private Camera cameraMain;

    private void OnEnable()
    {
        movementControl.action.Enable();
        jumpControl.action.Enable();
    }

    private void OnDisable()
    {
        movementControl.action.Disable();
        jumpControl.action.Disable();
    }

    private void Awake()
    {
        controller = gameObject.GetComponent<CharacterController>();
        cameraMainTransform = Camera.main.transform;
        cameraMain = Camera.main;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Start()
    {
        StartCoroutine("FindTargetsWithDelay", .2f);
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector2 movement = movementControl.action.ReadValue<Vector2>();
        Vector3 move = new Vector3(movement.x, 0, movement.y);
        move = cameraMainTransform.forward * move.z + cameraMainTransform.right * move.x;
        move.y = 0f;
        controller.Move(move * Time.deltaTime * playerSpeed);

        // Changes the height position of the player..
        if (jumpControl.action.triggered && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        //add gravity
        playerVelocity.y += gravityValue * Time.deltaTime;

        controller.Move(playerVelocity * Time.deltaTime);

        if(movement != Vector2.zero)
        {
            float targetAngle = Mathf.Atan2(movement.x, movement.y) * Mathf.Rad2Deg + cameraMainTransform.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
        }
    }

    IEnumerator FindTargetsWithDelay(float delay)
    {
       while(true)
        {
            yield return new WaitForSeconds(delay);
            fow.FindVisibleTargets();
        }
    }

    private IEnumerator JumpToTarget()
    {
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = new Vector3(0,0,0); //change

        float distance = (startPosition - targetPosition).magnitude;
        float speed = 10f; //create variable to change that
        float duration = distance / speed;
        for(float f = 0; f < 1; f+= Time.deltaTime/duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, f);
            yield return null;
        }
    }   
  




//https://gist.github.com/ditzel/68be36987d8e7c83d48f497294c66e08
    public static Vector3 Parabola(Vector3 start, Vector3 end, float height, float t)
    {
        Func<float, float> f = x => -4 * height * x * x + 4 * height * x;

        var mid = Vector3.Lerp(start, end, t);

        return new Vector3(mid.x, f(t) + Mathf.Lerp(start.y, end.y, t), mid.z);
    }
}

