var webSocket = null
var name = null

window.onload = function (){
    document.getElementById("enter_chat").onclick = function() {enterChatOnClick()}
    document.getElementById("send").onclick = function() {sendMessage(name + ": " + document.getElementById("message").value)}
}

function enterChatOnClick(){
    if (!("WebSocket" in window))
    {
        alert("WebSocket не поддерживается вашим браузером!");
    }
    if (webSocket === null) {
        let username = document.getElementById("username")
        webSocket = new WebSocket("ws://localhost:8080")
        webSocket.onopen = function () {
            name = username.value
            sendMessage(name + " entered the chat")
            username.disabled = true
            document.getElementById("enter_chat").textContent = 'Exit Chat'
            document.getElementById("input_message_form").style.display = 'block'
            webSocket.onmessage = function (evt){getMessage(evt.data)}
        }
    }
    else {
        let username = document.getElementById("username")
        sendMessage(username.value + " exited the chat")
        webSocket = null
        username.disabled = false
        name = null
        document.getElementById("enter_chat").textContent = 'Enter Chat'
        document.getElementById("input_message_form").style.display = 'none'
    }
}

function sendMessage(s){
    webSocket.send(s)
    //addLogItem(s)
}

function addLogItem(s){
    let p = document.createElement('p')
    p.textContent = s
    document.getElementById("log").appendChild(p)
}

function getMessage(s){
    addLogItem(s)
}