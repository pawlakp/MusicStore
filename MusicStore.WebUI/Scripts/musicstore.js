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

function changePassword(data) {
    $('#cellPass-' + data.Id).empty();
    $('#cellPass-' + data.Id).prepend('<input checked="checked" class="check-box" disabled="disabled" type="checkbox" />');
}

function changePasswordClient(data) {
    alert("Zamiana hasła nastąpi przy następnym logowaniu");
}



function deleteData(data) {
    $('#row-' + data.Id).fadeOut('slow')
}

function succesAssign(){
    alert("Poprawnie przypisano album do użytkownika")
}
    

    //var Genres = document.getElementById('Genres');
    ////var ID = document.getElementById('News');
    ////var IDD = document.getElementById('Products');

    //var sendHttpRequest = (method, url) => {
    //    var promise = new Promise((resolve, reject) => {
    //        var xhr = new XMLHttpRequest();

    //xhr.open(method, url);

    //        xhr.onload = () => {
    //    resolve(xhr.response);
    //        };

    //xhr.send();
    //    });

    //return promise;
    //};

    //window.addEventListener('load', function () {
    //    sendHttpRequest('GET', '@Url.Action("MenuAdmin", "Nav")').then((responseData) => { Genres.innerHTML += responseData })
    //    //sendHttpRequest('GET', '@Url.Action("NumberAllbumsView", "Admin")').then((responseData) => { IDD.innerHTML += responseData })
    //    //sendHttpRequest('GET', '@Url.Action("NumberAllbumsView", "Admin")').then((responseData) => { IDD.innerHTML += responseData })

    //})


