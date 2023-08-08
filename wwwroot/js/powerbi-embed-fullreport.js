const models = window['powerbi-client'].models;

function callDotNetSaveStudyMethod(study) {
    DotNet.invokeMethodAsync("TheradexPortal", "SaveStudyFromSlicer", study[0])
        .then(data => {
            // once async call is done, this is to display the result returned by .NET call
            console.log("Slicer data :" + data);
        });
}

function embedFullReport(reportContainer, accessToken, filterTargets, embedUrl, embedReportId, studyFilter, slicerSetStudy, studySlicer) {

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
        requireSingleSelection: false
    }

    const slicer_filter =
    {
        selector: new models.VisualSelector(studySlicer),
        state: {
            filters: [
                {
                    "$schema": "http://powerbi.com/product/schema#basic",
                    "target": {
                        "table": "PROTOCOL",
                        "column": "STUDY_ID"
                    },
                    "operator": "In",
                    "values": [slicerSetStudy]
                }
            ]
        }
    };

    // Build config
    var config = {
        type: 'report',
        tokenType: models.TokenType.Embed,
        accessToken: accessToken,
        embedUrl: embedUrl,
        id: embedReportId,
        permissions: models.Permissions.All,
        background: models.BackgroundType.Transparent,
        slicers: [slicer_filter], 
        settings: {
            filterPaneEnabled: true,
            navContentPaneEnabled: false,
            background: models.BackgroundType.Transparent
        },
        filters: [study_filter]
    };

    // Embed report
    let report = powerbi.embed(reportContainer, config);

    report.on('dataSelected', async function () {
        console.log("Visual clicked event triggred");

        // Get pages
        const pages = await report.getPages();

        // Retrieve the page that contain the slicer. For the sample report it will be the active page
        let pageWithSlicer = pages.filter(function (page) {
            return page.isActive;
        })[0];

        // Get visuals present on the active page
        const visuals = await pageWithSlicer.getVisuals();

        // Retrieve the target visual.
        let slicer = visuals.filter(function (visual) {
            return visual.type === "slicer" && visual.name === studySlicer;
        })[0];

        // Get the slicer state
        const state = await slicer.getSlicerState();
        console.log("Selected Protocol: " + state.filters[0].values);

        callDotNetSaveStudyMethod(state.filters[0].values);
    });

    window.fullReport = report;
    return report;
}

export { embedFullReport }