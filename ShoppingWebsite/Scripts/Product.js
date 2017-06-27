$(function () {
    $(".thumb").on('click', function () {
        var src = $(this).attr("src");
        $("#large-image").prop("src", src);
    });
});