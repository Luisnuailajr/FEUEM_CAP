
var btnFechar = $("#DetalhesModal");

btnFechar.on("click", fechar);

function fechar() {

    

    $("#minhaModal").modal("hide");
    abrirModalCurso();
}

function abrirModalCurso() {
    
    var controller = "Curso";

    $(document).ready(ConfigurarControles);

    function ConfigurarControles() {
        
        $("a.btn").click(function () {
            var acao = "AdicionarCurso";
            abrirModal(acao);
        });

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
                function () {
                    $("#minhaModal").modal('show');
                });
        }

    }

}
