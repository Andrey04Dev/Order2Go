// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).on('submit', '#frm_registrar', function (e) {
    e.preventDefault();
    $.ajax({
        beforeSend: function () {
            $("#frm_registrar button[type=submit]").prop("disabled", true);
        },
        type: this.method,
        url: this.action,
        data: $(this).serialize(),
        success: function (data) {
            alert("Usuario registrado con éxito.");
        },
        error: function (xhr, status) {
            alert(xhr.responseJSON.Message);
        },
        complete: function () {
            $("#frm_registrar button[type=submit]").prop("disabled", false);
        }
    })
})
/*$(document).on('submit', '#frm_login', function (e) {
    e.preventDefault();
    $.ajax({
        beforeSend: function () {
            $("#frm_login button[type=submit]").prop("disabled", true);
        },
        type: this.method,
        url: this.action,
        data: $(this).serialize(),
        success: function (data) {
            console.log(data)
            alert("Bienvenido " + data.nombre);
           // window.location = "/Home";
        },
        error: function (xhr, status) {
            alert(xhr.responseJSON.Message);
        },
        complete: function () {
            $("#frm_login button[type=submit]").prop("disabled", false);
        }
    })
})*/
$(document).on('submit', '#frm_agregarOperador', function (e) {
    e.preventDefault();
    $.ajax({
        beforeSend: function () {
            $("#frm_agregarOperador button[type=submit]").prop("disabled", true);
        },
        type: this.method,
        url: this.action,
        data: $(this).serialize(),
        success: function (data) {
            alert("El operador " + data.nombre + "ha sido registrado");
        },
        error: function (xhr, status) {
            alert(xhr.responseJSON.Message);
        },
        complete: function () {
            $("#frm_agregarOperador button[type=submit]").prop("disabled", false);
        }
    })
})
$(document).on('submit', '#frm_AgregarComercios', function (e) {
    e.preventDefault();
    $.ajax({
        beforeSend: function () {
            $("#frm_AgregarComercios button[type=submit]").prop("disabled", true);
        },
        type: this.method,
        url: this.action,
        data: $(this).serialize(),
        success: function (data) {
            alert("El comercio creado es " + data.nombre);
            console.log(data);
        },
        error: function (xhr, status) {
            alert(xhr.responseJSON.Message);
        },
        complete: function () {
            $("#frm_AgregarComercios button[type=submit]").prop("disabled", false);
        }
    })
})
/*$(document).on('submit', '#frm_AgregarProductoCarrito', function (e) {
    e.preventDefault();
    var IdCompra = 0;
    $.ajax({
        beforeSend: function () {
            $("#frm_AgregarProductoCarrito button[type=submit]").prop("disabled", true);
        },
        type: this.method,
        url: this.action,
        data: $(this).serialize(),
        success: function (data) {
            alert("Se ha agregado al carrito correctamente.");
            console.log(data.resultado);
            window.location("/Usuario/Index")
        },
        error: function (xhr, status) {
            alert("Algo salio mal");
        },
        complete: function () {
            $("#frm_AgregarProductoCarrito button[type=submit]").prop("disabled", false);
        }
    })
})*/
function ResetFields() {
    $("#nombre").val("")
    $("#cantidad").val("")
    $("#descripcion").val("")
    $("#precio").val("")
}