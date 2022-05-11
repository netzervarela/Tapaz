var hoy = moment(new Date()).format('YYYY-MM-DD');

$(document).ready(function () {
    GetTipoRazones();
    $('.identidad').mask('#############', { translation: { '#': { pattern: /[0-9]/, optional: true } } });
    $('.rtn').mask('##############', { translation: { '#': { pattern: /[0-9]/, optional: true } } });
    $('.numero').mask('####', { translation: { '#': { pattern: /[0-9]/, optional: true } } });
    $('.correo').mask('AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA', { translation: { 'A': { pattern: /[a-z-_.0-9 @]/, optional: true } } });
    $('.telefonoCelular').mask('AAAAAAAA', { translation: { 'A': { pattern: /[0-9]/, optional: true } } });
    $('.monto').mask('#############', { translation: { '#': { pattern: /[0-9]/, optional: true } } });
    $('.fecha').mask('##########', { translation: { '#': { pattern: /[0-9-]/, optional: true } } });
    $('.varchar50').mask('AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA', { translation: { 'A': { pattern: /[A-Za-z ÁÉÍÓÚáéúíóúÑñÄËÏÖÜäëïöüÀÈÌÒÙàèìòùÃÕãõçÇÂâÊêÎîÔôÛû]/, optional: true } } });
    $('.varchar200').mask('AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA', { translation: { 'A': { pattern: /[A-Za-z-,.: 0-9 ÁÉÍÓÚáéúíóúÑñÄËÏÖÜäëïöüÀÈÌÒÙàèìòùÃÕãõçÇÂâÊêÎîÔôÛû#]/, optional: true } } });

    //LimpiarTabla('#tableTipoRazones');
    $('#btnGuardar').click(function () {
        var date1 = $('#fecha').datetimepicker("getDate");
        if ($('#hidden_id').val() == '' || $('#hidden_id').val() == null) {
            existeCliente($('#identidad').val(), $('#rtn').val());
        }
        else {
            GuardarTipoRazon();
        }
        console.log('hidden_id: ' + $('#hidden_id').val());
    });

    $('#fecha').datetimepicker({
        
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
        },
        
    });

    $("#fecha1").on("dp.change", function (e) {
        this.data("DateTimePicker");
    });

    $('#btnEliminar').click(function () {
        var id = $('#hidden_id').val();
        var path = urljs + 'TipoRazones/delete';
        var posting = $.post(path, { id: Number(id) });
        posting.done(function (data, status, xhr) {
            if (data.Accion > 0) {
                GetTipoRazones();
                $('#modalEliminarTipoRazon').modal('hide');
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
    
});


var inputs = ['#rtn', '#identidad', '#primernombre', '#segundonombre', '#primerapellido', '#segundoapellido', '#genero', '#telcasa', '#celular', '#correo', '#fecha', '#estado', '#estcivil', '#direccion'];

function GetTipoRazones() {
    try {
        /*animacion de loading*/
        //LoadAnimation("body");

        var path = urljs + "/Clientes/GetAll";
        var posting = $.post(path, { param1: 'value1' });
        posting.done(function (data, status, xhr) {
            //console.log(data);
            LimpiarTabla("#tableTipoRazones");
            if (data.length) {
                if (data[0].Accion > 0) {
                    for (var i = data.length - 1; i >= 0; i--) {
                        addRow(data[i], "#tableTipoRazones");
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

//function GetTipoRazonBy(id) {
//    try {
//        /*animacion de loading*/
//        //LoadAnimation("body");

//        var path = urljs + "/TipoRazones/GetOne";
//        var posting = $.post(path, { id: Number(id) });
//        posting.done(function (data, status, xhr) {
//            console.log(data);
//            if (data.Accion > 0) {
//                $('#abreviatura').val(data.TipoAbreviatura);
//                $('#descripcion_tipo_razon').val(data.TipoDescripcion);
//                $('#tiene_listado_x').val(data.TipoTieneListadoExtra);
//                $('#etiqueta').val(data.TipoEtiquetaListadoExtra);
//                $('#origen').val(data.TipoOrigenListadoExtra);
//                $('#codigo_listado').val(data.TipoCodigoListadoExtra);
//                $('#tiene_listado_x').trigger('change');
//            }
//            else {
//                GenerarErrorAlerta(data.Mensaje, "error");
//                goAlert();
//            }
//        });
//        posting.fail(function (data, status, xhr) {
//            console.log(data);
//            GenerarErrorAlerta(xhr, "error");
//            goAlert();
//        });
//        posting.always(function (data, status, xhr) {
//            //RemoveAnimation("body");
//        });
//    }
//    catch (e) {
//        //RemoveAnimation("body");
//        console.log(e);
//    }
//}


//TRAE INFORMACION DEL CLIENTE
function GetInfoClientes(id) {
    try {
        /*animacion de loading*/
        //LoadAnimation("body");

        var path = urljs + "/TipoRazones/GetOne";
        //var posting = $.post(path, { id: Number(id) });
        
        var posting = $.post(path, { id: Number(id) });
        posting.done(function (data, status, xhr) {
            dataCitas = [];
            dataCitas = data;
            //console.log(data);
            if (dataCitas.length > 0) {
                for (var i = dataCitas.length - 1; i >= 0; i--) {

                    var initialDate = dataCitas[i].Edad;

                    //Dividimos la fecha primero utilizando el espacio para obtener solo la fecha y el tiempo por separado
                    var splitDate = initialDate.split(" ");
                    var date = splitDate[0].split("/");
                    var time = splitDate[1].split(":");

                    // Obtenemos los campos individuales para todas las partes de la fecha
                    var dd = date[0];
                    var mm = date[1];
                    var yyyy = date[2];
                    //var hh = time[0];
                    //var min = time[1];
                    //var ss = time[2];

                    // Creamos la fecha con Javascript
                    var fecha = yyyy.concat("/", mm, "/", dd);
                    //var fecha = (yyyy, mm, dd);


                    $('#cliCod').val(id);
                    $('#identidad').val(dataCitas[i].Identidad);
                    $('#rtn').val(dataCitas[i].RTN);
                    $('#primernombre').val(dataCitas[i].PrimerNombre);
                    $('#segundonombre').val(dataCitas[i].SegundoNombre);
                    $('#primerapellido').val(dataCitas[i].PrimerApellido);
                    $('#segundoapellido').val(dataCitas[i].SegundoApellido);
                    $('#genero').val(dataCitas[i].Genero);
                    $('#telcasa').val(dataCitas[i].TelCasa);
                    $('#celular').val(dataCitas[i].Cel);
                    $('#correo').val(dataCitas[i].Correo);
                    $('#Profesion').val(dataCitas[i].Profesion);
                    $('#DireccionTrabajo').val(dataCitas[i].DireccionTrabajo);
                    $('#fecha').val(fecha);
                    //$('#fecha').val(dataCitas[i].Edad);
                    $('#estado').val(dataCitas[i].EstadoCli);
                    $('#estcivil').val(dataCitas[i].EstadoCivil);
                    $('#direccion').val(dataCitas[i].Direccion);
                    $('#NomRef1').val(dataCitas[i].RefNom1);
                    $('#IdRef1').val(dataCitas[i].RefId1);
                    $('#CelRef1').val(dataCitas[i].RefCel1);
                    $('#TelRef1').val(dataCitas[i].RefTel1);
                    $('#NomRef2').val(dataCitas[i].RefNom2);
                    $('#IdRef2').val(dataCitas[i].RefId2);
                    $('#CelRef2').val(dataCitas[i].RefCel2);
                    $('#TelRef2').val(dataCitas[i].RefTel2);
                    $('#NumRef1').val(dataCitas[i].NumRef1);
                    $('#NumRef2').val(dataCitas[i].NumRef2);
                }
            }
            //else {
            //    GenerarErrorAlerta(data.Mensaje, "error");
            //    goAlert();
            //}
        });
        posting.fail(function (data, status, xhr) {
            //console.log(data);
            GenerarErrorAlerta(xhr, "error");
            goAlert();
        });
        posting.always(function (data, status, xhr) {
            //RemoveAnimation("body");
        });
    }
    catch (e) {
        //RemoveAnimation("body");
        //console.log(e);
    }
}

function addRow(ArrayData, tableID) {
    //var TipoTieneListadoExtra = "No";
    //if (ArrayData['TipoTieneListadoExtra']) {
    //    TipoTieneListadoExtra = "Si";
    //}

    var newRow = $(tableID).dataTable().fnAddData([
          ArrayData['Identidad'],
          ArrayData['RTN'],
          ArrayData['Nombre'],
          ArrayData['Genero'],
          ArrayData['Edad'],
          ArrayData['EstadoCli'],
          ArrayData['Direccion'],
          ArrayData['TelCasa'],
          ArrayData['Cel'],
          ArrayData['Correo'],
          "<button data-id='" + ArrayData['ClienteId'] + "' data-name='" + ArrayData['Identidad'] + "' title='Ver Prestamos' data-toggle='tooltip' onClick='location.href=\"../Prestamos/Prestamo/" + ArrayData['ClienteId'] + "\"' id='btnVerRazones" + ArrayData['ClienteId'] + "' class='btn btn-success text-center btn-sm LSM_CRE010'><i class='fa fa-eye'></i></button> &nbsp" +
          "<button data-id='" + ArrayData['ClienteId'] + "' data-name='" + ArrayData['Identidad'] + "' title='Ver Depositos a Plazo' data-toggle='tooltip' onClick='location.href=\"../Depositos/Deposito/" + ArrayData['ClienteId'] + "\"' id='btnVerRazones" + ArrayData['ClienteId'] + "' class='btn btn-success text-center btn-sm'><i class='fa fa-eye'></i></button> &nbsp" +
          /*"<button data-id='" + ArrayData['TipoId'] + "' data-name='" + ArrayData['TipoDescripcion'] + "' title='Ver Razones' data-toggle='tooltip' onClick='VerRazones(event)' id='btnVerRazones" + ArrayData['TipoId'] + "' class='btn btn-success text-center btn-sm'><i class='fa fa-eye'></i></button>" +
          "<a data-id='" + ArrayData['TipoId'] + "' data-name='" + ArrayData['TipoDescripcion'] + "' title='Ver Razones' data-toggle='tooltip' href=\" ../Razones/razones/" + ArrayData['TipoId'] + " \" id='btnVerRazones" + ArrayData['TipoId'] + "' class='btn btn-success text-center btn-sm'><i class='fa fa-eye'></i></a>" +*/
          "<button data-id='" + ArrayData['ClienteId'] + "' data-name='" + ArrayData['Nombre'] + "' title='Editar Información del Cliente' data-toggle='tooltip' onClick='verCliente(event)' id='btnEditar" + ArrayData['ClienteId'] + "' class='btn btn-primary text-center btn-sm'><i class='fa fa-pencil-square-o'></i></button>",
          //"<button data-id='" + ArrayData['ClienteId'] + "' data-name='" + ArrayData['TipoDescripcion'] + "' title='Eliminar Tipo Razon' data-toggle='tooltip' onClick='EliminarTipoRazon(event)' id='btnEliminarTipoRazon" + ArrayData['ClienteId'] + "' class='btn btn-danger botonVED text-center btn-sm'><i class='fa fa-trash-o'></i></button>",
          ArrayData['ClienteId']
    ]);

    var theNode = $(tableID).dataTable().fnSettings().aoData[newRow[0]].nTr;
    theNode.setAttribute('id', ArrayData['ClienteId']);
    $('td', theNode)[11].setAttribute('class', 'ClienteId hidden');
}

//function EditarTipoRazon(e) {
//    e.stopPropagation();

//    var id = $(e.currentTarget).attr('data-id');
//    var desc = $(e.currentTarget).attr('data-name');

//    $("#theHeader").html("Editar Inormación | " + desc);
//    $('#moda_tipo_razones').modal('show');
//    $('#hidden_id').val(id);
//    GetTipoRazonBy(id);
//}


//EDITAR INFORMACION DEL CLIENTE

function verCliente(e) {
    e.stopPropagation();

    var id = $(e.currentTarget).attr('data-id');
    var desc = $(e.currentTarget).attr('data-name');

    $("#theHeader").html("Editar Información | " + desc);
    $('#moda_tipo_razones').modal('show');
    $('#hidden_id').val(id);
    GetInfoClientes(id);
}

function EliminarTipoRazon(e) {
    e.stopPropagation();
    //console.log('Cancel ticket');
    var id = $(e.currentTarget).attr('data-id');
    var desc = $(e.currentTarget).attr('data-name');
    $('#hidden_id').val(id);

    $("#theHeaderEliminar").html("Eliminar Tipo Razon | " + desc);
    $('#modalEliminarTipoRazon').modal('show');
    $('#modalmessage').html('¿Desea eliminar el tipo de razon: <b>' + desc + '</b>?');
}

$('#tiene_listado_x').change(function () {
    if ($('#tiene_listado_x').val() == 1) {
        $('#div_listado_extra').removeClass('hidden');
        $('#etiqueta').data('requerido', true);
        $('#origen').data('requerido', true);
        $('#codigo_listado').data('requerido', true);
    }
    else {
        $('#div_listado_extra').addClass('hidden');
        $('#etiqueta').data('requerido', false).val('');
        $('#origen').data('requerido', false).val('');
        $('#codigo_listado').data('requerido', false).val('');
    }
});

function NuevoTipoRazon(e) {
    e.stopPropagation();
    var id = -1;
    $("#theHeader").html("Nuevo Tipo Cliente");
    $('#hidden_id').val("");
    $("#moda_tipo_razones").find('input[type = "text"]').val("");
    $('#descripcion_tipo_razon').val('');
    $('#tiene_listado_x').val(0);
    $('#tipostatus').val('ACT');
    $('#tiene_listado_x').trigger('change');
    $('#moda_tipo_razones').modal('show');
}

$('#moda_tipo_razones').on('hidden.bs.modal', function (e) {
    limpiarMensajesValidacion($('#form'));
})

function GuardarClientes() {
    try {
        var inputs = [];
        var mensaje = [];

        /*Recorremos el contenedor para obtener los valores*/
        $('.requerido').each(function () {
            /*Llenamos los arreglos con la info para la validacion*/
            // if ($(this).data('requerido') == true) {
            //console.log($(this)[0].tagName);
            inputs.push('#' + $(this).attr('id'));
            mensaje.push($(this).attr('attr-message'));
            
        });

        var rtn = $('#rtn').val();
        var telres = $('#telcasa').val();
        var telref1 = $('#TelRef1').val();
        var telref2 = $('#TelRef2').val();
        var NumRef1 = $('#NumRef1').val();
        var NumRef2 = $('#NumRef2').val();
        
        if (rtn == "") {
            rtn = "N";
        }
        if (telres == "") {
            telres = "N";
        }
        if (telref1 == "") {
            telref1 = "N";
        }
        if (telref2 == "") {
            telref2 = "N";
        }
        if (NumRef1 == "") {
            NumRef1 = "1";
        }
        if (NumRef2 == "") {
            NumRef2 = "2";
        }
        if ($('#segundonombre').val() == "") {
            $('#segundonombre').val("N");
        }
        if ($('#segundoapellido').val() == "") {
            $('#segundoapellido').val("N");
        }

        /*Si la validación es correcta ejecuta el ajax*/
        if (Validar(inputs, mensaje)) {

            var path = urljs + 'Clientes/SaveData';
            var id = $('#hidden_id').val();
            console.log('id: ' + $('#hidden_id').val());

            if (id == "") {
                id = -1;
            }
            //JSON data
            var dataType = 'application/json; charset=utf-8';
            var data = {

                //DATOS GENERALES DEL CLIENTE
                ClienteId: id,
                Identidad: $('#identidad').val(),
                RTN: rtn,
                PrimerNombre: $('#primernombre').val(),
                SegundoNombre: $('#segundonombre').val(),
                PrimerApellido: $('#primerapellido').val(),
                SegundoApellido: $('#segundoapellido').val(),
                Genero: $('#genero option:selected').val(),
                Edad: $('#fecha').val(),
                EstadoCli: $('#estado option:selected').val(),
                EstadoCivil: $('#estcivil').val(),
                Direccion: $('#direccion').val(),
                TelCasa: telres,
                Cel: $('#celular').val(),
                Correo: $('#correo').val(),
                Profesion: $('#Profesion').val(),
                DireccionTrabajo: $('#DireccionTrabajo').val(),

                //PRIMERA REFERENCIA DEL CLIENTE
                RefNom1: $('#NomRef1').val(),
                RefId1: $('#IdRef1').val(),
                RefCel1: $('#CelRef1').val(),
                RefTel1: telref1,
                NumRef1: NumRef1,


                //SEGUNDA REFERENCIA DEL CLIENTE
                RefNom2: $('#NomRef2').val(),
                RefId2: $('#IdRef2').val(),
                RefCel2: $('#CelRef2').val(),
                RefTel2: telref2,
                NumRef2: NumRef2
            }

            var posting = $.post(path, data);

            posting.done(function (data, status, xhr) {
                $('#moda_tipo_razones').modal('hide');
                GenerarSuccessAlerta(data.Mensaje, "success");
                goAlert();
                LimpiarInput(inputs, ['']);
                GetTipoRazones();
            });

            posting.fail(function (data, status, xhr) {
                console.log(status);
            });
        }
    } catch (e) {
        console.log(e);
    }
}

function existeCliente(identidad, rtn) {
    var resultado = false;
    try {
        var path = urljs + "/Clientes/CheckOne";
        var posting = $.post(path, { identidad: identidad, rtn: rtn });
        posting.done(function (data, status, xhr) {
            console.log('Registros: ' + data.cantidadRegistros);
            if (data.cantidadRegistros > 0) {
                $('#moda_tipo_razones').modal('hide');
                GenerarErrorAlerta('Registro ya existe en la base de datos.', "error");
                goAlert();
                resultado = true;
            }
            else {
                GuardarClientes();
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

//PASA PARAMETROS PARA EDITAR CLIENTE
$("#btnEditar").on('click', function (e) {
    var accion = 1
    //var hoy = moment(new Date()).format('YYYY-MM-DD');
    var fecha = $('#fecha').val();
    //var fecha = moment(new Date()).format('YYYY-MM-DD');
    
    if ($('#cliCod').val() != '' ) {
        //AprobarSolicitudPrestamo(accion, $("#CodPres").val(), $("#montosolicitado").val());
        EditarCliente(accion, 

            $('#cliCod').val(),
            $('#identidad').val(),
            $('#rtn').val(),
            $('#primernombre').val(),
            $('#segundonombre').val(),
            $('#primerapellido').val(),
            $('#segundoapellido').val(),
            $('#genero option:selected').val(),
            $('#telcasa').val(),
            $('#celular').val(),
            $('#correo').val(),
            $('#Profesion option:selected').val(),
            $('#DireccionTrabajo').val(),
            fecha,
            $('#estado').val(),
            $('#estcivil').val(),
            $('#direccion').val(),
            $('#NomRef1').val(),
            $('#IdRef1').val(),
            $('#TelRef1').val(),
            $('#CelRef1').val(),       
            $('#NomRef2').val(),
            $('#IdRef2').val(),
            $('#TelRef2').val(),
            $('#CelRef2').val(),
            $('#NumRef1').val(),
            $('#NumRef2').val()

        );
        $("#btnGuardar").addClass("disabled");
        //$("#btnRechazar").addClass("disabled");
    }
});

//EDITA INFORMACION DEL CLIENTE
function EditarCliente(Accion, cliCod, id, rtn, nom1, nom2, ap1, ap2, gen, telcasa, cel, correo, Profesion, DireccionTrabajo, fech, estado, estadocivil, direccion, nomref1, idref1, telref1, celref1, nomref2, idref2, telref2, celref2, numref1, numref2) {
    try {
        var path = urljs + "/Clientes/EditarCliente";
        var posting = $.post(path, { Accion: Accion, cliCod: cliCod, id: id, rtn: rtn, nom1: nom1, nom2: nom2, ap1: ap1, ap2: ap2, gen: gen, telcasa: telcasa, cel: cel, correo: correo, Profesion: Profesion, DireccionTrabajo: DireccionTrabajo, fech: fech, estado: estado, estadoCivil: estadocivil, direccion: direccion, nomref1: nomref1, idref1: idref1, telref1: telref1, celref1: celref1, nomref2: nomref2, idref2: idref2, telref2: telref2, celref2: celref2, numref1: numref1, NumRef2: numref2 });
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
    location.reload();
}