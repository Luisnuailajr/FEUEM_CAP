
var controller = "Categoria";

function ConfigurarControles() {
       
    $("a.categoria").click(function () {
        var acao = $(this).data("action");
        abrirModal(acao);
    });
}


function abrirModal(acao, parametro) {
    var url = "/{ctrl}/{acao}/{parametro}";
    url = url.replace("{ctrl}", controller);
    url = url.replace("{acao}", acao);

    if (parametro != null) {

        url = url.replace("{parametro}", parametro);

    } else {

        url = url.replace("{parametro}", "");
    }

    $("#conteudoModal").load(url,
        function() {
            $("#minhaModal").modal('show');
        });
}