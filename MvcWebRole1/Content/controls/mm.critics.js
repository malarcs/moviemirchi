﻿$(document).ready(function () {
    var ul = $(".critics-container").find("ul").css("height", "280px");
    critics = [
        { href: 'rajeev-masand', title: 'Rajeev Masand', name: 'Rajeev Masand', poster: 'rajeev-masand.jpg', aff: 'CNN IBN' },
        { href: 'tarun-adarsh', title: 'Tarun Adarsh', name: 'Tarun Adarsh', poster: 'tarun-adarsh.jpg', aff: 'Bollywood Hungama, Trade Guide' },
        { href: 'anupama-chopra', title: 'Anupama Chopra', name: 'Anupama Chopra', poster: 'anupama-chopra.jpg', aff: 'Hindustan Times' },
        { href: 'shubra-gupta', title: 'Shubra Gupta', name: 'Shubra Gupta', poster: 'shubra-gupta.jpg', aff: 'Indian Express' },
        { href: 'tushar-joshi', title: 'Tushar Joshi', name: 'Tushar Joshi', poster: 'tushar-joshi.jpg', aff: 'DNA' },
        { href: 'raja-sen', title: 'Raja Sen', name: 'Raja Sen', poster: 'raja-sen.jpg', aff: 'rajasen.com' },
        { href: 'shubha-shetty', title: 'Shubha Shetty', name: 'Shubha Shetty', poster: 'shubha-shetty.jpg', aff: 'Mid-day' },
        { href: 'pratim-gupta', title: 'Pratim Gupta', name: 'Pratim Gupta', poster: 'pratim-gupta.jpg', aff: 'Telegraph, Kolkata' },
        { href: 'vajir-singh', title: 'Vajir Singh', name: 'Vajir Singh', poster: 'vajir-singh.jpg', aff: 'Box office India' },
        { href: 'komal-nahta', title: 'Komal Nahta', name: 'Komal Nahta', poster: 'komal-nahta.jpg', aff: 'Film Information' },
        { href: 'sudhish-kamath', title: 'Sudhish Kamath', name: 'Sudhish Kamath', poster: 'sudhish-kamath.jpg', aff: 'The Hindu' },
        { href: 'sarit-ray', title: 'Sarit Ray', name: 'Sarit Ray', poster: 'sarit-ray.jpg', aff: 'Hindustan Times' },
        { href: 'gaurav-malani', title: 'Gaurav Malani', name: 'Gaurav Malani', poster: 'gaurav-malani.jpg', aff: 'Times of India' },
        { href: 'aseem-chhabra', title: 'Aseem Chhabra', name: 'Aseem Chhabra', poster: 'aseem-chhabra.jpg', aff: 'Rediff' }, ,
        { href: 'saibal-chatterjee', title: 'Saibal Chatterjee', name: 'Saibal Chatterjee', poster: 'saibal-chatterjee.jpg', aff: 'NDTV' },
        { href: 'rachit-gupta', title: 'Rachit Gupta', name: 'Rachit Gupta', poster: 'rachit-gupta.jpg', aff: 'Filmfare' }, ,
        { href: 'mihir-fadnavis', title: 'Mihir Fadnavis', name: 'Mihir Fadnavis', poster: 'mihir-fadnavis.jpg', aff: 'FirstPost' }
    ].forEach(function (critic) {
        ul.append(
            "<li class=\"reviewer\">" +
                "<a href=\"movie/reviewer/" + critic.href + "\" title=\"" + critic.title + "\">" +
                    "<div id=\"picAndCaption\" class=\"viewingDiv\">" +
                        "<div id=\"imageContainer\" class=\"viewer\">" +
                            "<img id=\"imageEl\" class=\"movie-poster shownImage\" title=\"" + critic.title + "\" alt=\"" + critic.title + "\" src=\"/Posters/Images/critic/" + critic.poster + "\" style=\"margin: auto; height: 250px; width: 193px; \" onerror=\"LoadDefaultCriticImage(this);\">" +
                            "<div class=\"captionAndNavigate\" style=\"width: 193px; padding: 15px;\">" +
                                "<div id=\"captionCredit\" style=\"width: 328px;\" class=\"multimediaCaption\">" +
                                    "<div id=\"photoCaption\">" +
                                        "<div class=\"img-movie-name\" style=\"font-size: 1.5em;\">" + critic.name + "</div>" +
                                        "<div class=\"img-movie-genre\" style=\"font-size: 1em;\">" + critic.aff + "</div>" +
                                    "</div>" +
                                "</div>" +
                            "</div>" +
                        "</div>" +
                    "</div>" +
                "</a>" +
            "</li>");
    });

    var totalReviewers = $(".critics-container ul li.reviewer").length;
    var pagerJson = { "pagerContainer": "critics-container", "tilesInPage": 5, "totalTileCount": totalReviewers, "pagerContainerId": "critics-pager", "tileWidth": "220" };
    PreparePaginationControl($(".critics-container"), pagerJson);
});