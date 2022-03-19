var studentsDataNoFilter;//TODO write to file on document close
var studentsData
const columnNames = ["Номер студенческого билета", "Фамилия", "Имя", "Отчество", "Дата рождения", "Номер группы", "E-mail"];
const addStudentIds = ["addStudent0", "addStudent1", "addStudent2", "addStudent3", "addStudent4", "addStudent5", "addStudent6"]
const keys = ["stud", "surname", "name", "patronymic", "birth_date", "group", "email"]
var filtered
var badInput

window.onload = function init() {
    $("#add_student_button").on("click", function(){addStudent()})
    $("#set_filter").on("click", function(){setFilter()})
    $("#clear_filter").on("click", function(){clearFilter()})
    $("#save").on("click", function(){save()})
    loadJSON()
};

function loadJSON(){
    $.getJSON("students_data.json", function (data) {
        studentsData = data
        studentsDataNoFilter = data
        filtered = false
        showTable()
    });
}

function showTable() {
    var element = document.getElementById("students");
    if (element != null)
        element.parentNode.removeChild(element);
    var table = document.createElement("table")
    table.id = "students"
    var header = table.insertRow()
    for (i = 0; i < columnNames.length; i++) {
        var cell = header.insertCell()
        const a = i
        cell.onclick = function(){sort(a)}
        cell.classList.add("clickable_cell")
        cell.innerHTML = columnNames[a]
    }
    for (i = 0; i < studentsData.length; i++) {
        var row = table.insertRow()
        for (k = 0; k < keys.length; k++) {
            var cell = row.insertCell()
            cell.innerHTML = studentsData[i][keys[k]]
        }
    }
    var row = table.insertRow()
    for (i = 0; i < columnNames.length; i++) {
        var cell = row.insertCell()
        const a = i
        cell.innerHTML = "<input class=\"add_student_field\" type=\"text\" id=\""+addStudentIds[a]+"\">"
    }
    var students_div = document.getElementById("students_div")
    students_div.appendChild(table)
}

function sort(row){
    switch (row){
        case 0: studentsData.sort((a,b) => a.stud.localeCompare(b.stud))
            break
        case 1: studentsData.sort((a,b) => a.surname.localeCompare(b.surname))
            break
        case 2: studentsData.sort((a,b) => a.name.localeCompare(b.name))
            break
        case 3: studentsData.sort((a,b) => a.patronymic.localeCompare(b.patronymic))
            break
        case 4: studentsData.sort((a,b) => a.birth_date.localeCompare(b.birth_date))
            break
        case 5: studentsData.sort((a,b) => a.group.localeCompare(b.group))
            break
        case 6: studentsData.sort((a,b) => a.email.localeCompare(b.email))
            break
    }
    showTable()
}

function addStudent() {
    badInput = false
    var newStudent = {
        stud: testRegex(0, /\d{2}-\d{2}/, "Номер студенческого билета введен неверно"),
        surname: testRegex(1, /[А-Я|Ё]{1}[а-я|ё]*(-[А-Я|Ё]{1}[а-я|ё]*)*/, "Фамиллия введена неверно"),
        name: testRegex(2, /[А-Я|Ё]{1}[а-я|ё]*/, "Имя введено неверно"),
        patronymic: testRegex(3, /[А-Я|Ё]{1}[а-я|ё]*/, "Отчество введено неверно"),
        birth_date: testRegex(4, /^(3[01]|[12][0-9]|0?[1-9])\.(1[012]|0?[1-9])\.((?:19|20)\d{2})$/, "Дата рождения введена неверно"),
        group: testRegex(5, /\d{2}-\d{3}/, "Номер группы введен неверно"),
        email: testRegex(6, /\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,6}/, "Email введен неверно")
    };
    if (!badInput) {
        studentsData.push(newStudent)
        if (filtered)
            studentsDataNoFilter.push(newStudent);
        showTable()
        if ($("#save").hasClass("nothing_to_save"))
            $("#save").removeClass("nothing_to_save")
    }
}

function setFilter() {
    var value = document.getElementById("filter_dropdown").value
    if (value === "empty")
    {
        clearFilter()
        return
    }
    filtered = true
    studentsData = studentsDataNoFilter.filter(x => x[value].startsWith(document.getElementById("filter").value))
    showTable()
}

function clearFilter() {
    studentsData = studentsDataNoFilter
    filtered = false
    showTable()
}

function testRegex(index, regex, alertText) {
    if (badInput)
        return
    var string = document.getElementById(addStudentIds[index]).value
    if (string === "") {
        myAlert("Заполнены не все поля данных студента")
    }
    else if (!regex.test(string)) {
        myAlert(alertText)
    }
    else
        return string
}

function myAlert(alertText){
    badInput = true
    alert(alertText)
}

function save() {
    contents = JSON.stringify(studentsData)
    $.ajax({
        type: 'POST',
        url: 'http://localhost:8080/save_table',
        data: contents,
        dataType: 'application/json'
    })
    $("#save").addClass("nothing_to_save")
}