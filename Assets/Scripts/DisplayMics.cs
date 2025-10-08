using UnityEngine;

namespace UnityWebGLMicrophone
{
    public class DisplayMics : MonoBehaviour
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        void Awake()
        {
            Microphone.Init();
            Microphone.QueryAudioInput();
        }

        void Update()
        {
            Microphone.Update();
        }
#endif
    }
}
