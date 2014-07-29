﻿function LoadSingleMovie(movieId) {
    var path = "../api/MovieInfo?q=" + movieId;
    var reviewPath = "../api/MovieReview?q=" + movieId;

    CallHandler(path, ShowMovie);
    //CallHandler(reviewPath, ShowMovieReviews);
}

var ShowMovie = function (data) {
    try {
        var result = JSON.parse(data);
        
        if (result.Status != undefined || result.Status == "Error") {
            $(".movie-content").html(result.UserMessage);
        }
        else {
            if (result.Movie != undefined) {
                $(".movie-content").append(GetTubeControl(result.Movie.Name, "movie-list", "movie-pager"));

                PopulatingMovies(result.Movie, "movie-list");

                ScaleElement($(".movie-list ul"));
                /*if (TILE_MODE == 0 && $(window).width() > 767)
                    ScaleElement($(".movie-list ul"));
                else
                    ScaleNewTileElement($(".movie-list ul"));
                */
                // Show all posters of current movie
                var poster = [], reviews = [], songs = [], trailers = [];

                poster = result.Movie.Posters;
                reviews = result.MovieReviews;
                songs = result.Movie.Songs;
                trailers = result.Movie.Trailers;
                //show movies details
                ShowMovieDetails(result.Movie);
                //populate movie's posters
                PopulatePosters(poster, result.Movie.Name);
                //populate movie's songs
                PopulateSongs(songs);
                //populate movie's trailers
                PopulateTrailers(trailers);

                ArrangeImages($(".movie-poster-details"));
                ShowMovieReviews(reviews);
                PrepareGenreLinks();

                $(".gallery a[rel^='prettyPhoto']").prettyPhoto({
                    animation_speed: 'normal',
                    theme: 'dark_square',
                    slideshow: false,
                    autoplay_slideshow: false,
                    show_title: true,
                    keyboard_shortcuts: true,
                    social_tools: false,
                    allow_resize: true,
                });
            } else {
                $(".movie-content").html("Unable to find movie.");
            }
        }
    } catch (e) {
        $(".movie-content").html("Unable to find movie.");
    }
}

