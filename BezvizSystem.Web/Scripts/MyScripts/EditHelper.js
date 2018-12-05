
var inputTable = document.getElementById('inputTable');
if (inputTable)
    var tbody = inputTable.getElementsByTagName('tbody')[0];

var addButton = document.getElementById("addVisitorBut");
var removeButton = document.getElementById("deleteVisitorBut");

if (addButton)
    addButton.onclick = clickAddHandler;
if(removeButton)
    removeButton.onclick = clickRemoveHandler;

function clickAddHandler() {
    f('lastTr');
}

function clickRemoveHandler() {
    removeFunction('lastTr');
}

function removeFunction(fromWhere) {
    let list = document.getElementsByName(fromWhere);
    let lastTr = list[list.length - 1];
    let count = lastNumber(lastTr);
    if (count == 0) return;

    removeTR(count, 'Surname');
    removeTR(count, 'Name');
    removeTR(count, 'SerialAndNumber');
    removeTR(count, 'Nationality');
    removeTR(count, 'Gender');
}

function f(whereInsert) {
    var list = document.getElementsByName(whereInsert);
    if (list.length == 0) return;
    // last element
    var lastTr = list[list.length - 1];
    // last number of visitors
    var count = lastNumber(lastTr) + 1;

    //create fragment of TR
    let fragmentSurname = createFragmentOfInput(count, 'Surname', 'Фамилия', 'Укажите фамилию туриста', "text-transform: uppercase");
    let fragmentName = createFragmentOfInput(count, 'Name', 'Имя', 'Укажите имя туриста', "text-transform: uppercase");
    let fragmentPassport = createFragmentOfInput(count, 'SerialAndNumber', 'Серия и номер паспорта','Укажите серию и номер паспорта туриста');
    let fragmentNationality = createFragmentOfInput(count, 'Nationality', 'Гражданство', '', true, "text-transform: uppercase");
//let fragmnetSpan = createFragmentOfSpan();


    let fragment = document.createDocumentFragment();
    fragment.appendChild(fragmentSurname);
    fragment.appendChild(fragmentName);
    fragment.appendChild(fragmentPassport);
    fragment.appendChild(fragmentNationality);    
    fragment.appendChild(createFragment(count, 'Gender', 'BithDate', 'Пол', 'Дата рождения', createByClone));

    tbody.insertBefore(fragment, lastTr.nextSibling);

    var id = makeId(count, 'Nationality');
    $(function () {
        $("#"+id).combobox();
        $("#toggle").click(function () {
            $("#" + id).toggle();
        });
    });

    $(".datepicker").datepicker({
        monthNames: ['Январь', 'Февраль', 'Март', 'Апрель', 'Май', 'Июнь', 'Июль', 'Август', 'Сентябрь', 'Октябрь', 'Ноябрь', 'Декабрь'],
        dayNamesMin: ['Пн', 'Вт', 'Ср', 'Чт', 'Пт', 'Сб', 'Вс'], dateFormat: 'dd.mm.yy'
    });
}

function createFragmentOfSpan() {
    let span = document.getElementsByClassName("custom-combobox")[0];
    return span.cloneNode(true);
}

function createFragmentOfInput(id, field, caption, msg, select, style){

    let label = createLabel(id, field,'control-label', caption);
    let input;
    if (select) {
        input = createByClone(id, field, 'form-control');
        input.value = '';
    }
    else input = createInput(id, field, 'text-box single-line', 'true', msg, style);

    let td = document.createElement('td');
    td.setAttribute('colspan', '2');

    if (field == 'Surname'){
        let idInput = createInput(id, 'Id', '', 'true', 'Требуется поле Id.', style);
        idInput.setAttribute('type', 'hidden');
        idInput.value = '0';
        td.appendChild(idInput);
    }
    td.appendChild(label);

    td.appendChild(input);

    //if (field == "Nationality") {
        
    //    input.className += " nationality";
    //    var span = createFragmentOfSpan();
    //    td.appendChild(span);
    //}    

    let tr = document.createElement('tr');
    tr.setAttribute('name','lastTr');
    tr.appendChild(td);

    return document.createDocumentFragment().appendChild(tr);
}

