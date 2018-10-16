
function showPanel(button, panel) {

    var button = document.getElementById(button);
    var panel = document.getElementById(panel);

    button.onclick = function () {
        if (panel.style.display != 'none')
            panel.style.display = 'none';
        else panel.style.display = '';
    }

}