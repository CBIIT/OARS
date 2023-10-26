const models = window['powerbi-client'].models;

function callDotNetSaveStudyMethod(study, operator) {
    DotNet.invokeMethodAsync("TheradexPortal", "SaveStudyFromSlicer", study, operator)
        .then(data => {
            // once async call is done, this is to display the result returned by .NET call
            console.log("Invoke call :" + data);
        });
}

function embedFullReport(reportContainer, accessToken, filterTargets, embedUrl, embedReportId, filterType, studyFilter, startingStudy) {

    // filterType = None, Single, Multi
    // studyFilter = list of studies to show in study id filter
    // startingStudy = the study that should be checked to start
    let singleSelection = false;
    let filterHidden = false;
    let filterPaneShown = true;
    if (filterType == "Single")
        singleSelection = true;
    else if (filterType == "Multi")
        singleSelection = false;
    else if (filterType == "None" || filterType == null)
        filterHidden = true;

    const study_filter =
    {
        $schema: "http://powerbi.com/product/schema#basic",
        target: {
            table: "PROTOCOL",
            column: "STUDY_ID"
        },
        operator: "In",
        values: studyFilter,
        filterType: models.FilterType.BasicFilter,
        requireSingleSelection: singleSelection,
        displaySettings: {
            isHiddenInViewMode: true,
            displayName: "Study ID"
        }
    }

    const study_filter2 =
    {
        $schema: "http://powerbi.com/product/schema#basic",
        target: {
            table: "PROTOCOL",
            column: "STUDY_ID"
        },
        operator: "In",
        values: startingStudy,
        filterType: models.FilterType.BasicFilter,
        requireSingleSelection: singleSelection,
        displaySettings: {
            isHiddenInViewMode: filterHidden,
            displayName: "Study ID"
        }
    }

    // Build config
    var config = {
        type: 'report',
        tokenType: models.TokenType.Embed,
        accessToken: accessToken,
        embedUrl: embedUrl,
        id: embedReportId,
        permissions: models.Permissions.All,
        background: models.BackgroundType.Transparent,
        settings: {
            filterPaneEnabled: filterPaneShown,
            navContentPaneEnabled: false,
            background: models.BackgroundType.Transparent
        },
        filters: [study_filter, study_filter2]
    };

    // Embed report
    let report = powerbi.embed(reportContainer, config);

    let curStudies = "";
    let newStudies = "";
    let operator = "";
    let filterCount = 0;
    let i = 0;

    report.on('rendered', async function () {
        console.log('rendered event');
        const filters = await report.getFilters();
        //console.log("Report Filters: " + filters);
        //console.log("# of filters: " + filters.length);
        filterCount = filters.length;
        console.log("Study Filter: " + filters[filterCount - 1]);
        console.log("Operator: " + filters[filterCount - 1].operator);
        newStudies = "";
        operator = filters[filterCount - 1].operator;
        console.log(filters[filterCount-1].values);
        for (i in filters[filterCount - 1].values) {
            if (newStudies == "") {
                newStudies = filters[filterCount - 1].values[i]
            }
            else {
                newStudies = newStudies + "," + filters[filterCount - 1].values[i];
            }
        }
        //console.log(newStudies);
        if (newStudies != curStudies) {
            console.log("Saving studies");
            console.log("Curr:" + curStudies + "   New: " + newStudies);
            curStudies = newStudies;
            await callDotNetSaveStudyMethod(newStudies, operator);
        }
    });

    window.fullReport = report;
    return report;
}

export { embedFullReport }