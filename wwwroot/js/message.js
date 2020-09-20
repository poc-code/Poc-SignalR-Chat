"use strict";

class UserMessage {
    constructor(name, message) {
        // invokes the setter
        this.name = name;
        this.message = message;
    }
    get name() {
        return this._name;
    }

    get message() {
        return this._message;
    }

    set name(value) {
        if (value.length < 4) {
            alert("Name is too short.");
            return;
        }
        this._name = value;
    }

    set message(value) {
        this._message = value;
    }
}

class MessageBlock {

    constructor(_usermessage) {
        this.userMessage = _usermessage;
        this.blocksent = _usermessage;
        this.blockreceive = _usermessage;

    }

    get userMessage() {
        return this._userMessage;
    }

    set userMessage(value) {
        this._userMessage = value;
    }

    get blocksent() {
        return this._blocksent;
    }

    set blocksent(value) {
        this._blocksent = "<div class=\"row msg_container base_sent\">";
        this._blocksent += "    <div class=\"col-md-10 col-xs-10\">";
        this._blocksent += "        <div class=\"messages msg_sent\">";
        this._blocksent += "            <p>" + value.message;
        this._blocksent += "            </p>";
        this._blocksent += "            <time datetime=\"" + Date.now() + "\">" + value.name +" - " + Date.now() + "</time>";
        this._blocksent += "        </div>";
        this._blocksent += "    </div>";
        this._blocksent += "    <div class=\"col-md-2 col-xs-2 avatar\">";
        this._blocksent += "        <img src=\"\\images\\avatar.png\" class=\" img-responsive \">";
        this._blocksent += "    </div>"
        this._blocksent += "</div>";
    }

    get blockreceive() {
        return this._blockreceive;
    }

    set blockreceive(value) {
        this._blockreceive = "<div class=\"row msg_container base_receive\">";
        this._blockreceive += "    <div class=\"col-md-2 col-xs-2 avatar\">";
        this._blockreceive += "        <img src=\"\\images\\avatar_anonimo.png\" class=\" img-responsive \">";
        this._blockreceive += "    </div>"
        this._blockreceive += "    <div class=\"col-md-10 col-xs-10\">";
        this._blockreceive += "        <div class=\"messages msg_receive\">";
        this._blockreceive += "            <p>" + value.message;
        this._blockreceive += "            </p>";
        this._blockreceive += "            <time datetime=\"" + Date.now() + "\">" + value.name + " - " + Date.now() + "</time>";
        this._blockreceive += "        </div>";
        this._blockreceive += "    </div>";
        this._blockreceive += "</div>";
    }
}
function parseJwt(token) {
    if (token === null) {
        myLoginFunction();
    } else {
        var base64Url = token.split('.')[1];
        var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
        var jsonPayload = decodeURIComponent(atob(base64).split('').map(function (c) {
            return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
        }).join(''));
        return JSON.parse(jsonPayload);
    }
    return false;
};


function myLoginFunction() {
    var person;
    var group;
    if (sessionStorage.getItem('credentials') === null) {
        person = prompt("Please enter your name:", "Your Name");
        var password = prompt("Please enter your password:", "Passaword")
        if (person === null || person === "" || password === "" || password === null) {
            alert("login inválido");
            setTimeout(3000);
            location.reload();
        } else {
            var settings = {
                "url": "api/v1/auth",
                "method": "POST",
                "timeout": 0,
                "headers": {
                    "Content-Type": "application/json"
                },
                "data": JSON.stringify({ "username": person, "password": password }),
            };

            $.ajax(settings).done(function (response) {
                sessionStorage.setItem("credentials", response['token']);
            });
        }
    }

    var payload = parseJwt(sessionStorage.getItem('credentials'));
    person = payload['unique_name'];
    group = payload['group'];

    debugger;
    document.getElementById("name").innerHTML = person;
    document.getElementById("group").innerHTML = group;
}

var connection = new signalR.HubConnectionBuilder()
    .withUrl("http://localhost:3420/chatservicehub")
    .configureLogging(signalR.LogLevel.Information)
    .build();
$("#send").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    var msg = message.replace(/&/g, "&").replace(/</g, "<").replace(/>/g, ">");
    let messageblock = new MessageBlock(new UserMessage(user, msg));
    if(sessionStorage.getItem("UserLogged") != user)
        $(".msg_container_base").append(messageblock.blockreceive);
    else
        $(".msg_container_base").append(messageblock.blocksent);

    $(".panel-body").scrollTop(window.innerHeight);
});

async function start() {
    connection.start().then(function () {
        $("#send").disabled = false;
    }).catch(function (err) {
        connection.reload();
        return console.error(err.toString());
    });
}

connection.onclose(async () => {
    await start();
});

// Start the connection.
start();

$("#send").on("click", function (event) {
    let user = $("#name").text();
    let message = $("#mensagem").val();
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

$(document).ready(function () {
    myLoginFunction();
    
    // Get the input field
    var input = document.getElementById("mensagem");

    // Execute a function when the user releases a key on the keyboard
    input.addEventListener("keyup", function (event) {
        // Number 13 is the "Enter" key on the keyboard
        if (event.keyCode === 13) {
            // Cancel the default action, if needed
            event.preventDefault();
            // Trigger the button element with a click
            document.getElementById("send").click();
            // clear field
            input.value = '';
        }
    });
    return true;
});

