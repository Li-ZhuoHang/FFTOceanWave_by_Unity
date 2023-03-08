using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 100f;      // 摄像机移动速度
    public float rotateSpeed = 100f;   // 摄像机旋转速度
    public float zoomSpeed = 10f;      // 摄像机缩放速度
    public float minZoomDistance = 2f; // 摄像机最小距离
    public float maxZoomDistance = 20f; // 摄像机最大距离

    private Vector3 lastMousePos;      // 上一帧鼠标位置
    private Vector3 currentMousePos;   // 当前鼠标位置
    private float rotateX = 0f;        // 摄像机绕x轴的旋转角度
    private float rotateY = 0f;        // 摄像机绕y轴的旋转角度
    private float zoomDistance = 10f;  // 摄像机与目标的距离

    void Start()
    {
        lastMousePos = Input.mousePosition;
        currentMousePos = Input.mousePosition;
    }

    void Update()
    {
        // 鼠标右键旋转摄像机
        if (Input.GetMouseButton(1))
        {
            currentMousePos = Input.mousePosition;
            Vector3 delta = currentMousePos - lastMousePos;
            rotateX += delta.x * rotateSpeed * Time.deltaTime;
            rotateY -= delta.y * rotateSpeed * Time.deltaTime;
            rotateY = Mathf.Clamp(rotateY, -90f, 90f);
            transform.rotation = Quaternion.Euler(-rotateY, rotateX, 0f);
            lastMousePos = currentMousePos;
        }
        else
        {
            lastMousePos = Input.mousePosition;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = 200f;
        }
        else
        {
            moveSpeed = 100f;
        }

        // 鼠标滚轮缩放摄像机
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        zoomDistance -= scroll * zoomSpeed;
        zoomDistance = Mathf.Clamp(zoomDistance, minZoomDistance, maxZoomDistance);

        // wasd键平移摄像机
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 dir = new Vector3(h, 0f, v).normalized;
        transform.position += transform.rotation * dir * moveSpeed * Time.deltaTime;

        // q键向上移动摄像机，e键向下移动摄像机
        float y = 0f;
        if (Input.GetKey(KeyCode.Q))
        {
            y = 1f;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            y = -1f;
        }
        transform.position += Vector3.up * y * moveSpeed * Time.deltaTime;

        // 更新摄像机的位置和朝向
        Vector3 targetPos = transform.position - transform.forward * zoomDistance;
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 10f);
        transform.LookAt(transform.position + transform.forward * 10f);
    }
}
