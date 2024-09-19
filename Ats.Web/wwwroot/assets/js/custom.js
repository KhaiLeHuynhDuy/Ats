jQuery(document).ready(function ($) {
    $('[data-toggle="tooltip"]').tooltip();
    /*End color ATS*/
    var color_ats = localStorage.getItem('color-ats');
    $('.color-custom').css("background-color", color_ats);
    $('.color-custom-v2,.red_label').css('color', color_ats);
    $('.list_menu .panel').css('border-color', color_ats);
    $('.content .search').css('background-color', color_ats);
    //$('.content .add_new_item').css('background-color', color_ats);
    $('.btn-ats-save').css('background-color', color_ats);
   
    if (color_ats === '#bd081c') {
        $('.content .form-control').addClass('focus_red');
        $('.ats-select').addClass('ats-select-red');
        $('.length-dayofmonth').addClass('length-dayofmonth-red');
        $('.ATS-custom-seleced').addClass('ATS-custom-seleced-red');
    }
    else if (color_ats === '#00a552') {
        $('.content .form-control').addClass('focus_green');
        $('.ats-select').addClass('ats-select-green');
        $('.length-dayofmonth').addClass('length-dayofmonth-green');
        $('.ATS-custom-seleced').addClass('ATS-custom-seleced-green');
    }
    else if (color_ats === '#0013a5') {
        $('.content .form-control').addClass('focus_blue');
        $('.ats-select').addClass('ats-select-blue');
        $('.length-dayofmonth').addClass('length-dayofmonth-blue');
        $('.ATS-custom-seleced').addClass('ATS-custom-seleced-blue');
    }

    $('.breadcrumb a.no-link').click(function (event) {
        event.preventDefault();
    });

    $('.flag-color-main').click(function (event) {
        event.preventDefault();
        
        var color_main = $(this).attr('flag-color');
        localStorage.setItem('color-ats', color_main);

        setTimeout(function () {
            location.reload();
        }, 1000);
               
    });

    /*End color ATS*/
   
    /*User Image*/

    $('.nav-left i.fa-bars').click(function (event) {
        event.preventDefault();
        $('.col_sidebar').toggleClass('small_width');
        $('.col_content').toggleClass('lg_width');
    });

    //$('#notification').delay(5000).fadeOut('slow');

    $(document).on('click', function (e) {
        if ($(e.target).closest('.flag-popup-show').length === 1) {
            if ($(e.target).closest('.flag-popup-show').find('.flag-popup-hidden').hasClass('show_div') === true) {
                $('.flag-popup-hidden').removeClass('show_div');
            }
            else {
                $('.flag-popup-hidden').removeClass('show_div');
                $(e.target).closest('.flag-popup-show').find('.flag-popup-hidden').addClass('show_div');
            }
        } else {
            $('.flag-popup-hidden').removeClass('show_div');
        }
    });

    var canvas = document.getElementById("canvas");
    if (canvas !== null) {
        drawClock();
    }

    $(window).resize(function () {
        set_min_height();
        set_col_bar_height();
    });
    set_min_height();
    set_col_bar_height();
});
/* set height col-bar */
function set_col_bar_height() {
    var h_browser = window.innerHeight || document.documentElement.clientHeight || document.body.clientHeight;
    var h_two = $('.col_content').height();
    $('.col_sidebar').css('height', h_two + 20);
}
/* set min height */
function set_min_height() {
    var h_browser = window.innerHeight || document.documentElement.clientHeight || document.body.clientHeight;
    //console.log(h_browser);
    $('.col_content').css('min-height', h_browser-70);
}
/* End set min height */

/*start set tree day */

/*End set tree day */
/*start clock*/
var canvas = document.getElementById("canvas");
if (canvas !== null) {
    var ctx = canvas.getContext("2d");
    var radius = canvas.height / 2;
    ctx.translate(radius, radius);
    radius = radius * 0.90;
    setInterval(drawClock, 1000);
}

function drawClock() {
    drawFace(ctx, radius);
    drawTime(ctx, radius);
}

