using UnityEngine;
using System.Runtime.InteropServices;

public class FrequencyBandAnalyser : MonoBehaviour
{
    public enum Bands { Eight = 8, SixtyFour = 64 }
    public Bands _Bands = Bands.Eight;

    [Header("平滑与强度控制")]
    [Range(0.01f, 1f)] public float smoothSpeed = 0.2f;   // 越小越平滑
    [Range(1f, 50f)] public float sensitivity = 10f;      // 放大倍数

    private AudioSource _audioSource;
    private float[] _samples = new float[512];
    private float[] _freqBands;

#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")] private static extern float GetMicrophoneVolume(int index);
#endif

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _freqBands = new float[(int)_Bands];
    }

    void Update()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        // WebGL: 用 JS 插件的实时音量代替 FFT
        float micVol = GetMicrophoneVolume(0);

        for (int i = 0; i < _freqBands.Length; i++)
        {
            float target = micVol * sensitivity * (1 + i * 0.05f);
            _freqBands[i] = Mathf.Lerp(_freqBands[i], target, smoothSpeed);
        }
#else
        // 普通平台: 正常 FFT 分析
        _audioSource.GetSpectrumData(_samples, 0, FFTWindow.Blackman);
        int count = 0;

        for (int i = 0; i < _freqBands.Length; i++)
        {
            float avg = 0f;
            int sampleCount = (int)Mathf.Pow(2, i) * 2;
            for (int j = 0; j < sampleCount; j++)
            {
                avg += _samples[count] * (count + 1);
                count++;
            }
            avg /= count;
            _freqBands[i] = Mathf.Lerp(_freqBands[i], avg * sensitivity, smoothSpeed);
        }
#endif
    }

    public float GetBandValue(int index, Bands bands)
    {
        if (index < 0 || index >= _freqBands.Length) return 0;
        return _freqBands[index];
    }
}