function createFragment(id, field1, field2, caption1, caption2, createFunction){

    let label1 = createLabel(id, field1,'control-label', caption1);
    let input1 = createFunction(id, field1, 'form-control');
    input1.value = '';
    let label2 = createLabel(id, field2,'control-label', caption2);
    let input2 = createFunction(id, field2, 'form-control datepicker');
    input2.value = '';

    let tr = document.createElement('tr');
    tr.setAttribute('name','lastTr');

    let td = document.createElement('td');
    td.className = 'left';
    td.appendChild(label1);
    td.appendChild(input1);
    tr.appendChild(td);

    td = document.createElement('td');
    td.className = 'left';
    td.setAttribute('align','right');
    td.appendChild(label2);
    td.appendChild(input2);
    tr.appendChild(td);

    return document.createDocumentFragment().appendChild(tr);
}


function createLabel(id, field, nameClass, caption) {
    let attrId = makeId(id, field);
    let label = document.createElement("label");
    label.setAttribute('for', attrId);
    label.className = nameClass;
    label.innerHTML = caption;
    return label;
}

function createInput(id, field, nameClass, dataVal, dataValRequired, style) {
    let attrId = makeId(id, field);
    let attrName = makeName(id, field);
    var input = document.createElement("input");
    input.setAttribute('data-val', dataVal);
    input.setAttribute('data-val-required', dataValRequired);
    input.setAttribute('id', attrId);
    input.setAttribute('name', attrName);
    input.setAttribute('style', style);
    input.className = nameClass;
    return input;
}

function createByClone(id, field, nameClass){
    let prevId = makeId(id - 1, field);
    let prevSelect = document.getElementById(prevId);

    if(!prevSelect) return null;

    let newSelect = prevSelect.cloneNode(true);
    let newID = makeId(id, field);
    newSelect.setAttribute('id', newID);
    let attrName = makeName(id, field);
    newSelect.setAttribute('name', attrName);
    newSelect.setAttribute('class', nameClass);
   
    return newSelect;
}

function makeId(id, field){
    return 'Infoes_' + id + '__' + field;
}

function makeName(id, field) {
    return 'Infoes[' + id + '].' + field;
}

function lastNumber(lastTR) {
    let label = lastTR.getElementsByTagName('label')[0];
    let attrForValue = label.getAttribute('for');
    let array = attrForValue.split('_');
    return +array[1];
}

function removeElement(id, field){
    let idCaption = makeId(id, field);

    let element = document.getElementById(idCaption);
    if (!element) return false;
    element.parentElement.removeChild(element);
    return true;
}

function removeTR(id, field){
    let idName = makeId(id, field);
    let element = document.getElementById(idName);
    if (!element) return;
    let tr = element.parentElement.parentElement;
    tr.parentElement.removeChild(tr);
}

function dateToString() {
    let date = new Date();
    let formatterDate = new Intl.DateTimeFormat('ru', {
        day: 'numeric',
        month: 'numeric',
        year: 'numeric'
    });
    let formatterTime = new Intl.DateTimeFormat('ru', {
        hour: 'numeric',
        minute: 'numeric',
        second: 'numeric'
    });

    return formatterDate.format(date) + ' ' + formatterTime.format(date);
}

//количество дней пребывания
var dateArrival = document.getElementById('DateArrival');
var dateDeparture = document.getElementById('DateDeparture');
var daysOfStay = document.getElementById('DaysOfStay');

dateArrival.onchange = changeHandler;// addEventListener("change", changeHandler);
dateDeparture.onchange = changeHandler;// addEventListener("change", changeHandler);

function changeHandler() {

    var date1 = getDate(dateArrival.value);
    var date2 = getDate(dateDeparture.value);
    var divDate = date2 - date1;
    if(isNaN(divDate)) {
        daysOfStay.value = '';
        return;
    }
    var daysLag = Math.ceil(divDate / (1000 * 3600 * 24));
    if (daysLag >= 0) daysLag += 1;
    daysOfStay.value = daysLag;
}

function getDate(date){
    var split = date.split('.');
    return new Date(split[2] + '-' + split[1] + '-' + split[0]);
}

