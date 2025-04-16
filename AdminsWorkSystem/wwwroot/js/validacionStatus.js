var datatable;

$(document).ready(function () {
    var url = window.location.search;//saber la url de la captura seleccionados
    if (url.includes("finalizado")) {
        loadDataTable("ObtenerTodosStatus?estado=finalizado");
    }
    else {
        if (url.includes("baja")) {
            loadDataTable("ObtenerTodosStatus?estado=baja");
        }
        else {
            if (url.includes("reingreso")) {
                loadDataTable("ObtenerTodosStatus?estado=reingreso");
            }
            else {
                if (url.includes("activo")) {
                    loadDataTable("ObtenerTodosStatus?estado=activo");
                }
                else {
                    loadDataTable("ObtenerTodosStatus?estado=todas");
                }
            }
        }
    }

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
            "url": "/Estudiantes/UsuarioAplicacion/"+url
        },
        "columns": [
            { "data": "matricula", "width": "10%" },
            //{
            //    "data": function nombreUsuario(data) {
            //        return data.usuarioAplicacion.nombres + " " + data.usuarioAplicacion.apellidoPaterno + " " + data.usuarioAplicacion.apellidoMaterno;
            //    }, "width": "20%"
            //},
            { "data": "nombres", "width": "10%" },         
            { "data": "grupoActual", "width": "10%" },
            { "data": "subPrograma", "width": "10%" },
            { "data": "actividad", "width": "10%" },
            { "data": "generacion", "width": "10%" },
            { "data": "unidades.nombre", "width": "10%" },
            { "data": "especialidades.nombre", "width": "10%" },
            {
                "data": "usuarioAplicacion.status",
                "render": function (data) {
                    if (data == "Activo") {
                        return "<span class='badge rounded-pill bg-success text-white'>Activo</span>";
                    }
                    else if (data == "Reingreso") {
                        return "<span class='badge rounded-pill bg-warning text-dark'>Reingreso</span>";
                    }
                    else if (data == "Baja") { return "<span class='badge rounded-pill bg-danger text-white'>Baja</span>"; }
                    else { return "<span class='badge rounded-pill bg-primary text-dark'>Finalizado</span>"; }

                }, "width": "30%",

            },

            //},
            //{
            //    "data": {
            //        id: "id", lockoutEnd: "lockoutEnd"
            //    },
            //    "render": function (data) {
            //        var hoy = new Date().getTime();
            //        var bloqueo = new Date(data.lockoutEnd).getTime();
            //        if (bloqueo > hoy) {
            //            // Usuario esta bloqueado
            //            return `
            //            <div class="text-center">
            //                <a onclick=BloquearDesbloquear('${data.id}') class="btn btn-danger text-white" style="cursor:pointer; width:150px;">
            //                    <i class="fas fa-lock-open"></i> Desbloquear
            //                </a>
            //            </div>
            //            `;
            //        }
            //        else {
            //            return `
            //            <div class="text-center">
            //                <a onclick=BloquearDesbloquear('${data.id}') class="btn btn-success text-white" style="cursor:pointer; width:150px;">
            //                    <i class="fas fa-lock"></i> Bloquear
            //                </a>
            //            </div>
            //            `;
            //        }


            //    }, "width": "20%",


            //}
        ]
    });
}


//function BloquearDesbloquear(id) {


//    $.ajax({
//        type: "POST",
//        url: '/Admin/Usuario/BloquearDesbloquear',
//        data: JSON.stringify(id),
//        contentType: "application/json",
//        success: function (data) {
//            if (data.success) {
//                toastr.success(data.message);
//                datatable.ajax.reload();
//            }
//            else {
//                toastr.error(data.message);
//            }
//        }
//    });

//}