document.addEventListener('DOMContentLoaded', function () {
    document.querySelector('.upload-icon').addEventListener('click', function () {
        document.querySelector('#avatar-file-input').click();
    });
});

const profilePic = document.querySelector(".avatar-img");
const userFile = document.querySelector("#avatar-file-input");
userFile.onchange = function () {
    profilePic.src = URL.createObjectURL(userFile.files[0]);
}
function toggleTheme() {
    var theme = getCookie("Theme");

    if (theme === "light") {
        setCookie("Theme", "dark", 30);
        applyDarkTheme();
    } else {
        setCookie("Theme", "light", 30);
        applyLightTheme();
    }
}

function setCookie(name, value, days) {
    var expires = "";
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        expires = "; expires=" + date.toUTCString();
    }
    document.cookie = name + "=" + (value || "") + expires + "; path=/";
}

function getCookie(name) {
    var cookieName = name + "=";
    var decodedCookie = decodeURIComponent(document.cookie);
    var cookieArray = decodedCookie.split(';');

    for (var i = 0; i < cookieArray.length; i++) {
        var cookie = cookieArray[i];
        while (cookie.charAt(0) == ' ') {
            cookie = cookie.substring(1);
        }
        if (cookie.indexOf(cookieName) == 0) {
            return cookie.substring(cookieName.length, cookie.length);
        }
    }
    return "";
}
var themeSelect = document.getElementById("themeSelect");

function onSave() {
    var selectedTheme = themeSelect.value;

    if (selectedTheme === "true") {
        setCookie("Theme", "dark", 30);
        applyDarkTheme();
    } else {
        setCookie("Theme", "light", 30);
        applyLightTheme();
    }
}