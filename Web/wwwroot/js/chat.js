"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message, data) {
    //var //msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    //var //encodedMsg = user + " says " + msg;

    var msg = `<h3>` + user + `</h3> <h4>` + message + `</h4>` + `<h3>` + data + `</h3>` + `<hr />`; 

    var div = document.createElement("div");
    div.innerHTML= msg;
    document.getElementById("messagesList").appendChild(div);
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;

    var data = document.getElementById("datainput").value;

    var projectid = document.getElementById("projectid").value;

    connection.invoke("SendMessage", user, message, data, projectid).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
    document.getElementById("messageInput").value = "";
    document.getElementById("messageInput").focus();
});

document.getElementById("messageInput").value = "";
document.getElementById("messageInput").focus();

document.getElementById("messageInput").addEventListener("keyup", function (event) {
    event.preventDefault();
    if (event.keyCode === 13) {
        document.getElementById("sendButton").click();
    }
});