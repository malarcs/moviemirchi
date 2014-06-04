﻿var MovieInformation = function () {
    MovieInformation.prototype.GetMovieInfoContainer = function (id, title) {

        var container = $("<div/>").attr("id", id).attr("class", "basic-movie-info");
        var sectionTitle = $("<div/>").html(title).attr("class", "section-title");
        $(container).append(sectionTitle);
        return container;
    }

    MovieInformation.prototype.BuildForm = function () {

        var formContainer = $("<div/>").attr("class", "form-container");

        var isEnabled = new FormBuilder().GetCheckBox("isEnabled", "Enabled", false);
        var uniqueName = new FormBuilder().GetTextField("txtUnique", "Unique Name", "Unique Name");
        var friendlyName = new FormBuilder().GetTextField("txtFriendly", "Friendly Name", "Friendly Name");
        var synopsis = new FormBuilder().GetTextArea("txtSynopsis", "Movie Synopsis", "Synopsis");
        var processedSynopsis = new FormBuilder().GetTextArea("txtProcessedSynopsis", "Processed Synopsis", "Processed Synopsis");
        var budget = new FormBuilder().GetTextArea("txtBudget", "Movie Budget/Stats", "Budget");
        // va
        var state = new FormBuilder().GetTextField("txtState", "Movie Current State", "State");


        $(formContainer).append(uniqueName);
        $(formContainer).append(friendlyName);
        $(formContainer).append(synopsis);
        $(formContainer).append(processedSynopsis);
        $(formContainer).append(budget);
        $(formContainer).append(state);
        $(formContainer).append(isEnabled);

        return formContainer;
    }
}