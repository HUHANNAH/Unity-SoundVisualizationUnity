using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    public Vector3 rotationSpeed = new Vector3(0, 30, 0); // 每秒自转角速度（度）
    public Transform orbitCenter;                         // 公转中心（可选）
    public float orbitSpeed = 0f;                         // 公转速度（度/秒）
    public float orbitRadius = 3f;                        // 公转半径

private float orbitAngle = 0f;

    void Update()
    {
        // 自转
        transform.Rotate(rotationSpeed * Time.deltaTime);

        // 公转
        if (orbitCenter != null && orbitSpeed != 0f)
        {
            orbitAngle += orbitSpeed * Time.deltaTime;
            float x = Mathf.Cos(orbitAngle * Mathf.Deg2Rad) * orbitRadius;
            float z = Mathf.Sin(orbitAngle * Mathf.Deg2Rad) * orbitRadius;
            transform.position = orbitCenter.position + new Vector3(x, 0, z);
        }
    }

}
