//document.addEventListener(onload(init()));
//document.onload(init())
window.onload = function init() {
    for (i = 0; i <= 10; i++) {
        b = $("#" + i)
        const a = i
        b.on("click", function(){typeDigit(a)})
        b.text(i)
    }
    $("#backspace").on("click", function(){backspace()})
    $("#clear").on("click", function(){clear()})
    $("#mplus").on("click", function(){mplus()})
    $("#mr").on("click", function(){mr()})
    $("#mc").on("click", function(){mc()})
    $("#plus").on("click", function(){operationEntered("+")})
    $("#minus").on("click", function(){operationEntered("-")})
    $("#multiply").on("click", function(){operationEntered("*")})
    $("#divide").on("click", function(){operationEntered("/")})
    $("#power").on("click", function(){operationEntered("^")})
    $("#evaluate").on("click", function(){evaluateClick()})
    $("#negative").on("click", function(){negative()})
    $("#dot").on("click", function(){dot()})
    $("#sqrt").on("click", function(){sqrt()})
    //alert("loaded")
}

function typeDigit(x) {
    p = $("#number")
    str = p.text()
    if (str === "0")
        p.text(x)
    else if (str === "-0")
        p.text("-" + x)
    else
        p.text(str + x)
}

function backspace(){
    p = $("#number")
    str = p.text()
    if (str.length === 1)
        p.text(0)
    else if (str.length === 2 && str.charAt(0) === "-")
        p.text("-0")
    else
        p.text(str.slice(0, -1))
}

function clear(){
    number = $("#number");
    operation = $("#operation")
    if (number.text() == 0 && operation.text().length > 0){
        old = $("#previous_number")
        number.text(old.text())
        operation.text("")
        old.text(0)
    }
    else
        number.text(0);
}

function mplus(){
    $("#memory").text(parseInt($("#number").text()))
}

function mr(){
    $("#number").text($("#memory").text())
}

function mc(){
    $("#memory").text(0)
}

function operationEntered(operation){
    oper = $("#operation")
    if (oper.text().length != 0) {
        $("#previous_number").text(evaluate())
        oper.text(operation)
        p.text(0)
        return
    }
    p = $("#number")
    $("#previous_number").text(p.text())
    oper.text(operation)
    p.text(0)
}

function evaluate(){
    oper = $("#operation")
    if (oper.text().length == 0)
        return
    var a = parseInt($("#previous_number").text())
    var b = parseInt($("#number").text())
    switch (oper.text()){
        case "+": return a + b
        case "-": return a - b;
        case "*": return a * b;
        case "/": return a / b;
        case "^": return Math.pow(a, b)
        default: return NaN;//TODO proper exception
    }
}

function evaluateClick() {
    $("#number").text(evaluate())
    $("#operation").text("")
    $("#previous_number").text(0)
}
//TODO: fractions, negative numbers
function negative() {
    p = $("#number")
    text = p.text()
    if (text.charAt(0) == "-")
        p.text(text.slice(1))
    else
        p.text("-" + text)
}

function dot() {
    p = $("#number")
    text = p.text()
    if (!text.includes("."))
        p.text(text + ".")
}

function sqrt() {
    p = $("#number")
    p.text(Math.sqrt(p.text()))
}