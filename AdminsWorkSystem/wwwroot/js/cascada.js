$(document).ready(function () {
    $('#unidades').attr('disabled', true);
    $('#especialidades').attr('disabled', true);
    ObtenerTodos();

    $('#unidades').change(function () {
        var unidadesId = $(this).val();
        if (unidadesId > 0) {
            ObtenerTodosEspecialidades(unidadesId);
        }
        else {
            alert("Seleccione Especialidad");
            $('#especialidades').attr('disabled', true);
            $('#unidades').attr('disabled', true);
            $('#especialidades').append('<option>--Selccione Especialidad--</option>');
            $('#unidades').append('<option>--Seleccione Unidad--</option>');
        }
    });


});
function ObtenerTodos() {
    $('#unidades').empty();

    $.ajax({
        url: '/Admin/Cascada/ObtenerTodos',
        success: function (response) {
            if (response != null && response != undefined && response.length > 0) {
                $('#unidades').attr('disabled', false);
                $('#unidades').append('<option>--Seleccione Unidad--</option>');
                $('#especialidades').append('<option>--Selccione Especialidad---</option>');
                $.each(response, function (i, data) {
                    $('#unidades').append('<option value=' + data.id + '>' + data.nombre + '</option>');
                });
            }
            else {
                $('#unidades').attr('disabled', false);
                $('#unidades').append('<option>--Unidades No Disponibles--</option>');
            }
        },
        error: function (error) {
            alert(error);
        }
    });
}

function ObtenerTodosEspecialidades(unidadesId) {
    $('#especialidades').empty();

    $.ajax({
        url: '/Admin/Cascada/ObtenerTodosEspecialidades?Id=' + unidadesId,
        success: function (response) {
            if (response != null && response != undefined && response.length > 0) {
                $('#especialidades').attr('disabled', false);
                $('#especialidades').append('<option>--Seleccione Especialidades--</option>');
                $.each(response, function (i, data) {
                    $('#especialidades').append('<option value=' + data.id + '>' + data.nombre + '</option>');
                });
            }
            else {
                $('#especialidades').attr('disabled', false);
                $('#especialidades').append('<option>--Especialidades No Disponibles--</option>');
            }
        },
        error: function (error) {
            alert(error);
        }
    });
}