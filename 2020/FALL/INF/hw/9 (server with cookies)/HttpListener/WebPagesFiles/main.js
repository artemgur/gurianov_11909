window.onload = function init() {
    let cookieValue = getCookieValue("colorCookie")
    document.getElementById("color_picker").value = cookieValue
    document.body.style.background = cookieValue
    document.getElementById("save_settings").onclick = function () {saveSettings()}
    document.getElementById("login").onclick = function () {logIn()}
    GetVisitorsNumber()
}

async function logIn(){
    let password = document.getElementById("password").value
    $.ajax({
        type: 'POST',
        url: 'http://localhost:8080/login',
        data: password,
        dataType: 'text/plain',
        success: function () { //Doesn't work properly
                alert("Log In Successful")
            },
        error:function () { //Doesn't work properly
                alert("Incorrect password")
            }
    })

}

async function GetVisitorsNumber() {
    let response = await fetch("http://localhost:8080/how_many_visitors")
    if (response.ok){
        contents = response.text().then(function (text){
            document.getElementById("visitorsNumber").innerHTML = text
        })
    }
}

function saveSettings() {
    let color = document.getElementById("color_picker").value
    //alert(color)
    document.body.style.background = color
    document.cookie = "colorCookie=" + color// + ";expires=" + "Fri, 25 Dec 2037 23:59:00 GMT"
    //document.path = 
}

function getCookieValue(a) {
    var b = document.cookie.match('(^|;)\\s*' + a + '\\s*=\\s*([^;]+)');
    return b ? b.pop() : '';
}

