var hoy = moment(new Date()).format('YYYY-MM-DD');

$(document).ready(function () {
    ListaTiposTrans();
    $("#FechaAj").addClass("hidden");
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

    $("#fecha1").on("dp.change", function (e) {
        $('#fecha2').data("DateTimePicker").minDate(e.date);
    });
});

function Otros_Ingresos(e) {
    var path = urljs + "/Caja/ValidaCajero";
    var posting = $.post(path, {});
    posting.done(function (data, status, xhr) {

        dataResultado = [];
        dataResultado = data;
        if ((data.Estado) != 1 && (data.Estado) != 2) {
            e.stopPropagation();
            var id = -1;
            $("#theHeader").html("Otros Ingresos");
            $('#hidden_id').val("");
            $("#Moda_otras_transacciones").find('input[type = "text"]').val("");
            $('#descripcion_tipo_razon').val('');
            $('#tiene_listado_x').val(0);
            $('#tipostatus').val('ACT');
            $('#tiene_listado_x').trigger('change');
            $('#Moda_otras_transacciones').modal('show');
            $("#btnGeneraPago").removeClass("disabled");
            $("#btnGeneraRecibo").addClass("disabled");
        }
        else {
            alert(data.Mensaje);
        }
    });
}

//LISTA DE Tipos de Transacciones
function ListaTiposTrans() {
    $("#ListaTiposTrans").empty().append(new Option('', '-2'));
    try {
        //var path = urljs + "/sucursales/GetAll";
        var path = urljs + "/Caja/ListaTiposTrans";
        var posting = $.post(path);
        posting.done(function (data, status, xhr) {
            if (data.length) {
                if (data[0].Accion > 0) {
                    var contador = 0;
                    //$("#ListaTiposTrans").empty().append(new Option('Todos los Estados', '3'));
                    for (var i = data.length - 1; i >= 0; i--) {
                        $("#ListaTiposTrans").append(new Option(data[contador].OTT_Descripcion, data[contador].OTT_Codigo));
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
            GenerarErrorAlerta(xhr, "error");
            goAlert();
        });
        posting.always(function (data, status, xhr) {
            //RemoveAnimation("body");
        });
    }
    catch (e) {

    }
}

var bandera = 0;
$("#btnAjuste").on('click', function (e) {
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

$("#btnGeneraPago").on('click', function (e) {
    //var ajuste = 1;
    var Fecha = $("#txt_fecha1").val();
    if (bandera == 1) {
    var path = urljs + "/Caja/ValidaCajeroAjuste";
    var posting = $.post(path, { Fecha: Fecha});
    posting.done(function (data, status, xhr){

        dataResultado = [];
        dataResultado = data;
        if ((data.Estado) != 1 && (data.Estado) != 2) {
    GenerarAjuste($("#Monto").val(), $("#Observacion").val(), $("#ListaTiposTrans").val(), $("#Clave").val(), $("#txt_fecha1").val());
    $("#btnGeneraPago").addClass("disabled");
    $("#btnGenerarSolicitud").removeClass("disabled");
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
            if ((data.Estado) != 1 && (data.Estado) != 2) {
            GenerarPago($("#Monto").val(), $("#Observacion").val(), $("#ListaTiposTrans").val(), $("#Clave").val());
            $("#btnGeneraPago").addClass("disabled");
            $("#btnGenerarSolicitud").removeClass("disabled");
        }
        else {
            alert(data.Mensaje);
        }
     });
    }
});

function GenerarPago(Monto, Observacion, ListaTiposTrans,Clave) {
        var path = urljs + "/Caja/OtrosPagos";
        var posting = $.post(path, { Monto: Monto, Observacion: Observacion, ListaTiposTrans: ListaTiposTrans, Clave:Clave});
        posting.done(function (data, status, xhr) {

            dataResultado = [];
            dataResultado = data;
            $("#NumTrans").val(data.Num);
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

function GenerarAjuste(Monto, Observacion, ListaTiposTrans, Clave, FechaAjuste) {
    var path = urljs + "/Caja/OtrosPagosAjuste";
    var posting = $.post(path, { Monto: Monto, Observacion: Observacion, ListaTiposTrans: ListaTiposTrans, Clave: Clave, FechaAjuste: FechaAjuste });
    posting.done(function (data, status, xhr) {

        dataResultado = [];
        dataResultado = data;
        $("#NumTrans").val(data.Num);
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
