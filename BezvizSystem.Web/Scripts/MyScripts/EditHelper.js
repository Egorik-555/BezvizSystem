
var inputTable = document.getElementById('inputTable');
var tbody = inputTable.getElementsByTagName('tbody')[0];
var addButton = document.getElementById("addVisitorBut");
var removeButton = document.getElementById("deleteVisitorBut");

addButton.onclick = clickAddHandler;
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
    let result1 = removeElement(count, 'UserInSystem');
    let result2 = removeElement(count, 'DateInSystem');
    if (!result1 || !result2) return;

    for(var i = 1; i <= 5; i++)
        removeTR(fromWhere);
}

function f(whereInsert) {
    var list = document.getElementsByName(whereInsert);

    // last element
    var lastTr = list[list.length - 1];
    // last number of visitors
    var count = lastNumber(lastTr) + 1;

    //create fragment of TR
    let fragmentSurname = createFragmentOfInput(count, 'Surname', 'Фамилия', 'кажите фамилию туриста',true);
    let fragmentName = createFragmentOfInput(count, 'Name', 'Имя','Укажите имя туриста');
    let fragmentPassport = createFragmentOfInput(count, 'SerialAndNumber', 'Серия и номер паспорта','Укажите серию и номер паспорта туриста');
    let fragmentNationality = createFragmentOfInput(count, 'Nationality', 'Гражданство','', false, true);


    let fragment = document.createDocumentFragment();
    fragment.appendChild(fragmentSurname);
    fragment.appendChild(fragmentName);
    fragment.appendChild(fragmentPassport);
    fragment.appendChild(fragmentNationality);
    fragment.appendChild(createFragment(count, 'Gender', 'BithDate', 'Пол', 'Дата рождения', createByClone));

    let hiddenInputUser = createByClone(count, 'UserInSystem');
    let hiddenInputDate = createByClone(count, 'DateInSystem');
    fragment.appendChild(hiddenInputUser);
    fragment.appendChild(hiddenInputDate);

    tbody.insertBefore(fragment, lastTr.nextSibling);
}

function createFragmentOfInput(id, field, caption, msg, separator, select){

    let label = createLabel(id, field,'control-label', caption );
    let input;
    if (select) input = createByClone(id, field);
    else input = createInput(id, field, 'text-box single-line', 'true', msg);

    let td = document.createElement('td');
    td.setAttribute('colspan', '2');

    if (separator) td.appendChild(document.createElement('hr'));

    td.appendChild(label);
    td.appendChild(input);

    let tr = document.createElement('tr');
    tr.setAttribute('name','lastTr');
    tr.appendChild(td);

    return document.createDocumentFragment().appendChild(tr);
}

function createFragment(id, field1, field2, caption1, caption2, createFunction){

    let label1 = createLabel(id, field1,'control-label', caption1);
    let input1 = createFunction(id, field1);
    input1.value = '';
    let label2 = createLabel(id, field2,'control-label', caption2);
    let input2 = createFunction(id, field2);
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

function createInput(id, field, nameClass, dataVal, dataValRequired) {
    let attrId = makeId(id, field);
    let attrName = makeName(id, field);
    var input = document.createElement("input");
    input.setAttribute('data-val', dataVal);
    input.setAttribute('data-val-required', dataValRequired);
    input.setAttribute('id', attrId);
    input.setAttribute('name', attrName);
    input.className = nameClass;
    return input;
}

function createByClone(id, field){
    let prevId = makeId(id - 1, field);
    let prevSelect = document.getElementById(prevId);

    let newSelect = prevSelect.cloneNode(true);
    let newID = makeId(id, field);
    newSelect.setAttribute('id', newID);
    let attrName = makeName(id, field);
    newSelect.setAttribute('name', attrName);

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

function removeTR(name){
    let list = document.getElementsByName(name);
    let lastTr = list[list.length - 1];
    lastTr.parentElement.removeChild(lastTr);
}




