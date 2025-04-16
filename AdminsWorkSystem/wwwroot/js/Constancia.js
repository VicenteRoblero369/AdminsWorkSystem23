var datatable;

$(document).ready(function () {
    var url = window.location.search;//saber la url de la captura seleccionados
    if (url.includes("solicitando")) {
        loadDataTable("ObtenerListaConstancia?estado=solicitando");
    }
           
        else {
            loadDataTable("ObtenerListaConstancia?estado=todas");
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
            "url": "/Admin/Constancia/"+url//concatenar con la url que recibe
        },
        "columns": [
            {
                "data": function nombreUsuario(data) {
                    return data.usuarioAplicacion.nombres + " " + data.usuarioAplicacion.apellidoPaterno + " " + data.usuarioAplicacion.apellidoMaterno;
                }, "width": "20%"
            }, 
            { "data": "myProperty", "width": "15%" },           
            { "data": "usuarioAplicacion.unidades.nombre", "width": "15%" },  
            { "data": "usuarioAplicacion.especialidades.nombre", "width": "15%" },    
            {
                "data": "estatus",
                "render": function (data) {
                    if (data == "Entregado") {
                        return "<span class='btn btn-success'>Entregado</span>";
                    }
                    else if (data == "Solicitando") { return "<span class='btn btn-info'>Solicitando</span>"; }
                    else { return "<span class='badge rounded-pill bg-primary text-white'></span>"; }

                }, "width": "30%",

            },            
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <a href="/Admin/Constancia/Upsert/${data}" class="btn btn-success text-white" style="cursor:pointer">
                                <i class="fas fa-list-ul"></i>
                            </a>                           
                        </div>
                        `;
                }, "width": "5%"
            }
        ]
    });
}

