"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
const toast = document.querySelector("#toast");
const messageField = document.querySelector("#messageField");
const onlineStatusBox = document.querySelector("#OnlineStatusBox");
const loader = document.querySelector("#loader");
const button_field = document.querySelectorAll(".button-field");
const messagesList = document.querySelector("#messagesList");
const sendButton = document.querySelector("#sendButton");
const messageInput = document.querySelector("#messageInput");
const yaziyor = document.querySelector("#yaziyor");
const yetki = document.querySelector("#yetki");
const chatLock = document.querySelector("#chatLock");
const chatUnLock = document.querySelector("#chatUnLock");
const userOutAdmin = document.querySelector("#userOutAdmin");
const userOutYetkili = document.querySelector("#userOutAdmin");
const userOutPersonel = document.querySelector("#userOutAdmin");


function showToast(message) {
    toast.innerText = message;
    toast.classList.add("show");
    setTimeout(function () {
        toast.classList.remove("show");
    }, 3000);
}

//bağlantığı başladığı zaman
connection.start().then(() => {
    connection.invoke("OldMessage").catch(function (err) { return console.error(err.toString()); });
}).catch(function (err) {
    return console.error(err.toString());
});
//mesaj alanını gizleme
connection.on("UnShowMessageField", () => {
    messageField.style.display = "none";
    onlineStatusBox.style.display = "none";
    loader.style.display = "flex";
});
connection.on("ShowMessageField", () => {
    messageField.style.display = "block";
    onlineStatusBox.style.display = "block";
    loader.style.display = "none";

    messagesList.scrollTop = 9999999;
});
//eski mesajları listeleme
connection.on("OldMessageList", (ad, mesaj, yetki, resim,tarih) => {
    var li = document.createElement("li");
    messagesList.appendChild(li);

    li.innerHTML = `
    <div style="display: flex; flex-direction: column; max-width: 300px; background-color: #dcf8c6; border-radius: 10px; padding: 10px; margin-bottom: 10px;">
        <div style="display: flex; align-items: center;">
            ${resim ? `<img style="width: 40px; height: 40px; border-radius: 50%; margin-right: 10px;" src="/img/user/${resim}" alt="${yetki}">` : ''}
            <div style="display: flex; flex-direction: column;">
                <div style="font-weight: bold;">${ad}</div>
            </div>
        </div>
        <span style="
            font-size: 13px;
            border: 1px solid #fff;
            width: 40px;
            padding: 2px;
        ">${yetki}</span>
        <div style="background-color: #e2f7fe; border-radius: 10px; padding: 10px; margin-top: 5px; overflow-wrap:break-word; max-height:200px; overflow:hidden; text-overflow:ellipsis;">
            ${mesaj}
        </div>
        <div style="display:flex;justify-content:space-between;">
            <span>${tarih.split(' ')[1]}</span>
            <span>${tarih.split(' ')[0]}</span>
        </div>
    </div>
                `;

    if (yetki === "Bot") {
        li.innerHTML = `
            <div class="text-black w-25 mb-3" style="background-color:orangered;">
                <div class="d-inline-block" style="width: 20%;">
                    <img class="w-100"
                    src="/img/OleyBot.png"
                    alt="Bot">
                </div>
                <div class="d-inline-block h5 w-75 text-center p-0 m-0 mx-auto" style="background-color:yellow;">Oley Bot</div>
                <div class="h6 p-0 m-0 text-center text-white" style="background-color:red;">${mesaj}</div>
            </div>
            `;
    }

    messagesList.scrollTop = 9999999;
});
//Sohbeti Kilitle
connection.on("DisabledDocument", () => {
    sendButton.disabled = true;
    messageInput.disabled = true;
});
connection.on("EnabledDocument", () => {
    sendButton.disabled = false;
    messageInput.disabled = false;
    messageInput.focus();
});
//sohbete katıldı toast mesajı
connection.on("Toast", (isim) => { showToast(isim); });
//butona tıklandığı zaman mesaj gönder
if (sendButton) {
    sendButton.addEventListener("click", () => {
        var message = messageInput.value;
        connection.invoke("SendMessage", message).catch(function (err) {
            return console.error(err.toString());
        });
        messageInput.value = "";
    });
}
//Textbox Enter keypress event
if (messageInput) {
    messageInput.addEventListener("keypress", (event) => {
        if (event.key === "Enter") {
            sendButton.click();
        }
        else {//yazıyor bilgisi
            connection.invoke("YaziyorGosterme").catch(function (err) { return console.error(err.toString()); });
        }
    });
}
//yazıyor bilgisi gösterme
connection.on("Yaziyor", (ad) => {
    var text = yaziyor;
    text.style.opacity = "1";
    if (text.innerText.includes("yazıyor...") === true && text.innerText.includes(ad) === false) {
        text.innerText = ` ${ad} ${text.innerText}`;
    }
    else if (text.innerText.includes("yazıyor...") === false && text.innerText.includes(ad) === false) {
        text.innerText = `${text.innerText} ${ad} yazıyor...`;
    }
    setTimeout(() => {
        text.style.opacity = "0";
        text.innerText = text.innerText.replace(ad, "")
    }, 5000);
});
//mesaj gönderme
connection.on("ReceiveMessage", (ad, mesaj, yetki, resim,tarih) => {
    var li = document.createElement("li");
    messagesList.appendChild(li);

    li.innerHTML = `
    <div style="display: flex; flex-direction: column; max-width: 300px; background-color: #dcf8c6; border-radius: 10px; padding: 10px; margin-bottom: 10px;">
        <div style="display: flex; align-items: center;">
            <img style="width: 40px; height: 40px; border-radius: 50%; margin-right: 10px;" src="/img/${resim}" alt="${yetki}">
            <div style="display: flex; flex-direction: column;">
                <div style="font-weight: bold;">${ad}</div>
            </div>
        </div>
        <span style="
            font-size: 13px;
            border: 1px solid #fff;
            width: 40px;
            padding: 2px;
        ">${yetki}</span>
        <div style="background-color: #e2f7fe; border-radius: 10px; padding: 10px; margin-top: 5px; overflow-wrap:break-word; max-height:200px; overflow:hidden; text-overflow:ellipsis;">
            ${mesaj}
        </div>
        <div style="display:flex;justify-content:space-between;">
            <span>${tarih.split(' ')[1]}</span>
            <span>${tarih.split(' ')[0]}</span>
        </div>
    </div>
                `;


    if (yetki === "Admin" && mesaj.includes("<anket>")) {
        sendButton.disabled = true;
        messageInput.disabled = true;
        var anketSonuc = prompt(mesaj.split("<anket>")[1]);
        var anketIsim = mesaj.split("<anket>")[1];
        li.innerHTML = `
            <div class="text-black w-25 mb-3" style="background-color:orangered;">
                <div class="d-inline-block" style="width: 20%;">
                    <img class="w-100"
                    src="/img/OleyBot.png"
                    alt="Bot">
                </div>
                <div class="d-inline-block h5 w-75 text-center p-0 m-0 mx-auto" style="background-color:yellow;">Oley Bot</div>
                <div class="h6 p-0 m-0 text-center text-white" style="background-color:red;">${ad} Anket Açtı: ${anketIsim}</div>
            </div>
            `;
        connection.invoke("AnketSonuc", anketSonuc).catch(function (err) { return console.error(err.toString()); });
    }
    else if (yetki === "Bot") {
        li.innerHTML = `
            <div class="text-black w-25 mb-3" style="background-color:orangered;">
                <div class="d-inline-block" style="width: 20%;">
                    <img class="w-100"
                    src="/img/OleyBot.png"
                    alt="Admin">
                </div>
                <div class="d-inline-block h5 w-75 text-center p-0 m-0 mx-auto" style="background-color:yellow;">Oley Bot</div>
                <div class="h6 p-0 m-0 text-center text-white" style="background-color:red;">${mesaj}</div>
            </div>
            `;
    }

    messagesList.scrollTop = 9999999;
});
//timeout
connection.on("ShowLockIcon", () => {
    sendButton.innerHTML = "<i class='fa-solid fa-lock'></i>"
});
connection.on("ShowUnlockIcon", () => {
    sendButton.innerHTML = "Gönder"
});
//online durumu kutucuğa yazdırma
connection.on("OnlineStatusBox", (isim, id) => {
    let state = true;
    let pTags = document.querySelectorAll("#OnlineStatusBox p");
    pTags.forEach((p) => {
        if (p.id.toString() === id.toString()) {
            state = false;
            return;
        }
    })
    if (state) {
        var p = document.createElement("p");
        p.innerHTML = `
            <p id="${id}">
                <span class="online-user-icon">🟢</span>
                <span class="online-user">${isim}</span>
            </p>
        `;
        OnlineStatusBox.appendChild(p);
    }
})
connection.on("OnlineStatusBoxOut", (id) => {
    let pTags = document.querySelectorAll("#OnlineStatusBox p");
    pTags.forEach((p) => {
        if (p.id.toString() === id.toString()) {
            p.remove();
            return;
        }
    })
});
//admin tarafından kullanıcıyı atma
connection.on("UserOut", (haricKisi) => {
    if (!yetki.innerText.includes(haricKisi) && !yetki.innerText.includes("Admin")) {
        connection.invoke("SessionClear").catch(function (err) { return console.error(err.toString()); });
        window.location = "http://semihbogaz.com.tr/chatapp";
    }
});