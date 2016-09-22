function slideToggleDivButton(divId, buttonId, text1, text2) {
    $(divId).slideToggle(1000);
    (function() {
        if ($(buttonId).text() == text2) {
            $(buttonId).text(text1);
        } else {
            $(buttonId).text(text2);
        }
    })();
}

function slideToggleDiv(divId) {
    if (arguments.length > 1) {
        $(divId).slideToggle(arguments[2]);
    } else {
        $(divId).slideToggle(1000);
    }
}

function doubleSlideToggleDiv(divId) {
    slideToggleDiv(divId);
    slideToggleDiv(divId);
}

function hideShowDiv(divId) {
    $(divId).toggle('slow');
}

function DoubleHide(divId1, divId2, action) {

    if (divId1 != null) {
        slideToggleDiv(divId1);
    }
    if (divId2 != null) {
        hideShowDiv(divId2);
    }
    if (action != null) {
        var func = action;
        window.setTimeout(func, 1100);
    }
}