var ShowMovieDetails = function (movie) {
    var movieDetalis = $("<div/>").addClass("movie-description");

    var directors = "", directorsList = "";
    var writers = "", writerList = "";
    var producers = "", producersList = "";
    var music = "", musicList = "";
    var cast = "", actorList = "";
    var artistCounter = 0;
    var songsList = "";

    var casts = [];
    casts = JSON.parse(movie.Casts);

    if (casts != "undefined" && casts != null && casts.length > 0) {
        for (var c = 0; c < casts.length; c++) {
            if (casts[c].role.toLowerCase() == "director" && casts[c].name != null && directors.indexOf(casts[c].name) == -1) {
                if (casts[c].charactername == null) {
                    directors += "<a href=\"/Artists/" + casts[c].name.split(' ').join('-') + "\" title='click here to view profile'>" + casts[c].name + "</a>, ";
                    directorsList += "<li class='team-item'><a href=\"/Artists/" + casts[c].name.split(' ').join('-') + "\">" + casts[c].name + "</a></li>";
                }
            }
            else if (casts[c].role.toLowerCase() == "writer" && casts[c].name != null && writers.indexOf(casts[c].name) == -1) {
                writers += "<a  href=\"/Artists/" + casts[c].name.split(' ').join('-') + "\" title='click here to view profile'>" + casts[c].name + "</a>, ";
                writerList += "<li class='team-item'><a href=\"/Artists/" + casts[c].name.split(' ').join('-') + "\">" + casts[c].name + "</a></li>";
            }
            else if (casts[c].role.toLowerCase() == "music" && casts[c].name != null && music.indexOf(casts[c].name) == -1) {
                if (casts[c].charactername == null) {
                    music += "<a  href=\"/Artists/" + casts[c].name.split(' ').join('-') + "\" title='click here to view profile'>" + casts[c].name + "</a>, ";
                    musicList += "<li class='team-item'><a href=\"/Artists/" + casts[c].name.split(' ').join('-') + "\">" + casts[c].name + "</a></li>";
                }
            }
            else if (casts[c].role.toLowerCase() == "producer" && casts[c].name != null && producers.indexOf(casts[c].name) == -1) {
                if (casts[c].charactername == "producer") {
                    producers += "<a href=\"/Artists/" + casts[c].name.split(' ').join('-') + "\" title='click here to view profile'>" + casts[c].name + "</a>, ";
                    producersList += "<li class='team-item'><a href=\"/Artists/" + casts[c].name.split(' ').join('-') + "\">" + casts[c].name + "</a></li>";
                }
            }
            else if (casts[c].role.toLowerCase() == "actor" && cast.indexOf(casts[c].name) == -1 && artistCounter < 8) {
                cast += "<a  href=\"/Artists/" + casts[c].name.split(' ').join('-') + "\" title='click here to view profile'> " + casts[c].name + "</a>, ";
                actorList += "<li class='cast-item'><span class='cast-details'><a href=\"/Artists/" + casts[c].name.split(' ').join('-') + "\">" + casts[c].name + "</a></span><span class='cast-details-right'>" + casts[c].charactername + "</span></li>";
                artistCounter++;
            }
        }
    }
    else {
        // Need to remove element if we dont have data to render it on screen
        //$("#item3").remove();
    }

    $(movieDetalis).append(new RatingControl().GetRatingControl(JSON.parse(movie.MyScore), movie));
    $(movieDetalis).append(GetMovieSynopsis(movie.Synopsis));
    $(movieDetalis).append(GetMovieGenre(movie.Genre));
    $(movieDetalis).append(GetMovieCast(CleanCastString(cast)));
    $(movieDetalis).append(GetMovieDirector(CleanCastString(directors)));
    $(movieDetalis).append(GetMovieProducer(CleanCastString(producers)));
    $(movieDetalis).append(GetMovieMusicDirector(CleanCastString(music)));
    $(movieDetalis).append(GetMovieWriter(CleanCastString(writers)));
    $(movieDetalis).append(GetMovieStats(movie.Stats));
    $(".tube-container:first").append(movieDetalis);
}

// images is JSON object
var PopulatePosters = function (images, movieName) {
    var poster = [];
    poster = JSON.parse(images);

    if (poster != "undefined" && poster != null && poster.length > 1) {

        var ul = $("<ul/>").attr("class", "gallery clearfix");

        for (var p = 0; p < poster.length; p++) {
            var img = $("<img/>")
            img.attr("class", "gallery-image");
            img.attr("alt", movieName);
            img.attr("src", PUBLIC_BLOB_URL + poster[p]);
            img.error(function () {
                $(this).hide();
            });

            var li = $("<li/>");
            var a = $("<a/>").attr("href", PUBLIC_BLOB_URL + poster[p]).attr("rel", "prettyPhoto[gallery]");

            $(a).append(img);
            $(li).append(a);
            $(ul).append(li);
            //$(".movie-poster-details").append(img);
        }

        $(".movie-poster-details").append(ul);

        /*Pagination for posters */
        if ($(window).width() < 768) {
            var pager = new Pager($(".movie-poster-details"), "#posters-pager");
        }
        else {
            PreparePaginationControl($(".movie-poster-details"), { pagerContainerId: "posters-pager", tileWidth: "350" });
            $(".movie-poster-details").append($("#posters-pager"));

            // Write a code for window resize
        }

        $(".link-container").show();
    }
    else {
        $(".movie-poster-details").hide();
        $(".link-container").find("div.section-title").each(function () {
            if ($(this).html() == "Posters") {
                $(this).hide();
            }
        });
    }
}

