$(document).ready(function () {
    jQuery.ajaxSetup({ async: false });
    //checkUserAccess('CONF050');
    jQuery.ajaxSetup({ async: true });

    GetRazones($('#hidden_tipo_id').val());
    GetTipoRazones();

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
            GuardarDeposito();
        //}
    });
});

var inputsDeposito = ['#montoDeposito', '#estado', '#Tipo', '#tasainteres', '#plazo', '#PagoIntereses', '#observaciones', '#Beneficiario1',
'#ID_Bene1', '#Porc_Bene1', '#Beneficiario2', '#ID_Bene2', '#Porc_Bene2', '#Beneficiario3', '#ID_Bene3', '#Porc_Bene3'];

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

function GetDatosDepositoPF(id) {

    try {
        var path = urljs + "/Depositos/GetDatosDeposito";
        var posting = $.post(path, { id: id });
        posting.done(function (data, status, xhr) {
            dataCitas = [];
            dataCitas = data;
            if (dataCitas.length > 0) {
                for (var i = dataCitas.length - 1; i >= 0; i--) {
                    Nombre = dataCitas[i].Nombre
                    monto = dataCitas[i].DPF_Monto
                    plazo = dataCitas[i].DPF_Plazo
                    FechaAper = dataCitas[i].DPF_Fecha_Apert
                    TasaInt = dataCitas[i].DPF_Tasa_interes
                    Tipo = dataCitas[i].DPF_Tipo
                    Benef1 = dataCitas[i].DPF_Beneficiario_1
                    Benef2 = dataCitas[i].DPF_Beneficiario_2
                    Benef3 = dataCitas[i].DPF_Beneficiario_3
                    IdBenef1 = dataCitas[i].DPF_ID_Bene_1
                    IdBenef2 = dataCitas[i].DPF_ID_Bene_2
                    IdBenef3 = dataCitas[i].DPF_ID_Bene_3
                    FechaVenc = dataCitas[i].FechaVencimiento
                    Periocidad = dataCitas[i].DPF_Pago_intereses
                    DpfCodigo = dataCitas[i].DPF_Codigo
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

        var path = urljs + "/Caja/CertificadoDeposito";
        var posting = $.post(path, {
            Nombre: Nombre, DPF_Monto: monto, DPF_Plazo: plazo,
            DPF_Fecha_Apert: FechaAper, DPF_Tasa_interes: TasaInt, DPF_Tipo: Tipo,
            DPF_Beneficiario_1: Benef1, DPF_Beneficiario_2: Benef2, DPF_Beneficiario_3: Benef3,
            DPF_ID_Bene_1: IdBenef1, DPF_ID_Bene_2: IdBenef2, DPF_Beneficiario_3: IdBenef3,
            FechaVencimiento: FechaVenc, DPF_Pago_intereses: Periocidad, DPF_Codigo: DpfCodigo
        });

        posting.done(function (data, status, xhr) {
            console.log(data);
            if (data == 1) {
                window.open('/Formatos/CERTIFICADO_DE_DEPOSITO_1.pdf');
            }
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

    var newRow = $(tableID).dataTable().fnAddData([
        ArrayData['DPF_Fecha_Apert'],
        ArrayData['DPF_Monto'],
        ArrayData['DPF_Plazo'],
        ArrayData['DPF_Tasa_interes'],
        ArrayData['DPF_Saldo'],
        ArrayData['DPF_Estado'],
        ArrayData['TDP_Descripcion'],
         "<button data-id='" + ArrayData['DPF_Codigo'] + "' title='Ver DPF' data-toggle='tooltip' onClick='GetDatosDeposito(" + ArrayData['DPF_Codigo'] + ")' id='btnGenerarPago" + ArrayData['DPF_Codigo'] + "' class='btn btn-primary text-center btn-sm'><i class='fa fa-pencil-square-o'></i></button>" + 
         "<button data-id='" + ArrayData['DPF_Codigo'] + "' data-tipo_id='" + ArrayData['DPF_Codigo'] + "' data-name='" + ArrayData['RazonDescripcion'] + "' title='Generar Certificado' data-toggle='tooltip' onClick='GetDatosDepositoPF(" + ArrayData['DPF_Codigo'] + ")' id='btngenerarcontrato" + ArrayData['DPF_Codigo'] + "' class='btn btn-success text-center btn-sm'><i class='glyphicon glyphicon-thumbs-up'></i></button>",
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

//GENERA CERTIFICADO DE DEPOSITO
function GeneraCertificado(DPF_Codigo) {

    var path = urljs + "/Depositos/GeneraCertificado";
    var posting = $.post(path, { DPF_Codigo: DPF_Codigo });

    posting.done(function (data, status, xhr) {
        console.log(data);
        if (data == 1) {
            window.open('../CertificadoDeposito' + DPF_Codigo + '.pdf');
        }
    });
    
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

function NuevoDeposito(e) {
    e.stopPropagation();
    var id = -1;
    $("#theHeader").html("Nuevo Deposito a Plazo Fijo");
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


//INGRESAR NUEVO DEPOSITO
function GuardarDeposito() {
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
        var DPF_Codigo = $('#Codigo').val();
        var DPF_Monto = $('#montoDeposito').val();
        //var estado = $('#estado').val();
        var DPF_Tipo = $('#Tipo').val();
        var DPF_Tasa_interes = $('#tasainteres').val();
        var DPF_Plazo = $('#plazo').val();
        var DPF_Pago_intereses = $('#PagoIntereses').val();
        var DPF_Observacion = $('#observaciones').val();
        var DPF_Beneficiario_1 = $('#Beneficiario1').val();
        var DPF_ID_Bene_1 = $('#ID_Bene1').val();
        var DPF_Porc_1 = $('#Porc_Bene1').val();
        var DPF_Beneficiario_2 = $('#Beneficiario2').val();
        var DPF_ID_Bene_2 = $('#ID_Bene2').val();
        var DPF_Porc_2 = $('#Porc_Bene2').val();
        var DPF_Beneficiario_3 = $('#Beneficiario3').val();
        var DPF_ID_Bene_3 = $('#ID_Bene3').val();
        var DPF_Porc_3 = $('#Porc_Bene3').val();

        //VALIDACION DE CAMPOS QUE NO SON OBLIGATORIOS Y QUE VENGAN VACIOS, SI VIENE VACIO SE ASIGNAN VALORES

        if (DPF_Codigo == "") {
            DPF_Codigo = -1;
        }

        if (DPF_Observacion == "") {
            DPF_Observacion = "n";
        }
        if (DPF_Beneficiario_2 == "") {
            DPF_Beneficiario_2 = "n";
        }
        if (DPF_ID_Bene_2 == "") {
            DPF_ID_Bene_2 = "n";
        }
        if (DPF_Porc_2 == "") {
            DPF_Porc_2 = 0;
        }
        if (DPF_Beneficiario_3 == "") {
            DPF_Beneficiario_3 = "n";
        }
        if (DPF_ID_Bene_3 == "") {
            DPF_ID_Bene_3 = "n";
        }
        if (DPF_Porc_3 == "") {
            DPF_Porc_3 = 0;
        }
        
        if (Validar(inputs, mensaje)) {

            var path = urljs + 'Depositos/SaveData';
            var id = $('#hidden_id').val();
            console.log('id: ' + $('#hidden_id').val());

            if (id == "") {
                id = -1;
            }

            //JSON data
            var dataType = 'application/json; charset=utf-8';

            var data = {

                // DATOS Deposito
                ClienteId: client,
                DPF_Codigo : DPF_Codigo,
                DPF_Monto : $('#montoDeposito').val(),
                DPF_Tipo : $('#Tipo option:selected').val(),
                DPF_Tasa_interes : $('#tasainteres').val(),
                DPF_Plazo : $('#plazo').val(),
                DPF_Pago_intereses: $('#PagoIntereses option:selected').val(),
                DPF_Beneficiario_1 : $('#Beneficiario1').val(),
                DPF_ID_Bene_1 : $('#ID_Bene1').val(),
                DPF_Porc_1: $('#Porc_Bene1').val(),

                DPF_Observacion: DPF_Observacion,
                DPF_Beneficiario_2: DPF_Beneficiario_2,
                DPF_ID_Bene_2: DPF_ID_Bene_2,
                DPF_Porc_2: DPF_Porc_2,
                DPF_Beneficiario_3: DPF_Beneficiario_3,
                DPF_ID_Bene_3: DPF_ID_Bene_3,
                DPF_Porc_3: DPF_Porc_3,
                
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

//ACCION DE LA TABLA VER DATOS DEL DEPOSITO

function GetDatosDeposito(id) {
    //$("#btnGeneraReciboPagoPrestamo").addClass("disabled");

    $('#theHeader').html("Ver Deposito a Plazo");
    try {
        LimpiarInput(inputsDeposito, ['']);
        var path = urljs + "/Depositos/GetDatosDeposito";
        var posting = $.post(path, { id: id });
        posting.done(function (data, status, xhr) {
            dataCitas = [];
            dataCitas = data;
            if (dataCitas.length > 0) {
                for (var i = dataCitas.length - 1; i >= 0; i--) {

                    //DATOS DE DEPOSITO

                    $('#Codigo').val(dataCitas[i].DPF_Codigo);
                    $('#montoDeposito').val(dataCitas[i].DPF_Monto);
                    $('#estado').val(dataCitas[i].DPF_Estado);
                    if ($('#estado').val() != 0) {
                        $("#btnGenerarDeposito").addClass("disabled");
                    }
                    if ($('#estado').val() == 0) {
                        $("#btnGenerarDeposito").removeClass("disabled");
                    }
                    $('#Tipo').val(dataCitas[i].DPF_Tipo);
                    $('#tasainteres').val(dataCitas[i].DPF_Tasa_interes);
                    $('#plazo').val(dataCitas[i].DPF_Plazo);
                    $('#PagoIntereses').val(dataCitas[i].DPF_Pago_intereses);
                    $('#observaciones').val(dataCitas[i].DPF_Observacion);

                    //DATOS DE LOS BENEFICIARIOS
                    $('#Beneficiario1').val(dataCitas[i].DPF_Beneficiario_1);
                    $('#ID_Bene1').val(dataCitas[i].DPF_ID_Bene_1);
                    $('#Porc_Bene1').val(dataCitas[i].DPF_Porc_1);
                    $('#Beneficiario2').val(dataCitas[i].DPF_Beneficiario_2);
                    $('#ID_Bene2').val(dataCitas[i].DPF_ID_Bene_2);
                    $('#Porc_Bene2').val(dataCitas[i].DPF_Porc_2);
                    $('#Beneficiario3').val(dataCitas[i].DPF_Beneficiario_3);
                    $('#ID_Bene3').val(dataCitas[i].DPF_ID_Bene_3);
                    $('#Porc_Bene3').val(dataCitas[i].DPF_Porc_3);
                    
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


//TRANSACCIONES DE DEPOSITOS
$("#btnTransDepositos").on('click', function (e) {
    var codigo = $('#Codigo').val();
    //if ($("#txt_prestamo").val() != '') {
    GetTransaccionesDepositos(codigo);
    // GetTransaccionesPrestamos($("#txt_prestamo").val());
    //}
});

function GetTransaccionesDepositos(codigo) {
    try {
        var path = urljs + "/Depositos/GetTransaccionesDepositos";
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
                        addRowTransaccionesDepositos(data[i], "#tableCitas", counter);
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
    $('#Transacciones_Depositos').modal('show');
}

//LLENA LA LISTA DE TRANSACCIONES DE Depositos
function addRowTransaccionesDepositos(ArrayData, tableID, counter) {
    var newRow = $(tableID).dataTable().fnAddData([
        counter,
        ArrayData['Fecha'],
        ArrayData['TRD_Deposito'],
        ArrayData['TRD_Retiro'],
        ArrayData['TRD_Interes'],
        //ArrayData['TRP_Mora'],
        ArrayData['TRD_Agrego'],
        ArrayData['Saldo'],
        //estadosArray[parseInt(ArrayData['PRES_Estado'])],
        //accionesHTML,
        ArrayData['TRD_Codigo_DPF']
    ]);

    var theNode = $(tableID).dataTable().fnSettings().aoData[newRow[0]].nTr;
    theNode.setAttribute('id', ArrayData['TRD_Codigo_DPF']);
    $('td', theNode)[7].setAttribute('class', 'TRD_Codigo_DPF hidden');
}


//AGREGAR TRANSACCION DE DEPOSITO
$("#btnGenerarDeposito").on('click', function (e) {
    var path = urljs + "/Caja/ValidaCajero";
    var posting = $.post(path, {});
    posting.done(function (data, status, xhr) {

        dataResultado = [];
        dataResultado = data;
        //alert(data.Mensaje);
        if ((data.Estado) != 1 && (data.Estado) != 2) {
            GenerarRegistroDeposito($("#Codigo").val(), $("#montoDeposito").val(), $("#TipoPagoDeposito").val());
            //$("#btnGeneraPagoPrestamo").addClass("disabled");
            //$("#btnGeneraReciboPagoPrestamo").removeClass("disabled");
        }
        else {
            alert(data.Mensaje);
        }
    });
    
               
});

function GenerarRegistroDeposito(codigo,Deposito, TipoPago) {
    try {
        var path = urljs + "/Depositos/GeneraDeposito";
        var posting = $.post(path, { codigo: codigo, Deposito: Deposito, TipoPago : TipoPago });
        posting.done(function (data, status, xhr) {

            dataResultado = [];
            dataResultado = data;
            //$("#NumRecibo").val(data.Num);
            if (data.Num != -2) {
                $("#btnGenerarDeposito").addClass("disabled");
                //$("#btnGeneraReciboPagoPrestamo").removeClass("disabled");
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