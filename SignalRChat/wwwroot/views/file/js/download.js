$(document).ready(function () {
    $(".download-btn").on("click", function (event) {
        event.preventDefault();

        var guid = $(this).data("id");

        document.querySelector(".loader").classList.remove("d-none");
        document.querySelectorAll("dl.row.file-details *:not(.loader)").forEach((item) => {
            item.classList.add("d-none")
        });
        document.querySelector("dl.row.file-details").classList.add("d-flex");
        document.querySelector("dl.row.file-details").classList.add("justify-content-center");

        var form = $('<form action="/file/download/' + guid + '" method="post"></form>');
        form.append('<input type="hidden" name="guid" value="' + guid + '" />');
        $('body').append(form);

        form.submit();
        form.remove();

        document.querySelector(".loader").classList.add("d-none");
        document.querySelectorAll("dl.row.file-details *:not(.loader)").forEach((item) => {
            item.classList.remove("d-none")
        });
        document.querySelector("dl.row.file-details").classList.remove("d-flex");
        document.querySelector("dl.row.file-details").classList.remove("justify-content-center");
    });
});