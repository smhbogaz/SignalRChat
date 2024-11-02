import 'https://cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/toastr.min.js';
import 'https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/js/bootstrap.bundle.min.js';
import '../../../js/jquery.js';
import '../../../js/signalr/dist/browser/signalr.js';
import './chat.js'; 



$("#opinion").on("submit", function (event) {
    event.preventDefault();

    var formData = $(this).serialize();
    var url=$(this).attr('action');
    var form=$(this);

    $.ajax({
        url: url,
        type: 'POST',
        data: formData,
        beforeSend: function () {
            form.find('*').toggleClass('d-none');
            form.find('.loader-two').removeClass('d-none');
        },
        success: function (response) {
            if (response.success) {
                swal("Başarılı!", response.message, "success");
            }
            else {
                swal("Hata!", response.message, "error");
            }
        },
        error: function (xhr, status, error) {
            swal("Bir hata oluştu", xhr.responseText, "error");
        },
        complete: function () {
            form.find('*').toggleClass('d-none');
            form.find('.loader-two').addClass('d-none');
        }
    });
});

$("#updateForm").on("submit", function (event) {
    event.preventDefault();
    var formData = new FormData(this);
    var url=$(this).attr('action');
    var form=$(this);
    
    $.ajax({
        url: url,
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,
        beforeSend: function () {
            form.find('*').toggleClass('d-none');
            form.find('.loader-two').removeClass('d-none');
        },
        success: function (response) {
            if (response.success) {
                swal("Başarılı!", response.message, "success");
            } else {
                swal("Hata!", response.message, "error");
            }
        },
        error: function (xhr, status, error) {
            swal("Bir hata oluştu", xhr.responseText, "error");
        },
        complete: function () {
            form.find('*').toggleClass('d-none');
            form.find('.loader-two').addClass('d-none');
        }
    });
});