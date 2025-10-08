using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FFTLineRenderer_v1 : MonoBehaviour
{
    public FrequencyBandAnalyser _FFT;//创建槽：引用提取出频率的音频
    public FrequencyBandAnalyser.Bands _FreqBands;//创建槽：选择一个获取频率平均值的方式（8/64）
    LineRenderer _Line;
    float _Spacing = .2f;
    public float _LineLength = 8;
    public float _Strength = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _Line = GetComponent<LineRenderer>();
            if(_FreqBands == FrequencyBandAnalyser.Bands.Eight)
        {
            _Line.positionCount = 8;

        }
        else
        {
            _Line.positionCount = 64;
        }
        _Spacing = _LineLength / _Line.positionCount;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < _Line.positionCount; i++)
        {
            float xPos = i * _Spacing;
            float yPos = _FFT.GetBandValue(i, _FreqBands)*_Strength;
            Vector3 Pos = new Vector3 (xPos, yPos,0);
            _Line.SetPosition  (i,Pos);
        }
    }
}
