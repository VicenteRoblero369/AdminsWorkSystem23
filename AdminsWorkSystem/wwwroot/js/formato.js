﻿var datatable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    datatable = $('#tblDatos').DataTable({
        "ajax": {
            "url": "/Admin/Formatos/ObtenerTodos"
        },
        "columns": [
            { "data": "nombre", "width": "15%" },
          
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <a href="/Admin/Formatos/Upsert/${data}" class="btn btn-success text-white" style="cursor:pointer">
                                <i class="fas fa-edit"></i>
                            </a>                            
                            <a onclick=Delete("/Admin/Formatos/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
                                <i class="fas fa-trash"></i>
                            </a>
                        </div>
                        `;
                }, "width": "20%"
            }

        ]
    });
}


function Delete(url) {

    swal({
        title: "Esta Seguro que quiere Eliminar?",
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