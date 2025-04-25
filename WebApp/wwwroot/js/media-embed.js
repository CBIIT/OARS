function setPlaybackRate(videoId, rate) {
    let vid = document.getElementById(videoId);
    vid.defaultPlaybackRate = rate;
    vid.load();
    vid.play();
}