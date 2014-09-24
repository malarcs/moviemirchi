﻿function LoadReviewsByReviewer(reviewer) {
    reviewer = reviewer.split('-').join(' ');
    reviewer = reviewer.replace('%7c', '-');
    var reviewPath = "/api/ReviewerInfo?name=" + reviewer;
    CallHandler(reviewPath, ShowReviews);
}

var ShowReviews = function (data) {
    try {
        var result = JSON.parse(data);
        if (result.Status != undefined || result.Status == "Error") {
            $(".movies").html(result.UserMessage);
        }
        else {
            if (result.ReviewsDetails != undefined && result.ReviewsDetails != null && result.ReviewsDetails.length > 0) {
                $(".movies").append(GetTubeControl(result.Name, "review-list", "review-pager"));

                var fileName = "/images/user.png";
                var name = result.Name;
                var affiliation = "";

                for (k = 0; k < critics.length; k++) {
                    if (critics[k] != null && critics[k] != undefined && critics[k].name == result.Name) {
                        fileName = PUBLIC_BLOB_URL + critics[k].poster;
                        affiliation = critics[k].aff;
                        break;
                    }
                }

                $(".movies").find(".review-list").each(function () {
                    $(this).prepend(ShowPersonBio(affiliation));
                    $(this).find("img").removeAttr("style").css("width", "225px").css("float", "left");
                    InitBio();
                    $(".bio-pic").append($("<img/>").attr("src", fileName));
                    // Need to populate this text from DB
                    $(".intro-text").css("margin-left", "0px").html("Currently this critic does not have any biography on <a href=\"/home\">Movie Mirchi</a>");

                });

                var reviews = [];
                //var reviewTitle = GetTubeControl("Reviews", "reviews", "review-list-pager", null, "review_list_pagger");

                /*$(".review-list").find("ul:first").each(function () {
                    $("<div class=\"section-title large-fonts\" style=\"margin-left: 0%\">Reviews</div>").insertBefore(this);
                });*/

                reviews = result.ReviewsDetails;
                ShowReviewsByReviewer(reviews);
            }
        }
    } catch (e) {
        $(".movies").html("Unable to get reviewer details.");
    }
}

var hasArchivedReviews = false;
var hasLatestReviews = false;

var ShowReviewsByReviewer = function (review) {
    // VS - For production, following line shall be uncommented. Other line is used for demo purposes, 
    // when movies does not have any associated reivews
    //if (review != "undefined" && review != null && review.length > 0) {

    $(".movies").append(GetTubeControl("Latest Reviews", "review-list-now-playing", "now-pager"));
    /*$(".movies").append(GetTubeControl("Upcoming", "review-list-upcoming", "upcoming-pager"));*/
    $(".movies").append(GetTubeControl("Archived Reviews", "review-list-other", "other-pager"));

    if (review != "undefined" && review != null) {
        $(".link-container").show();
        GetReviewerReviews("movie-review-details", review);
    }
    else {
        $(".movie-review-details").hide();
        $(".link-container").find("div.section-title").each(function () {
            if ($(this).html() == "Reviews") {
                $(this).hide();
            }
        });
    }

    /*Pagination for movies */
    if ($(window).width() < 768) {
        new Pager($(".review-list-now-playing"), "#now-pager");
        new Pager($(".review-list-other"), "#other-pager");
    }
    else {
        PreparePaginationControl($(".review-list-now-playing"), { pagerContainerId: "now-pager", tileWidth: "300" });
        PreparePaginationControl($(".review-list-other"), { pagerContainerId: "other-pager", tileWidth: "300" });
    }

    $(window).resize(function () {
        if ($(window).width() < 768) {
            new Pager($(".review-list-now-playing"), "#now-pager");
            new Pager($(".review-list-other"), "#other-pager");
        }
        else {
            PreparePaginationControl($(".review-list-now-playing"), { pagerContainerId: "now-pager", tileWidth: "300" });
            PreparePaginationControl($(".review-list-other"), { pagerContainerId: "other-pager", tileWidth: "300" });
        }
    });

    if (!hasArchivedReviews) {
        $("#other_movie").hide();
    }
    if (!hasLatestReviews) {
        $("#now_playing").hide();
    }
}

