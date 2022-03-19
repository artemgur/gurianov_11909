const filenames = ["biography.html", "photos.html", "contacts.html", "cv.html"]
var previouslyClicked = -1

window.onload = function init(){
    for (i = 0; i <= 3; i++){
        const a = i
        document.getElementById(i).onclick = function (){openPage(a)}
    }
}

async function openPage(pageNumber) {
    response = await fetch(filenames[pageNumber])
    if (response.ok){
        contents = response.text().then(function (text){
            document.getElementById("content").innerHTML = text
        })
    }
    document.getElementById(pageNumber).className = "selected"
    if (previouslyClicked > -1)
        document.getElementById(previouslyClicked).className = "not_selected"
    previouslyClicked = pageNumber
}