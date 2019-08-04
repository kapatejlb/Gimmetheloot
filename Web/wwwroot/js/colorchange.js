var a = true;
function myFunction() {
    var body = document.body;
    if (a) {
        body.style.backgroundColor = "#d06b70";
        //body.style.color = 'white';
        a = false;
    }
    else {
        body.style.backgroundColor = "white";
        //body.style.color = 'black';
        a = true;
    }
}