

$(window).on("load", function () {

    "use strict";


    setTimeout(function () {
        $(".loader__logo").removeClass('fadeIn').addClass('fadeOut');
    }, 600);

    setTimeout(function () {
        $('body').addClass('loaded');
        $('body').removeClass('overflow-hidden');
        $(".loader").addClass('fade loaded');
    }, 1200);

    var bgndTriangles = $('#stars-js');
    if (bgndTriangles.length) {
        particlesJS('stars-js', {
            "particles": {
                "number": {
                    "value": 100,
                    "density": {
                        "enable": false,
                        "value_area": 2604.1872173865
                    }
                },
                "color": {
                    "value": "#ffffff"
                },
                "shape": {
                    "type": "star",
                    "stroke": {
                        "width": 0,
                        "color": "#000000"
                    },
                    "polygon": {
                        "nb_sides": 5
                    },
                    "image": {
                        "src": "http://wiki.lexisnexis.com/academic/images/f/fb/Itunes_podcast_icon_300.jpg",
                        "width": 100,
                        "height": 100
                    }
                },
                "opacity": {
                    "value": 0.1,
                    "random": false,
                    "anim": {
                        "enable": false,
                        "speed": 0.01,
                        "opacity_min": 0.08,
                        "sync": false
                    }
                },
                "size": {
                    "value": 4.5,
                    "random": true,
                    "anim": {
                        "enable": false,
                        "speed": 40,
                        "size_min": 0.1,
                        "sync": false
                    }
                },
                "line_linked": {
                    "enable": false
                },
                "move": {
                    "enable": true,
                    "speed": 1,
                    "direction": "top",
                    "random": true,
                    "straight": true,
                    "out_mode": "out",
                    "bounce": false,
                    "attract": {
                        "enable": false,
                        "rotateX": 600,
                        "rotateY": 1200
                    }
                }
            },
            "interactivity": {
                "detect_on": "canvas",
                "events": {
                    "onhover": {
                        "enable": true,
                        "mode": "bubble"
                    },
                    "onclick": {
                        "enable": true,
                        "mode": "repulse"
                    },
                    "resize": true
                },
                "modes": {
                    "grab": {
                        "distance": 200,
                        "line_linked": {
                            "opacity": 1
                        }
                    },
                    "bubble": {
                        "distance": 170,
                        "size": 8,
                        "duration": 2,
                        "opacity": 0.3,
                        "speed": 3
                    },
                    "repulse": {
                        "distance": 200,
                        "duration": 0.4
                    },
                    "push": {
                        "particles_nb": 4
                    },
                    "remove": {
                        "particles_nb": 2
                    }
                }
            },
            "retina_detect": true
        });
    }

});

$(function () {

    "use strict";


    $('#showreel-trigger').magnificPopup({
        type: 'iframe',
        mainClass: 'mfp-fade',
        removalDelay: 160,
        preloader: false,
        fixedContentPos: false,
        callbacks: {
            beforeOpen: function () { $('body').addClass('overflow-hidden'); },
            close: function () { $('body').removeClass('overflow-hidden'); }
        }
    });

    $('.skillbar').skillBars({
        from: 0,
        speed: 4000,
        interval: 100,
    });

    var bgndKenburns = $('#bgndKenburns');
    if (bgndKenburns.length) {
        bgndKenburns.vegas({
            timer: false,
            delay: 10000,
            transition: 'fade2',
            transitionDuration: 2000,
            slides: [
                { src: "/views/home/img/backgrounds/1080x1440-kenburns-1.webp" },
                { src: "/views/home/img/backgrounds/1080x1440-kenburns-2.webp" },
                { src: "/views/home/img/backgrounds/1080x1440-kenburns-3.webp" }
            ],
            animation: ['kenburnsUp', 'kenburnsDown', 'kenburnsLeft', 'kenburnsRight']
        });
    }

    $('#countdown').countdown({ until: $.countdown.UTCDate(+10, 2024, 10, 7), format: 'D' });
    $('#countdown-large').countdown({ until: $.countdown.UTCDate(+10, 2025, 1, 1), format: 'DHMS' });

    $('.notify-form').ajaxChimp({
        callback: mailchimpCallback,
        url: 'https://besaba.us10.list-manage.com/subscribe/post?u=e8d650c0df90e716c22ae4778&amp;id=54a7906900'
    });

    function mailchimpCallback(resp) {
        if (resp.result === 'success') {
            $('.notify').find('.form').addClass('is-hidden');
            $('.notify').find('.subscription-ok').addClass('is-visible');
            setTimeout(function () {
                $('.notify').find('.subscription-ok').removeClass('is-visible');
                $('.notify').find('.form').delay(300).removeClass('is-hidden');
                $('.notify-form').trigger("reset");
            }, 5000);
        } else if (resp.result === 'error') {
            $('.notify').find('.form').addClass('is-hidden');
            $('.notify').find('.subscription-error').addClass('is-visible');
            setTimeout(function () {
                $('.notify').find('.subscription-error').removeClass('is-visible');
                $('.notify').find('.form').delay(300).removeClass('is-hidden');
                $('.notify-form').trigger("reset");
            }, 5000);
        }
    }
    

    $("#contact-form").on("submit",function (event) {
        event.preventDefault();
        var th = $(this);
        $.ajax({
            type: "POST",
            url: th.attr('action'),
            data: th.serialize()
        }).done(function () {
            $('.contact').find('.form').addClass('is-hidden');
            $('.contact').find('.reply-group').addClass('is-visible');
            setTimeout(function () {
                $('.contact').find('.reply-group').removeClass('is-visible');
                $('.contact').find('.form').delay(300).removeClass('is-hidden');
                th.trigger("reset");
            }, 5000);
        });
    });



});
