var hoy = moment(new Date()).format('YYYY-MM-DD');

$(document).ready(function () {

    $('.monto').mask('#############', { translation: { '#': { pattern: /[0-9]/, optional: true } } });
    $('.fecha').mask('##########', { translation: { '#': { pattern: /[0-9-]/, optional: true } } });
    var FechaActual = 0
    GetDatosCajero(hoy, FechaActual);

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

    //$("#fecha1").on("dp.change", function (e) {
    //    $('#fecha2').data("DateTimePicker").minDate(e.date);
    //});
});



$("#btnAgregarRegCajero").on('click', function (e) {
    AgregaRegCajero();
        //$("#btnAprobar").addClass("disabled");
        //$("#btnRechazar").addClass("disabled");
});

function AgregaRegCajero() {
    try {
        var path = urljs + "/Caja/AgregaRegCajero";
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

$("#btnBuscarRegCajero").on('click', function (e) {
    $("#btnCierreCaja").addClass("hidden");
    $("#btnGuardarDatosArqueo").addClass("hidden");
    if ($("#FechaCajero").val() != '') {
        FechaActual=1
        GetDatosCajero($("#FechaCajero").val(),FechaActual);
        //$("#btnAprobar").addClass("disabled");
        //$("#btnRechazar").addClass("disabled");
    }
});

function GetDatosCajero(fecha,FechaActual) {
    var path = urljs + "/Caja/GetDatosCajero";
    var posting = $.post(path, { fecha: fecha, FechaActual: FechaActual });
    posting.done(function (data, status, xhr) {
        dataCitas = [];
        dataCitas = data;
        //e.stopPropagation();
        //$('#theHeader').html("Gestión de Solicitudes");
        if (dataCitas.length > 0) {
            for (var i = dataCitas.length - 1; i >= 0; i--) {
                //if (dataCitas[i].CA_Codigo_Cajero == cajero) {
                    $('#InicioDia').val(dataCitas[i].CA_Valor_Inicial_Dia);
                    $('#RecEfectivo').val(dataCitas[i].CA_Valor_Recib);
                    $('#EntrEfectivo').val(dataCitas[i].CA_Valor_Entrega);
                    $('#EntrTesorero').val(dataCitas[i].CA_Valor_Entrega_Supervisor);
                    $('#RecTesorero').val(dataCitas[i].CA_Valor_Recib_Supervisor);
                    $('#SaldoCaja').val(dataCitas[i].SaldoCaja);
                    $('#CA_B_1').val(dataCitas[i].CA_B_1);
                    $('#MontoB1').val(dataCitas[i].CA_B_1*1);
                    $('#CA_B_2').val(dataCitas[i].CA_B_2);
                    $('#MontoB2').val(dataCitas[i].CA_B_2*2);
                    $('#CA_B_5').val(dataCitas[i].CA_B_5);
                    $('#MontoB5').val(dataCitas[i].CA_B_5*5);
                    $('#CA_B_10').val(dataCitas[i].CA_B_10);
                    $('#MontoB10').val(dataCitas[i].CA_B_10*10);
                    $('#CA_B_20').val(dataCitas[i].CA_B_20);
                    $('#MontoB20').val(dataCitas[i].CA_B_20*20);
                    $('#CA_B_50').val(dataCitas[i].CA_B_50);
                    $('#MontoB50').val(dataCitas[i].CA_B_50*50);
                    $('#CA_B_100').val(dataCitas[i].CA_B_100);
                    $('#MontoB100').val(dataCitas[i].CA_B_100*100);
                    $('#CA_B_500').val(dataCitas[i].CA_B_500);
                    $('#MontoB500').val(dataCitas[i].CA_B_500*500);
                    $('#CA_M_1').val(dataCitas[i].CA_M_1);
                    $('#MontoM1').val(dataCitas[i].CA_M_1*0.01);
                    $('#CA_M_2').val(dataCitas[i].CA_M_2);
                    $('#MontoM2').val(dataCitas[i].CA_M_2*0.02);
                    $('#CA_M_5').val(dataCitas[i].CA_M_5);
                    $('#MontoM5').val(dataCitas[i].CA_M_5*0.05);
                    $('#CA_M_10').val(dataCitas[i].CA_M_10);
                    $('#MontoM10').val(dataCitas[i].CA_M_10*0.10);
                    $('#CA_M_20').val(dataCitas[i].CA_M_20);
                    $('#MontoM20').val(dataCitas[i].CA_M_20*0.20);
                    $('#CA_M_50').val(dataCitas[i].CA_M_50);
                    $('#MontoM50').val(dataCitas[i].CA_M_50 * 0.50);
                    $('#TotalMonedas').val(parseFloat($('#MontoM1').val()) +
                    parseFloat($('#MontoM2').val()) + parseFloat($('#MontoM5').val()) +
                    parseFloat($('#MontoM10').val()) + parseFloat($('#MontoM20').val()) +
                    parseFloat($('#MontoM50').val()));
                    $('#TotalBilletes').val(parseInt($('#MontoB1').val()) +
                    parseInt($('#MontoB2').val()) + parseInt($('#MontoB5').val()) +
                    parseInt($('#MontoB10').val()) + parseInt($('#MontoB20').val()) +
                    parseInt($('#MontoB50').val()) + parseInt($('#MontoB100').val()) +
                    parseInt($('#MontoB500').val()));
                    $('#TotalDinero').val(parseInt($('#TotalBilletes').val()) +
                    parseFloat($('#TotalMonedas').val()));
                    $('#CA_Secuencia').val(dataCitas[i].CA_Secuencia);
                    $('#CA_Cajero_Estado').val(dataCitas[i].CA_Cajero_Estado);
                    //$('#FechaCajero').format('YYYY-MM-DD');
                    var initialDate = dataCitas[i].CA_Fecha;

                    if (initialDate != null) {
                        //Dividimos la fecha primero utilizando el espacio para obtener solo la fecha y el tiempo por separado
                        var splitDate = initialDate.split(" ");
                        var date = splitDate[0].split("/");
                        var time = splitDate[1].split(":");

                        // Obtenemos los campos individuales para todas las partes de la fecha
                        var dd = date[0];
                        var mm = date[1];
                        var yyyy = date[2];

                        // Creamos la fecha con Javascript
                        var fecha = yyyy.concat("/", mm, "/", dd);
                    }

                
               
                    $('#FechaCajero').val(fecha);
                   

                //}
            }
            //activarTab('ver_cita');
        }
        //$('#ver_cita').modal('show');
    });
posting.fail(function (data, status, xhr) {
    GenerarErrorAlerta(xhr, "error");
    goAlert();
});
posting.always(function (data, status, xhr) {
    $('.nav-tabs a[href="#ver_cita"]').closest('li').addClass('hide');
});
  
}

$("#btnBuscarTransCajero").on('click', function (e) {
    //var cajero = 'jbonilla' || $("#CA_Secuencia").val() != null || $("#CA_Cajero_Estado").val() != 1;
    if ($("#CA_Secuencia").val() != '' && $("#CA_Secuencia").val() != null ) {
    GetPlanPago($("#FechaCajero").val());
    }
});

//SE AGREGA NUEVA FUNCIÓN.
function GetPlanPago(fecha) {
    try {
        var path = urljs + "/Caja/GetTransaccionesCajero";
        var posting = $.post(path, { fecha: fecha }); //Aqui se ponen las variables

        posting.done(function (data, status, xhr) {
            dataCitas = [];
            dataCitas = data;
            LimpiarTabla("#TransCajero");
            if (data.length > 0) {
                if (data[0].Accion > 0) {
                    var counter = 1;
                    for (var i = data.length - 1; i >= 0; i--) {
                        if (data[i].flag_historico == 0) {
                            addRowCitaPendiente(data[i], "#TransCajero", counter);
                        }
                        else {
                            addRowTransCajero(data[i], "#TransCajero", counter);
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
    $('#Transac_Cajero').modal('show');
}

function addRowTransCajero(ArrayData, tableID, counter) //Aqui se cambia y modifica Ver cita.*********************************************************
{
    var newRow = $(tableID).dataTable().fnAddData([

        ArrayData['TC_Numero'],
        ArrayData['TC_Cajero'],
        ArrayData['TC_Fecha'],
        ArrayData['TC_Mto_Entrega'],
        ArrayData['TC_Mto_Recib']

    ]);

    var theNode = $(tableID).dataTable().fnSettings().aoData[newRow[0]].nTr;
    theNode.setAttribute('Num', ArrayData['TC_Numero']);
    $('td', theNode)[5];
    //$('td', theNode)[5].setAttribute('class', 'TC_Numero');
}

//TRANSACCIONES DE CAJERO

$("#btnAgregarTransCajero").on('click', function (e) {
    if ($("#CA_Cajero_Estado").val() != 1) {
    e.stopPropagation();
    var id = -1;
    $("#theHeader").html("Transacciones de Cajero");
    $('#hidden_id').val("");
    $("#AgregaTransCajero").find('input[type = "text"]').val("");
    $('#descripcion_tipo_razon').val('');
    $('#tiene_listado_x').val(0);
    $('#tipostatus').val('ACT');
    $('#tiene_listado_x').trigger('change');
    $('#AgregaTransCajero').modal('show');
    $("#btnGeneraTrans").removeClass("disabled");
    }
    else {
        alert("No puede realizar transacciones de cajero cuando la caja ya fue cerrada para este dia.");
    }
});


$("#btnGeneraTrans").on('click', function (e) {
    //var Cajero = ((string)(Session["usuario"]))
    //if ($("#PrestamoId").val() != '') {
    GeneraTransCajero($("#Monto").val(), $("#TipoTransCaja").val());
    $("#btnGeneraTrans").addClass("disabled");
    //$("#btnGeneraRecibo").removeClass("disabled");  
    //}
});

function GeneraTransCajero(Monto, Tipo, Cajero) {
    try {
        var path = urljs + "/Caja/GeneraTransCajero";
        var posting = $.post(path, { Monto: Monto, Tipo:Tipo, Cajero:Cajero });
        posting.done(function (data, status, xhr) {

            dataResultado = [];
            dataResultado = data;
            alert(data.Mensaje);

            $("#btnGeneraComprobante").removeClass("disabled"); //despues de la transaccion aparece boton de imprimir comprobante

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

//Iteración de billetes
$('#CA_B_1').change(function (e) {
    var valor = $(this).val();
    $('#MontoB1').val(valor * 1);
});

$('#CA_B_2').change(function (e) {
    var valor = $(this).val();
    $('#MontoB2').val(valor * 2);
});

$('#CA_B_5').change(function (e) {
    var valor = $(this).val();
    $('#MontoB5').val(valor * 5);
});

$('#CA_B_10').change(function (e) {
    var valor = $(this).val();
    $('#MontoB10').val(valor * 10);
});

$('#CA_B_20').change(function (e) {
    var valor = $(this).val();
    $('#MontoB20').val(valor * 20);
});

$('#CA_B_50').change(function (e) {
    var valor = $(this).val();
    $('#MontoB50').val(valor * 50);
});

$('#CA_B_100').change(function (e) {
    var valor = $(this).val();
    $('#MontoB100').val(valor * 100);
});

$('#CA_B_500').change(function (e) {
    var valor = $(this).val();
    $('#MontoB500').val(valor * 500);
});

//Total Billetes
$('input').change(function (e) {
  
    $('#TotalBilletes').val(parseInt($('#MontoB1').val()) +
        parseInt($('#MontoB2').val()) + parseInt($('#MontoB5').val()) +
        parseInt($('#MontoB10').val()) + parseInt($('#MontoB20').val()) +
        parseInt($('#MontoB50').val()) + parseInt($('#MontoB100').val()) +
        parseInt($('#MontoB500').val()));
});

//Iteración de monedas

$('#CA_M_1').change(function (e) {
    var valor = $(this).val();
    $('#MontoM1').val(valor * 0.01);
});

$('#CA_M_2').change(function (e) {
    var valor = $(this).val();
    $('#MontoM2').val(valor * 0.02);
});

$('#CA_M_5').change(function (e) {
    var valor = $(this).val();
    $('#MontoM5').val(valor * 0.05);
});

$('#CA_M_10').change(function (e) {
    var valor = $(this).val();
    $('#MontoM10').val(valor * 0.10);
});

$('#CA_M_20').change(function (e) {
    var valor = $(this).val();
    $('#MontoM20').val(valor * 0.20);
});

$('#CA_M_50').change(function (e) {
    var valor = $(this).val();
    $('#MontoM50').val(valor * 0.50);
});

////Total Moneda
$('input').change(function (e) {

    $('#TotalMonedas').val(parseFloat($('#MontoM1').val()) +
        parseFloat($('#MontoM2').val()) + parseFloat($('#MontoM5').val()) +
        parseFloat($('#MontoM10').val()) + parseFloat($('#MontoM20').val()) +
        parseFloat($('#MontoM50').val()));
});

////Total Dinero
$('input').change(function (e) {

    $('#TotalDinero').val(parseInt($('#TotalBilletes').val()) +
                    parseFloat($('#TotalMonedas').val()));
});

var bandera=0;
$("#btnArqueoCaja").on('click', function (e) {
    //AgregaRegCajero();
    if ($("#CA_Cajero_Estado").val() != 1) {
        if ($("#CA_Secuencia").val() != '') {
        if (bandera==0)
        {
        $("#btnCierreCaja").removeClass("hidden");
        //$("#btnGuardarDatosArqueo").removeClass("hidden");
        $(".inputRequired").removeAttr("disabled");
        bandera=1
        }
        else
        {
            $("#btnCierreCaja").addClass("hidden");
            $("#btnGuardarDatosArqueo").addClass("hidden");
            $(".inputRequired").disabled = true;
            //$(".inputRequired").setattr("disabled");
            bandera = 0
        }
            //$("#btnRechazar").addClass("disabled");
        }
            }
            else
            {
                alert("No puede realizar modificaciones en los registros cuando la caja ya fue cerrada para la fecha actual.");
            }   
});

function disableBtn() {
    $(".inputRequired").disabled = false;
}

$("#btnGuardarDatosArqueo").on('click', function (e) {
    if ($("#CA_Secuencia").val() != '') {
    GuardarDatosArqueo($("#CA_Secuencia").val(),
                       $("#CA_B_1").val(),
                       $("#CA_B_2").val(),
                       $("#CA_B_5").val(),
                       $("#CA_B_10").val(),
                       $("#CA_B_20").val(),
                       $("#CA_B_50").val(),
                       $("#CA_B_100").val(),
                       $("#CA_B_500").val(),
                       $("#CA_M_1").val(),
                       $("#CA_M_2").val(),
                       $("#CA_M_5").val(),
                       $("#CA_M_10").val(),
                       $("#CA_M_20").val(),
                       $("#CA_M_50").val());
        //$("#btnAprobar").addClass("disabled");
        //$("#btnRechazar").addClass("disabled");
    }
});

function GuardarDatosArqueo(CA_Secuencia, CA_B_1, CA_B_2, CA_B_5, CA_B_10, CA_B_20, CA_B_50, CA_B_100, CA_B_500, CA_M_1, CA_M_2, CA_M_5, CA_M_10, CA_M_20, CA_M_50) {
    try {
        var path = urljs + "/Caja/GuardarDatosArqueo";
        var posting = $.post(path, { CA_Secuencia: CA_Secuencia, CA_B_1: CA_B_1, CA_B_2: CA_B_2, CA_B_5: CA_B_5, CA_B_10: CA_B_10, CA_B_20: CA_B_20, CA_B_50: CA_B_50, CA_B_100: CA_B_100, CA_B_500: CA_B_500, CA_M_1: CA_M_1, CA_M_2: CA_M_2, CA_M_5: CA_M_5, CA_M_10: CA_M_10, CA_M_20: CA_M_20, CA_M_50: CA_M_50 });
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




var Diferencia;
$("#btnCierreCaja").on('click', function (e) {
    var Mensaje;
    var txt;

    if ($("#CA_Secuencia").val() != '') {
        GuardarDatosArqueo($("#CA_Secuencia").val(),
            $("#CA_B_1").val(),
            $("#CA_B_2").val(),
            $("#CA_B_5").val(),
            $("#CA_B_10").val(),
            $("#CA_B_20").val(),
            $("#CA_B_50").val(),
            $("#CA_B_100").val(),
            $("#CA_B_500").val(),
            $("#CA_M_1").val(),
            $("#CA_M_2").val(),
            $("#CA_M_5").val(),
            $("#CA_M_10").val(),
            $("#CA_M_20").val(),
            $("#CA_M_50").val());
        //$("#btnAprobar").addClass("disabled");
        //$("#btnRechazar").addClass("disabled");
    }

    if ($("#CA_Cajero_Estado").val() != 1) {
    if (confirm("Esta seguro en realizar el Cierre de Caja para este dia?")) {
        Diferencia = parseFloat($("#TotalDinero").val()) - parseFloat($("#SaldoCaja").val());
        //var dif = Diferencia.toFixed(2);
        

        if (Diferencia < 0) {
            alert("Faltante en Caja de" + " " + Diferencia);
                Autorizacion_Cierre();
                //CierreCaja($("#CA_Secuencia").val(), Diferencia)
                //Mensaje = 'Faltante en Caja';
            }
        else
            if (Diferencia > 0) {
                alert("Sobrante en Caja" + " " + Diferencia);
                CierreCaja($("#CA_Secuencia").val(), Diferencia)
                    //Mensaje = 'Sobrante en Caja';
                }
            else
                if (Diferencia == 0) {
                        alert("Caja Cuadrada");
                    CierreCaja($("#CA_Secuencia").val(), Diferencia)
                        //Mensaje = 'Caja Cuadrada'
                    }
            //CierreCaja($("#CA_Secuencia").val(), Diferencia)
    } else {
        txt = "You pressed Cancel!";
        }
    } else {
        alert("El cierre de caja para este dia ya fue realizado, Proceso Cancelado");
    }
    
});

function CierreCaja(Secuencia, Diferencia) {
    try {
        var path = urljs + "/Caja/CierreCaja";
        var posting = $.post(path, { Secuencia: Secuencia, Diferencia: Diferencia });
        posting.done(function (data, status, xhr) {

            dataResultado = [];
            dataResultado = data;
            alert(data.Mensaje);
            GetEstadoCajero($('#FechaCajero').val(),Secuencia);
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

function Autorizacion_Cierre() {
    //var path = urljs + "/Caja/ValidaCajero";
    //var posting = $.post(path, {});
    //posting.done(function (data, status, xhr) {

    //    dataResultado = [];
    //    dataResultado = data;
    //    e.stopPropagation();
    //    var id = -1;
        $("#theHeader").html("Autorización para Cierre");
        $('#hidden_id').val("");
        $("#Moda_Autorizacion_Cierre").find('input[type = "text"]').val("");
        $('#descripcion_tipo_razon').val('');
        $('#tiene_listado_x').val(0);
        $('#tipostatus').val('ACT');
        $('#tiene_listado_x').trigger('change');
        $('#Moda_Autorizacion_Cierre').modal('show');
        //$("#btnGeneraPago").removeClass("disabled");
        //$("#btnGeneraRecibo").addClass("disabled");
    //});
}

$('#Autorizar').click(function (e) {
    var user = $("#user").val();
    var pass = $("#password").val();
    var CodigoModulo = "SGRC";
    if (user != "" && pass != "") {
        try {
            var path = urljs + "/Caja/AutorizaCierre";
            var posting = $.post(path, { user: user, pass: pass, CodigoModulo: CodigoModulo });
            posting.done(function (data, status, xhr) {
                //console.log(data);
                dataResultado = [];
                dataResultado = data;
                if (data.Estado == 1) {
                    alert(data.Mensaje);
                    CierreCaja($("#CA_Secuencia").val(), Diferencia)
                    $('#Moda_Autorizacion_Cierre').modal('hide');
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

//trae estado del cajero para validar la aparicion del boton de imprimir boton arqueo
function GetEstadoCajero(Fecha, SecuenciaE) {
    var path = urljs + "/Caja/GetEstadoCajero";
    var posting = $.post(path, { Fecha: Fecha, SecuenciaE: SecuenciaE });
    posting.done(function (data, status, xhr) {
        dataCitas = [];
        dataCitas = data;
        //e.stopPropagation();
        //$('#theHeader').html("Gestión de Solicitudes");
        if (dataCitas.length > 0) {
            for (var i = dataCitas.length - 1; i >= 0; i--) {
                //if (dataCitas[i].CA_Codigo_Cajero == cajero) {
                $('#CajeroEstado').val(dataCitas[i].CA_Cajero_Estado); //MODIFICAR DESPUES CON VALOR CORRECTO

                //}
            }

            //aparece boton imprimir reporte arqueo
            $("#btnImprimirReporteArqueo").removeClass("hidden");

            //valida color del boton
            if ($("#CajeroEstado").val() == -1 || $("#CajeroEstado").val() == 0) {
                $("#btnImprimirReporteArqueo").addClass("btn-danger");
            } else if ($("#CajeroEstado").val() == 1) {
                $("#btnImprimirReporteArqueo").addClass("btn-success");
            }

            //activarTab('ver_cita');
        }
        //$('#ver_cita').modal('show');
    });

    posting.fail(function (data, status, xhr) {
        GenerarErrorAlerta(xhr, "error");
        goAlert();
    });
}

//Valida  estado de la sesion de la caja al hacer click al boton 
//si es 0 o -1 mande alerta de error que no puede imprimir el reporte
// si es 1 imprimir reporte
$("#btnImprimirReporteArqueo").on('click', function (e) {
    if ($("#CajeroEstado").val() == -1 || $("#CajeroEstado").val() == 0) { //si la sesion esta abierta
        alert("Error al  imprimir el reporte: Aun no se ha cerrado caja para la sesion actual");
        $("#btnImprimirReporteArqueo").addClass("hidden");
        window.location.reload();
    } else if ($("#CajeroEstado").val() == 1) { //si la session esta cerrada
        
        var fecha = $("#FechaCajero").val()
        GetDatosReporteArqueo(fecha); //Genera pdf del reporte de arqueo caja

    }
});

//Trae los datos necesarios para el reporte de arqueo para luego meterlos como parametros  
//en la funcion GeneraPdfReporteArqueo que genera el pdf 
function GetDatosReporteArqueo(fecha) {

    try {
        var path = urljs + "/Caja/GetDatosReporteArqueo";
        var posting = $.post(path, { fecha: fecha });
        posting.done(function (data, status, xhr) {
            dataCitas = [];
            dataCitas = data;
            if (dataCitas.length > 0) {
                for (var i = dataCitas.length - 1; i >= 0; i--) {
                    NombreCajero = dataCitas[i].NombreCajero
                    CA_Fecha = dataCitas[i].CA_Fecha
                    CA_Valor_Inicial_Dia = dataCitas[i].CA_Valor_Inicial_Dia
                    CA_Valor_Recib = dataCitas[i].CA_Valor_Recib
                    CA_Valor_Entrega = dataCitas[i].CA_Valor_Entrega
                    CA_Valor_Recib_Supervisor = dataCitas[i].CA_Valor_Recib_Supervisor
                    CA_Valor_Entrega_Supervisor = dataCitas[i].CA_Valor_Entrega_Supervisor
                    CA_B_1 = dataCitas[i].CA_B_1
                    CA_B_2 = dataCitas[i].CA_B_2
                    CA_B_5 = dataCitas[i].CA_B_5
                    CA_B_10 = dataCitas[i].CA_B_10
                    CA_B_20 = dataCitas[i].CA_B_20
                    CA_B_50 = dataCitas[i].CA_B_50
                    CA_B_100 = dataCitas[i].CA_B_100
                    CA_B_500 = dataCitas[i].CA_B_500
                    CA_M_1 = dataCitas[i].CA_M_1
                    CA_M_2 = dataCitas[i].CA_M_2
                    CA_M_5 = dataCitas[i].CA_M_5
                    CA_M_10 = dataCitas[i].CA_M_10
                    CA_M_20 = dataCitas[i].CA_M_20
                    CA_M_50 = dataCitas[i].CA_M_50
                    Tot_B1 = dataCitas[i].Tot_B1
                    Tot_B2 = dataCitas[i].Tot_B2
                    Tot_B5 = dataCitas[i].Tot_B5
                    Tot_B10 = dataCitas[i].Tot_B10
                    Tot_B20 = dataCitas[i].Tot_B20
                    Tot_B50 = dataCitas[i].Tot_B50
                    Tot_B100 = dataCitas[i].Tot_B100
                    Tot_B500 = dataCitas[i].Tot_B500
                    Tot_M1 = dataCitas[i].Tot_M1
                    Tot_M2 = dataCitas[i].Tot_M2
                    Tot_M5 = dataCitas[i].Tot_M5
                    Tot_M10 = dataCitas[i].Tot_M10
                    Tot_M20 = dataCitas[i].Tot_M20
                    Tot_M50 = dataCitas[i].Tot_M50
                    SaldoEnCaja = dataCitas[i].SaldoEnCaja
                    TotalBilletes = dataCitas[i].TotalBilletes
                    TotalMonedas = dataCitas[i].TotalMonedas
                    TotalArqueo = dataCitas[i].TotalArqueo
                    Sobrante = dataCitas[i].Sobrante
                    Faltante = dataCitas[i].Faltante
                    EstadoCajero = dataCitas[i].EstadoCajero

                    GeneraPdfReporteArqueo(NombreCajero, CA_Fecha, CA_Valor_Inicial_Dia, CA_Valor_Recib, CA_Valor_Entrega, CA_Valor_Recib_Supervisor,
                        CA_Valor_Entrega_Supervisor, CA_B_1, CA_B_2, CA_B_5, CA_B_10, CA_B_20, CA_B_50, CA_B_100, CA_B_500, CA_M_1, CA_M_2, CA_M_5,
                        CA_M_10, CA_M_20, CA_M_50, Tot_B1, Tot_B2, Tot_B5, Tot_B10, Tot_B20, Tot_B50, Tot_B100, Tot_B500, Tot_M1, Tot_M2, Tot_M5, Tot_M10,
                        Tot_M20, Tot_M50, SaldoEnCaja, TotalBilletes, TotalMonedas,
                        TotalArqueo, Sobrante, Faltante, EstadoCajero)
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

//Genera Pdf para el reporte de arqueo
function GeneraPdfReporteArqueo(NombreCajero, CA_Fecha, CA_Valor_Inicial_Dia, CA_Valor_Recib, CA_Valor_Entrega, CA_Valor_Recib_Supervisor,
                                CA_Valor_Entrega_Supervisor, CA_B_1, CA_B_2, CA_B_5, CA_B_10, CA_B_20, CA_B_50, CA_B_100, CA_B_500, CA_M_1, CA_M_2, CA_M_5,
                                CA_M_10, CA_M_20, CA_M_50, Tot_B1, Tot_B2, Tot_B5, Tot_B10, Tot_B20, Tot_B50, Tot_B100, Tot_B500, Tot_M1, Tot_M2, Tot_M5, Tot_M10,
                                Tot_M20, Tot_M50, SaldoEnCaja, TotalBilletes, TotalMonedas,
                                TotalArqueo, Sobrante, Faltante, EstadoCajero) {
    var path = urljs + "/Caja/GeneraPdfReporteArqueo";
    var posting = $.post(path, {
        NombreCajero: NombreCajero,
        CA_Fecha: CA_Fecha,
        CA_Valor_Inicial_Dia: CA_Valor_Inicial_Dia,
        CA_Valor_Recib: CA_Valor_Recib,
        CA_Valor_Entrega: CA_Valor_Entrega,
        CA_Valor_Recib_Supervisor: CA_Valor_Recib_Supervisor,
        CA_Valor_Entrega_Supervisor: CA_Valor_Entrega_Supervisor,
        CA_B_1: CA_B_1,
        CA_B_2: CA_B_2,
        CA_B_5: CA_B_5,
        CA_B_10:CA_B_10,
        CA_B_20:CA_B_20,
        CA_B_50: CA_B_50,
        CA_B_100: CA_B_100,
        CA_B_500: CA_B_500,
        CA_M_1: CA_M_1,
        CA_M_2: CA_M_2,
        CA_M_5: CA_M_5,
        CA_M_10: CA_M_10,
        CA_M_20: CA_M_20,
        CA_M_50: CA_M_50,
        Tot_B1: Tot_B1,
        Tot_B2: Tot_B2,
        Tot_B5: Tot_B5,
        Tot_B10: Tot_B10,
        Tot_B20: Tot_B20,
        Tot_B50: Tot_B50,
        Tot_B100: Tot_B100,
        Tot_B500: Tot_B500,
        Tot_M1: Tot_M1,
        Tot_M2: Tot_M2,
        Tot_M5: Tot_M5,
        Tot_M10: Tot_M10,
        Tot_M20: Tot_M20,
        Tot_M50: Tot_M50,
        SaldoEnCaja:SaldoEnCaja,
        TotalBilletes:TotalBilletes,
        TotalMonedas: TotalMonedas,
        TotalArqueo: TotalArqueo,
        Sobrante: Sobrante,
        Faltante: Faltante,
        EstadoCajero: EstadoCajero
    });

    posting.done(function (data, status, xhr) {
        console.log(data);
        if (data == 1) {
            window.open('/PDF/FormatoArqueoCaja1.pdf');
            window.location.reload(); //refresca pagina para que se borren los datos de arqueo anterior y quite boton de imprimir reporte
        }
    });

}

$("#btnImprimirReporteArqueo").on('click', function (e) {
    var recibo = $("#NumTrans").val()
    GetDatosReciboTC(recibo);    
});

