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
        $('#Menu').toggle('show');
        click = 2;
        $(window).scrollTop(0);
    })

    $(window).on('load resize', function (event) {
        if (click > 1) {
            click = 0;
            var windowSize = $(window).width(); // Could've done $(this).width()
            if (windowSize > 768) {
                $('#Menu').show();
            }
        }
    });

    $("#Text1").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Home/MiniFind",
                type: "POST",
                dataType: "json",
                data: { searchedItem: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.Name, value: item.Name };
                    }))


                }
            })
        },

    });


});


//var TMP = document.getElementById('Menu');
//var sendHttpRequest = (method, url) => {
//    var promise = new Promise((resolve, reject) => {
//        var xhr = new XMLHttpRequest();

//        xhr.open(method, url);

//        xhr.onload = () => {
//            resolve(xhr.response);
//        };

//        xhr.send();
//    });

//    return promise;
//};

//window.addEventListener('load', function () {
//    sendHttpRequest('GET', '@Url.Action("Menu", "Nav")').then((responseData) => { TMP.innerHTML += responseData })

//})

function processData(data) {
    $('#row-' + data.DeleteId).fadeOut('slow')
    $('#suma').text(data.CartTotal);

}
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


function addcart(data) {
    alert(data);

}

function addToWishlist(data) {
    if (data.Wishlist == true)
        alert("Dodano do listy życzeń")
    else {
        alert("Usunięto z listy życzeń")
        $('#row-' + data.AlbumId).fadeOut('slow')
    }
}



    

