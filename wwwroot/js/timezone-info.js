function getTimezoneOffset() {
    return new Date().getTimezoneOffset();
}

function getTimeZone() {
    return new Date().toLocaleTimeString('en-us', { timeZoneName: 'short' });
}