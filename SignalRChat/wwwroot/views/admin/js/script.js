import 'https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/js/bootstrap.bundle.min.js';
import 'https://cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/toastr.min.js';
import '../../../js/jquery.js';
import '../../../js/signalr/dist/browser/signalr.js';
import './AdminChat.js';

const liste = document.querySelector("#liste");
const select = document.querySelector("#Kullanicilar");
const kullaniciButton = document.querySelector("#KullaniciBtn");

window.addEventListener("load", () => {
    if (liste) {
        liste.scrollTop = 9999999999;
    }
});

if (select) {
    select.addEventListener("change", () => {
        var name = select.options[select.selectedIndex].text;
        kullaniciButton.textContent = name + " Mesajlarını Sil";
    });
}

$(document).ready(function () {
    $('.update-member-form').on('submit', function (event) {
        event.preventDefault();
        var formData = $(this).serialize();
        swal({
            title: "Emin misiniz?",
            text: "Seçilii kullanıcı bilgilerini güncellemek üzeresiniz!",
            icon: "warning",
            buttons: true,
            dangerMode: true,
        }).then((willUpdate) => {
            if (willUpdate) {
                $.ajax({
                    url: "admin/updatemember",
                    type: 'POST',
                    data: formData,
                    success: function (response) {
                        if (response.success) {
                            swal("Başarılı!", response.message, "success");
                        } else {
                            swal("Hata!", response.message, "error");
                        }
                    },
                    error: function (response) {
                        swal("Bir hata oluştu", response.message, "error");
                    }
                });
            } else {
                swal("İşlem iptal edildi");
            }
        });
    });
    $('.delete-member-form').on('submit', function (event) {
        event.preventDefault();
        var formData = $(this).serialize();
        var url=$(this).attr('action');
        swal({
            title: "Emin misiniz?",
            text: "Seçilii kullanıcıyı silmek üzeresiniz!",
            icon: "warning",
            buttons: true,
            dangerMode: true,
        }).then((willDelete) => {
            if (willDelete) {
                $.ajax({
                    url: url,
                    type: 'POST',
                    data: formData,
                    success: function (response) {
                        if (response.success) {
                            swal("Başarılı!", response.message, "success");
                        } else {
                            swal("Hata!", response.message, "error");
                        }
                    },
                    error: function (response) {
                        swal("Bir hata oluştu", response.message, "error");
                    }
                });
            } else {
                swal("İşlem iptal edildi");
            }
        });
    });
    $('.delete-image-form').on('submit', function (event) {
        event.preventDefault();
        var formData = $(this).serialize();
        var url = $(this).attr('action');
        swal({
            title: "Emin misiniz?",
            text: "Seçilii kullanıcı resmini silmek üzeresiniz!",
            icon: "warning",
            buttons: true,
            dangerMode: true,
        }).then((willDelete) => {
            if (willDelete) {
                $.ajax({
                    url: url,
                    type: 'POST',
                    data: formData,
                    success: function (response) {
                        if (response.success) {
                            swal("Başarılı!", response.message, "success");
                        } else {
                            swal("Hata!", response.message, "error");
                        }
                    },
                    error: function (response) {
                        swal("Bir hata oluştu", response.message, "error");
                    }
                });
            } else {
                swal("İşlem iptal edildi");
            }
        });
    });
    $('.kullanici-önerileri-form').on('submit', function (event) {
        event.preventDefault();
        var formData = $(this).serialize();
        var url = $(this).attr('action');
        swal({
            title: "Emin misiniz?",
            text: "Kullanıcı önerisi silmek üzeresiniz!",
            icon: "warning",
            buttons: true,
            dangerMode: true,
        }).then((willDelete) => {
            if (willDelete) {
                $.ajax({
                    url: url,
                    type: 'POST',
                    data: formData,
                    success: function (response) {
                        if (response.success) {
                            swal("Başarılı!", response.message, "success");
                        } else {
                            swal("Hata!", response.message, "error");
                        }
                    },
                    error: function (response) {
                        swal("Bir hata oluştu", response.message, "error");
                    }
                });
            } else {
                swal("İşlem iptal edildi");
            }
        });
    });
    $('.delete-message-for-user').on('submit', function (event) {
        event.preventDefault();
        var formData = $(this).serialize();
        var url = $(this).attr('action');
        swal({
            title: "Emin misiniz?",
            text: "Üyenin tüm mesajlarını silmek üzeresiniz",
            icon: "warning",
            buttons: true,
            dangerMode: true,
        }).then((willDelete) => {
            if (willDelete) {
                $.ajax({
                    url: url,
                    type: 'POST',
                    data: formData,
                    success: function (response) {
                        if (response.success) {
                            swal("Başarılı!", response.message, "success");
                        } else {
                            swal("Hata!", response.message, "error");
                        }
                    },
                    error: function (response) {
                        swal("Bir hata oluştu", response.message, "error");
                    }
                });
            } else {
                swal("İşlem iptal edildi");
            }
        });
    });
    $('.delete-message-for-single').on('submit', function (event) {
        event.preventDefault();
        var formData = $(this).serialize();
        var url = $(this).attr('action');
        swal({
            title: "Emin misiniz?",
            text: "Mesajı silmek üzeresiniz",
            icon: "warning",
            buttons: true,
            dangerMode: true,
        }).then((willDelete) => {
            if (willDelete) {
                $.ajax({
                    url: url,
                    type: 'POST',
                    data: formData,
                    success: function (response) {
                        if (response.success) {
                            swal("Başarılı!", response.message, "success");
                        } else {
                            swal("Hata!", response.message, "error");
                        }
                    },
                    error: function (response) {
                        swal("Bir hata oluştu", response.message, "error");
                    }
                });
            } else {
                swal("İşlem iptal edildi");
            }
        });
    });
    
    
})