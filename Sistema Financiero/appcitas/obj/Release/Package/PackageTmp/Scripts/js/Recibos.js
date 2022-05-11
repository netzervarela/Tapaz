var hoy = moment(new Date()).format('YYYY-MM-DD');

function ListaRecibos(e) {
    e.stopPropagation();
    var id = -1;
    GetRecibos();
    $("#theHeader").html("Gestión de Recibos");
    $('#hidden_id').val("");
    $("#ListaRecibos").find('input[type = "text"]').val("");
    $('#descripcion_tipo_razon').val('');
    $('#tiene_listado_x').val(0);
    $('#tipostatus').val('ACT');
    $('#tiene_listado_x').trigger('change');
    $('#ModalListaRecibos').modal('show');
}

$("#btnGenerarSolicitud").on('click', function (e) {
    var recibo = $("#NumTrans").val()
    GenerarReciboOtrasTransac(recibo);
});

//GENERA PDF DEL RECIBO DE PAGO DE OTRAS TRANSACCIONES 
function GeneraPdfReciboOtrasTransac(re_numero, OT_Clave, servicio, re_observacion, re_fecha, re_total_rec) {
    var path = urljs + "/Caja/GeneraPdfReciboOtrasTransac";
    var posting = $.post(path, {
        re_numero: re_numero,
        OT_Clave: OT_Clave,
        servicio: servicio,
        re_observacion: re_observacion,
        re_fecha: re_fecha,
        re_total_rec: re_total_rec
    });

    posting.done(function (data, status, xhr) {
        console.log(data);
        if (data == 1) {
            window.open('/PDF/ReciboOtros1.pdf');
        }
    });

}


function GenerarReciboOtrasTransac(Recibo) {

    try {
        var path = urljs + "/Caja/GeneraReciboOtrasTransac";
        var posting = $.post(path, { Recibo: Recibo });
        posting.done(function (data, status, xhr) {
            dataCitas = [];
            dataCitas = data;
            if (dataCitas.length > 0) {
                for (var i = dataCitas.length - 1; i >= 0; i--) {
                    re_numero = dataCitas[i].re_numero
                    OT_Clave = dataCitas[i].OT_Clave
                    servicio = dataCitas[i].servicio
                    re_observacion = dataCitas[i].re_observacion
                    re_fecha = dataCitas[i].re_fecha
                    re_total_rec = dataCitas[i].re_total_rec
                    GeneraPdfReciboOtrasTransac(re_numero, OT_Clave, servicio, re_observacion, re_fecha, re_total_rec)
                }
            }
        });
        posting.fail(function (data, status, xhr) {
            GenerarErrorAlerta(xhr, "error");
            goAlert();
        });
        posting.always(function (data, status, xhr) {
            //RemoveAnimation("body");
        });
    }
    catch (e) {
        //RemoveAnimation("body");
    }
}

//REIMPRIMIR RECIBO
function ReimprimirRecibo(e) {
    e.stopPropagation();

    var recibo = $(e.currentTarget).attr('data-id');
    var tipo = $(e.currentTarget).attr('data-tipo_id');
    var path = urljs + "/Caja/ReimprimirRecibo";
    var posting = $.post(path, { recibo: recibo, tipo: tipo });

    posting.done(function (data, status, xhr) {
        console.log(data);
        if (data == 1) {
            if (tipo == 0) {
                window.open('/PDF/ReciboPrestamo1.pdf');
            } else {
                window.open('/PDF/ReciboOtros1.pdf');
            }

        }
    });

}

function GenerarSolicitud(recibo) {

    var path = urljs + "/Caja/ReciboOtros";
    var posting = $.post(path, { recibo: recibo });

    posting.done(function (data, status, xhr) {
        console.log(data);
        if (data == 1) {
            window.open('../ReciboOtrasTransacciones' + recibo + '.pdf');
        }
    });
    

    //var path = urljs + "/Prestamos/ReciboOtros";
    //var posting = $.post(path, { NumRecibo = $("#NumTrans").val() });
}
  
function GetRecibos() {
    try {
        /*animacion de loading*/
        //LoadAnimation("body");

        var path = urljs + "/Caja/GetRecibos";
        var posting = $.post(path, {});
        posting.done(function (data, status, xhr) {
            //console.log(data);
            LimpiarTabla("#ListaRecibos");
            if (data.length) {
                if (data[0].Accion > 0) {
                    $('.titulo').empty().append('Listado de préstamos del cliente: ' + data[0].NomPrestamo);
                    //client = data[0].ClienteId;
                    for (var i = data.length - 1; i >= 0; i--) {
                        addRow(data[i], "#ListaRecibos");
                    }
                }
                else {
                    GenerarErrorAlerta(data[0].Mensaje, "error");
                    goAlert();
                }
            }
        });
        posting.fail(function (data, status, xhr) {
            console.log(data);
            GenerarErrorAlerta(xhr, "error");
            goAlert();
        });
        posting.always(function (data, status, xhr) {
            //RemoveAnimation("body");
        });
    }
    catch (e) {
        //RemoveAnimation("body");
        console.log(e);
    }
}

