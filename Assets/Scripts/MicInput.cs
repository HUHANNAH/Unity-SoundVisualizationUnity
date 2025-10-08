using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MicInput : MonoBehaviour
{
    private AudioSource _audioSource;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();

#if UNITY_WEBGL && !UNITY_EDITOR
        // --- WebGL �£��ò���� Microphone API ---
        if (Microphone.devices.Length > 0)
        {
            string micName = Microphone.devices[0];
            Debug.Log("Using WebGL mic: " + micName);
        }
        else
        {
            Debug.LogError("No microphone detected (WebGL).");
        }
#else
        // --- �� Editor �� PC �£��� Unity �Դ��� Microphone ---
        if (UnityEngine.Microphone.devices.Length > 0)
        {
            string micName = UnityEngine.Microphone.devices[0];
            Debug.Log("Using mic: " + micName);

            _audioSource.clip = UnityEngine.Microphone.Start(micName, true, 10, 44100);
            _audioSource.loop = true;

            // �ȴ�¼����ʼ
            while (!(UnityEngine.Microphone.GetPosition(micName) > 0)) { }

            _audioSource.Play();
        }
        else
        {
            Debug.LogError("No microphone detected (Editor/Standalone).");
        }
#endif
    }
}
