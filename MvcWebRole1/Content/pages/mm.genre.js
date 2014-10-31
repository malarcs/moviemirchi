﻿function PrepareGenrePage() {
    var json = [
                    { "title": "Now Playing", "section": "nowplaying-movie-list-tube" },
                    { "title": "Upcoming", "section": "upcoming-movie-list-tube" },
                    { "title": "Previous", "section": "previous-movie-list-tube" }
    ];

    $(".nav-bar-container").append(GetNavBar(json));

    var name = document.location.href.substring(document.location.href.lastIndexOf("/") + 1);
    if (name.indexOf("?") > -1)
        name = name.substring(0, name.indexOf("?"));
    name = toPascalCase(name);

    $(".movies").append(GetTubeControl(name + " Movies", "genre-name"));
    $(".movies").append(GetTubeControl("Now Playing", "nowplaying-movie-list", "nowplaying-movies-pager"));
    $(".movies").append(GetTubeControl("Upcoming Releases", "upcoming-movie-list", "upcoming-movies-pager"));
    $(".movies").append(GetTubeControl("Previous Movies", "previous-movie-list", "previous-movies-pager"));

    $(".section-title").each(function () {
        new Util().AppendLoadImage($(this));
    });

    var apiPath = "/api/GenreMovies?type=" + name;
    CallHandler(apiPath, PopulateMovies);
}