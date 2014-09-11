﻿var ArrangeImages = function (container) {
    //var width = $(document).width();
    $(container).find("img").each(function () {
        var ratio = this.width / this.height;
        var newWidth = 200 * ratio;
        $(this).width(newWidth + "px").height("200px").css("max-width", "400px");
    });
}

var ScaleElement = function (element) {
    var currentElement = null;

    // If bit is set to new tile mode then return.
    if (TILE_MODE == 1) {
        return;
    }

    $(element).find("li.movie").each(function () {
        $(this).find(".movie-poster,.captionAndNavigate").hover(function (e) {

            if ($(window).width() < 768) {
                e.stopPropagation();
                return false;
            }

            var element = this;

            if ($(this).attr("class") == "captionAndNavigate")
                element = $(this).prev();

            currentElement = element;
            $(element).attr("isactive", "true");
            $(element).attr("org-w", $(element).css("width")).css("width", "300px").css("top", "-35px").css("left", "-50px");
            $(element).attr("org-h", $(element).css("height")).css("height", "400px");
            $(element).css("z-index", "100").css("position", "absolute");
            $(element).parent().css("height", "500px").css("z-index", "98");
            $(element).parent().find(".captionAndNavigate").css("width", "300px").css("height", "400px").css("z-index", "100").css("top", "-35px").css("left", "-50px");

            $(element).parent().find(".additives").css("position", "relative").css("top", "10px").css("float", "right");

            $(element).parent().find(".movie-synopsis").each(function () {
                $(this).show();
            });

        }, function (e) {
            if ($(window).width() < 768) {
                e.stopPropagation();
                return false;
            }

            var element = this;
            if ($(this).attr("class") == "captionAndNavigate")
                element = $(this).prev();


            if ($(element).attr("org-h") == "400px") {
                $(element).attr("org-h", "340px");
                $(element).attr("org-w", "225px");
            }

            $(element).css("width", $(element).attr("org-w")).css("height", $(element).attr("org-h")).css("z-index", "1").css("position", "relative").css("top", "0px").css("left", "0px");
            $(element).parent().css("height", $(element).attr("org-h"));
            $(element).parent().css("z-index", "1");
            $(element).parent().find(".captionAndNavigate").css("width", $(element).attr("org-w")).css("top", "auto").css("left", "0px").css("z-index", "1").css("height", "auto");
            $(element).parent().find(".movie-synopsis").hide();
            $(element).parent().find(".additives").css("position", "absolute").css("top", "auto").css("bottom", "10px").css("float", "clear").css("right", "10px");
        });
        //$(this).find(".movie-poster,.captionAndNavigate").unbind("hover");
    });
}

var LoadDefaultCriticImage = function (img) {
    $(img).attr("src", "/Images/user.png");
}