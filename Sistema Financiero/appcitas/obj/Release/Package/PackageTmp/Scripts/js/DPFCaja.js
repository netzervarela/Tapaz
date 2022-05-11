
var dataCitas = [];

$(document).ready(function () {
    //jQuery.ajaxSetup({ async: false });
    ////checkUserAccess('CAJ010');
    //jQuery.ajaxSetup({ async: true });

    GetRazones($('#hidden_tipo_id').val());
    GetTipoRazones();

    $('.monto').mask('#############', { translation: { '#': { pattern: /[0-9.]/, optional: true } } });
    $('.varchar100').mask('AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA', { translation: { 'A': { pattern: /[A-Za-z-,.: 0-9 ÁÉÍÓÚáéúíóúÑñÄËÏÖÜäëïöüÀÈÌÒÙàèìòùÃÕãõçÇÂâÊêÎîÔôÛû#]/, optional: true } } });


    //LimpiarTabla('#tableRazones');
    $('#btnGuardar').click(function () {
        //if ($('#hidden_id').val() == '' || $('#hidden_id').val() == null) {
        //    existeRazon($('#descripcion_tipo_razon').val(), $('#abreviatura').val());
        //}
        //else {
            GuardarRazon();
        //}
    });
});

$(function () {
    $('#fecha1').datetimepicker({
        locale: 'es',
        maxDate: hoy,
        defaultDate: hoy,
        format: 'YYYY-MM-DD',
        ignoreReadonly: true,
        icons: {
            time: "fa fa-clock-o",
            date: "fa fa-calendar",
            up: "fa fa-arrow-up",
            down: "fa fa-arrow-down",
            left: "fa fa-arrow-left",
            right: "fa fa-arrow-right"
        }
    });

    $("#fecha1").on("dp.change", function (e) {
        $('#fecha2').data("DateTimePicker").minDate(e.date);
    });
});

$("#btnGeneraReciboPagoPrestamo").on('click', function (e) {
    var recibo = $("#NumRecibo").val()
    GenerarSolicitud(recibo);
});

function GenerarReciboDPF(id) {

    try {
        var path = urljs + "/Caja/GetReciboDPF";
        var posting = $.post(path, { id: id });
        posting.done(function (data, status, xhr) {
            dataCitas = [];
            dataCitas = data;
            if (dataCitas.length > 0) {
                for (var i = dataCitas.length - 1; i >= 0; i--) {
                    CodigoDPF = dataCitas[i].DPF_Codigo
                    CodCliente = dataCitas[i].CodigoCliente
                    Cantidad = dataCitas[i].Cantidad
                    IdCliente = dataCitas[i].ClienteId
                    Fecha = dataCitas[i].Fecha
                    Deposito = dataCitas[i].NumDeposito
                    //$('#txt_Hora').val(data.CitaHora);
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

        var path = urljs + "/Caja/ReciboInteresDPF";
        var posting = $.post(path, {
            DPF_Codigo: CodigoDPF, CodigoCliente: CodCliente,
            Cantidad: Cantidad, ClienteId: IdCliente, Fecha: Fecha, NumDeposito: Deposito
        });

        posting.done(function (data, status, xhr) {
            console.log(data);
            if (data == 1) {
                window.open('/PDF/ReciboInteresesPF.pdf');
            }
        });

    }
    catch (e) {
        //RemoveAnimation("body");

    }
}

function GenerarSolicitud(recibo) {

    var path = urljs + "/Caja/ReciboPagoPrestamo";
    var posting = $.post(path, { recibo: recibo });

    posting.done(function (data, status, xhr) {
        console.log(data);
        if (data == 1) {
            window.open('../ReciboPagoPrestamo' + recibo + '.pdf');
        }
    });
    

    //var path = urljs + "/Prestamos/ReciboOtros";
    //var posting = $.post(path, { NumRecibo = $("#NumTrans").val() });
}

function ValidaCajero() {
    try {
        var path = urljs + "/Caja/ValidaCajero";
        var posting = $.post(path, {});
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

$('#btnEliminar').click(function () {
    var id = $('#hidden_id').val();
    var path = urljs + 'Razones/delete';
    var posting = $.post(path, { id: Number(id) });
    posting.done(function (data, status, xhr) {
        if (data.Accion > 0) {
            GetRazones($('#hidden_tipo_id').val());
            $('#modalEliminarRazon').modal('hide');
            GenerarSuccessAlerta(data.Mensaje, "success");
            goAlert();
            LimpiarInput(inputs, ['']);
        }
        else {
            GenerarErrorAlerta(data.Mensaje, "successModalEliminarPlantillas");
            goAlert();
        }
    });
    posting.fail(function (data, status, xhr) {
        //sendError(data);
        GenerarErrorAlerta(xhr, "error");
        goAlert();
    });
    posting.always(function () {
        /*console.log("Se cargo los proyectos correctamente");*/
        //RemoveAnimation("body");
    });
});

var inputs = ['#descripcion_tipo_razon', '#abreviatura', '#cod_tipo_razon'];
var client = 0;
function GetRazones(ClienteId, Nombre) {
    try {
        client = ClienteId;
        var path = urljs + "/Depositos/GetDepositos";
        var posting = $.post(path, { ClienteId: Number(ClienteId) });
        posting.done(function (data, status, xhr) {
            //console.log(data);
            LimpiarTabla("#tableRazones");
            if (data.length) {
                if (data[0].Accion > 0) {
                    $('.titulo').empty().append('Listado de Depositos a Plazo del cliente: ' + data[0].Nombre);
                    client = data[0].ClienteId;
                    for (var i = data.length - 1; i >= 0; i--) {
                        addRow(data[i], "#tableRazones");
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

function GetRazonBy(id, tipo_id) {
    try {
        /*animacion de loading*/
        //LoadAnimation("body");

        var path = urljs + "/Razones/GetOne";
        var posting = $.post(path, { id: Number(id), tipo_id: Number(tipo_id) });
        posting.done(function (data, status, xhr) {
            console.log(data);
            if (data.Accion > 0) {
                $('#descripcion_tipo_razon').val(data.RazonDescripcion);
                $('#abreviatura').val(data.RazonAbreviatura);
                $('#cod_tipo_razon').val(data.TipoId).trigger('change');
                $('#cod_tipo_razon').attr('disabled', 'disabled');
                $('#razongroup').val(data.RazonGroup).trigger('change');
                $('#razonstatus').val(data.RazonStatus);
            }
            else {
                GenerarErrorAlerta(data.Mensaje, "error");
                goAlert();
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
        ArrayData['DPF_Fecha_Apert'],
        ArrayData['DPF_Monto'],
        ArrayData['DPF_Plazo'],
        ArrayData['DPF_Tasa_interes'],
        ArrayData['DPF_Saldo'],
        ArrayData['DPF_Estado'],
        ArrayData['TDP_Descripcion'],
         "<button data-id='" + ArrayData['DPF_Codigo'] + "' title='Generar Pago' data-toggle='tooltip' onClick='GetDatosPagoIntDPF(" + ArrayData['DPF_Codigo'] + ")' id='btnGenerarPago" + ArrayData['DPF_Codigo'] + "' class='btn btn-primary text-center btn-sm'><i class='fa fa-pencil-square-o'></i></button>", +
        //"<button data-id='" + ArrayData['DPF_Codigo'] + "' data-tipo_id='" + ArrayData['DPF_Codigo'] + "' data-name='" + ArrayData['RazonDescripcion'] + "' title='Generar Certificado' data-toggle='tooltip' onClick='GeneraFormato(" + ArrayData['DPF_Codigo'] + ")' id='btngenerarcontrato" + ArrayData['DPF_Codigo'] + "' class='btn btn-success text-center btn-sm'><i class='glyphicon glyphicon-thumbs-up'></i></button>",
        //"<button data-id='" + ArrayData['PrestamoId'] + "' data-tipo_id='" + ArrayData['PrestamoId'] + "' data-name='" + ArrayData['RazonDescripcion'] + "' title='Generar Solicitud' data-toggle='tooltip' onClick='GenerarSolicitud(" + ArrayData['PrestamoId'] + ")' id='btngenerarcontrato" + ArrayData['PrestamoId'] + "' class='btn btn-info text-center btn-sm'><i class='fa fa-pencil-square-o'></i></button>" +
        //"<button data-id='" + ArrayData['PrestamoId'] + "' data-tipo_id='" + ArrayData['PrestamoId'] + "' data-name='" + ArrayData['RazonDescripcion'] + "' title='Generar Pagaré' data-toggle='tooltip' onClick='GeneraPagare(" + ArrayData['PrestamoId'] + ")' id='btngenerarPagare" + ArrayData['PrestamoId'] + "' class='btn btn-success text-center btn-sm'><i class='glyphicon glyphicon-thumbs-up'></i></button>",
        ////"<button data-id='" + ArrayData['PrestamoId'] + "' data-tipo_id='" + ArrayData['Nombre'] + "' data-name='" + ArrayData['RazonDescripcion'] + "' title='Estudio SocioEconomico' data-toggle='tooltip' onClick='GenerarEstudioSocioEconimico(" + ArrayData['PrestamoId'] + ")' id='btngenerarEstudio" + ArrayData['PrestamoId'] + "' class='btn btn-info text-center btn-sm'><i class='fa fa-pencil-square-o'></i></button>",
        //"<button data-id='" + ArrayData['RazonId'] + "' data-tipo_id='" + ArrayData['TipoId'] + "' data-name='" + ArrayData['RazonDescripcion'] + "' title='Eliminar Préstamo' data-toggle='tooltip' onClick='EliminarRazon(event)' id='btnEliminarRazon" + ArrayData['RazonId'] + "' class='btn btn-danger botonVED text-center btn-sm'><i class='fa fa-trash-o'></i></button>",
        ArrayData['ClienteId']
    ]);

    var theNode = $(tableID).dataTable().fnSettings().aoData[newRow[0]].nTr;
    theNode.setAttribute('id', ArrayData['ClienteId']);
    $('td', theNode)[8].setAttribute('class', 'RazonId hidden');
}
function EditarRazon(e) {
    e.stopPropagation();

    var id = $(e.currentTarget).attr('data-id');
    var tipo_id = $(e.currentTarget).attr('data-tipo_id');
    var desc = $(e.currentTarget).attr('data-name');

    $("#theHeader").html("Editar | " + desc);
    $('#moda_razones').modal('show');
    $('#hidden_id').val(id);
    jQuery.ajaxSetup({ async: false });
    var infojson = jQuery.parseJSON('{"input": "#razongroup", ' +
        '"url": "configuracion/getParametosByIdEncabezado/", ' +
        '"val": "ConfigItemID", ' +
        '"type": "GET", ' +
        '"data": "GRPR", ' +
        '"text": "ConfigItemDescripcion"}');
    console.log(infojson);
    $('#cod_tipo_razon').val($('#hidden_tipo_id').val()).trigger('change');
    LoadComboBox(infojson);
    GetRazonBy(id, tipo_id);
    jQuery.ajaxSetup({ async: true });
}



function GetTipoRazones() {
    try {
        /*animacion de loading*/
        //LoadAnimation("body");

        var path = urljs + "/TipoRazones/GetAll";
        var posting = $.post(path, { param1: 'value1' });
        posting.done(function (data, status, xhr) {
            //console.log(data);
            LimpiarTabla("#tableTipoRazones");
            if (data.length) {
                if (data[0].Accion > 0) {
                    $('#cod_tipo_razon').empty();
                    var contador = 0;
                    for (var i = data.length - 1; i >= 0; i--) {
                        $('#cod_tipo_razon').append(new Option(data[contador].TipoAbreviatura, data[contador].TipoId));
                        contador++;
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

function NuevaRazon(e) {
    e.stopPropagation();
    var id = -1;
    $("#theHeader").html("Nuevo Préstamo");
    $('#hidden_id').val("");
    $("#moda_razones").find('input[type = "text"]').val("");
    $('#descripcion_tipo_razon').val('');
    var infojson = jQuery.parseJSON('{"input": "#razongroup", ' +
        '"url": "configuracion/getParametosByIdEncabezado/", ' +
        '"val": "ConfigItemID", ' +
        '"type": "GET", ' +
        '"data": "GRPR", ' +
        '"text": "ConfigItemDescripcion"}');
    console.log(infojson);
    $('#cod_tipo_razon').val($('#hidden_tipo_id').val()).trigger('change');
    LoadComboBox(infojson);
    $('#razonstatus').val('ACT');
    $('#moda_razones').modal('show');
}
function GuardarRazon() {
    try {
        var inputs = [];
        var mensaje = [];


        /*Recorremos el contenedor para obtener los valores*/
        $('.requerido').each(function () {
            /*Llenamos los arreglos con la info para la validacion*/
            inputs.push('#' + $(this).attr('id'));
            mensaje.push($(this).attr('attr-message'));

            /*Creamos el json*/
            /*var json = {
                id : $(this).attr('id'),
                val : $(this).val()
            };*/

            /*data.push(json);*/
        });
        /*Si la validación es correcta ejecuta el ajax*/
        if (Validar(inputs, mensaje)) {

            var path = urljs + 'Prestamos/SaveData';
            var id = $('#hidden_id').val();
            console.log('id: ' + $('#hidden_id').val());

            if (id == "") {
                id = -1;
            }
            //JSON data
            var dataType = 'application/json; charset=utf-8';
            var data = {
                //ClienteId: $('#cod_tipo_razon option:selected').val(),
                ClienteId: client,
                //RazonId: id,
                //PrestamoId = client,
                
                MontoSolicitado: $('#montosolicitado').val(),
                PlazoMeses: $('#plazo').val(),
                Garantia: $('#garantia option:selected').val(),
                Interes: $('#tasainteres').val(),
                Destino: $('#destino option:selected').val(),
                FrecPago: $('#frecuenciapago option:selected').val(),
                Observaciones: $('#observaciones').val()
                //Interes: $('#razonstatus').val()
            }


            var posting = $.post(path, data);

            posting.done(function (data, status, xhr) {
                console.log(data);
                $('#moda_razones').modal('hide');
                GenerarSuccessAlerta(data.Mensaje, "success");
                goAlert();
                LimpiarInput(inputs, ['']);
                GetRazones($('#hidden_tipo_id').val());
            });

            posting.fail(function (data, status, xhr) {
                console.log(status);
            });


        }
    } catch (e) {
        console.log(e);
    }
}


//function GenerarPago(id) {

//    //e.stopPropagation();
//    $('#theHeader').html("Pago a Prestamo");
//    dataCitas = [];
//    dataCitas = data;
//    //$('#hidden_id').val("");
//    if (dataCitas.length > 0) {
//        for (var i = dataCitas.length - 1; i >= 0; i--) {
//            //if (dataCitas[i].PRES_Codigo == id) {
//                $('#Capital').val(dataCitas[i].Capital);
//                $('#Intereses').val(dataCitas[i].Intereses);
//                $('#Mora').val(dataCitas[i].Mora);
//                $('#Total').val(dataCitas[i].Total);
//                $('#Saldo').val(dataCitas[i].Saldo);
//                //$('#txt_segmento').val(dataCitas[i].PRES_Estado);
//                //$('#txt_monto_sol').val(dataCitas[i].PRES_mto_Solicitado);
//                //$('#txt_fecha').val(dataCitas[i].PRES_Fecha_Solicitud);
//           // }
//        }
//        //activarTab('ver_cita');
//    }
//    $('#moda_razones').modal('show');
//}

function GetDatosPagoIntDPF(id) {
    $("#btnGeneraReciboPagoIntDPF").addClass("disabled");
    //$("#Mora").attr("disabled");
    //$("#Mora").addClass("disabled");
    $('#theHeader').html("Pago de Interes DPF");
    try {
        var path = urljs + "/Caja/GetPagoIntDPF";
        var posting = $.post(path, { id: id });
        posting.done(function (data, status, xhr) {
            dataCitas = [];
            dataCitas = data;
            if (dataCitas.length > 0) {
                for (var i = dataCitas.length - 1; i >= 0; i--) {
                //$('#Capital').val(dataCitas[i].Capital);
                $('#Intereses').val(dataCitas[i].Intereses);
                //$('#Mora').val(dataCitas[i].Mora);
                //$('#Total').val(parseFloat($('#Capital').val()) + parseFloat($('#Intereses').val()) + parseFloat($('#Mora').val()));;
                $('#Saldo').val(dataCitas[i].Saldo);
                $('#CodigoDPF').val(dataCitas[i].CodigoDPF);
                    //$('#txt_Hora').val(data.CitaHora);
                }
            }
            $('#moda_razones').modal('show');

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

//$('#Capital').change(function (e) {
//    $('#Total').val(parseFloat($('#Capital').val()) +
//                    parseFloat($('#Intereses').val()) +
//                    parseFloat($('#Mora').val()));
//});

$('#Intereses').change(function (e) {
    $('#Total').val(parseFloat($('#Capital').val()) +
                    parseFloat($('#Intereses').val()) +
                    parseFloat($('#Mora').val()));
});

//$('#Mora').change(function (e) {
//    $('#Total').val(parseFloat($('#Capital').val()) +
//                    parseFloat($('#Intereses').val()) +
//                    parseFloat($('#Mora').val()));
//});




var bandera = 0;
$("#btnAjusteDPF").on('click', function (e) {
    if (bandera == 0) {
        $("#FechaAj").removeClass("hidden");
        $("#txtAjuste").removeClass("hidden");
        bandera = 1
    }
    else {
        $("#FechaAj").addClass("hidden");
        $("#txtAjuste").addClass("hidden");
        bandera = 0
    }
});

$("#btnGeneraPagoIntDPF").on('click', function (e) {

    //var ajuste = 1
    var Fecha = $("#txt_fecha1").val();
    if (bandera == 1) {
        var path = urljs + "/Caja/ValidaCajeroAjuste";
        var posting = $.post(path, { Fecha: Fecha});
        posting.done(function (data, status, xhr){

        dataResultado = [];
        dataResultado = data;
        if ((data.Estado) != 1 && (data.Estado) != 2) {
        GenerarAjusteDPF($("#CodigoDPF").val(),$("#Intereses").val(), $("#Observacion").val(), $("#txt_fecha1").val());
        //$("#btnGeneraPagoPrestamo").addClass("disabled");
        //$("#btnGeneraReciboPagoPrestamo").removeClass("disabled");
        }
        else {
            alert(data.Mensaje);
        }
        });
    }
    else{
        var path = urljs + "/Caja/ValidaCajero";
        var posting = $.post(path, {});
        posting.done(function (data, status, xhr) {

            dataResultado = [];
            dataResultado = data;
            //alert(data.Mensaje);
            if ((data.Estado) != 1 && (data.Estado) != 2) {
                GenerarPagoIntDPF($("#CodigoDPF").val(),$("#Intereses").val(), $("#Observacion").val());
                //$("#btnGeneraPagoPrestamo").addClass("disabled");
                //$("#btnGeneraReciboPagoPrestamo").removeClass("disabled");
            }
            else {
                alert(data.Mensaje);
            }
        });
    }
});

$("#btnGeneraReciboPagoIntDPF").on('click', function (e) {

    GenerarReciboDPF($("#CodigoDPF").val());
    
});

function GenerarReciboDPF(id) {

    try {
        var path = urljs + "/Caja/GetReciboDPF";
        var posting = $.post(path, { id: id });
        posting.done(function (data, status, xhr) {
            dataCitas = [];
            dataCitas = data;
            if (dataCitas.length > 0) {
                for (var i = dataCitas.length - 1; i >= 0; i--) {
                    CodigoDPF = dataCitas[i].CodigoDPF
                    CodCliente = dataCitas[i].CodigoCliente
                    Cantidad = dataCitas[i].Cantidad
                    IdCliente = dataCitas[i].ClienteId
                    Fecha = dataCitas[i].Fecha
                    Deposito = dataCitas[i].NumDeposito
                    ImprimirReciboDPF(CodigoDPF, CodCliente, Cantidad, IdCliente, Fecha, Deposito)
                    //$('#txt_Hora').val(data.CitaHora);
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

function ImprimirReciboDPF(CodigoDPF, CodCliente, Cantidad, IdCliente, Fecha, Deposito) {

    try {
        var path = urljs + "/Caja/ReciboInteresDPF";
        var posting = $.post(path, {
            CodigoDPF: CodigoDPF, CodigoCliente: CodCliente,
            Cantidad: Cantidad, ClienteId: IdCliente, Fecha: Fecha, NumDeposito: Deposito
        });

        posting.done(function (data, status, xhr) {
            console.log(data);
            if (data == 1) {
                window.open('/Formatos/RECIBOS_DE_INTERESES_PF_1.pdf');
            }
        });

    }
    catch (e) {
        //RemoveAnimation("body");

    }
}

function GenerarPagoIntDPF(codigo,Intereses,Observacion) {
    try {
        var path = urljs + "/Caja/PagoIntDPF";
        var posting = $.post(path, { codigo: codigo, Intereses: Intereses, Observacion: Observacion });
        posting.done(function (data, status, xhr) {

            dataResultado = [];
            dataResultado = data;
            $("#NumRecibo").val(data.Num);
            if (data.Num != -2)
                {
                $("#btnGeneraPagoIntDPF").addClass("disabled");
                $("#btnGeneraReciboPagoIntDPF").removeClass("disabled");
                }
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

function GenerarAjusteDPF(codigo,Intereses,Observacion, FechaAjusteDPF) {
    try {
        var path = urljs + "/Caja/PagoIntDPFAjuste";
        var posting = $.post(path, { codigo: codigo,Intereses: Intereses, Observacion: Observacion, FechaAjusteDPF: FechaAjusteDPF });
        posting.done(function (data, status, xhr) {

            dataResultado = [];
            dataResultado = data;
            $("#NumRecibo").val(data.Num);
            if (data.Num != -2)
                {
                $("#btnGeneraPagoIntDPF").addClass("disabled");
                $("#btnGeneraReciboPagoIntDPF").removeClass("disabled");
                }
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

$('#btnModificaMora').click(function (e) {
    //var path = urljs + "/Caja/ValidaCajero";
    //var posting = $.post(path, {});
    //posting.done(function (data, status, xhr) {

    //    dataResultado = [];
    //    dataResultado = data;
    //    e.stopPropagation();
    //    var id = -1;
    $("#theHeader").html("Autorización para modificar mora");
    $('#hidden_id').val("");
    $('#user').val("");
    $('#password').val("");
    $("#Moda_Autorizacion_Mora").find('input[type = "text"]').val("");
    $('#descripcion_tipo_razon').val('');
    $('#tiene_listado_x').val(0);
    $('#tipostatus').val('ACT');
    $('#tiene_listado_x').trigger('change');
    $('#Moda_Autorizacion_Mora').modal('show');
    //$("#btnGeneraPago").removeClass("disabled");
    //$("#btnGeneraRecibo").addClass("disabled");
    //});
});

$('#AutorizaModMora').click(function (e) {
    var user = $("#user").val();
    var pass = $("#password").val();
    var CodigoModulo = "SGRC";
    if (user != "" && pass != "") {
        try {
            var path = urljs + "/Caja/AutorizaModMora";
            var posting = $.post(path, { user: user, pass: pass, CodigoModulo: CodigoModulo });
            posting.done(function (data, status, xhr) {
                //console.log(data);
                dataResultado = [];
                dataResultado = data;
                if (data.Estado == 1) {
                    alert(data.Mensaje);
                    $("#Mora").removeAttr("disabled");
                    //CierreCaja($("#CA_Secuencia").val(), Diferencia)
                    $('#Moda_Autorizacion_Mora').modal('hide');
                } else {
                    alert(data.Mensaje);
                }

                //alert(data.Mensaje);
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
        //AutorizaCierre(user, pass, "SGRC");
    }
    else {
        $("#user").focus();
    }
});