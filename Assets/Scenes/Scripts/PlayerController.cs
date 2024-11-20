using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float cameraSensitivity = 90;
    public float moveSpeed = 5;
    public float fastMoveFactor = 1.5f;

    private float rotationX = 0.0f;
    private float rotationY = 0.0f;
    private Transform cameraTransform;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // 子オブジェクトであるカメラを取得
        cameraTransform = GetComponentInChildren<Camera>().transform;
    }

    void Update()
    {
        // マウス操作による視点の回転
        rotationX += Input.GetAxis("Mouse X") * cameraSensitivity * Time.deltaTime;
        rotationY += Input.GetAxis("Mouse Y") * cameraSensitivity * Time.deltaTime;
        rotationY = Mathf.Clamp(rotationY, -90, 90);

        // プレイヤー（人型オブジェクト）自体の水平回転
        transform.localRotation = Quaternion.AngleAxis(rotationX, Vector3.up);

        // カメラの上下回転
        cameraTransform.localRotation = Quaternion.AngleAxis(rotationY, Vector3.left);

        // 移動速度
        float adjustedMoveSpeed = moveSpeed;
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            adjustedMoveSpeed *= fastMoveFactor;
        }

        // 移動処理
        Vector3 forward = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;
        Vector3 right = new Vector3(transform.right.x, 0, transform.right.z).normalized;

        transform.position += forward * adjustedMoveSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
        transform.position += right * adjustedMoveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;

        // カーソルロックの切り替え
        if (Input.GetKeyDown(KeyCode.End))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
}
