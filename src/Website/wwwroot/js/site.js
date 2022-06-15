function onCardHover(id) {
    var progressBar = document.getElementById(id)

    switch (progressBar.style.width) {
        case "100%":
            progressBar.setAttribute("style", "width: 0%")
            break;
        case "0%":
            progressBar.setAttribute("style", "width: 100%")
            break;
        default:
            break;
    }
}

function onCardClick(url) {
    window.location.href = `./home/post?url=${url}`;
}