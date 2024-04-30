function timeOutCall(dotnethelper) {
    //document.onmousemove = resetTimeDelay;
    document.onkeydown = resetTimeDelay;
    document.onclick = resetTimeDelay;

    function resetTimeDelay() {
        dotnethelper.invokeMethodAsync("TimerInterval");
    }
}