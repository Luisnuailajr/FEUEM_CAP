var btnAcaoDocente = $("#gravarDocente");

var formularioDocente = $("#formCrudDocente");

btnAcaoDocente.on("click", submeterDocente);

function submeterDocente() {

    if (formularioDocente.valid()) {

        var urlDocente = formularioDocente.prop("action");
        var metodoDocente = formularioDocente.prop("method");
        var dadosFormularioDocente = formularioDocente.serialize();

        $.ajax({
            url: urlDocente,
            type: metodoDocente,
            data: dadosFormularioDocente,
            success: tratarRetornoDocente
        });
    }
}

function tratarRetornoDocente(resultadoServidor) {

    if (resultadoServidor.resultado) {
        toastr["success"](resultadoServidor.mensagem);
        window.location.href("@Url.Action('Index'','Docente')");

    } else {
        toastr["error"](resultadoServidor.mensagem);
    }
}