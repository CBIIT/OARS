const models = window['powerbi-client'].models;

function callDotNetSaveStudyMethod(study) {
    DotNet.invokeMethodAsync("TheradexPortal", "SaveStudyFromSlicer", study)
        .then(data => {
            // once async call is done, this is to display the result returned by .NET call
            console.log("Slicer data :" + data);
        });
}

function embedFullReport(reportContainer, accessToken, filterTargets, embedUrl, embedReportId, studyFilter, startingStudy) {

    // studyFilter = list of studies to show in study id filter
    // startingStudy = the study that should be checked to start

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
        requireSingleSelection: true,
        displaySettings: { isHiddenInViewMode: true }
    }

    const study_filter2 =
    {
        $schema: "http://powerbi.com/product/schema#basic",
        target: {
            table: "PROTOCOL",
            column: "STUDY_ID"
        },
        operator: "In",
        values: [startingStudy],
        filterType: models.FilterType.BasicFilter,
        requireSingleSelection: true,
        displaySettings: { isHiddenInViewMode: false }
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
            filterPaneEnabled: true,
            navContentPaneEnabled: false,
            background: models.BackgroundType.Transparent
        },
        filters: [study_filter, study_filter2]
    };

    // Embed report
    let report = powerbi.embed(reportContainer, config);

    let curStudies = "";
    let newStudies = "";
    let filterCount = 0;
    let i = 0;

    report.on('rendered', async function () {
        console.log('rendered event');
        const filters = await report.getFilters();
        console.log("Report Filters: " + filters);
        console.log("# of filters: " + filters.length);
        filterCount = filters.length;

        newStudies = "";
        console.log(filters[filterCount-1].values);
        for (i in filters[filterCount - 1].values) {
            if (newStudies == "") {
                newStudies = filters[filterCount - 1].values[i]
            }
            else {
                newStudies = newStudies + "," + filters[filterCount - 1].values[i];
            }
        }
        console.log(newStudies);
        if (newStudies != curStudies) {
            console.log("Saving studies");
            console.log("Curr:" + curStudies + "   New: " + newStudies);
            curStudies = newStudies;
            await callDotNetSaveStudyMethod(newStudies);
        }
    });

    window.fullReport = report;
    return report;
}

export { embedFullReport }