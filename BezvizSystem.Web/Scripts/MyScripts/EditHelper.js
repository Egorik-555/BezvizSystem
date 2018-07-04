var addButton = document.getElementById('addVisitorBut');

function changeAttributeSub(elem, nameTag, nameAttLabels){
    var labels = elem.getElementsByTagName(nameTag);
    var infoVisitors = document.querySelectorAll('div[name=infoVisitor]');
    var count = infoVisitors.length;

    if (labels.length > 0) {

        var oldVal;
        var newVal;

        for(var i = 0; i < labels.length; i++){
            oldVal = labels[i].getAttribute(nameAttLabels);
            newVal = oldVal.replace(/_\d+__/g, '_'+(count).toString()+'__');
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

        var hr = infoVisitor.nextElementSibling;
        var parent = hr.parentNode;

        parent.insertBefore(newInfoVisitor, hr.nextElementSibling);
        parent.insertBefore(document.createElement('hr'), newInfoVisitor.nextElementSibling)
    }
};


if (addButton){
    addButton.onclick = onClickAdd;
}


//количество дней пребывания
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

    alert(date1 - date2);
}




