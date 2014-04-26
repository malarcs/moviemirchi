﻿function PopulatingMovies(movie) {
    var movieContainer = $(".movie-list ul");

    var poster = [];
    poster = JSON.parse(movie.Posters);
    var src = (poster != null && poster.length > 0) ?
        "/Posters/Images/" + poster[poster.length - 1] :
        "/Posters/Images/default-movie.jpg";

    var anchor = $("<a/>");
    var list = $("<li/>");

    list.attr("class", "movie")
    //anchor.attr("href", "Movie?name=" + movie.UniqueName);
    anchor.attr("href", "Movie/" + movie.UniqueName);
    anchor.attr("title", movie.Name);
    //anchor.append(img);


    var synopsis = movie.Synopsis.length > 500 ? movie.Synopsis.substring(0, 500) + "..." : movie.Synopsis;

    var html =
    "<div id=\"picAndCaption\" class=\"viewingDiv " + movie.UniqueName + "\">" +
        "<div id=\"imageContainer\" class=\"viewer\" style=\"height: 400px;\">" +
            "<img id=\"imageEl\" onload=\"MovieImageLoaded(this);\" class=\"movie-poster shownImage\" title=\"" + movie.Name + "\" alt=\"" + movie.Name + "\" src=\"" + src + "\" style=\"margin: auto;\">" +
            "<div class=\"captionAndNavigate\" style=\"width:263px;padding: 15px\">" +
                "<div id=\"captionCredit\" style=\"width: 398px;\" class=\"multimediaCaption\">" +
                    "<div id=\"photoCaption\">" +
                        "<div class=\"img-movie-name\">" + movie.Name + "</div>" +
                        "<div class=\"img-movie-genre\">" + movie.Genre + "</div>" +
                        "<div class=\"img-movie-date\">" + movie.Month + "</div>" +
                        GetRateControl(movie.Ratings) +
                        "<div class=\"movie-synopsis\" style=\"display: none;\">" + synopsis + "</div>" +
                    "</div>" +
                "</div>" +
            "</div>" +
        "</div>" +
    "</div>";

    anchor.append(html);
    //anchor.append(GetRateControl(6));
    list.append(anchor);
    movieContainer.append(list);

    return list;
}

function MovieImageLoaded(img) {
    var width = $(document).width();
    var imgWidth = parseInt($(img).css("width").replace("px"));
    var imgHeight = parseInt($(img).css("height").replace("px"));

    var ratio = imgWidth / imgHeight;
    var newWidth = 400 * ratio;
    if (newWidth > 263) {
        newWidth = 263;
    }

    $(img).css("width", newWidth + "px").css("height", "400px");
}