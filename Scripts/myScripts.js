function slideToggleDiv(divId, buttonId, text1, text2) {
    $(divId).slideToggle(1000);
    (function() {
        if ($(buttonId).text() == text2) {
            $(buttonId).text(text1);
        } else {
            $(buttonId).text(text2);
        }
    })();
}

