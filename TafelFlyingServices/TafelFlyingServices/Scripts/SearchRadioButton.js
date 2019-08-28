$(document).ready(function() {
    if ($("#search-city").prop("checked")) {
        $(".search-country").removeClass("hide");
    } else {
        $(".search-country").addClass("hide");
    }
    $(".search-radio-button").change(function() {
        if ($("#search-city").prop("checked")) {
            $(".search-country").removeClass("hide");
        } else {
            $(".search-country").addClass("hide");
        }
    });
});