function drawFace(ctx, radius) {
    var grad;
    ctx.beginPath();
    ctx.arc(0, 0, radius, 0, 2 * Math.PI);
    ctx.fillStyle = '#E5E5E5';
    ctx.fill();
    grad = ctx.createRadialGradient(0, 0, radius * 0.95, 0, 0, radius * 1.05);
    grad.addColorStop(0, '#bd081c');
    grad.addColorStop(0.5, '#bd081c');
    grad.addColorStop(1, '#bd081c');
    ctx.strokeStyle = grad;
    ctx.lineWidth = radius * 0.05;
    ctx.stroke();
    ctx.beginPath();
    ctx.arc(0, 0, radius * 0.1, 0, 2 * Math.PI);
    ctx.fillStyle = '#bd081c';
    ctx.fill();
}

function drawTime(ctx, radius) {
    var now = new Date();
    var hour = now.getHours();
    var minute = now.getMinutes();
    var second = now.getSeconds();
    //hour
    hour = hour % 12;
    hour = (hour * Math.PI / 6) +
        (minute * Math.PI / (6 * 60)) +
        (second * Math.PI / (360 * 60));
    drawHand(ctx, hour, radius * 0.5, radius * 0.07);
    //minute
    minute = (minute * Math.PI / 30) + (second * Math.PI / (30 * 60));
    drawHand(ctx, minute, radius * 0.8, radius * 0.07);

}

function drawHand(ctx, pos, length, width) {
    ctx.beginPath();
    ctx.lineWidth = width;
    ctx.lineCap = "round";
    ctx.moveTo(0, 0);
    ctx.rotate(pos);
    ctx.lineTo(0, -length);
    ctx.stroke();
    ctx.rotate(-pos);
}
/*End clock*/


/*====================================================Start Custom ATS Selected===============================================================*/
var x, i, j, selElmnt, a, b, c;
/*look for any elements with the class "custom-select":*/
x = document.getElementsByClassName("ats-select");
for (i = 0; i < x.length; i++) {
    selElmnt = x[i].getElementsByTagName("select")[0];
    /*for each element, create a new DIV that will act as the selected item:*/
    a = document.createElement("DIV");
    a.setAttribute("class", "select-selected");
    a.innerHTML = selElmnt.options[selElmnt.selectedIndex].innerHTML;
    x[i].appendChild(a);
    /*for each element, create a new DIV that will contain the option list:*/
    b = document.createElement("DIV");
    b.setAttribute("class", "select-items select-hide");
    for (j = 0; j < selElmnt.length; j++) {
        /*for each option in the original select element,
        create a new DIV that will act as an option item:*/
        c = document.createElement("DIV");
        c.innerHTML = selElmnt.options[j].innerHTML;
        c.addEventListener("click", function (e) {
            /*when an item is clicked, update the original select box,
            and the selected item:*/
            var y, i, k, s, h;
            s = this.parentNode.parentNode.getElementsByTagName("select")[0];
            h = this.parentNode.previousSibling;
            for (i = 0; i < s.length; i++) {
                if (s.options[i].innerHTML === this.innerHTML) {
                    s.selectedIndex = i;
                    h.innerHTML = this.innerHTML;
                    y = this.parentNode.getElementsByClassName("same-as-selected");
                    for (k = 0; k < y.length; k++) {
                        y[k].removeAttribute("class");
                    }
                    this.setAttribute("class", "same-as-selected");
                    break;
                }
            }
            h.click();
        });
        b.appendChild(c);
    }
    x[i].appendChild(b);
    a.addEventListener("click", function (e) {
        /*when the select box is clicked, close any other select boxes,
        and open/close the current select box:*/
        e.stopPropagation();
        closeAllSelect(this);
        this.nextSibling.classList.toggle("select-hide");
        this.classList.toggle("select-arrow-active");
    });
}
function closeAllSelect(elmnt) {
    /*a function that will close all select boxes in the document,
    except the current select box:*/
    var x, y, i, arrNo = [];
    x = document.getElementsByClassName("select-items");
    y = document.getElementsByClassName("select-selected");
    for (i = 0; i < y.length; i++) {
        if (elmnt === y[i]) {
            arrNo.push(i);
        } else {
            y[i].classList.remove("select-arrow-active");
        }
    }
    for (i = 0; i < x.length; i++) {
        if (arrNo.indexOf(i)) {
            x[i].classList.add("select-hide");
        }
    }
}
/*if the user clicks anywhere outside the select box,
then close all select boxes:*/
document.addEventListener("click", closeAllSelect);
/*=====End Custom ATS Selected===========*/