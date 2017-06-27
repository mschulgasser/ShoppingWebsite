$(function () {
    $("#show-add-modal").on('click', function () {
        $(".modal").modal();
    });
    var imageCount = 0;
    $("#add-image").on('click', function() {
        $(".modal-body").append(`<input type="file" class="form-control" name="files[` + imageCount + `]"/>`);
        imageCount++;
    });
    $(".delete").on('click', function () {
        var id = $(this).data('id');
        console.log(id);
        $.post("/admin/deleteproduct", { id: id }, function () {
            window.location.reload();
        });
    });
    $(".edit").on('click', function () {
        var id = $(this).data('id');
        var button = $(this);
        $("#category-id").val(button.data("category-id"));
        $("#title").val(button.data("title"));
        $("#description").val(button.data("description"));
        $("#price").val(button.data("price"));
        $("#add-form").prop("action", "/admin/updateproduct");
        $(".modal-title").text("Edit Product");
        $(".modal-button").text("Update");
        $(".modal-body").append(`<input type="hidden" value="` + id + `" name="id"></input>`);
        //$.get("/admin/getimages", { productId: id }, function (data) {
        //    alert('in getimages');
        //    var imageCount2 = 0;
        //    data.images.forEach(function (image) {
        //        console.log(image);
        //        $(".modal-body").append(`<input type="file" class="form-control" value="` + image.fileName +  `" name="files[` + imageCount2 + `]"/>`);
        //        imageCount2++;
        //    });
        //});
        $(".modal").modal();
    });
});