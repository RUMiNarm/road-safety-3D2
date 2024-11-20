using UnityEngine;
using System.Collections;

public class FlyCamera : MonoBehaviour
{
    public float cameraSensitivity = 90;
    public float normalMoveSpeed = 10;      // 現在のコードでいう「Shiftを押していないときの速度」
    public float slowMoveFactor = 0.25f;    // 現在のコードでいう「Ctrlを押しているときの速度」
    public float fastMoveFactor = 1;        // 移動キーのみの速度にするため、1に設定

    private float rotationX = 0.0f;
    private float rotationY = 0.0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // カメラの回転を上下方向も含めて許可
        rotationX += Input.GetAxis("Mouse X") * cameraSensitivity * Time.deltaTime;
        rotationY += Input.GetAxis("Mouse Y") * cameraSensitivity * Time.deltaTime;
        rotationY = Mathf.Clamp(rotationY, -90, 90);

        transform.localRotation = Quaternion.AngleAxis(rotationX, Vector3.up);
        transform.localRotation *= Quaternion.AngleAxis(rotationY, Vector3.left);

        // 水平方向の移動用のベクトル
        Vector3 forward = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;
        Vector3 right = new Vector3(transform.right.x, 0, transform.right.z).normalized;

        // デフォルト移動速度はCtrlを押したときと同じにする
        float moveSpeed = normalMoveSpeed * slowMoveFactor;
        
        // Shiftを押すと、現在の「移動キーのみの速度」に増加
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            moveSpeed = normalMoveSpeed;
        }

        // 水平移動を適用
        transform.position += forward * moveSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
        transform.position += right * moveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;

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