var ShowMovieReviews = function (review) {
    // VS - For production, following line shall be uncommented. Other line is used for demo purposes, 
    // when movies does not have any associated reivews
    //if (review != "undefined" && review != null && review.length > 0) {
    if (review != "undefined" && review != null) {
        $(".link-container").show();
        //(review.length > 0) ? GetReviewControl("movie-review-details", review) : GetDefaultReviewControl("movie-review-details", review);                        
        GetReviewControl("movie-review-details", review);
        if (review.length <= 0)
            $(".movie-review-details").html("<b>Currently this movie does not have any reviews.</b>");
        else {
            if ($(window).width() < 768) {
                var pager = new Pager($(".movie-review-details"), "#review-pager");
            }
            else {
                PreparePaginationControl($(".movie-review-details"), { pagerContainerId: "review-pager", tileWidth: "500" });
                $(".movie-review-details").append($("#review-pager"));

                $(window).resize(function () {
                    PreparePaginationControl($(".movie-review-details"), { pagerContainerId: "review-pager", tileWidth: "500" });
                });
            }
            
        }
    }
    else {
        $(".movie-review-details").hide();
        $(".link-container").find("div.section-title").each(function () {
            if ($(this).html() == "Reviews") {
                $(this).hide();
            }
        });
    }
}

var GetMovieGenre = function (genre) {
    genre = genre.length == 0 ? "-" : genre;
    return GetMovieDataHolder("Genre:", genre);
}

var GetMovieCast = function (movieCast) {
    movieCast = movieCast.length == 0 ? "-" : movieCast;
    return GetMovieDataHolder("Stars:", movieCast);
}

var GetMovieDirector = function (movieCast) {
    movieCast = movieCast.length == 0 ? "-" : movieCast;
    return GetMovieDataHolder("Directors:", movieCast);
}

var GetMovieProducer = function (movieCast) {
    movieCast = movieCast.length == 0 ? "-" : movieCast;
    return GetMovieDataHolder("Producers:", movieCast);
}

var GetMovieMusicDirector = function (movieCast) {
    movieCast = movieCast.length == 0 ? "-" : movieCast;
    return GetMovieDataHolder("Music Director:", movieCast);
}

var GetMovieWriter = function (movieCast) {
    movieCast = movieCast.length == 0 ? "-" : movieCast;
    return GetMovieDataHolder("Writer:", movieCast);
}

var GetMovieStats = function (movieStats) {
    movieStats = movieStats.length == 0 ? "-" : movieStats;
    return GetMovieDataHolder("Statistics:", movieStats);
}

var GetMovieSynopsis = function (synopsis) {
    synopsis = synopsis.length == 0 ? "-" : synopsis;
    return GetMovieDataHolder("Synopsis:", synopsis);
}

var GetMovieDataHolder = function (label, data) {
    return "<div class=\"movie-data-row\"><span class=\"movie-data-label\">" + label + "</span><span class=\"movie-data\">" + data + "<span/></div>";
}

var CleanCastString = function (str) {
    if (str.length > 0) {
        str = str.substring(0, str.lastIndexOf(","));
    }

    return str;
}

var PopulateSongs = function (song) {
    var songs = [];
    songs = JSON.parse(song);
    SongList(songs, "Song");
}

var SongList = function (videos, type) {
    var ul = $("<ul/>");
    var songHasLink = false;
    for (i = 0; i < videos.length; i++) {
        var img = $("<img/>").attr("class", "song-thumb").attr("src", videos[i].Thumb);
        var url = "";
        if (videos[i].YoutubeURL != null && videos[i].YoutubeURL != "undefined") {
            url = videos[i].YoutubeURL.trim();
        }

        var li = $("<li/>").attr("class", "song").attr("video-link", url + "?autoplay=1").attr("title", "Play YouTube " + type + " - " + videos[i].SongTitle).click(function () {
            $(document).scrollTop(0);
            trackSongLink(videos[i].YoutubeURL);
            DisplayModal($(this).attr("video-link"));
        });

        var playImg = $("<img/>").attr("class", "song-play").attr("video-link", url + "?autoplay=1").attr("src", "../images/play-video.png").attr("title", "Play YouTube " + type).click(function () {
            $(document).scrollTop(0);
            DisplayModal();
        });

        var title = $("<span/>").html(new Util().GetEllipsisText(videos[i].SongTitle, 16)).attr("title", videos[i].SongTitle);

        $(li).append(img);
        $(li).append(playImg);

        $(li).append(title);
        if (videos[i].YoutubeURL != undefined && videos[i].YoutubeURL != "" && videos[i].YoutubeURL != null) {
            $(ul).append(li);
            songHasLink = true;
        }
    }

    if (songHasLink) {
        $(".songs").append(ul);
        $(".songs").attr("id", "movie_songs")

        if ($(window).width() < 768) {
            var pager = new Pager($(".songs"), "#songs-pager");
        }
        else {
            PreparePaginationControl($(".songs"), { pagerContainerId: "songs-pager", tileWidth: "500" });

            $(window).resize(function () {
                PreparePaginationControl($(".songs"), { pagerContainerId: "songs-pager", tileWidth: "500" });
            });
        }

        $(".songs").append($("#songs-pager"));
        $(".songs").attr("style", "display:block !important;");
    }
}

