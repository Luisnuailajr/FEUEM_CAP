var btnAcaoDep = $("#GravarDep");

var formularioDep = $("#FormularioAdicaoDep");

btnAcaoDep.on("click", submeter);

function submeter() {

    if (formularioDep.valid()) {

        var url = formularioDep.prop("action");
        var metodo = formularioDep.prop("method");
        var dadosFormulario = formularioDep.serialize();

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

        //$("#minhaModal").modal("hide");

        //$("#gridDados").bootgrid("reload");

    } else {

        toastr["error"](resultadoServidor.mensagem);

    }
}
