using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class FFTLineCircleV2 : MonoBehaviour
{
    public FrequencyBandAnalyser _FFT;
    public FrequencyBandAnalyser.Bands _FreqBands = FrequencyBandAnalyser.Bands.Eight;

    public enum Mode { Expand, Wave }
    public Mode mode = Mode.Wave;

    public float _Radius = 5f;          // 基础半径
    public float _Strength = 1f;        // Expand 模式的增幅系数（整体呼吸）
    public float _WaveAmplitude = 1f;   // Wave 模式下的顶点上下振幅（Y 轴）
    public float _WaveBaseY = 0f;       // Wave 模式下的基础 Y（高度偏移）
    public int PointsPerBand = 8;       // 每个频段在圆上细分多少点（越大越平滑）
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

        // 当频段或细分数可能被改动（运行时修改 Inspector），自动修正
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
            // 整体呼吸：用所有频段平均值来控制半径
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
            // Wave 模式：每个顶点在 Y 轴上下起伏（垂直波动），并对频段数值做线性插值以平滑过渡
            for (int i = 0; i < _pointCount; i++)
            {
                // 角度（环上的位置）
                float angle = i * Mathf.PI * 2f / _pointCount;

                // 计算这个顶点对应的“频段位置”，非整数 => 在两个频段间插值
                float bandPos = ((float)i / (float)_pointCount) * _bandCount; // [0, bandCount)
                int b0 = Mathf.Clamp(Mathf.FloorToInt(bandPos), 0, _bandCount - 1);
                int b1 = Mathf.Clamp(b0 + 1, 0, _bandCount - 1);
                float t = bandPos - b0; // 在两个频段之间的插值权重

                float v0 = _FFT.GetBandValue(b0, _FreqBands);
                float v1 = _FFT.GetBandValue(b1, _FreqBands);
                float bandValue = Mathf.Lerp(v0, v1, t); // 平滑值

                // 顶点 Y = 基础 + 频段值 * 振幅
                float y = _WaveBaseY + bandValue * _WaveAmplitude;

                // 半径保持恒定（只做 Y 方向的波动）
                float x = Mathf.Cos(angle) * _Radius;
                float z = Mathf.Sin(angle) * _Radius;

                _line.SetPosition(i, new Vector3(x, y, z));
            }
        }
    }
}
