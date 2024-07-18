< !DOCTYPE html >
< html >
< head >
    < title > Unity WebGL Speech Recognition</title>
    <script>
        var unityInstance;
let recognition;

function startRecognition()
{
    if ('webkitSpeechRecognition' in window) {
    recognition = new webkitSpeechRecognition();
} else if ('SpeechRecognition' in window) {
    recognition = new SpeechRecognition();
} else
{
    alert('Your browser does not support the Web Speech API');
    return;
}

recognition.continuous = true;
recognition.interimResults = false;

recognition.onresult = function(event) {
    let transcript = event.results[event.resultIndex][0].transcript.trim();
    unityInstance.SendMessage('SpeechManager', 'OnSpeechResult', transcript);
};

recognition.start();
        }

        function stopRecognition()
{
    if (recognition)
    {
        recognition.stop();
    }
}

function onUnityLoad()
{
    startRecognition();
}
    </ script >
</ head >
< body onload = "onUnityLoad()" >
    < div id = "unity-container" class= "unity-desktop" >
        < canvas id = "unity-canvas" ></ canvas >
        < div id = "unity-loading-bar" >
            < div id = "unity-logo" ></ div >
            < div id = "unity-progress-bar-empty" >
                < div id = "unity-progress-bar-full" ></ div >
            </ div >
        </ div >
    </ div >
    < script src = "Build/UnityLoader.js" ></ script >
    < script >
        unityInstance = UnityLoader.instantiate("unity-container", "Build/Build.json", { onProgress: UnityProgress});
    </ script >
</ body >
</ html >
