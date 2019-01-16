$(function () {

    addNumber();

});

function addNumber() {

    $('label[for$=Surname]').before(function (index, html) {
        var ind = index + 1;
        var number = $('label[class=number]', $(this).parent());

        if (number.length === 0)
        {
            return $('<label class="number"> ' + ind + ' </label>');
        }
        else return;
    });

}