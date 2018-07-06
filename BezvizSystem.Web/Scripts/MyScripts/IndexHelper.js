var table = document.getElementsByTagName('table')[0];

table.onclick = onRowClickHandler;

function onRowClickHandler(event){
    var target = event.target;

    while(target != table){
        if(target.tagName == "TR"){

            var id = target.getAttribute('id');

            location.href=@Url.Action("Index", "Mark")
            return;
        }
        target = target.parentNode;
        alert(target);
    }

    alert(target);

}

