var originalColor = $('.modal-pick').css("color");
$(document).ready(function () {
    $('#colorPickerModal').on('show.bs.modal', function (e) {
        $('#colorPicker').simplecolorpicker()
            .on('change', function () {
                $('.color-picker-plane').css('color', $('#colorPicker').val());
            });
        $('.modal-save').on('click', function () {
            originalColor = $('select[name="Color"]').val();
        });
    });

    $('#colorPickerModal').on('hidden.bs.modal', function (e) {
        $('.modal-pick').css('color', originalColor);
        $('.plane-header').css('background-color', originalColor);
        $("#colorPicker option").each(function(){
            if($(this).text() == originalColor){
                $(this).selected = true;
            }
            else {
                $(this).selected = false;
            }
        });
    });

    $("#BlackOrWhite").on("change", function () {
        if ($(this).prop("checked")) {
            $('.plane-header').find('h2').css("color", "#000000");
        } else {
            $('.plane-header').find('h2').css("color", "#ffffff");
        }
    });

});