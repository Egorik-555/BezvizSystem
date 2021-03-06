var addButton = document.getElementById('addVisitorBut');
var deleteButton = document.getElementById('deleteVisitorBut');

function changeAttributeSub(elem, nameTag, nameAttLabels){
    var labels = elem.getElementsByTagName(nameTag);
    var infoVisitors = document.querySelectorAll('div[name=infoVisitor]');
    var count = infoVisitors.length;

    if (labels.length > 0) {

        var oldVal;
        var newVal;

        for(var i = 0; i < labels.length; i++){
            oldVal = labels[i].getAttribute(nameAttLabels);
            newVal = oldVal.replace(/_\d+__/g, '_' + (count).toString() + '__');
            labels[i].setAttribute(nameAttLabels, newVal);
        }
    }
}

function changeAttributeBracket(elem, nameTag, nameAttLabels){
    var labels = elem.getElementsByTagName(nameTag);
    var infoVisitors = document.querySelectorAll('div[name=infoVisitor]');
    var count = infoVisitors.length;

    if (labels.length > 0) {

        var oldVal;
        var newVal;

        for(var i = 0; i < labels.length; i++){
            oldVal = labels[i].getAttribute(nameAttLabels);
            newVal = oldVal.replace(/\[\d+\]/g, '['+(count).toString()+']');
            labels[i].setAttribute(nameAttLabels, newVal);
        }
    }
}

function clearField(nameElem) {
    var inputs = nameElem.getElementsByTagName('input');
    for (var i = 0; i < inputs.length; i++) {
        inputs[i].value = '';
    }

    var selects = nameElem.getElementsByTagName('select');
    for (var i = 0; i < selects.length; i++) {
        selects[i].value = '';
    }
}

function onClickAdd(){
    var infoVisitors = document.querySelectorAll('div[name=infoVisitor]');
    var count = infoVisitors.length;
    var infoVisitor = infoVisitors.item(count - 1);

    if (infoVisitor) {
        var newInfoVisitor = infoVisitor.cloneNode(true);
        changeAttributeSub(newInfoVisitor, 'label', 'for');
        changeAttributeSub(newInfoVisitor, 'input', 'id');
        changeAttributeSub(newInfoVisitor, 'select', 'id');
        changeAttributeBracket(newInfoVisitor, 'input', 'name');
        changeAttributeBracket(newInfoVisitor, 'span', 'data-valmsg-for');
        changeAttributeBracket(newInfoVisitor, 'select', 'name');

        clearField(newInfoVisitor);

        var parent = infoVisitor.parentNode;
        parent.insertBefore(newInfoVisitor, infoVisitor.nextElementSibling);

        var newId = document.getElementById("Infoes_" + count + "__Id");
        newId.value = "0";     
        var newStatusOfOperation = document.getElementById("Infoes_" + count + "__StatusOfOperation");
        newStatusOfOperation.value = "1";   
    }
}


function onClickDelete() {
    var infoVisitors = document.querySelectorAll('div[name=infoVisitor]');
    //var hrs = document.querySelectorAll('div[name=infoVisitor] + hr');
    var count = infoVisitors.length;

    if (count > 1){
        var parent = infoVisitors.item(count - 1).parentNode;
        var delElem = infoVisitors.item(count - 1);
        //var delHr = hrs.item(count - 1);

        parent.removeChild(delElem);
        //parent.removeChild(delHr)
    }
}


if (addButton){
    addButton.onclick = onClickAdd;
}
if (deleteButton){
    deleteButton.onclick = onClickDelete;
}


//���������� ���� ����������
var form = document.forms[0];
var dateArrival = form.DateArrival;
var dateDeparture = form.DateDeparture;

dateArrival.addEventListener("change", changeHandler);
dateDeparture.addEventListener("change", changeHandler);

function changeHandler() {

    var form = document.forms[0];
    var dateArrival = form.DateArrival;
    var dateDeparture = form.DateDeparture;

    var date1 = new Date(dateArrival.value);
    var date2 = new Date(dateDeparture.value);
    var divDate = date2 - date1;
    var daysLag = Math.ceil(Math.abs(divDate) / (1000 * 3600 * 24)) + 1;
    form.DaysOfStay.value = daysLag;
}




