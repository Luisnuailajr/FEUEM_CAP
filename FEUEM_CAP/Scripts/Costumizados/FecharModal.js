
var btnFechar = $("#DetalhesModal");

btnFechar.on("click", fechar);

function fechar() {

    $("#minhaModal").modal("hide");

    $("#gridDados").bootgrid("reload");
}