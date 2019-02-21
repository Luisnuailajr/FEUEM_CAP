
var btnAcao = $("input[type='button']");
var formulario = $("#formCrud");

btnAcao.on("click", submeter);

function submeterGravacao() {
    if (formulario.valid()) {

        var url = formulario.prop("action");
        var metodo = formulario.prop("method");
        var dadosFormulario = formulario.serialize();

        $.ajax({
            url: url,
            type: metodo,
            data: dadosFormulario,
            success: tratarRetorno
        });
    }
}
function tratarRetorno(resultadoServidor) {

    if (resultadoServidor.resultado) {

        toastr["success"](resultadoServidor.mensagem);

        window.location.href = '@Url.Action("Curso","Index")';

    } else {

        toastr["error"](resultadoServidor.mensagem);

    }
}

function mostrarMensagem(resultadoServidor) {

    if (resultadoServidor.resultado) {

        toastr["success"](resultadoServidor.mensagem);
    } else {

        toastr["error"](resultadoServidor.mensagem);

    }
}