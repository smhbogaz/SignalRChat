$(document).ready(function () {
    $("form").on("submit", function (event) {
        event.preventDefault();

        var formData = new FormData(this);
        var url = $(this).attr('action');

        $.ajax({
            url: url,
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            beforeSend: function () {
                document.querySelector(".loader").classList.remove("d-none");
                document.querySelectorAll("form *:not(.loader)").forEach((item) => {
                    item.classList.add("d-none")
                });
                document.querySelector("form").classList.add("d-flex");
                document.querySelector("form").classList.add("justify-content-center");

            },
            success: function (response) {
                if (response.success) {
                    swal("Başarılı!", response.message, "success");
                } else {
                    swal("Hata!", response.message, "error");
                }
            },
            error: function (response) {
                swal("Bir hata oluştu", response.message, "error");
            },
            complete: function () {
                document.querySelector(".loader").classList.add("d-none");
                document.querySelectorAll("form *:not(.loader)").forEach((item) => {
                    item.classList.remove("d-none")
                });
                document.querySelector("form").classList.remove("d-flex");
                document.querySelector("form").classList.remove("justify-content-center");
            }
        });

    });
});