var PopulateTrailers = function (trailer) {
    try {
        var trailers = [];
        trailers = JSON.parse(trailer);

        TrailerList(trailers, "Trailer");
    }
    catch (e) {
    };
}

var TrailerList = function (videos, type) {
    var ul = $("<ul/>");
    var songHasLink = false;
    var j = 0;
    for (i = 0; i < videos.length; i++) {
        var img = $("<img/>").attr("class", "song-thumb").attr("src", videos[i].Thumb);
        var url = "";
        if (videos[i].YoutubeURL != null && videos[i].YoutubeURL != "undefined") {
            url = videos[i].YoutubeURL.trim();
        }

        var li = $("<li/>").attr("class", "song").attr("video-link", url + "?autoplay=1").attr("title", "Play YouTube " + type + " - " + videos[i].Title).click(function () {
            $(document).scrollTop(0);
            DisplayModal($(this).attr("video-link"));
        });

        var playImg = $("<img/>").attr("class", "song-play").attr("video-link", url + "?autoplay=1").attr("src", "../images/play-video.png").attr("title", "Play YouTube " + type).click(function () {
            $(document).scrollTop(0);
            trackVideoLink(videos[i].YoutubeURL);
            DisplayModal();
        });

        var title = $("<span/>").html(new Util().GetEllipsisText(videos[i].Title, 16)).attr("title", videos[i].Title);

        $(li).append(img);
        $(li).append(playImg);

        $(li).append(title);
        if (videos[i].YoutubeURL != undefined && videos[i].YoutubeURL != "" && videos[i].YoutubeURL != null && videos[i].Thumb != undefined && videos[i].Thumb != "" && videos[i].Thumb != null) {
            $(ul).append(li);
            songHasLink = true;
            j++;
        }

        //if (j == 2) break;
    }

    if (songHasLink) {
        $(".trailers").append(ul);
        $(".trailers").attr("id", "movie_trailers");

        if ($(window).width() < 768) {
            var pager = new Pager($(".trailers"), "#trailer-pager");
        }
        else {
            PreparePaginationControl($(".trailers"), { pagerContainerId: "trailer-pager", tileWidth: "500" });
            
            $(window).resize(function () {
                PreparePaginationControl($(".trailers"), { pagerContainerId: "trailer-pager", tileWidth: "500" });
            });
        }

        $(".trailers").append($("#trailer-pager"));
        $(".trailers").attr("style", "display:block !important;");
    }
}

$("#overlay").click(function () {
    RemoveModal();
});

$("#modalMsg").click(function () {
    return;
});

function DisplayModal(url) {
    $("#overlay").attr("class", "OverlayEffect");
    $("#modalMsg").attr("class", "ShowModal");
    $("#modalMsg").find("iframe").each(function () {
        $(this).attr("src", url);
    });

}

function RemoveModal() {
    $("#modalMsg").attr("class", "HideModal");
    $("#overlay").attr("class", "");
    $("#modalMsg").find("iframe").each(function () {
        $(this).attr("src", "");
    });

    return false;
}