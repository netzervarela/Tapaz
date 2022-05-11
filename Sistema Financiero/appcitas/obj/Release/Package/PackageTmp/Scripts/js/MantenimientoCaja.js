var hoy = moment(new Date()).format('YYYY-MM-DD');

$(document).ready(function () {
    //ListaTiposTrans();
    //ValidaCajero();

    $('.monto').mask('#############', { translation: { '#': { pattern: /[0-9.]/, optional: true } } });
    $('.varchar100').mask('AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA', { translation: { 'A': { pattern: /[A-Za-z ÁÉÍÓÚáéúíóúÑñÄËÏÖÜäëïöüÀÈÌÒÙàèìòùÃÕãõçÇÂâÊêÎîÔôÛû]/, optional: true } } });
    $('.varchar50').mask('AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA', { translation: { 'A': { pattern: /[A-Za-z ÁÉÍÓÚáéúíóúÑñÄËÏÖÜäëïöüÀÈÌÒÙàèìòùÃÕãõçÇÂâÊêÎîÔôÛû 0-9.,-]/, optional: true } } });

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

    //$("#fechaCierre").on("dp.change", function (e) {
    //    $('#fechaCierre').data("DateTimePicker").minDate(e.date);
    //});
});

function Mantenimiento_dias(e) {
    var path = urljs + "/Caja/ValidaCajero";
    var posting = $.post(path, {});
    posting.done(function (data, status, xhr) {

        dataResultado = [];
        dataResultado = data;
            e.stopPropagation();
            var id = -1;
            $("#theHeader").html("Mantenimiento de Cierres");
            $('#hidden_id').val("");
            $("#Moda_mantenimiento_dias").find('input[type = "text"]').val("");
            $('#descripcion_tipo_razon').val('');
            $('#tiene_listado_x').val(0);
            $('#tipostatus').val('ACT');
            $('#tiene_listado_x').trigger('change');
            $('#Moda_mantenimiento_dias').modal('show');
            $("#btnGeneraPago").removeClass("disabled");
            $("#btnGeneraRecibo").addClass("disabled");
    });
  }


$("#btnBuscar").on('click', function (e) {
    //var recibo = $("#NumRecibo").val()
    GetCierreDia($("#fechaCierre").val(), $("#Cajero").val());
});

function GetCierreDia(Fecha, Cajero) {
    
    try {
        var path = urljs + "/Caja/GetCierreDia";
        var posting = $.post(path, { Fecha: Fecha, Cajero:Cajero });
        posting.done(function (data, status, xhr) {
            dataCitas = [];
            dataCitas = data;
            if (dataCitas.length > 0) {
                for (var i = dataCitas.length - 1; i >= 0; i--) {
                    $('#EstadoDia').val(dataCitas[i].CA_Cajero_Estado);
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

$("#btnGuardarDia").on('click', function (e) {
    //var recibo = $("#NumRecibo").val()
    ActualizaDiaCierre($("#fechaCierre").val(), $("#Cajero").val(), $("#EstadoDia").val());
});

function ActualizaDiaCierre(Fecha, Cajero, Estado) {
    var path = urljs + "/Caja/ActualizaDiaCierre";
    var posting = $.post(path, { Fecha:Fecha, Cajero:Cajero, Estado:Estado });
    posting.done(function (data, status, xhr) {

        dataResultado = [];
        dataResultado = data;
        //$("#NumTrans").val(data.Num);
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

//PROYECCIÓN PLAN DE PAGO

function Proyeccion_Plan(e) {
    //var path = urljs + "/Prestamos/ProyeccionPlanPago";
    //var posting = $.post(path, {});
    //posting.done(function (data, status, xhr) {

    //    dataResultado = [];
    //    dataResultado = data;
        e.stopPropagation();
        var id = -1;
        $("#theHeader").html("Proyección de Plan de Pago");
        $('#hidden_id').val("");
        $("#Proyeccion_Plan_Pago").find('input[type = "text"]').val("");
        $('#descripcion_tipo_razon').val('');
        $('#tiene_listado_x').val(0);
        $('#tipostatus').val('ACT');
        $('#tiene_listado_x').trigger('change');
        $('#Proyeccion_Plan_Pago').modal('show');
   // });
}

$("#btnGenerarProyeccion").on('click', function (e) {
    var monto = $('#montosolicitadoPr').val();
    var tasa = $('#tasainteresPr').val();
    var plazo = $('#plazoPr').val();
    var frecuency = $('#frecuenciapagoPr').val();
    var fecha = $('#FechaPr').val();
    var tipo = $('#TipoCuotaPr').val();
    GeneraPlanPago(monto, tasa, plazo, frecuency, fecha, tipo);
});

//Plan de pago.
function GeneraPlanPago(monto, tasa, plazo, frecuency, fecha, tipo) {
    try {
        var path = urljs + "/Prestamos/GeneraPlanPago";
        var posting = $.post(path, { monto: monto, tasa: tasa, plazo: plazo, frecuency: frecuency, fecha: fecha, tipo: tipo }); //Aqui se ponen las variables

        posting.done(function (data, status, xhr) {
            dataCitas = [];
            dataCitas = data;
            LimpiarTabla("#tablePlanPagoPr");
            if (data.length > 0) {
                if (data[0].Accion > 0) {
                    var counter = 1;
                    for (var i = data.length - 1; i >= 0; i--) {
                        if (data[i].flag_historico == 0) {
                            addRowCitaPendiente(data[i], "#tablePlanPagoPr", counter);
                        }
                        else {
                            addRowPlanPagoPr(data[i], "#tablePlanPagoPr", counter);
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
    $('#plan_pago_Pr').modal('show');
}

function addRowPlanPagoPr(ArrayData, tableID, counter) {
    var newRow = $(tableID).dataTable().fnAddData([

        ArrayData['Num'],
        ArrayData['Fecha'],
        ArrayData['Capital'],
        ArrayData['Interes'],
        ArrayData['Total'],
        ArrayData['Saldo'],
        //ArrayData['Codigo']

    ]);

    var theNode = $(tableID).dataTable().fnSettings().aoData[newRow[0]].nTr;
    theNode.setAttribute('id', ArrayData['Num']);
    $('td', theNode)[5]
}