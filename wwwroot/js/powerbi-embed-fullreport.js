const models = window['powerbi-client'].models;

function embedFullReport(reportContainer, accessToken, filterTargets, embedUrl, embedReportId) {
    // Get study filter from local storage
    let filterData = localStorage.getItem('selectedStudies');
    let filterStudyIds = [];
    if (filterData) {
        let studies = JSON.parse(filterData);
        filterStudyIds = studies.map((s) => s.Study_Id);
    }

    // Build filter object
    let filters = [];
    if (filterStudyIds && filterStudyIds.length > 0) {
        filterTargets.forEach((target) => {
            filters.push({
                $schema: 'http://powerbi.com/product/schema#basic',
                target: {
                    table: target.table,
                    column: target.column
                },
                operator: 'In',
                filterType: models.FilterType.BasicFilter,
                values: filterStudyIds
            })
        });
    }
    console.log(filters);

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
            filterPaneEnabled: false,
            navContentPaneEnabled: false,
            background: models.BackgroundType.Transparent
        },
        filters: filters
    };

    // Embed report
    let report = powerbi.embed(reportContainer, config);
    window.fullReport = report; // TODO - remove temp debugging
    return report;
}

export { embedFullReport }