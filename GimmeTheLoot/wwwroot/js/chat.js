"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message, date) {

    var msg = `<h3>` + user + " commented at " + date + `</h3> <h4>` + message + `</h4>` + `<hr />`;

    //msg = `<div> <b>` + user + `</b>` + "comemnted at" + date + `</div> <div> <a>` + message + `</a> </div>`;  

    var div = document.createElement("div");
    div.innerHTML = msg;
    document.getElementById("messagesList").appendChild(div);
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("user").value;
    var message = document.getElementById("NewCommentary").value;

    var projid = document.getElementById("id").value;


    connection.invoke("SendMessage", user, message, projid).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
    document.getElementById("NewCommentary").value = "";
    document.getElementById("NewCommentary").focus();
});

document.getElementById("NewCommentary").value = "";
document.getElementById("NewCommentary").focus();

document.getElementById("NewCommentary").addEventListener("keyup", function (event) {
    event.preventDefault();
    if (event.keyCode === 13) {
        document.getElementById("sendButton").click();
    }
});