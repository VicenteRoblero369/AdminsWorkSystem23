var datatable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    datatable = $('#tblDatos').DataTable({
        "language": {
            "lengthMenu": "Mostrar _MENU_ Registros Por Pagina",
            "zeroRecords": "Ningun Registro",
            "info": "Mostrar page _PAGE_ de _PAGES_",
            "infoEmpty": "no hay registros",
            "infoFiltered": "(filtered from _MAX_ total registros)",
            "search": "Buscar",
            "paginate": {
                "first": "Primero",
                "last": "Último",
                "next": "Siguiente",
                "previous": "Anterior"
            }
        },
        "ajax": {
            "url": "/Admin/Usuario/ObtenerOrdenLista"
        },
        "columns": [
            { "data": "userName", "width": "10%" },
            { "data": "nombres", "width": "10%" },
            { "data": "apellidoPaterno", "width": "10%" },
            { "data": "apellidoMaterno", "width": "10%" },
            { "data": "email", "width": "15%" },
            { "data": "sexo", "width": "10%" },
            { "data": "lenguaMaterna", "width": "15%" },
            { "data": "matricula", "width": "10%" },
            { "data": "generacion", "width": "10%" },
            { "data": "phoneNumber", "width": "10%" },
            { "data": "unidades.nombre", "width": "10%" },
            { "data": "especialidades.nombre", "width": "10%" },
            { "data": "role", "width": "15%" },
            {
                "data": "status",
                "render": function (data) {
                    if (data == "Activo") {
                        return "<span class='badge bg-success text-ligth'>Activo</span>";
                    }
                    else if (data == "Baja") {
                        return "<span class='badge bg-danger'>Baja</span>";
                    }
                    else if (data == "Reingreso") {
                        return "<span class='badge bg-warning'>Reingreso</span>";
                    }
                    else {
                        return "<span class='badge rounded-pill bg-warning text-dark'></span>";
                    }
                }, "width": "30%",

            },
            {
                "data": {
                    id: "id", lockoutEnd: "lockoutEnd"
                },
                "render": function (data) {
                    var hoy = new Date().getTime();
                    var bloqueo = new Date(data.lockoutEnd).getTime();
                    if (bloqueo > hoy) {
                        // Usuario esta bloqueado
                        return `
                        <div class="text-center">
                            <a onclick=BloquearDesbloquear('${data.id}') class="btn btn-danger text-white" style="cursor:pointer; width:150px;">
                                <i class="fas fa-lock-open"></i> Desbloquear
                            </a>
                        </div>
                        `;
                    }
                    else {
                        return `
                        <div class="text-center">
                            <a onclick=BloquearDesbloquear('${data.id}') class="btn btn-success text-white" style="cursor:pointer; width:150px;">
                                <i class="fas fa-lock"></i> Bloquear
                            </a>
                        </div>
                        `;
                    }


                }, "width": "20%",


            },
            {
                "data": "id",
                "render": function (data) {
                    return `   
                        <div class="text-center">
                            <a onclick=Delete("/Admin/Usuario/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
                                <i class="fas fa-trash"></i>
                            </a>
                        </div>
                        `;
                }, "width": "20%"
            }
            
        ]
    });
}


function BloquearDesbloquear(id) {


    $.ajax({
        type: "POST",
        url: '/Admin/Usuario/BloquearDesbloquear',
        data: JSON.stringify(id),
        contentType: "application/json",
        success: function (data) {
            if (data.success) {
                toastr.success(data.message);
                datatable.ajax.reload();
            }
            else {
                toastr.error(data.message);
            }
        }
    });

}
function Delete(url) {

    swal({
        title: "Esta Seguro que quiere Eliminar Usuario?",
        text: "Este Registro no se podra recuperar",
        icon: "warning", 
        buttons: true,
        dangerMode: true
    }).then((borrar) => {
        if (borrar) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        datatable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}