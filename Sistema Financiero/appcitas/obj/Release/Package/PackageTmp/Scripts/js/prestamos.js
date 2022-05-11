$(document).ready(function () {
    jQuery.ajaxSetup({ async: false });
    //checkUserAccess('CONF050');
    jQuery.ajaxSetup({ async: true });

    GetRazones($('#hidden_tipo_id').val());
    //GetTipoRazones();

    $('.identidad').mask('####-####-#####', { translation: { '#': { pattern: /[0-9]/, optional: true } } });
    $('.rtn').mask('####-####-######', { translation: { '#': { pattern: /[0-9]/, optional: true } } });
    $('.numero').mask('####', { translation: { '#': { pattern: /[0-9]/, optional: true } } });
    $('.correo').mask('AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA', { translation: { 'A': { pattern: /[a-z-_.0-9 @]/, optional: true } } });
    $('.telefonoCelular').mask('AAAA-AAAA', { translation: { 'A': { pattern: /[0-9]/, optional: true } } });
    $('.monto').mask('#############', { translation: { '#': { pattern: /[0-9.]/, optional: true } } });
    $('.fecha').mask('##########', { translation: { '#': { pattern: /[0-9-]/, optional: true } } });
    $('.varchar50').mask('AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA', { translation: { 'A': { pattern: /[A-Za-z ÁÉÍÓÚáéúíóúÑñÄËÏÖÜäëïöüÀÈÌÒÙàèìòùÃÕãõçÇÂâÊêÎîÔôÛû]/, optional: true } } });
    $('.varchar200').mask('AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA', { translation: { 'A': { pattern: /[A-Z a-z-,.: 0-9 ÁÉÍÓÚáéúíóúÑñÄËÏÖÜäëïöüÀÈÌÒÙàèìòùÃÕãõçÇÂâÊêÎîÔôÛû#]/, optional: true } } });


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

var inputsPrestamo = ['#montosolicitado', '#plazo', '#garantia', '#tasainteres', '#destino', '#frecuenciapago', '#TipoCuota', '#observaciones', '#Personas', '#NegocioPropio', '#Salario', '#Finca', '#Otros', '#Renta', '#Servicios', '#Prestamos', '#Transporte',
    '#Alimentacion', '#Vestuario', '#Otros1', '#Observaciones1' ];

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
var Nombre 
function GetRazones(ClienteId, NomPrestamo) {
    try {
        client = ClienteId;
        var path = urljs + "/Prestamos/GetAll";
        var posting = $.post(path, { ClienteId: Number(ClienteId) });
        posting.done(function (data, status, xhr) {
            //console.log(data);
            LimpiarTabla("#tableRazones");
            if (data.length) {
                if (data[0].Accion > 0) {
                    $('.titulo').empty().append('Listado de préstamos del cliente: ' + data[0].NomPrestamo);
                    Nombre = data[0].NomPrestamo;
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

function GeneraPdfContratoPrestamo(PrestamoId, identidad, Nombre, CLI_Direccion, Fec,
    PRES_Plazo_Meses, PRES_Plazo_Meses, NumCuotas,
    PRES_Porc_Interes, DES_Descripcion, GAR_Descripcion,
    Monto) {
    var path = urljs + "/Prestamos/GeneraPdfContratoPrestamo";
    var posting = $.post(path, {
        IdPres: PrestamoId,
        identidad: identidad,
        Nombre: Nombre,
        CLI_Direccion: CLI_Direccion,
        Monto: Monto,
        Fec: Fec,
        PRES_Plazo_Meses: PRES_Plazo_Meses,
        NumCuotas: NumCuotas,
        PRES_Porc_Interes: PRES_Porc_Interes,
        DES_Descripcion: DES_Descripcion,
        GAR_Descripcion: GAR_Descripcion

    });

    posting.done(function (data, status, xhr) {
        console.log(data);
        if (data == 1) {
            window.open('/PDF/ContratoPrestamo1.pdf');
        }
    });

}

function GeneraContratoPrestamo(PrestamoId) {

    try {
        var path = urljs + "/Prestamos/GeneraContratoPrestamo";
        var posting = $.post(path, { PrestamoId: PrestamoId });
        posting.done(function (data, status, xhr) {
            dataCitas = [];
            dataCitas = data;
            if (dataCitas.length > 0) {
                for (var i = dataCitas.length - 1; i >= 0; i--) {
                    identidad = dataCitas[i].identidad
                    Nombre = dataCitas[i].Nombre
                    CLI_Direccion = dataCitas[i].CLI_Direccion
                    Fec = dataCitas[i].Fec
                    PRES_Plazo_Meses = dataCitas[i].PRES_Plazo_Meses
                    NumCuotas = dataCitas[i].NumCuotas
                    PRES_Porc_Interes = dataCitas[i].PRES_Porc_Interes
                    DES_Descripcion = dataCitas[i].DES_Descripcion
                    GAR_Descripcion = dataCitas[i].GAR_Descripcion
                    Monto = dataCitas[i].Monto
                    GeneraPdfContratoPrestamo(PrestamoId, identidad, Nombre, CLI_Direccion, Fec,
                        PRES_Plazo_Meses, PRES_Plazo_Meses, NumCuotas,
                        PRES_Porc_Interes, DES_Descripcion, GAR_Descripcion,
                        Monto)
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
    var accionesHTML = '';
    var newRow = $(tableID).dataTable().fnAddData([
        ArrayData['FecSolicitud'] = moment((ArrayData['FecSolicitud'])).format('YYYY-MM-DD'),
        ArrayData['MontoSolicitado'],
        ArrayData['MontoAprobado'],
        ArrayData['PlazoMeses'],
        ArrayData['Interes'],
        ArrayData['Saldo'],
        ArrayData['Estado'],
        ArrayData['Garantia'],
         "<button data-id='" + ArrayData['PrestamoId'] + "' title='Ver Préstamo' data-toggle='tooltip' onClick='GetDatosPrestamos(" + ArrayData['PrestamoId'] + ")' id='btnGenerarPago" + ArrayData['PrestamoId'] + "' class='btn btn-primary text-center btn-sm'><i class='fa fa-eye'></i></button>" +
         "<button data-id='" + ArrayData['PrestamoId'] + "' data-tipo_id='" + ArrayData['PrestamoId'] + "' data-name='" + ArrayData['RazonDescripcion'] + "' title='Generar Contrato' data-toggle='tooltip' onClick='GeneraContratoPrestamo(" + ArrayData['PrestamoId'] + ")' id='btngenerarcontrato" + ArrayData['PrestamoId'] + "' class='btn btn-success text-center btn-sm'><i class='fa fa-file-text'></i></button>" +
        "<button data-id='" + ArrayData['PrestamoId'] + "' data-tipo_id='" + ArrayData['PrestamoId'] + "' data-name='" + ArrayData['RazonDescripcion'] + "' title='Generar Solicitud' data-toggle='tooltip' onClick='GenerarSolicitud(" + ArrayData['PrestamoId'] + ")' id='btngenerarcontrato" + ArrayData['PrestamoId'] + "' class='btn btn-info text-center btn-sm'><i class='fa fa-pencil-square-o'></i></button>" +
         "<button data-id='" + ArrayData['PrestamoId'] + "' data-tipo_id='" + ArrayData['PrestamoId'] + "' data-name='" + ArrayData['RazonDescripcion'] + "' title='Generar Pagaré' data-toggle='tooltip' onClick='GeneraPagare(" + ArrayData['PrestamoId'] + ")' id='btngenerarPagare" + ArrayData['PrestamoId'] + "' class='btn btn-danger text-center btn-sm'><i class='fa fa-pencil'></i></button>" +
         "<button data-id='" + ArrayData['PrestamoId'] + "' data-tipo_id='" + ArrayData['PrestamoId'] + "' data-name='" + ArrayData['RazonDescripcion'] + "' title='Estudio SocioEconomico' data-toggle='tooltip' onClick='GenerarEstudioSocioEconimico(" + ArrayData['PrestamoId'] + ")' id='btngenerarEstudio" + ArrayData['PrestamoId'] + "' class='btn btn-info text-center btn-sm'><i class='fa fa-pencil-square-o'></i></button>" +
        '<div class="btn-group text-center">' +
        '<button type="button" class="btn btn-warning dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><i class="fa fa-bars" aria-hidden="true"></i></button>' +
        '<ul class="dropdown-menu centrar-menu text-left">' +
        "<li>" +
         "<a  href='#' data-id='" + ArrayData['PrestamoId'] + "' " +
        "title='1er nota de crédito' " +
        "data-toggle='tooltip' " +
        "data-placement='left' " +
         "onClick='GetDatosPagoPrestamo1(" + ArrayData['PrestamoId'] + ")'>" +
        "<i class='fa fa-file' style='color:yellow'></i> 1er nota de crédito" +
        "</a>" +
        "</li>" +
        "<li>" +
         "<a  href='#' data-id='" + ArrayData['PrestamoId'] + "' " +
        "title='2da nota de crédito' " +
        "data-toggle='tooltip' " +
        "data-placement='left' " +
         "onClick='GetDatosPagoPrestamo2(" + ArrayData['PrestamoId'] + ")'>" +
        "<i class='fa fa-file' style='color:orange'></i> 2da nota de crédito" +
        "</a>" +
         "</li>" +
         "<li>" +
         "<a  href='#' data-id='" + ArrayData['PrestamoId'] + "' " +
         "title='3er nota de crédito' " +
         "data-toggle='tooltip' " +
         "data-placement='left' " +
         "onClick='GetDatosPagoPrestamo3(" + ArrayData['PrestamoId'] + ")'>" +
         "<i class='fa fa-file' style='color:red'></i> 3er nota de crédito" +
         "</a>" +
         "</li>" +
        '</ul>' +
        '</div>',
        //"<button data-id='" + ArrayData['PrestamoId'] + "' data-tipo_id='" + ArrayData['Nombre'] + "' data-name='" + ArrayData['RazonDescripcion'] + "' title='Estudio SocioEconomico' data-toggle='tooltip' onClick='GenerarEstudioSocioEconimico(" + ArrayData['PrestamoId'] + ")' id='btngenerarEstudio" + ArrayData['PrestamoId'] + "' class='btn btn-info text-center btn-sm'><i class='fa fa-pencil-square-o'></i></button>",
        //"<button data-id='" + ArrayData['RazonId'] + "' data-tipo_id='" + ArrayData['TipoId'] + "' data-name='" + ArrayData['RazonDescripcion'] + "' title='Eliminar Préstamo' data-toggle='tooltip' onClick='EliminarRazon(event)' id='btnEliminarRazon" + ArrayData['RazonId'] + "' class='btn btn-danger botonVED text-center btn-sm'><i class='fa fa-trash-o'></i></button>",
        ArrayData['ClienteId']
    ]);

    var theNode = $(tableID).dataTable().fnSettings().aoData[newRow[0]].nTr;
    theNode.setAttribute('id', ArrayData['ClienteId']);
    $('td', theNode)[9].setAttribute('class', 'RazonId hidden');
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            
function GetDatosPagoPrestamo1(id) {

    try {
        var path = urljs + "/Caja/GetDatosPagoPrestamo";
        var posting = $.post(path, { id: id });
        posting.done(function (data, status, xhr) {
            dataCitas = [];
            dataCitas = data;
            if (dataCitas.length > 0) {
                for (var i = dataCitas.length - 1; i >= 0; i--) {
                    Nombre = dataCitas[i].Nombre
                    monto = parseFloat(dataCitas[i].Capital) + parseFloat(dataCitas[i].Intereses) + parseFloat(dataCitas[i].Mora)
                    GetRQ1(Nombre, monto)
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

function GetRQ1(Nombre, monto) {

    try {

        var pathpdf = urljs + "/Caja/RQ1";
        var postingpdf = $.post(pathpdf, { Nombre: Nombre, monto: monto });

        postingpdf.done(function (data, status, xhr) {
            console.log(data);
            if (data == 1) {
                window.open('/PDF/RQ1_Copia.pdf');
            }
        });

    }
    catch (e) {
        //RemoveAnimation("body");

    }
}

function GetDatosPagoPrestamo2(id) {

    try {
        var path = urljs + "/Caja/GetDatosPagoPrestamo";
        var posting = $.post(path, { id: id });
        posting.done(function (data, status, xhr) {
            dataCitas = [];
            dataCitas = data;
            if (dataCitas.length > 0) {
                for (var i = dataCitas.length - 1; i >= 0; i--) {
                    Nombre = dataCitas[i].Nombre
                    monto = parseFloat(dataCitas[i].Capital) + parseFloat(dataCitas[i].Intereses) + parseFloat(dataCitas[i].Mora)
                    GetRQ2(Nombre, monto)
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

function GetRQ2(Nombre, monto) {

    try {

        var path = urljs + "/Caja/RQ2";
        var posting = $.post(path, { Nombre: Nombre, monto: monto });

        posting.done(function (data, status, xhr) {
            console.log(data);
            if (data == 1) {
                window.open('/PDF/RQ2_Copia.pdf');
            }
        });

    }
    catch (e) {
        //RemoveAnimation("body");

    }
}

function GetDatosPagoPrestamo3(id) {

    try {
        var path = urljs + "/Caja/GetDatosPagoPrestamo";
        var posting = $.post(path, { id: id });
        posting.done(function (data, status, xhr) {
            dataCitas = [];
            dataCitas = data;
            if (dataCitas.length > 0) {
                for (var i = dataCitas.length - 1; i >= 0; i--) {
                    Nombre = dataCitas[i].Nombre
                    monto = parseFloat(dataCitas[i].Capital) + parseFloat(dataCitas[i].Intereses) + parseFloat(dataCitas[i].Mora)
                    GetRQ3(Nombre, monto)
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

function GetRQ3(Nombre, monto) {

    try {

        var path = urljs + "/Caja/RQ3";
        var posting = $.post(path, { Nombre: Nombre, monto: monto });

        posting.done(function (data, status, xhr) {
            console.log(data);
            if (data == 1) {
                window.open('/PDF/RQ3_Copia.pdf');
            }
        });

    }
    catch (e) {
        //RemoveAnimation("body");

    }
}

//GENERA CONTRATO DE PRESTAMO
function GeneraFormato(PrestamoId) {

    var path = urljs + "/Prestamos/GeneraFormato";
    var posting = $.post(path, { PrestamoId: PrestamoId });

    posting.done(function (data, status, xhr) {
        console.log(data);
        if (data == 1) {
            window.open('../ContratoPrestamo' + PrestamoId + '.pdf');
        }
    });
    
}

//GENERA SOLICITUD DE PRESTAMO
function GenerarSolicitud(PrestamoId) {

    var path = urljs + "/Prestamos/SolicitudCredito";
    var posting = $.post(path, { PrestamoId: PrestamoId });

    posting.done(function (data, status, xhr) {
        console.log(data);
        if (data == 1) {
            window.open('../SolicitudCredito' + PrestamoId + '.pdf');
        }
    });
    
}

//GENERA PAGARE
function GeneraPagare(PrestamoId) {

    var path = urljs + "/Prestamos/GeneraPagare";
    var posting = $.post(path, { PrestamoId: PrestamoId });

    posting.done(function (data, status, xhr) {
        console.log(data);
        if (data == 1) {
            window.open('../Pagare' + PrestamoId + '.pdf');
        }
    });
   
}

////GENERA ESTUDIO SOCIOECONOMICO
//function GenerarEstudioSocioEconimico(PrestamoId) {

//    var path = urljs + "/Prestamos/EstudioSocioEconomico";
//    var posting = $.post(path, { PrestamoId: PrestamoId });

//    posting.done(function (data, status, xhr) {
//        console.log(data);
//        if (data == 1) {
//            window.open('../SocioEconomico' + PrestamoId + '.pdf');
//        }
//    });
    
//}

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

function EliminarRazon(e) {
    e.stopPropagation();
    //console.log('Cancel ticket');
    var id = $(e.currentTarget).attr('data-id');
    var desc = $(e.currentTarget).attr('data-name');
    $('#hidden_id').val(id);

    $("#theHeaderEliminar").html("Eliminar Razon | " + desc);
    $('#modalEliminarRazon').modal('show');
    $('#modalmessage').html('¿Desea eliminar la razon: <b>' + desc + '</b>?');
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

    $("#btnGuardar").removeClass('hidden');

    $('#Codigo').attr("disabled", false);
    $('#montosolicitado').attr("disabled", false);
    $('#plazo').attr("disabled", false);
    $('#garantia').attr("disabled", false);
    $('#tasainteres').attr("disabled", false);
    $('#destino').attr("disabled", false);
    $('#frecuenciapago').attr("disabled", false);
    $('#TipoCuota').attr("disabled", false);
    $('#observaciones').attr("disabled", false);

    //DATOS DEL ESTUDIO SOCIOECONOMICO
    $('#Personas').attr("disabled", false);
    $('#NegocioPropio').attr("disabled", false);
    $('#Salario').attr("disabled", false);
    $('#Finca').attr("disabled", false);
    $('#Otros').attr("disabled", false);
    $('#Renta').attr("disabled", false);
    $('#Servicios').attr("disabled", false);
    $('#Prestamos').attr("disabled", false);
    $('#Transporte').attr("disabled", false);
    $('#Alimentacion').attr("disabled", false);
    $('#Vestuario').attr("disabled", false);
    $('#Otros1').attr("disabled", false);
    $('#Observaciones1').attr("disabled", false);

    e.stopPropagation();
    var id = -1;
    $("#theHeader").html("Nuevo Préstamo");
    $('#hidden_id').val("");
    $("#moda_razones").find('input[type = "text"]').val("");
    $('#descripcion_tipo_razon').val('');
    $('#moda_razones').modal('show');
}


//INGRESAR NUEVO PRESTAMO
function GuardarRazon() {
    try {
        var inputs = [];
        var mensaje = [];

        /*Recorremos el contenedor para obtener los valores*/
        $('.requerido').each(function () {
            /*Llenamos los arreglos con la info para la validacion*/
            inputs.push('#' + $(this).attr('id'));
            mensaje.push($(this).attr('attr-message'));
        });

        //DECLARACION DE VARIABLES, CONTIENE EL VALOR DEL CAMPO
        var obser = $('#observaciones').val();
        var TipoCuota = $('#TipoCuota').val();
        var obser1 = $('#Observaciones1').val();
        var personas = $('#Personas').val();
        var negocio = $('#NegocioPropio').val();
        var salario = $('#Salario').val();
        var finca = $('#Finca').val();
        var otrosIngresos = $('#Otros').val();
        var renta = $('#Renta').val();
        var servicios = $('#Servicios').val();
        var prestamos = $('#Prestamos').val();
        var transporte = $('#Transporte').val();
        var alimentacion = $('#Alimentacion').val();
        var vestuario = $('#Vestuario').val();
        var otrosEgresos = $('#Otros1').val();

        var NomAval1 = $('#NomAval1').val();
        var IdAval1 = $('#IdAval1').val();
        var CelAval1 = $('#CelAval1').val();
        var TelAval1 = $('#TelAval1').val();
        var DirAval1 = $('#DirAval1').val();
        var NumAval1 = $('#NumAval1').val();

        var NomAval2 = $('#NomAval2').val();
        var IdAval2 = $('#IdAval2').val();
        var CelAval2 = $('#CelAval2').val();
        var TelAval2 = $('#TelAval2').val();
        var DirAval2 = $('#DirAval2').val();
        var NumAval2 = $('#NumAval2').val();

        //VALIDACION DE CAMPOS QUE NO SON OBLIGATORIOS Y QUE VENGAN VACIOS, SI VIENE VACIO SE ASIGNAN VALORES
        if (obser == "" ) {
            obser = "Sin Observaciones";
        }
        if (obser1 == "" ) {
            obser1 = "Sin Observaciones";
        }
        if (personas == "" ) {
            personas = 0;
        }
        if (negocio == "") {
            negocio = 0;
        }
        if (salario == "") {
            salario = 0;
        }
        if (finca == "") {
            finca = 0;
        }
        if (otrosIngresos == "") {
            otrosIngresos = 0;
        }
        if (renta == "") {
            renta = 0;
        }
        if (servicios == "") {
            servicios = 0;
        }
        if (prestamos == "") {
            prestamos = 0;
        }
        if (transporte == "") {
            transporte = 0;
        }
        if (alimentacion == "") {
            alimentacion = 0;
        }
        if (vestuario == "") {
            vestuario = 0;
        }
        if (otrosEgresos == "") {
            otrosEgresos = 0;
        }
        
        //AVALES

        if (NomAval1 == "") {
            NomAval1 = "N";
        }
        if (IdAval1 == "") {
            IdAval1 = "N";
        }
        if (CelAval1 == "") {
            CelAval1 = "N";
        }
        if (TelAval1 == "") {
            TelAval1 = "N";
        }
        if (DirAval1 == "") {
            DirAval1 = "N";
        }
        

        if (NomAval2 == "") {
            NomAval2 = "N";
        }
        if (IdAval2 == "") {
            IdAval2 = "N";
        }
        if (CelAval2 == "") {
            CelAval2 = "N";
        }
        if (TelAval2 == "") {
            TelAval2 = "N";
        }
        if (DirAval2 == "") {
            DirAval2 = "N";
        }
       

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

                // DATOS PRESTAMO
                ClienteId: client,
                MontoSolicitado: $('#montosolicitado').val(),
                PlazoMeses: $('#plazo').val(),
                Garantia: $('#garantia option:selected').val(),
                Interes: $('#tasainteres').val(),
                Destino: $('#destino option:selected').val(),
                FrecPago: $('#frecuenciapago option:selected').val(),
                TipoCuota: $('#TipoCuota option:selected').val(),
                Observaciones: obser,

                // DATOS ESTDIO SOCIOECONOMICO 
                Personas: personas,
                NegocioPropio: negocio,
                Salario: salario,
                Finca: finca,
                Otros: otrosIngresos,
                Renta: renta,
                ServicioPublico: servicios,
                Prestamo: prestamos,
                Transporte: transporte,
                Alimentacion: alimentacion,
                Vestuario: vestuario,
                Otros1: otrosEgresos,
                Observaciones1: obser1,

                //AVALES
                NomAval1: NomAval1,
                IdAval1: IdAval1,
                CelAval1: CelAval1,
                TelAval1: TelAval1,
                DirAval1: DirAval1,
                NumAval1: NumAval1,

                NomAval2: NomAval2,
                IdAval2: IdAval2,
                CelAval2: CelAval2,
                TelAval2: TelAval2,
                DirAval2: DirAval2,
                NumAval2: NumAval2,
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

//TOTAL INGRESOS ESTUDIO SOCIOECONOMICO
$('input').change(function (e) {

    var NegocioPropio = parseInt($('#NegocioPropio').val());
    var Salario = parseInt($('#Salario').val());
    var Finca = parseInt($('#Finca').val());
    var OtrosIngresos = parseInt($('#Otros').val());
    var TotalIngresos = NegocioPropio + Salario + Finca + OtrosIngresos;
    
    $('#TotalIngreso').val(TotalIngresos); 

});

//TOTAL EGRESOS ESTUDIO SOCIOECONOMICO
$('input').change(function (e) {

    var Renta = parseInt($('#Renta').val());
    var Servicios = parseInt($('#Servicios').val());
    var Prestamos = parseInt($('#Prestamos').val());
    var Transporte = parseInt($('#Transporte').val());
    var Alimentacion = parseInt($('#Alimentacion').val());
    var Vestuario = parseInt($('#Vestuario').val());
    var OtrosEgresos = parseInt($('#Otros1').val());
    var TotalEgresos = Renta + Servicios + Prestamos + Transporte + Alimentacion + Vestuario + OtrosEgresos;

    $('#Egresos').val(TotalEgresos);

});

function GenerarSolicitud(PrestamoId) {

    var path = urljs + "/Prestamos/SolicitudCredito";
    var posting = $.post(path, { PrestamoId: PrestamoId });

    posting.done(function (data, status, xhr) {
        dataCitas = [];
        dataCitas = data;
        if (dataCitas.length > 0) {
            for (var i = dataCitas.length - 1; i >= 0; i--) {
                Nombre = dataCitas[0].Nombre
                Identidad = dataCitas[0].CLI_Identidad
                Direccion = dataCitas[0].CLI_Direccion
                EstadoCivil = dataCitas[0].Estado_Civil
                Celular = dataCitas[0].CLI_Cel
                MontoSolicitado = dataCitas[0].PRES_Mto_Solicitado
                Interes = dataCitas[0].PRES_Porc_Interes
                MontoAprobado = dataCitas[0].PRES_Mto_Aprobado
                Desc = dataCitas[0].DES_Descripcion
                DescGar = dataCitas[0].GAR_Descripcion
                FormPago = dataCitas[0].FormaPago
                Observ = dataCitas[0].PRES_Observaciones
                Fecha = dataCitas[0].Fecha
                FechaAprob = dataCitas[0].PRES_Fecha_Aprob
                refNomb = dataCitas[0].REF_Nombre

                DescPro = dataCitas[0].PRO_Descripcion
                DirTrab = dataCitas[0].CLI_Direc_Trabajo

                if (dataCitas[i].REF_Num == 1) {
                    refCelu = dataCitas[i].REF_Celular
                    refNomb = dataCitas[i].REF_Nombre
                }
                else if (dataCitas[i].REF_Num == 2) {
                    refCelu2 = dataCitas[i].REF_Celular
                    refNomb2 = dataCitas[i].REF_Nombre
                }
                //$('#txt_Hora').val(data.CitaHora);
            }


            //GenerarSolicitudFormato(dataCitas);

        }

    });

    var path = urljs + "/Prestamos/SolicitudCreditoImp";
    var posting = $.post(path, {

        Nombre: Nombre, Direccion: Direccion, Celular: Celular,
        Identidad: Identidad, EstadoCivil: EstadoCivil, MontoSolicitado: MontoSolicitado,
        TasaInteres: Interes, MontoAprobado: MontoAprobado, ObjetivoPrestamo: Desc,
        TipoGarantia: DescGar, FormaPago: FormPago, Observaciones: Observ,
        Fecha: Fecha, FechaAprobacion: FechaAprob, NomRef1: refNomb,
        CelRef1: refCelu, Profesion: DescPro, dirTrabajo: DirTrab, NomRef2: refNomb2,
        CelRef2: refCelu2
    });



    posting.done(function (data, status, xhr) {
        console.log(data);
        if (data == 1) {
            window.open('/PDF/SolicitudCredito.pdf');
        }
    });

}

function GenerarEstudioSocioEconimico(PrestamoId) {

    var path = urljs + "/Prestamos/EstudioSocioEconomico";
    var posting = $.post(path, { PrestamoId: PrestamoId });

    posting.done(function (data, status, xhr) {
        dataCitas = [];
        dataCitas = data;
        if (dataCitas.length > 0) {
            for (var i = dataCitas.length - 1; i >= 0; i--) {
                Nombre = dataCitas[i].Nombre
                CLI_Direccion = dataCitas[i].CLI_Direccion
                Cel = dataCitas[i].CLI_Cel
                NumeDependient = dataCitas[i].ESE_NumeroDependientes
                ActivAgrop = dataCitas[i].ESE_ActividadAgropecuaria
                OtrosIngresos = dataCitas[i].ESE_Otros_Ingresos
                Renta = dataCitas[i].ESE_Renta
                ServPublico = dataCitas[i].ESE_ServiciosPublicos
                Prestamos = dataCitas[i].ESE_Prestamos
                Transporte = dataCitas[i].ESE_Transporte
                Alimentacion = dataCitas[i].ESE_Alimentacion
                Vestuario = dataCitas[i].ESE_Vestuario
                otrosEgresos = dataCitas[i].ESE_Otros_Egresos
                Observacion = dataCitas[i].ESE_Observaciones
                Salario = dataCitas[i].ESE_Salario
                Negocio = dataCitas[i].ESE_NegocioPropio
                TotalIngresos = dataCitas[i].totalIngresos
                totalEgresos = dataCitas[i].totalEgresos

                //$('#txt_Hora').val(data.CitaHora);
            }
        }


    });

    var path = urljs + "/Prestamos/EstudioSocioeconomicoImp";
    var posting = $.post(path, {
        Nombre: Nombre, Direccion: CLI_Direccion, Celular: Cel,
        NumDependientes: NumeDependient, Renta: Renta, ServiciosPub: ServPublico,
        Prestamos: Prestamos, Transporte: Transporte, Alimentacion: Alimentacion,
        Vestimenta: Vestuario, otrosEgresos: otrosEgresos, Observaciones: Observacion,
        Salario: Salario, ActividadAgricola: ActivAgrop, otrosIngresos: OtrosIngresos,
        Negocio: Negocio, TotalIngresos: TotalIngresos, TotalEgresos: totalEgresos
    });

    posting.done(function (data, status, xhr) {
        console.log(data);
        if (data == 1) {
            4
            window.open('/PDF/EstudioEconomico.pdf');
        }
    });

}

//CAPACIDAD DE PAGO ESTUDIO SOCIOECONOMICO
$('input').change(function (e) {

    var TotalIngreso = parseInt($('#TotalIngreso').val());
    var TotalEgreso = parseInt($('#Egresos').val());
    var CapacidadPago = TotalIngreso - TotalEgreso;

    $('#CapPago').val(CapacidadPago);

});

function existeRazon(descripcion, abreviatura) {
    var resultado = false;
    try {
        var path = urljs + "/Razones/CheckOne";
        var posting = $.post(path, { descripcion: descripcion, abreviatura: abreviatura });
        posting.done(function (data, status, xhr) {
            console.log('Registros: ' + data.cantidadRegistros);
            if (data.cantidadRegistros > 0) {
                $('#moda_razones').modal('hide');
                GenerarErrorAlerta('Registro ya existe en la base de datos.', "error");
                goAlert();
                resultado = true;
                console.log('Res1: ' + resultado);
            }
            else {
                GuardarRazon();
            }
        });
        posting.fail(function (data, status, xhr) {
            GenerarErrorAlerta(xhr, "error");
            goAlert();
        });
        posting.always(function (data, status, xhr) {
        });
    }
    catch (e) {
        console.log(e);
    }
}

//ACCION DE LA TABLA VER DATOS DEL PRESTAMO

function GetDatosPrestamos(id) {
    //$("#btnGeneraReciboPagoPrestamo").addClass("disabled");

    $("#btnGuardar").addClass('hidden');

    $('#Codigo').attr("disabled",true);
    $('#montosolicitado').attr("disabled",true);
    $('#plazo').attr("disabled",true);
    $('#garantia').attr("disabled",true);
    $('#tasainteres').attr("disabled",true);
    $('#destino').attr("disabled",true);
    $('#frecuenciapago').attr("disabled",true);
    $('#TipoCuota').attr("disabled",true);
    $('#observaciones').attr("disabled",true);

    //DATOS DEL ESTUDIO SOCIOECONOMICO
    $('#Personas').attr("disabled",true);
    $('#NegocioPropio').attr("disabled",true);
    $('#Salario').attr("disabled",true);
    $('#Finca').attr("disabled",true);
    $('#Otros').attr("disabled",true);
    $('#Renta').attr("disabled",true);
    $('#Servicios').attr("disabled",true);
    $('#Prestamos').attr("disabled",true);
    $('#Transporte').attr("disabled",true);
    $('#Alimentacion').attr("disabled",true);
    $('#Vestuario').attr("disabled",true);
    $('#Otros1').attr("disabled",true);
    $('#Observaciones1').attr("disabled",true);

    $('#theHeader').html("Ver Préstamo");
    try {
        LimpiarInput(inputsPrestamo, ['']);
        var path = urljs + "/Prestamos/GetDatosPrestamos";
        var posting = $.post(path, { id: id });
        posting.done(function (data, status, xhr) {
            dataCitas = [];
            dataCitas = data;
            if (dataCitas.length > 0) {
                for (var i = dataCitas.length - 1; i >= 0; i--) {

                    //DATOS DE PRESTAMO
                    $('#Codigo').val(dataCitas[i].Codigo);
                    $('#montosolicitado').val(dataCitas[i].MontoSolicitado);
                    $('#plazo').val(dataCitas[i].PlazoMeses);
                    $('#garantia').val(dataCitas[i].Garantia);
                    $('#tasainteres').val(dataCitas[i].Interes);
                    $('#destino').val(dataCitas[i].Destino);
                    $('#frecuenciapago').val(dataCitas[i].FrecPago);
                    $('#TipoCuota').val(dataCitas[i].TipoCuota);
                    $('#observaciones').val(dataCitas[i].Observaciones);

                    //DATOS DEL ESTUDIO SOCIOECONOMICO
                    $('#Personas').val(dataCitas[i].Personas);
                    $('#NegocioPropio').val(dataCitas[i].NegocioPropio);
                    $('#Salario').val(dataCitas[i].Salario);
                    $('#Finca').val(dataCitas[i].Finca);
                    $('#Otros').val(dataCitas[i].Otros);
                    $('#Renta').val(dataCitas[i].Renta);
                    $('#Servicios').val(dataCitas[i].ServicioPublico);
                    $('#Prestamos').val(dataCitas[i].Prestamo);
                    $('#Transporte').val(dataCitas[i].Transporte);
                    $('#Alimentacion').val(dataCitas[i].Alimentacion);
                    $('#Vestuario').val(dataCitas[i].Vestuario);
                    $('#Otros1').val(dataCitas[i].Otros1);
                    $('#Observaciones1').val(dataCitas[i].Observaciones1);

                    var Renta = parseInt($('#Renta').val());
                    var Servicios = parseInt($('#Servicios').val());
                    var Prestamos = parseInt($('#Prestamos').val());
                    var Transporte = parseInt($('#Transporte').val());
                    var Alimentacion = parseInt($('#Alimentacion').val());
                    var Vestuario = parseInt($('#Vestuario').val());
                    var OtrosEgresos = parseInt($('#Otros1').val());
                    var TotalEgresos = Renta + Servicios + Prestamos + Transporte + Alimentacion + Vestuario + OtrosEgresos;

                    $('#Egresos').val(TotalEgresos);

                    var NegocioPropio = parseInt($('#NegocioPropio').val());
                    var Salario = parseInt($('#Salario').val());
                    var Finca = parseInt($('#Finca').val());
                    var OtrosIngresos = parseInt($('#Otros').val());
                    var TotalIngresos = NegocioPropio + Salario + Finca + OtrosIngresos;

                    $('#TotalIngreso').val(TotalIngresos);

                    var TotalIngreso = parseInt($('#TotalIngreso').val());
                    var TotalEgreso = parseInt($('#Egresos').val());
                    var CapacidadPago = TotalIngreso - TotalEgreso;

                    $('#CapPago').val(CapacidadPago);
                    $('#Nombre').val(Nombre);

                   
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

$("#btnPlanPago").on('click', function (e) {
    var CodPrest = $('#Codigo').val();
    var monto = 100000;
    var tasa = 19.0;
    var plazo = 12;
    var frecuency = 'Q';
    var fecha = '2018-07-22';
    var tipopres = "cd";
    GetPlanPago(CodPrest);

});

//Plan de pago.
function GetPlanPago(CodPrest) {
    try {
        var path = urljs + "/citas/GetPlanPagos";
        var posting = $.post(path, { CodPrest: CodPrest }); //Aqui se ponen las variables

        posting.done(function (data, status, xhr) {
            dataCitas = [];
            dataCitas = data;
            LimpiarTabla("#tablePlanPago");
            if (data.length > 0) {
                if (data[0].Accion > 0) {
                    var counter = 1;
                    for (var i = data.length - 1; i >= 0; i--) {
                        if (data[i].flag_historico == 0) {
                            addRowCitaPendiente(data[i], "#tablePlanPago", counter);
                        }
                        else {
                            addRowPlanPago(data[i], "#tablePlanPago", counter);
                        }
                        counter++;
                    }
                    //$("#texto_busqueda_encontrado").text(Id);
                    //activarTab('resultado_busqueda_encontrado');
                }
                else {
                    //$("#texto_busqueda").text(Id);
                    //activarTab('resultado_busqueda_ninguno');
                    /*GenerarErrorAlerta(data[0].Mensaje, "error");
                    goAlert();*/
                }
            }
            else {
                //$("#texto_busqueda").text(Id);
                //activarTab('resultado_busqueda_ninguno');
            }
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
    $('#plan_pago').modal('show');
}

function addRowPlanPago(ArrayData, tableID, counter) 
{
    var newRow = $(tableID).dataTable().fnAddData([

        ArrayData['Num'],
        ArrayData['Fecha'],
        ArrayData['Capital'],
        ArrayData['Interes'],
        ArrayData['Total'],
        ArrayData['Saldo'],
        ArrayData['Codigo']

    ]);

    var theNode = $(tableID).dataTable().fnSettings().aoData[newRow[0]].nTr;
    theNode.setAttribute('id', ArrayData['Codigo']);
    $('td', theNode)[6].setAttribute('class', 'Codigo hidden');
}

//IMPRIMIR PLAN DE PAGO

$("#btnImprimirPlanPago").on('click', function (e) {
    var CodPrest = $('#Codigo').val();
    var Nombre = $('#Nombre').val();
    var FrecPago = $('#frecuenciapago').val();

    ImprimirPlanPago(CodPrest,Nombre,FrecPago);

    
});

function ImprimirPlanPago(CodPrest, Nombre, FrecPago) {

    var path = urljs + "/Citas/ImprimirPlanPago";
    var posting = $.post(path, { CodPrest: CodPrest, Nombre: Nombre, FrecPago: FrecPago });

    posting.done(function (data, status, xhr) {
        console.log(data);
        if (data == 1) {
            window.open('../PlanPago' + CodPrest + '.pdf');
        }
    });
    

}



//TRANSACCIONES DE CREDITOS
$("#btnTransPrestamos").on('click', function (e) {
    var codigo = $('#Codigo').val();
    //if ($("#txt_prestamo").val() != '') {
    GetTransaccionesPrestamos(codigo);
    // GetTransaccionesPrestamos($("#txt_prestamo").val());
    //}
});

function GetTransaccionesPrestamos(codigo) {
    try {
        var path = urljs + "/Clientes/GetTransaccionesPrestamos";
        var posting = $.post(path, { codigo: codigo });
        posting.done(function (data, status, xhr) {
            dataCitas = [];
            dataCitas = data;
            LimpiarTabla("#tableCitas");
            if (data.length > 0) {
                if (data[0].Accion > 0) {
                    var counter = 1;
                    for (var i = 0; i < data.length; i++) {
                        //for (var i = data.length - 1; i >= 0; i--) {
                        addRowTransaccionesPrestamo(data[i], "#tableCitas", counter);
                        counter++;
                    }
                    //$("#texto_busqueda_encontrado").text(Id);
                    //activarTab('resultado_busqueda_encontrado');
                }
                else {
                    //$("#texto_busqueda").text(Id);
                    //activarTab('resultado_busqueda_ninguno');
                    /*GenerarErrorAlerta(data[0].Mensaje, "error");
                    goAlert();*/
                }
            }
            else {
                //$("#texto_busqueda").text(Id);
                //activarTab('resultado_busqueda_ninguno');
            }
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
    $('#Transacciones_Prestamo').modal('show');
}

//LLENA LA LISTA DE TRANSACCIONES DE PRESTAMOS
function addRowTransaccionesPrestamo(ArrayData, tableID, counter) {
    var newRow = $(tableID).dataTable().fnAddData([
        counter,
        ArrayData['Fecha'],
        ArrayData['TRP_Otorgado'],
        ArrayData['TRP_Capital'],
        ArrayData['TRP_Interes'],
        ArrayData['TRP_Mora'],
        ArrayData['TRP_Agrego'],
        ArrayData['Saldo'],
        //estadosArray[parseInt(ArrayData['PRES_Estado'])],
        //accionesHTML,
        ArrayData['TRP_Codigo_Prestamo']
    ]);

    var theNode = $(tableID).dataTable().fnSettings().aoData[newRow[0]].nTr;
    theNode.setAttribute('id', ArrayData['TRP_Codigo_Prestamo']);
    $('td', theNode)[8].setAttribute('class', 'TRP_Codigo_Prestamo hidden');
}

//AVALES
$("#btnAvales").on('click', function (e) {
    e.stopPropagation();
    var id = -1;
    $("#theHeader").html("Avales");
    $('#hidden_id').val("");
    //$("#Modal_Avales").find('input[type = "text"]').val("");
    $('#descripcion_tipo_razon').val('');
    $('#tiene_listado_x').val(0);
    $('#tipostatus').val('ACT');
    $('#tiene_listado_x').trigger('change');
    $('#Modal_Avales').modal('show');
    //}
});