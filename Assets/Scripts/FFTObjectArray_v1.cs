using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FFTObjectArray_v1 : MonoBehaviour
{
    public FrequencyBandAnalyser _FFT;
    public FrequencyBandAnalyser.Bands _FreqBands = FrequencyBandAnalyser.Bands.Eight;


GameObject[] _FFTGameObjects;
    public GameObject _ObjectToSpawn;

    public float _Radius = 5f;                // 圆环半径
    public Vector3 _ScaleStrength = Vector3.up;
    public Vector3 _RotationController = new Vector3(-90, 0, 0);

    public float _FloatStrength = 0.5f;       // 上下浮动强度
    public float _BreathStrength = 0.2f;      // 呼吸（向外扩张）强度

    Vector3 _BaseScale;

    void Start()
    {
        _FFTGameObjects = new GameObject[(int)_FreqBands];
        _BaseScale = _ObjectToSpawn.transform.localScale;

        for (int i = 0; i < _FFTGameObjects.Length; i++)
        {
            GameObject newFFTObject = Instantiate(_ObjectToSpawn);
            newFFTObject.transform.SetParent(transform);

            // 初始排布在圆周上
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

            // 上下浮动（错落感）
            float y = Mathf.Sin(angle * 2 + Time.time) * _FloatStrength;

            // 呼吸（半径扩张，受频率驱动）
            float bandValue = _FFT.GetBandValue(i, _FreqBands);
            float scaleFactor = 1 + bandValue * _BreathStrength;

            float x = Mathf.Cos(angle) * _Radius * scaleFactor;
            float z = Mathf.Sin(angle) * _Radius * scaleFactor;

            _FFTGameObjects[i].transform.localPosition = new Vector3(x, y, z);

            // 缩放还是和频率挂钩
            _FFTGameObjects[i].transform.localScale =
                _BaseScale + (_ScaleStrength * bandValue);
        }
    }

}
