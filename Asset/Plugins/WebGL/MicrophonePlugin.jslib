// -------------------------------------------------------------
// WebGL Microphone Plugin (Unity 6 / 2022+ compatible version)
// Author: ChatGPT (fixed compatibility for new WebGL runtime)
// Place in: Assets/Plugins/WebGL/WebGLMicrophone.jslib
// -------------------------------------------------------------

// --- Compatibility patch for modern Unity builds ---
if (typeof writeStringToMemory === 'undefined') {
    var writeStringToMemory = function (str, buffer) {
        var length = lengthBytesUTF8(str) + 1;
        stringToUTF8(str, buffer, length);
    };
}
if (typeof _malloc === 'undefined' && typeof Module !== 'undefined') {
    var _malloc = Module._malloc;
}
if (typeof lengthBytesUTF8 === 'undefined' && typeof Module !== 'undefined') {
    var lengthBytesUTF8 = Module.lengthBytesUTF8;
}

// --- Plugin body ---
var MicrophonePlugin = {

    buffer: undefined,

    Init: function () {
        console.log("WebGLMicrophone.Init()");
        document.volume = 0;
        document.position = 0;

        var length = 1024;
        this.buffer = new ArrayBuffer(4 * length);
        document.dataArray = new Float32Array(this.buffer, 0, length);

        navigator.getUserMedia = navigator.getUserMedia ||
                                 navigator.webkitGetUserMedia ||
                                 navigator.mozGetUserMedia;

        if (navigator.getUserMedia) {
            var constraints = { audio: true };
            navigator.getUserMedia(constraints,
                function (stream) {
                    console.log('Microphone stream success', stream);
                    document.audioContext = new (window.AudioContext || window.webkitAudioContext)();
                    document.analyser = document.audioContext.createAnalyser();
                    document.analyser.minDecibels = -90;
                    document.analyser.maxDecibels = -10;
                    document.analyser.smoothingTimeConstant = 0.85;

                    document.source = document.audioContext.createMediaStreamSource(stream);
                    document.source.connect(document.analyser);

                    document.tempSize = 1024;
                    document.tempArray = new Float32Array(document.tempSize);

                    document.readDataOnInterval = function () {
                        if (!document.analyser) return;
                        document.analyser.getFloatTimeDomainData(document.tempArray);
                        document.volume = 0;
                        for (var i = 0; i < document.tempArray.length; i++) {
                            document.volume = Math.max(document.volume, Math.abs(document.tempArray[i]));
                        }
                        setTimeout(document.readDataOnInterval, 50);
                    };
                    document.readDataOnInterval();
                },
                function (error) {
                    console.error('getUserMedia error', error);
                }
            );
        } else {
            console.error("getUserMedia not supported in this browser");
        }
    },

    QueryAudioInput: function () {
        console.log("QueryAudioInput()");
        document.mMicrophones = [];
        if (!navigator.mediaDevices || !navigator.mediaDevices.enumerateDevices) {
            console.log("enumerateDevices() not supported");
            return;
        }
        navigator.mediaDevices.enumerateDevices()
            .then(function (devices) {
                devices.forEach(function (device) {
                    if (device.kind === 'audioinput') {
                        document.mMicrophones.push(device.label || "Unknown Mic");
                    }
                });
            })
            .catch(function (err) {
                console.error(err.name + ": " + err.message);
            });
    },

    GetNumberOfMicrophones: function () {
        var mics = document.mMicrophones;
        return (mics && mics.length) ? mics.length : 0;
    },

    GetMicrophoneDeviceName: function (index) {
        var mics = document.mMicrophones;
        var name = (mics && mics[index]) ? mics[index] : "Microphone";
        var buffer = _malloc(lengthBytesUTF8(name) + 1);
        stringToUTF8(name, buffer, lengthBytesUTF8(name) + 1);
        return buffer;
    },

    GetMicrophoneVolume: function (index) {
        return (document.volume !== undefined) ? document.volume : 0;
    }
};

mergeInto(LibraryManager.library, MicrophonePlugin);
