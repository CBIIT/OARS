function callDotNetTimeout() {
    DotNet.invokeMethodAsync("TheradexPortal", "Timeout")
        .then(data => {
            // once async call is done, this is to display the result returned by .NET call
            console.log("Invoke call :" + data);
        });
}

var initial;
var timeoutGlobal;

function initializeInactivityTimer() {
    //console.log("Timeout:" + timeoutMS);
    initial = window.setTimeout(
        function () {
            //alert("Timed Out: " + timeoutMS);
            callDotNetTimeout();
        }, timeoutMS);
}

document.body.onclick = function () {
    //console.log("Reset Timer-Click")
    clearTimeout(initial)
    initializeInactivityTimer();
}

document.body.onkeypress = function () {
    //console.log("Reset Timer-Key")
    clearTimeout(initial)
    initializeInactivityTimer();
}