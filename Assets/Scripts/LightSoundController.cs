using UnityEngine;

public class LightSoundController : MonoBehaviour
{
    public FrequencyBandAnalyser _FFT;
    public FrequencyBandAnalyser.Bands _FreqBands = FrequencyBandAnalyser.Bands.Eight;

    public int _BandIndex = 0;
    public float _RotationStrength = 500f;  // 原来的50太小了
    public float _Gain = 150f;             // 新增：放大频率值

    private float _smoothedValue = 0f;

    void Update()
    {
        float bandValue = _FFT.GetBandValue(_BandIndex, _FreqBands) * _Gain;

        // 平滑过渡（避免乱抖）
        _smoothedValue = Mathf.Lerp(_smoothedValue, bandValue, Time.deltaTime * 10f);

        // 用放大的值去转动
        transform.Rotate(Vector3.up, _smoothedValue * _RotationStrength * Time.deltaTime);
    }
}
