﻿function LoadSingleMovie(movieId) {
    var path = "/api/MovieInfo?q=" + movieId;
    var reviewPath = "/api/MovieReview?q=" + movieId;

    CallHandler(path, ShowMovie);
    //CallHandler(reviewPath, ShowMovieReviews);
}

var ShowMovie = function (data) {
    try {

        var result = JSON.parse(data);

        $(".section-title").each(function () {
            new Util().RemoveLoadImage($(this));
        });

        if (result.Status != undefined || result.Status == "Error") {
            $(".movie-content").html(result.UserMessage);
        }
        else {
            if (result.Movie != undefined) {
                //$(".movie-content").append(GetTubeControl(result.Movie.Name, "movie-list", "movie-pager"));
                var poster = JSON.parse(result.Movie.Posters);
                var src = (poster != null && poster.length > 0) ? PUBLIC_BLOB_URL + poster[poster.length - 1] : PUBLIC_BLOB_URL + "default-movie.jpg";

                $(".movie-details").append("<div class=\"movie-poster-container\"><img src=\"" + src + "\" class=\"movie-poster\"  /></div>");
                //PopulatingMovies(result.Movie, "movie-list", { disableClick: true });

                //SetTileSize(".movie-list");

                //ScaleElement($(".movie-list ul"));
                /*if (TILE_MODE == 0 && $(window).width() > 767)
                    ScaleElement($(".movie-list ul"));
                else
                    ScaleNewTileElement($(".movie-list ul"));
                */
                // Show all posters of current movie
                var reviews = [], songs = [], trailers = [];

                //poster = result.Movie.Posters;
                reviews = result.MovieReviews;
                songs = result.Movie.Songs;
                trailers = result.Movie.Trailers;
                //show movies details
                ShowMovieDetails(result.Movie);
                //populate movie's posters
                //PopulatePosters(poster, result.Movie.Name, result.Movie.Pictures);
                if (poster.length > 1) {
                    $(".movie-photos").append(LoadPhotoTube(poster, "Gallery", result.Movie.Pictures));
                    var tubeWidth = $(window).width() - Math.round($(window).width() / 3);
                    $(".photo-tube-container").css("width", tubeWidth + "px");

                    InitMovieTube(".photo-tube-container");
                }

                if (songs.length > 0) {
                    PopulateSongs(songs);
                    var tubeWidth = $(window).width() - Math.round($(window).width() / 3);
                    $(".song-tube-container").css("width", tubeWidth + "px");
                    InitTrailerTube(".song-tube-container");
                }

                if (trailers.length > 0) {
                    PopulateTrailers(trailers);
                    var tubeWidth = $(window).width() - Math.round($(window).width() / 3);
                    $(".trailer-tube-container").css("width", tubeWidth + "px");
                    InitTrailerTube(".trailer-tube-container");
                }

                if (reviews.length > 0) {
                    $(".movie-reviews").append(LoadReviewsTube(reviews, "Reviews"));
                    var tubeWidth = $(window).width() - Math.round($(window).width() / 3);
                    $(".review-tube-container").css("width", tubeWidth + "px");
                    InitTrailerTube(".review-tube-container");
                }

                //ShowMovieReviews(reviews);
                PrepareGenreLinks();
                $(".gallery a[rel^='prettyPhoto']").prettyPhoto({
                    animation_speed: 'normal',
                    theme: 'dark_square',
                    slideshow: false,
                    autoplay_slideshow: false,
                    show_title: true,
                    keyboard_shortcuts: true,
                    social_tools: false,
                    allow_resize: true
                });

                TrackRecentMovieVisit(result.Movie.Name);
                $(".footer").show();
            } else {
                //$(".movie-content").html("Unable to find movie.");
            }
        }
    } catch (e) {
        //$(".movie-content").html("Unable to find movie.");
    }
}

