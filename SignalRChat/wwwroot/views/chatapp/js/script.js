import 'https://cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/toastr.min.js';
import '../../../js/jquery.js';



$("#change").on("submit", function (event) {
    event.preventDefault();

    var formData = $(this).serialize();
    var url=$(this).attr('action');
    $.ajax({
        url: url,
        type: 'POST',
        data: formData,
        beforeSend: function () {
            document.querySelector(".card-front .loader").classList.remove("d-none");
            document.querySelector(".card-front .center-wrap").classList.add("d-none");
        },
        success: function (response) {
            if (response.success) {
                swal("Başarılı!", response.message, "success");
                if (response.redirectUrl) {
                    setTimeout(function () {
                        window.location.href = response.redirectUrl;
                    }, 2000); // 2 saniye bekle ve yönlendir
                }
            }
            else {
                swal("Hata!", response.message, "error");
            }
        },
        error: function (xhr, status, error) {
            swal("Bir hata oluştu", xhr.responseText, "error");
        },
        complete: function () {
            document.querySelector(".card-front .loader").classList.add("d-none");
            document.querySelector(".card-front .center-wrap").classList.remove("d-none");
        },
    });
});
$("#login").on("submit", function (event) {
    event.preventDefault();
    var formData = $(this).serialize();
    var url = $(this).attr('action');
    $.ajax({
        url: url,
        type: 'POST',
        data: formData,
        beforeSend: function () {
            document.querySelector(".card-front .loader").classList.remove("d-none");
            document.querySelector(".card-front .center-wrap").classList.add("d-none");
        },
        success: function (response) {
            if (response.success) {
                swal("Başarılı!", response.message, "success");
                if (response.redirectUrl) {
                    setTimeout(function () {
                        window.location.href = response.redirectUrl;
                    }, 2000); // 2 saniye bekle ve yönlendir
                }
            }
            else {
                swal("Hata!", response.message, "error");
            }
        },
        error: function (xhr, status, error) {
            swal("Bir hata oluştu", xhr.responseText, "error");
        },
        complete: function () {
            document.querySelector(".card-front .loader").classList.add("d-none");
            document.querySelector(".card-front .center-wrap").classList.remove("d-none");
        },
    });
});
$("#register").on("submit", function (event) {
    event.preventDefault();

    var form = $(this);
    var url =$(this).attr('action');

    $.ajax({
        url: url,
        type: "POST",
        data: form.serialize(),
        beforeSend: function () {
            document.querySelector(".card-back .loader").classList.remove("d-none");
            document.querySelector(".card-back .center-wrap").classList.add("d-none");
        },
        success: function (response) {
            if (response.success) {
                swal("Başarılı!", response.message, "success");
                if (response.redirectUrl) {
                    setTimeout(function () {
                        window.location.href = response.redirectUrl;
                    }, 2000);
                }
            } else {
                swal("Hata!", response.message, "error");
            }
        },
        error: function () {
            swal("Bir hata oluştu", xhr.responseText, "error");
        },
        complete: function () {
            document.querySelector(".card-back .loader").classList.add("d-none");
            document.querySelector(".card-back .center-wrap").classList.remove("d-none");
        }
    });
});
$("#reset").on("submit", function (event) {
    event.preventDefault();

    var formData = $(this).serialize();
    var url=$(this).attr('action');

    $.ajax({
        url: url,
        type: 'POST',
        data: formData,
        beforeSend: function () {
            document.querySelector(".card-front .loader").classList.remove("d-none");
            document.querySelector(".card-front .center-wrap").classList.add("d-none");
        },
        success: function (response) {
            if (response.success) {
                swal("Başarılı!", response.message, "success");
                if (response.redirectUrl) {
                    setTimeout(function () {
                        window.location.href = response.redirectUrl;
                    }, 2000); // 2 saniye bekle ve yönlendir
                }
            }
            else {
                swal("Hata!", response.message, "error");
            }
        },
        error: function (xhr, status, error) {
            swal("Bir hata oluştu", xhr.responseText, "error");
        },
        complete: function () {
            document.querySelector(".card-front .loader").classList.add("d-none");
            document.querySelector(".card-front .center-wrap").classList.remove("d-none");
        },
    });
});
$('#verifyForm').on('submit', function (e) {
    e.preventDefault();

    var formData = $(this).serialize(); // Form verilerini al

    $.ajax({
        url: '/ChatApp/Verify',
        type: 'POST',
        data: formData,
        beforeSend: function () {
            document.querySelector(".card-front .loader").classList.remove("d-none");
            document.querySelector(".card-front .center-wrap").classList.add("d-none");
        },
        success: function (response) {
            if (response.success) {
                swal("Başarılı!", response.message, "success");
                if (response.redirectUrl) {
                    setTimeout(function () {
                        window.location.href = response.redirectUrl;
                    }, 2000); // 2 saniye bekle ve yönlendir
                }
            }
            else {
                swal("Hata!", response.message, "error");
            }
        },
        error: function (error) {
            swal("Bir hata oluştu", xhr.responseText, "error");
        },
        complete: function () {
            document.querySelector(".card-front .loader").classList.add("d-none");
            document.querySelector(".card-front .center-wrap").classList.remove("d-none");
        },
    });
});