$(document).ready(function() {
    if ($("#ddlRole option:selected").text() == "Pilot") {
        $("#IsAPilot").prop("checked", true);
        $("#IsAPilot").attr("disabled", true);
    } else {
        $("#IsAPilot").attr("disabled", false);
        if (!($("#IsAPilot").prop("checked"))) {
            if (!$(".view-to-public").hasClass('hide')) $(".view-to-public").addClass('hide');
        }
    }
    $("#ddlRole").change(function() {
        var role = $("#ddlRole option:selected").text();
        if (role == "Pilot") {
            $("#IsAPilot").prop("checked", true);
            $("#IsAPilot").attr("disabled", true);
            $(".view-to-public").removeClass('hide');
        } else {
            $("#IsAPilot").attr("disabled", false);
            $("#IsAPilot").prop("checked", false);
            $("#ViewToPublic").prop("checked", false);
            if (!$(".view-to-public").hasClass('hide')) $(".view-to-public").addClass('hide');
        }
    });

    $("#IsAPilot").change(function() {
        if (this.checked) $(".view-to-public").removeClass('hide');
        else {
            $("#ViewToPublic").prop("checked", false);
            if (!$(".view-to-public").hasClass('hide')) $(".view-to-public").addClass('hide');
        }
    });
});