var ShowMovieDetails = function (movie) {
    var movieDetalis = $("<div/>").addClass("movie-description");

    var directors = "",
        directorsList = "",
        writers = "",
        writerList = "",
        producers = "",
        producersList = "",
        music = "",
        musicList = "",
        singer = "",
        singerList = "",
        cast = "",
        actorList = "",
        artistCounter = 0,
        songsList = "";

    var casts = [];
    casts = JSON.parse(movie.Cast);
    if (casts != "undefined" && casts != null && casts.length > 0) {
        for (var c = 0; c < casts.length; c++) {

            var name = null;
            if (casts[c].name != null)
                name = casts[c].name.split(' ').join('-').toLowerCase();

            if (casts[c].role.toLowerCase() == "director" && casts[c].name != null && directors.indexOf(casts[c].name) == -1) {
                if (casts[c].charactername == null) {
                    directors += "<a href=\"/artists/" + name + "\" title='click here to view profile'>" + casts[c].name + "</a>, ";
                    directorsList += "<li class='team-item'><a href=\"/artists/" + name + "\">" + casts[c].name + "</a></li>";
                }
            }
            else if (casts[c].role.toLowerCase() == "writer" && casts[c].name != null && writers.indexOf(casts[c].name) == -1) {
                writers += "<a  href=\"/artists/" + name + "\" title='click here to view profile'>" + casts[c].name + "</a>, ";
                writerList += "<li class='team-item'><a href=\"/artists/" + name + "\">" + casts[c].name + "</a></li>";
            }
            else if (casts[c].role.toLowerCase() == "music" && casts[c].name != null && music.indexOf(casts[c].name) == -1) {

                if (casts[c].charactername == null || casts[c].charactername.indexOf("music") > -1) {
                    music += "<a  href=\"/artists/" + name + "\" title='click here to view profile'>" + casts[c].name + "</a>, ";
                    musicList += "<li class='team-item'><a href=\"/artists/" + name + "\">" + casts[c].name + "</a></li>";
                } else if (casts[c].charactername == "playback singer") {
                    singer += "<a  href=\"/artists/" + name + "\" title='click here to view profile'>" + casts[c].name + "</a>, ";
                    singerList += "<li class='team-item'><a href=\"/artists/" + name + "\">" + casts[c].name + "</a></li>";
                }
            }
            else if (casts[c].role.toLowerCase() == "producer" && casts[c].name != null && producers.indexOf(casts[c].name) == -1) {
                if (casts[c].charactername == "producer") {
                    producers += "<a href=\"/artists/" + name + "\" title='click here to view profile'>" + casts[c].name + "</a>, ";
                    producersList += "<li class='team-item'><a href=\"/artists/" + name + "\">" + casts[c].name + "</a></li>";
                }
            }
            else if (casts[c].role.toLowerCase() == "actor" && cast.indexOf(casts[c].name) == -1 && artistCounter < 8) {
                cast += "<a  href=\"/artists/" + name + "\" title='click here to view profile'> " + casts[c].name + "</a>, ";
                actorList += "<li class='cast-item'><span class='cast-details'><a href=\"/artists/" + name + "\">" + casts[c].name + "</a></span><span class='cast-details-right'>" + casts[c].charactername + "</span></li>";
                artistCounter++;
            }
        }
    }
    else {
        // Need to remove element if we dont have data to render it on screen
        //$("#item3").remove();
    }

    //$(".movie-poster-container").append(new RatingControl().GetRatingControl(JSON.parse(movie.MyScore), movie));
    $(movieDetalis).append(GetMovieSynopsis(movie.Synopsis));
    $(movieDetalis).append(GetMovieGenre(movie.Genre));
    $(movieDetalis).append(GetMovieCast(CleanCastString(cast)));
    $(movieDetalis).append(GetMovieDirector(CleanCastString(directors)));
    $(movieDetalis).append(GetMovieProducer(CleanCastString(producers)));
    $(movieDetalis).append(GetMovieMusicDirector(CleanCastString(music)));
    $(movieDetalis).append(GetMovieSinger(CleanCastString(singer)));
    $(movieDetalis).append(GetMovieWriter(CleanCastString(writers)));
    //$(movieDetalis).append(GetMovieStats(movie.Stats));
    $(".movie-details").append(movieDetalis);
}

