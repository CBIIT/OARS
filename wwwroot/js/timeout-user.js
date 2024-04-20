function timeOutCall(dotnethelper) {
    //document.onmousemove = resetTimeDelay;
    document.onkeypress = resetTimeDelay;
    document.onclick = resetTimeDelay;

    function resetTimeDelay() {
        dotnethelper.invokeMethodAsync("TimerInterval");
    }
}