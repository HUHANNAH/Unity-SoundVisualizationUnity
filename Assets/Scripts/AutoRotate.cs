using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    public Vector3 rotationSpeed = new Vector3(0, 30, 0); // ÿ����ת���ٶȣ��ȣ�
    public Transform orbitCenter;                         // ��ת���ģ���ѡ��
    public float orbitSpeed = 0f;                         // ��ת�ٶȣ���/�룩
    public float orbitRadius = 3f;                        // ��ת�뾶

private float orbitAngle = 0f;

    void Update()
    {
        // ��ת
        transform.Rotate(rotationSpeed * Time.deltaTime);

        // ��ת
        if (orbitCenter != null && orbitSpeed != 0f)
        {
            orbitAngle += orbitSpeed * Time.deltaTime;
            float x = Mathf.Cos(orbitAngle * Mathf.Deg2Rad) * orbitRadius;
            float z = Mathf.Sin(orbitAngle * Mathf.Deg2Rad) * orbitRadius;
            transform.position = orbitCenter.position + new Vector3(x, 0, z);
        }
    }

}
