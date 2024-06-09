using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;
    public float distance = 5.0f;
    public float smoothTime = 0f;
    public float mouseSensitivity = 2.0f;

    private Vector3 velocity = Vector3.zero;
    private float yaw = 0.0f;
    private float pitch = 15.0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (target == null)
        {
            Debug.LogError("Target not assigned for ThirdPersonCamera.");
            return;
        }

        yaw = target.eulerAngles.y;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (DialogueSystem.Instance.IsInDialogue || InventorySystem.Instance.IsInventoryOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (Cursor.lockState == CursorLockMode.Locked)
        {
            float rotateX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float rotateY = Input.GetAxis("Mouse Y") * mouseSensitivity;
            RotateCamera(rotateX, -rotateY);
        }
    }

    void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        UpdateCameraPosition();
    }

    private void RotateCamera(float rotateX, float rotateY)
    {
        yaw += rotateX;
        pitch += rotateY;

        pitch = Mathf.Clamp(pitch, 0f, 60f);
    }

    private void UpdateCameraPosition()
    {
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 offset = new(0, 0, -distance);
        Vector3 rotatedOffset = rotation * offset;

        Vector3 desiredPosition = target.position + rotatedOffset;
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);
        transform.position = smoothedPosition;

        transform.LookAt(target.position);
    }
}