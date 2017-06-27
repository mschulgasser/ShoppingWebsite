$(function () {
    $("#show-add-modal").on("click", function () {
        $(".modal").modal();
    });
    $("#add").on("click", function () {
        var params = {
            name: $("#name").val()
        }
        $.post("/admin/addcategory", params, function(){
            $(".modal").hide();
            window.location.reload();
        });
    });
    $(".delete").on("click", function () {
        var id = $(this).data('id');
        $.post("/admin/deletecategory", { id: id }, function () {
            window.location.reload();
        });
    });
    $(".edit").on('click', function () {
        var button = $(this);
        var name = button.data('name');
        var id = button.data('id');
        $("#category-name-" + id).text("");
        $("#category-name-" + id).append(`<input type="text" class="form-control" id="edit-input" value="`
            + name + `" />`);
        button.hide();
        $("#category-actions-" + id).append(`<button class="btn btn-warning update" data-id="` + id + `">Update</button>`);
        var id = button.data('id');
        $(".edit").prop("disabled", true);

    });
    $(".table").on("click", ".update", function () {
        var id = $(this).data('id');
        var params = {
            id: id,
            name: $("#edit-input").val()
        }
        console.log(params);
        $.post("/admin/updatecategory", params, function () {
            window.location.reload();
        });
    });
});