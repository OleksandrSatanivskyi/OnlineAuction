document.addEventListener('DOMContentLoaded', function () {
    var theme = getCookie("Theme");

    if (theme === "light") {
        applyLightTheme();
    }
    else {
        applyDarkTheme();
    }
});

function applyLightTheme() {
    var elementsWithDarkBackground = document.querySelectorAll('.bg-dark');
    elementsWithDarkBackground.forEach(function (element) {
        element.classList.remove('bg-dark');
        element.classList.add('bg-light');
    });

    var elementsWithLightText = document.querySelectorAll('.text-light');
    elementsWithLightText.forEach(function (element) {
        element.classList.remove('text-light');
        element.classList.add('text-dark');
    });

    var elementsWithLightBorder = document.querySelectorAll('.border-light');
    elementsWithLightBorder.forEach(function (element) {
        element.classList.remove('border-light');
        element.classList.add('border-primary');
    });

    var elementsWithDarkClass = document.querySelectorAll('.dark');
    elementsWithDarkClass.forEach(function (element) {
        element.classList.remove('dark');
    });

    var googleButton = document.querySelector('.continue-with-google-btn');
    if (googleButton) {
        googleButton.style.borderColor = 'var(--primary-color)';
        googleButton.style.color = 'black';
    }
}

function applyDarkTheme() {
    var elementsWithLightBackground = document.querySelectorAll('.bg-light');
    elementsWithLightBackground.forEach(function (element) {
        element.classList.remove('bg-light');
        element.classList.add('bg-dark');
    });

    var elementsWithDarkText = document.querySelectorAll('.text-dark');
    elementsWithDarkText.forEach(function (element) {
        element.classList.remove('text-dark');
        element.classList.add('text-light');
    });

    var elementsWithDarkBorder = document.querySelectorAll('.border-primary');
    elementsWithDarkBorder.forEach(function (element) {
        element.classList.remove('border-primary');
        element.classList.add('border-light');
    });

    var googleButton = document.querySelector('.continue-with-google-btn');
    if (googleButton) {
        googleButton.style.borderColor = 'white';
        googleButton.style.color = '#ffffff';
    }
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