function addRow(ArrayData, tableID) {

    var newRow = $(tableID).dataTable().fnAddData([
        ArrayData['RE_Numero'],
        //ArrayData['MontoSolicitado'],
        ArrayData['RE_Fecha'],
        //ArrayData['PlazoMeses'],
        //ArrayData['Interes'],
        ArrayData['RE_Observacion'],
        ArrayData['RE_Total_Rec'],
        ArrayData['RE_agrego'],
        ArrayData['Estado'],
        //ArrayData['RE_Tipo'],
        //"<button data-id='" + ArrayData['RE_Documento'] + "' data-name='" + ArrayData['RE_Documento'] + "' title='Ver Prestamos' data-toggle='tooltip' onClick='AnularRecibo(" + ArrayData['RE_Documento'] + ")" + ArrayData['RE_Documento'] + "\"' id='btnVerRazones" + ArrayData['RE_Documento'] + "' class='btn btn-success text-center btn-sm'><i class='fa fa-times-circle'></i></button>" +
        //"<button data-id='" + ArrayData['RE_Documento'] + "' data-name='" + ArrayData['RE_Documento'] + "' title='Editar Préstamos' data-toggle='tooltip' onClick='EditarTipoRazon(" + ArrayData['RE_Documento'] + ")' id='btnEditarTipoRazon" + ArrayData['RE_Documento'] + "' class='btn btn-primary text-center btn-sm'><i class='fa fa-pencil-square-o'></i></button>",
        //"<button data-id='" + ArrayData['RE_Documento'] + ArrayData['RE_Tipo'] + "' data-tipo_id='" + ArrayData['RE_Documento'] + ArrayData['RE_Tipo'] + "' data-name='" + ArrayData['RE_Documento'] + ArrayData['RE_Tipo'] + "' title='Reimprimir recibo' data-toggle='tooltip' onClick='ReimprimirRecibo(" + ArrayData['RE_Documento'] + ArrayData['RE_Tipo'] + ")' id='ReimprimirRecibo" + ArrayData['RE_Documento'] + ArrayData['RE_Tipo'] + "' class='btn btn-success text-center btn-sm'><i class='glyphicon glyphicon-thumbs-up'></i></button>" +
        "<button data-id='" + ArrayData['RE_Numero'] + "' data-tipo_id='" + ArrayData['RE_Tipo'] + "' data-name='" + ArrayData['RE_Numero'] + "' title='Reimprimir recibo' data-toggle='tooltip' onClick='ReimprimirRecibo(event)' id='ReimprimirRecibo" + ArrayData['RE_Numero'] + "' class='btn btn-success text-center btn-sm'><i class='glyphicon glyphicon-thumbs-up'></i></button>" +
        "<button data-id='" + ArrayData['RE_Documento'] + "' title='Anular Recibo' data-toggle='tooltip' onClick='AnularRecibo(" + ArrayData['RE_Documento'] + ")' id='AnularReciboCaja" + ArrayData['RE_Documento'] + "' class='btn btn-primary text-center btn-sm'><i class='fa fa-times-circle'></i></button>"
        //"<button data-id='" + ArrayData['RE_Documento'] + "' title='Anular Recibo' data-toggle='tooltip' onClick='AnularRecibo(" + ArrayData['RE_Documento'] + ")' id='AnularReciboCaja" + ArrayData['RE_Documento'] + "' class='btn btn-primary text-center btn-sm'><i class='fa fa-times-circle'></i></button>"
        //+ "<button data-id='" + ArrayData['RE_Numero'] + "' title='Anular Recibo' data-toggle='tooltip' onClick='GetDatosPagoPrestamo(" + ArrayData['RE_Numero'] + ")' id='btnGenerarPag" + ArrayData['RE_Numero'] + "' class='btn btn-primary text-center btn-sm'><i class='fa fa-pencil-square-o'></i></button>"
        
        //"<button data-id='" + ArrayData['RazonId'] + "' data-tipo_id='" + ArrayData['TipoId'] + "' data-name='" + ArrayData['RazonDescripcion'] + "' title='Generar Pago' data-toggle='tooltip' onClick='EditarRazon(event)' id='btnEditarRazon" + ArrayData['RazonId'] + "' class='btn btn-primary text-center btn-sm'><i class='fa fa-pencil-square-o'></i></button>", 
        //+ "<button data-id='" + ArrayData['RazonId'] + "' data-tipo_id='" + ArrayData['TipoId'] + "' data-name='" + ArrayData['RazonDescripcion'] + "' title='Eliminar Préstamo' data-toggle='tooltip' onClick='EliminarRazon(event)' id='btnEliminarRazon" + ArrayData['RazonId'] + "' class='btn btn-danger botonVED text-center btn-sm'><i class='fa fa-trash-o'></i></button>",
        //ArrayData['ClienteId']
    ]);

    var theNode = $(tableID).dataTable().fnSettings().aoData[newRow[0]].nTr;
    theNode.setAttribute('id', ArrayData['RE_Documento']);
    $('td', theNode)[7]//.setAttribute('class', 'RE_Tipo hidden');
}

//REIMPRIMIR RECIBO
//function ReimprimirRecibo(e) {
//    e.stopPropagation();
    
//    var recibo = $(e.currentTarget).attr('data-id');
//    var tipo = $(e.currentTarget).attr('data-tipo_id');
//    var path = urljs + "/Caja/ReimprimirRecibo";
//    var posting = $.post(path, { recibo: recibo, tipo: tipo});

//    posting.done(function (data, status, xhr) {
//        console.log(data);
//        if (data == 1) {
//            window.open('../PDF/pdf_prueba.pdf');
//        }
//    });

//}


function AnularRecibo(Documento) {
    try {
        var path = urljs + "/Caja/AnularRecibo";
        var posting = $.post(path, { Documento: Documento });
        posting.done(function (data, status, xhr) {

            dataResultado = [];
            dataResultado = data;
            alert(data.Mensaje);
        });
        posting.fail(function (data, status, xhr) {
            GenerarErrorAlerta(xhr, "error");
            goAlert();
        });
        posting.always(function (data, status, xhr) {
            $('.nav-tabs a[href="#ver_cita"]').closest('li').addClass('hide');
        });
    }
    catch (e) {
        //RemoveAnimation("body");

    }
    //location.reload();
}
