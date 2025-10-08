using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FFTObjectArray_v1 : MonoBehaviour
{
    public FrequencyBandAnalyser _FFT;
    public FrequencyBandAnalyser.Bands _FreqBands = FrequencyBandAnalyser.Bands.Eight;


GameObject[] _FFTGameObjects;
    public GameObject _ObjectToSpawn;

    public float _Radius = 5f;                // Բ���뾶
    public Vector3 _ScaleStrength = Vector3.up;
    public Vector3 _RotationController = new Vector3(-90, 0, 0);

    public float _FloatStrength = 0.5f;       // ���¸���ǿ��
    public float _BreathStrength = 0.2f;      // �������������ţ�ǿ��

    Vector3 _BaseScale;

    void Start()
    {
        _FFTGameObjects = new GameObject[(int)_FreqBands];
        _BaseScale = _ObjectToSpawn.transform.localScale;

        for (int i = 0; i < _FFTGameObjects.Length; i++)
        {
            GameObject newFFTObject = Instantiate(_ObjectToSpawn);
            newFFTObject.transform.SetParent(transform);

            // ��ʼ�Ų���Բ����
            float angle = i * Mathf.PI * 2 / _FFTGameObjects.Length;
            float x = Mathf.Cos(angle) * _Radius;
            float z = Mathf.Sin(angle) * _Radius;

            newFFTObject.transform.localPosition = new Vector3(x, 0, z);
            newFFTObject.transform.LookAt(transform.position);
            newFFTObject.transform.Rotate(_RotationController);

            _FFTGameObjects[i] = newFFTObject;
        }
    }

    void Update()
    {
        for (int i = 0; i < _FFTGameObjects.Length; i++)
        {
            float angle = i * Mathf.PI * 2 / _FFTGameObjects.Length;

            // ���¸���������У�
            float y = Mathf.Sin(angle * 2 + Time.time) * _FloatStrength;

            // �������뾶���ţ���Ƶ��������
            float bandValue = _FFT.GetBandValue(i, _FreqBands);
            float scaleFactor = 1 + bandValue * _BreathStrength;

            float x = Mathf.Cos(angle) * _Radius * scaleFactor;
            float z = Mathf.Sin(angle) * _Radius * scaleFactor;

            _FFTGameObjects[i].transform.localPosition = new Vector3(x, y, z);

            // ���Ż��Ǻ�Ƶ�ʹҹ�
            _FFTGameObjects[i].transform.localScale =
                _BaseScale + (_ScaleStrength * bandValue);
        }
    }

}