// images is JSON object
var PopulatePosters = function (images, movieName, picture) {

    var poster = [];
    var pictures = [];

    poster = images;//JSON.parse(images);

    if (picture && picture != "") {
        pictures = JSON.parse(picture);
    }

    if (poster && poster.length > 1) {

        var ul = $("<ul/>").attr("class", "gallery clearfix");

        for (var p = 0; p < poster.length; p++) {

            var img = $("<img/>")
            img.attr("class", "gallery-image");
            img.attr("alt", movieName);
            img.attr("src", PUBLIC_BLOB_URL + poster[p]);
            img.error(function () {
                $(this).hide();
            });

            // Track the poster click event in GA
            img.click(function () {
                trackPhotoLink($(this).attr("src"));
            });

            var li = $("<li/>").css("display", "inline-block").css("text-align", "center").css("float", "left");
            var a = $("<a/>").attr("href", PUBLIC_BLOB_URL + poster[p]).attr("rel", "prettyPhoto[gallery]").css("float", "left").css("position", "relative");
            var source;

            if (pictures.length == 0 || pictures[p] == null || pictures[p].source == null || pictures[p].source == "undefined" || pictures[p].source == "") {
                source = $("<span/>").html("Source: IMDB");
            } else {
                source = $("<span/>").html("Source: ").append($("<a/>").attr("href", pictures[p].source).html("View").attr("target", "new"));
            }

            $(source).css(
                {
                    "display": "block",
                    "position": "absolute",
                    "top": "170px",
                    "left": "5px",
                    "float": "left",
                    "padding-left": "10px",
                    "background-color": "white",
                    "padding-right": "10px",
                    "border-radius": "0px 3px 3px 0px",
                    "border": "1px solid #333",
                    "border-left": "0px",
                    "opacity": "0.8"
                }
           );

            $(a).append(img);
            $(a).append(source);
            $(li).append(a);
            //$(li).append(source);
            $(ul).append(li);
            //$(".movie-poster-details").append(img);
        }

        $(".movie-photos").append(ul);

        ArrangeImages($(".movie-photos"));

        /*Pagination for posters 
        if ($(window).width() < 768) {
            new Pager($(".movie-poster-details"), "#posters-pager");
        }
        else {
            PreparePaginationControl($(".movie-poster-details"), { pagerContainerId: "posters-pager", tileWidth: "370" });
            $(".movie-poster-details").append($("#posters-pager"));
        }*/

        /*$(window).resize(function () {
            if ($(window).width() < 768) {
                new Pager($(".movie-poster-details"), "#posters-pager");
            }
            else {
                PreparePaginationControl($(".movie-poster-details"), { pagerContainerId: "posters-pager", tileWidth: "370" });
                $(".movie-poster-details").append($("#posters-pager"));
            }
        });*/

        $(".link-container").show();
    }
    else {
        $(".movie-poster-details").hide();

        // Remove Posters link from the top nav
        $(".top-nav-bar").find("li").each(function () {
            if ($(this).attr("link-id") == "mov_poster") {
                $(this).remove();
            }
        });

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
        if (review.length <= 0) {
            //$(".movie-review-details").html("<b>Currently this movie does not have any reviews.</b>");
            $(".review-details").hide();
        }
        else {
            if ($(window).width() < 768) {
                new Pager($(".movie-review-details"), "#review-pager");
            }
            else {
                PreparePaginationControl($(".movie-review-details"), { pagerContainerId: "review-pager", tileWidth: "360" });
                $(".movie-review-details").append($("#review-pager"));
            }
        }

        $(window).resize(function () {
            if ($(window).width() < 768) {
                new Pager($(".movie-review-details"), "#review-pager");
            }
            else {
                PreparePaginationControl($(".movie-review-details"), { pagerContainerId: "review-pager", tileWidth: "360" });
                $(".movie-review-details").append($("#review-pager"));
            }
        });
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
    if (!genre) {
        GetMovieDataHolder("Genre:", "-");
    } else {
        genre = genre.length == 0 ? "-" : genre;
        return GetMovieDataHolder("Genre:", genre);
    }
}

var GetMovieCast = function (movieCast) {
    if (!movieCast) {
        return GetMovieDataHolder("Stars:", "-");
    }
    else {
        movieCast = movieCast.length == 0 ? "-" : movieCast;
        return GetMovieDataHolder(
            "Stars:",
            "<span itemprop=\"actor\" itemscope itemtype=\"http://schema.org/Person\">" +
            "<span itemprop=\"name\">" +
                movieCast +
            "</span></span>");
    }
}

var GetMovieDirector = function (movieCast) {
    if (!movieCast) {
        return GetMovieDataHolder("Directors:", "-");
    }
    else {
        movieCast = movieCast.length == 0 ? "-" : movieCast;
        return GetMovieDataHolder(
            "Directors:",
            "<span itemprop=\"director\" itemscope itemtype=\"http://schema.org/Person\">" +
            "<span itemprop=\"name\">" +
                movieCast +
            "</span></span>");
    }
}

var GetMovieProducer = function (movieCast) {
    if (!movieCast) {
        return GetMovieDataHolder("Producers:", "-");
    }
    else {
        movieCast = movieCast.length == 0 ? "-" : movieCast;
        return GetMovieDataHolder("Producers:", movieCast);
    }
}

var GetMovieMusicDirector = function (movieCast) {
    if (!movieCast) {
        return GetMovieDataHolder("Music:", "-");
    }
    else {
        movieCast = movieCast.length == 0 ? "-" : movieCast;
        return GetMovieDataHolder("Music:", movieCast);
    }
}
var GetMovieSinger = function (movieCast) {
    if (!movieCast) {
        return GetMovieDataHolder("Singer:", "-");
    }
    else {
        movieCast = movieCast.length == 0 ? "-" : movieCast;
        return GetMovieDataHolder("Singer:", movieCast);
    }
}

var GetMovieWriter = function (movieCast) {
    if (!movieCast) {
        return GetMovieDataHolder("Writer:", "-");
    }
    else {
        movieCast = movieCast.length == 0 ? "-" : movieCast;
        return GetMovieDataHolder("Writer:", movieCast);
    }
}

var GetMovieStats = function (movieStats) {
    if (!movieStats)
        return GetMovieDataHolder("Statistics:", "-");
    else {
        movieStats = movieStats.length == 0 ? "-" : movieStats;
        return GetMovieDataHolder("Statistics:", movieStats);
    }
}

var GetMovieSynopsis = function (synopsis) {
    if (!synopsis)
        GetMovieDataHolder("Synopsis:", "-");
    else {
        synopsis = synopsis.length == 0 ? "-" : synopsis;
        return GetMovieDataHolder("Synopsis:", synopsis);
    }
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

    $(".movie-songs").append(LoadSongTube(songs, "Songs"));
    //SongList(songs, "Song");
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
            DisplayModal();
            trackSongLink(url);
            $('html, body').animate({ scrollTop: 0 }, 500);
        });

        var playImg = $("<img/>").attr("class", "song-play").attr("video-link", url + "?autoplay=1").attr("src", "../images/play-video.png").attr("title", "Play YouTube " + type);

        var title = $("<span/>").html(new Util().GetEllipsisText(videos[i].SongTitle, 16)).attr("title", videos[i].SongTitle);

        var a = $("<a/>").attr("href", "#modalMsg").attr("role", "button").attr("data-toggle", "modal").attr("video-link", url + "?autoplay=1").click(function () {
            DisplayModal($(this).attr("video-link"));
            trackSongLink(url);
            $('html, body').animate({ scrollTop: 0 }, 500);
        });

        $('#modalMsg').on('hidden', function () {
            RemoveModal();
        });

        $(a).append(img);
        $(a).append(playImg);

        $(a).append(title);
        $(li).append(a);

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
            PreparePaginationControl($(".songs"), { pagerContainerId: "songs-pager", tileWidth: "250" });
        }

        $(window).resize(function () {
            if ($(window).width() < 768) {
                var pager = new Pager($(".songs"), "#songs-pager");
            }
            else {
                PreparePaginationControl($(".songs"), { pagerContainerId: "songs-pager", tileWidth: "250" });
            }
        });

        $(".songs").append($("#songs-pager"));
        $(".songs").attr("style", "display:block !important;");
    }
    else {
        // Remove Songs link from the top nav
        $(".top-nav-bar").find("li").each(function () {
            if ($(this).attr("link-id") == "movie_songs") {
                $(this).remove();
            }
        });

    }
}

