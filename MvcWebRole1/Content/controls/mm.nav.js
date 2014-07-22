/*
    Expected Json Structure
    [
        {   "title" : "Upcoming", "section" : "upcoming" },
        {   "title" : "Now Playing", "section" : "now-playing" }
    ]
*/
var GetNavBar = function (json) {
    if (json == null || json == "undefined")
        return;

    var list = $("<ul/>").attr("class", "top-nav-bar");

    for (i = 0; i < json.length; i++) {
        var item = $("<li/>").attr("link-id", json[i].section).html(json[i].title).click(function () {
            $('html, body').animate({
                scrollTop: $("#" + $(this).attr('link-id')).offset().top - 120
            }, 500);
            return false;
        });

        $(list).append(item);
    }

    $('html, body').bind('DOMMouseScroll', function (e) {
        if (e.originalEvent.detail > 0) {
            $(".nav-bar-container").slideUp();
        } else {
            $(".nav-bar-container").slideDown();
        }

        ClearSearchReults();
    });

    //IE, Opera, Safari
    $('html, body').bind('mousewheel', function (e) {
        if (e.originalEvent.wheelDelta < 0) {
            $(".nav-bar-container").slideUp();
        } else {
            $(".nav-bar-container").slideDown();
        }

        ClearSearchReults();
    });

    return list;
}