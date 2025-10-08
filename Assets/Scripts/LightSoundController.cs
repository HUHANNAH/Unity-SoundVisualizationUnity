using UnityEngine;

public class LightSoundController : MonoBehaviour
{
    public FrequencyBandAnalyser _FFT;
    public FrequencyBandAnalyser.Bands _FreqBands = FrequencyBandAnalyser.Bands.Eight;

    public int _BandIndex = 0;
    public float _RotationStrength = 500f;  // ԭ����50̫С��
    public float _Gain = 150f;             // �������Ŵ�Ƶ��ֵ

    private float _smoothedValue = 0f;

    void Update()
    {
        float bandValue = _FFT.GetBandValue(_BandIndex, _FreqBands) * _Gain;

        // ƽ�����ɣ������Ҷ���
        _smoothedValue = Mathf.Lerp(_smoothedValue, bandValue, Time.deltaTime * 10f);

        // �÷Ŵ��ֵȥת��
        transform.Rotate(Vector3.up, _smoothedValue * _RotationStrength * Time.deltaTime);
    }
}