var PopulateTrailers = function (trailer) {
    try {
        var trailers = [];
        trailers = JSON.parse(trailer);
        $(".movie-trailers").append(LoadTrailerTube(trailers, "Trailer"));
        //TrailerList(trailers, "Trailer");
    }
    catch (e) {
        $(".top-nav-bar").find("li").each(function () {
            if ($(this).attr("link-id") == "movie_trailers") {
                $(this).remove();
            }
        });
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

        var li = $("<li/>").attr("class", "song").attr("video-link", url + "?autoplay=1").attr("title", "Play YouTube " + type + " - " + videos[i].Title);
        var playImg = $("<img/>").attr("class", "song-play").attr("video-link", url + "?autoplay=1").attr("src", "../images/play-video.png").attr("title", "Play YouTube " + type);

        var title = $("<span/>").html(new Util().GetEllipsisText(videos[i].Title, 16)).attr("title", videos[i].Title);

        var a = $("<a/>").attr("href", "#modalMsg").attr("role", "button").attr("data-toggle", "modal").attr("video-link", url + "?autoplay=1").click(function () {
            DisplayModal($(this).attr("video-link"));
            trackVideoLink(url);
            $('html, body').animate({ scrollTop: 0 }, 500);
        });

        /*$('#modalMsg').on('hidden', function () {
            RemoveModal();
        });*/

        $(a).append(img);
        $(a).append(playImg);

        $(a).append(title);
        $(li).append(a);

        if (videos[i].YoutubeURL != undefined && videos[i].YoutubeURL != "" && videos[i].YoutubeURL != null && videos[i].Thumb != undefined && videos[i].Thumb != "" && videos[i].Thumb != null) {
            $(ul).append(li);
            songHasLink = true;
            j++;
        }
    }

    if (songHasLink) {
        $(".movie-trailers").append(ul);
        $(".movie-trailers").attr("id", "movie_trailers");

        /*if ($(window).width() < 768) {
            var pager = new Pager($(".trailers"), "#trailer-pager");
        }
        else {
            PreparePaginationControl($(".trailers"), { pagerContainerId: "trailer-pager", tileWidth: "250" });
        }

        $(window).resize(function () {
            if ($(window).width() < 768) {
                var pager = new Pager($(".trailers"), "#trailer-pager");
            }
            else {
                PreparePaginationControl($(".trailers"), { pagerContainerId: "trailer-pager", tileWidth: "250" });
            }
        });
        */
        //$(".movie-trailers").append($("#trailer-pager"));
        //$(".movie-trailers").attr("style", "display:block !important;");
    }
    /*else {
        // Remove Video link from the top nav
        $(".top-nav-bar").find("li").each(function () {
            if ($(this).attr("link-id") == "movie_trailers") {
                $(this).remove();
            }
        });
    }*/
}

function DisplayModal(url) {
    //$("#overlay").attr("class", "OverlayEffect");
    //$("#modalMsg").attr("class", "ShowModal");
    $("#modal-video").find("iframe").each(function () {
        $(this).attr("src", url);
    });
}

function RemoveModal() {
    $("#modal-video").find("iframe").each(function () {
        $(this).attr("src", "");
    });

    return false;
}