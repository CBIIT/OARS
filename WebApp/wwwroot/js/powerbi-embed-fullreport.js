const models = window['powerbi-client'].models;

function callDotNetSaveStudyMethod(study, operator) {
    DotNet.invokeMethodAsync("TheradexPortal", "SaveStudyFromSlicer", study, operator)
        .then(data => {
            // once async call is done, this is to display the result returned by .NET call
            console.log("Invoke call :" + data);
        });
}

function createFilter(filterTable, filterColumn, filterDisplayName, filterType, filterData) {

    let singleSelection = false;
    let filterHidden = false;
    let dataOperator = "";
    let dataValues = [];

    if (filterType == "Single-Hidden") {
        singleSelection = true;
        filterHidden = true;
    }
    else if (filterType == "Single-Visible") {
        singleSelection = true;
        filterHidden = false;
    }
    else if (filterType == "Multi-Hidden") {
        singleSelection = false;
        filterHidden = true;
    }
    else {
        singleSelection = false;
        filterHidden = false;
    }

    if (filterData == "All") {
        dataOperator = "All";
        dataValues = [];
    }
    else {
        dataOperator = "In";
        dataValues = filterData.split(",");
    }

    const newFilter =
    {
        $schema: "http://powerbi.com/product/schema#basic",
        target: {
            table: filterTable,
            column: filterColumn
        },
        operator: dataOperator,
        values: dataValues,
        filterType: models.FilterType.BasicFilter,
        requireSingleSelection: singleSelection,
        displaySettings: {
            isHiddenInViewMode: filterHidden,
            displayName: filterDisplayName
        }
    }

    return newFilter;
}

function embedFullReport(reportContainer, accessToken, embedUrl, embedReportId, filterTable, filterColumn, filterDisplayName, filterType, filterData) {

    // filterName & following params should be arrays with a count of the # of params being sent in
    // In the case of studyid as a full list, then a sublist for the current_protocols, there will be sent in as two filters.
    // Parameters in:
    // filterTable  : table to filter
    // filterField  : column in table to filter
    // filterDisplayName    : Display this as the name of the filter in report
    // filterData   : full list of data in filter box

    let filterPaneShown = true;
    let reportFilters = [];

    if (filterTable != null) {
        for (var j = 0; j < filterTable.length; j++) {
            reportFilters.push(createFilter(filterTable[j], filterColumn[j], filterDisplayName[j], filterType[j], filterData[j]));
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
        filters: reportFilters
    };

    // Embed report
    let report = powerbi.embed(reportContainer, config);
    let curStudies = "";
    let newStudies = "";
    let operator = "";
    let filterCount = 0;
    let i = 0;

    report.on('rendered', async function () {
        //console.log('rendered event');
        const filters = await report.getFilters();
        //console.log("Report Filters: " + filters);
        //console.log("# of filters: " + filters.length);
        filterCount = filters.length;
        if (filters.length > 0 && filters[0].target.table == "PROTOCOL" && filters[0].target.column == "STUDY_ID") {
            //console.log("Study Filter: " + filters[filterCount - 1].displaySettings.displayName);
            //console.log("Operator: " + filters[filterCount - 1].operator);
            newStudies = "";
            operator = filters[filterCount - 1].operator;
            //console.log(filters[filterCount - 1].values);
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
                //console.log("Saving studies");
                //console.log("Curr:" + curStudies + "   New: " + newStudies);
                curStudies = newStudies;

                await callDotNetSaveStudyMethod(newStudies, operator);
            }
        }
        else
            console.log("No filters or First filter is not Study Id");
    });

    window.fullReport = report;
    return report;
}

export { embedFullReport }