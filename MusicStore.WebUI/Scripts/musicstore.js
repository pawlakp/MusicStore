$(document).ready(function () {
    var a = 0;
    var click = 0;
    $("#btn1").click(function () {
        if (a < 1) {
            $("#NameLabel").append("<div id='LabelName'>Podaj nazwę wytwórni<input class='form-control' type='text' id='LabelName' name='LabelName'></div>");
            a = 2;

        }
        else {
            $("#LabelName").remove()
            a = 0;
        }
    });

    $('#menubutton').click(function () {
        $('#menu').toggle('show');
        click = 2;
        $(window).scrollTop(0);
    })

    $(window).on('load resize', function (event) {
        if (click > 1) {
            click = 0;
            var windowSize = $(window).width(); // Could've done $(this).width()
            if (windowSize > 768) {
                $('#menu').show();
            }
        }
    });

});