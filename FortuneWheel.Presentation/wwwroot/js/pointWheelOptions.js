function isValidHexColor(hex) {
    var hexColorRegex = /^#([A-Fa-f0-9]{6})$/;

    return hexColorRegex.test(hex);
}

function setInitialColorPickerValueForIndex(index) {
    var hexColorField = document.getElementById("colorHex" + index);
    var colorPicker = document.getElementById("colorPicker" + index);
    colorPicker.value = hexColorField.value;
}

function updateColorPreview() {
    var colorPicker = document.getElementById("colorPicker");
    var hexColorField = document.getElementById("colorHex");
    var colorValue = colorPicker.value;
    hexColorField.value = colorValue;
}

function updateColorPicker() {
    var hexColorField = document.getElementById("colorHex");
    var colorPicker = document.getElementById("colorPicker");
    var hexColorValue = hexColorField.value;
    colorPicker.value = hexColorValue;
}

function updateColorPickerForIndex(index) {
    var hexColorField = document.getElementById("colorHex" + index);
    var colorPicker = document.getElementById("colorPicker" + index);
    colorPicker.value = hexColorField.value;
}

function updateColorHexForIndex(index) {
    var hexColorField = document.getElementById("colorHex" + index);
    var colorPicker = document.getElementById("colorPicker" + index);
    hexColorField.value = colorPicker.value;
}