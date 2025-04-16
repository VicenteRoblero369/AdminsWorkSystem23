var datatable;

$(document).ready(function () {
    var url = window.location.search;//saber la url de la captura seleccionados
    if (url.includes("pendiente")) {
        loadDataTable("ObtenerOrdenLista?estado=pendiente");
    }
    else {
        if (url.includes("aprobado")) {
            loadDataTable("ObtenerOrdenLista?estado=aprobado");
        }
        else {
            if (url.includes("enviado")) {
                loadDataTable("ObtenerOrdenLista?estado=enviado");
            }
            else {
                if (url.includes("cancelado")) {
                    loadDataTable("ObtenerOrdenLista?estado=cancelado");
                }
                else {
                    if (url.includes("activo")) {
                        loadDataTable("ObtenerOrdenLista?estado=activo");
                    }
                    else {
                        if (url.includes("baja")) {
                            loadDataTable("ObtenerOrdenLista?estado=baja");
                        }
                        else {
                            if (url.includes("reingreso")) {
                                loadDataTable("ObtenerOrdenLista?estado=reingreso");
                            }
                            else {
                                if (url.includes("finalizado")) {
                                    loadDataTable("ObtenerOrdenLista?estado=finalizado");
                                }
                                else {
                                    loadDataTable("ObtenerOrdenLista?estado=todas");
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    
});

function loadDataTable(url) {
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
            "url": "/Estudiantes/EvidenciaOReporte/"+url//concatenar con la url que recibe
        },
        "columns": [
            //{ "data": "id", "width": "10%" },
            { "data": "usuarioAplicacion.matricula", "width": "15%" },
            {
                "data": function nombreUsuario(data) {
                    return data.usuarioAplicacion.nombres + " " + data.usuarioAplicacion.apellidoPaterno + " " + data.usuarioAplicacion.apellidoMaterno;
                }, "width": "20%"
            },
            { "data": "grupoActual", "width": "15%" },
            { "data": "subPrograma", "width": "15%" },
            { "data": "actividad", "width": "15%" },  
            { "data": "usuarioAplicacion.generacion", "width": "15%" },  
            { "data": "usuarioAplicacion.unidades.nombre", "width": "15%" },  
            { "data": "usuarioAplicacion.especialidades.nombre", "width": "15%" },
            { "data": "imagenes", "width": "15%" },
            //{
            //    "data": "usuarioAplicacion.status",
            //    "render": function (data) {
            //        if (data == "Activo") {
            //            return "<span class='btn btn-success'>Activo</span>";
            //        }
            //        else if (data == "Baja") { return "<span class='btn btn-danger'>Baja</span>"; }
            //        else if (data == "Reingreso") { return "<span class='btn btn-warning'>Reingreso</span>"; }
            //        else { return "<span class='badge rounded-pill bg-primary text-white'>Finalizado</span>"; }

            //    }, "width": "30%",

            //},   
            {
                "data": "estado",
                "render": function (data) {
                    if (data == "Aprobado") {
                        return "<span class='btn btn-success'>Aprobado</span>";
                    }
                    else if (data == "Enviado") { return "<span class='btn btn - light disabled'>Enviado</span>"; }                 
                    else if (data == "Cancelado") { return "<span class='btn btn-danger disabled'>Cancelado</span>"; }
                    else { return "<span class='badge rounded-pill bg-primary text-white'>Finalizado</span>"; }
                    
                }, "width": "30%",

            },
            
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <a href="/Estudiantes/EvidenciaOReporte/Detalle/${data}" class="btn btn-success text-white" style="cursor:pointer">
                                <i class="fas fa-list-ul"></i>
                            </a>                           
                        </div>
                        `;
                }, "width": "5%"
            }
        ]
    });
}

