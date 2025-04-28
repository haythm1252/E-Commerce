$(document).ready(function () {
    $("#Image").on("change", function (e) {
        $(".imagePreview").attr("src", window.URL.createObjectURL(e.target.files[0])).removeClass("d-none");
        $(".no-image").addClass("d-none");
    });
});