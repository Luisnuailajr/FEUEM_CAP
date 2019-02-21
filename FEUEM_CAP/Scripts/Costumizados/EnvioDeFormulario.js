
var btnAcao = $("#botaoDeAccao");
var formulario = $("#formCrud");

btnAcao.on("click", submeter);

function submeter() {
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

        $("#minhaModal").modal("hide");

        $("#gridDados").bootgrid("reload");

    } else {

        toastr["error"](resultadoServidor.mensagem);

    }
}