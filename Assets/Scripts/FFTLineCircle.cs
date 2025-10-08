using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class FFTLineCircleV2 : MonoBehaviour
{
    public FrequencyBandAnalyser _FFT;
    public FrequencyBandAnalyser.Bands _FreqBands = FrequencyBandAnalyser.Bands.Eight;

    public enum Mode { Expand, Wave }
    public Mode mode = Mode.Wave;

    public float _Radius = 5f;          // �����뾶
    public float _Strength = 1f;        // Expand ģʽ������ϵ�������������
    public float _WaveAmplitude = 1f;   // Wave ģʽ�µĶ������������Y �ᣩ
    public float _WaveBaseY = 0f;       // Wave ģʽ�µĻ��� Y���߶�ƫ�ƣ�
    public int PointsPerBand = 8;       // ÿ��Ƶ����Բ��ϸ�ֶ��ٵ㣨Խ��Խƽ����
    public float _LineWidth = 0.12f;

    private LineRenderer _line;
    private int _bandCount;
    private int _pointCount;

    void Start()
    {
        _line = GetComponent<LineRenderer>();
        _line.loop = true;

        _bandCount = Mathf.Max(1, (int)_FreqBands);
        PointsPerBand = Mathf.Max(1, PointsPerBand);
        _pointCount = _bandCount * PointsPerBand;

        _line.positionCount = _pointCount;
        _line.startWidth = _LineWidth;
        _line.endWidth = _LineWidth;
    }

    void Update()
    {
        if (_FFT == null) return;

        // ��Ƶ�λ�ϸ�������ܱ��Ķ�������ʱ�޸� Inspector�����Զ�����
        int desiredBandCount = Mathf.Max(1, (int)_FreqBands);
        int desiredPointCount = desiredBandCount * Mathf.Max(1, PointsPerBand);
        if (desiredPointCount != _pointCount)
        {
            _bandCount = desiredBandCount;
            _pointCount = desiredPointCount;
            _line.positionCount = _pointCount;
        }

        if (mode == Mode.Expand)
        {
            // ���������������Ƶ��ƽ��ֵ�����ư뾶
            float avg = 0f;
            for (int b = 0; b < _bandCount; b++)
                avg += _FFT.GetBandValue(b, _FreqBands);
            avg /= _bandCount;
            float radius = _Radius + avg * _Strength;

            for (int i = 0; i < _pointCount; i++)
            {
                float angle = i * Mathf.PI * 2f / _pointCount;
                float x = Mathf.Cos(angle) * radius;
                float z = Mathf.Sin(angle) * radius;
                _line.SetPosition(i, new Vector3(x, _WaveBaseY, z));
            }
        }
        else // Mode.Wave
        {
            // Wave ģʽ��ÿ�������� Y �������������ֱ������������Ƶ����ֵ�����Բ�ֵ��ƽ������
            for (int i = 0; i < _pointCount; i++)
            {
                // �Ƕȣ����ϵ�λ�ã�
                float angle = i * Mathf.PI * 2f / _pointCount;

                // ������������Ӧ�ġ�Ƶ��λ�á��������� => ������Ƶ�μ��ֵ
                float bandPos = ((float)i / (float)_pointCount) * _bandCount; // [0, bandCount)
                int b0 = Mathf.Clamp(Mathf.FloorToInt(bandPos), 0, _bandCount - 1);
                int b1 = Mathf.Clamp(b0 + 1, 0, _bandCount - 1);
                float t = bandPos - b0; // ������Ƶ��֮��Ĳ�ֵȨ��

                float v0 = _FFT.GetBandValue(b0, _FreqBands);
                float v1 = _FFT.GetBandValue(b1, _FreqBands);
                float bandValue = Mathf.Lerp(v0, v1, t); // ƽ��ֵ

                // ���� Y = ���� + Ƶ��ֵ * ���
                float y = _WaveBaseY + bandValue * _WaveAmplitude;

                // �뾶���ֺ㶨��ֻ�� Y ����Ĳ�����
                float x = Mathf.Cos(angle) * _Radius;
                float z = Mathf.Sin(angle) * _Radius;

                _line.SetPosition(i, new Vector3(x, y, z));
            }
        }
    }
}
