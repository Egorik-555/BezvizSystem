
var check = document.getElementById("ProfileUserActive");
var date = document.getElementById("activeDate");

if (check.checked) date.hidden = true;
else date.hidden = false;

check.onclick = function () {

    ProfileUserNotActiveToDate.value = "";

    if (date.hidden) {
        date.hidden = false;
    }
    else date.hidden = true;

}