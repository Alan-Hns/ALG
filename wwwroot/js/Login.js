jQuery(document).ready(function ($) {

    $('#UserName').focus();

    $('#Ingress').on('click', function () {
        if ($('#UserName').val() != "" & $('#Password').val() != "") {
            Validate($('#UserName').val(), $('#Password').val());
        }
        else {
            Swal.fire(
                'Error',
                'Favor de Ingresar Usuario y Contraseña',
                'error'
            );
        }
    });

    function Validate(UserName, Password) {
        var record = {
            UserName: UserName,
            Password: Password
        };

        $.ajax({
            url: '/Login/GetUsuarios',
            async: false,
            type: 'POST',
            data: record,
            beforeSend: function (xhr, opts) {
            },
            complete: function () {
            },
            success: function (data) {
                if (data.status == true) {
                    window.location.href = "/Home/Index";
                }
                else if (data.status == false) {
                    Swal.fire(
                        'Error',
                        data.message,
                        'error'
                    );
                }
            },
            error: function (data) {
                Swal.fire(
                    'Error',
                    data.message,
                    'error'
                );
            }
        });
    }
});