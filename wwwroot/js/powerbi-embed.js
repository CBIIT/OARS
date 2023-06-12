const LAYOUT = {
    MARGIN: 16,
    VISUAL_ASPECT_RATIO: 9 / 16,
    VISUAL_MIN_WIDTH: 600,
    SLICER_HEIGHT: 70
};

const models = window['powerbi-client'].models;

// Wrap class init in a function call - Blazor interop cannot instantiate new classes directly
function initCustomLayoutReport(dotnetRef, reportContainer, accessToken, embedUrl, embedReportId, embedPageId, embedVisualIds, embedSlicerIds) {
    let report = new PowerBiEmbed(dotnetRef, reportContainer, accessToken, embedUrl, embedReportId, embedPageId, embedVisualIds, embedSlicerIds);
    return report;
}

class PowerBiEmbed {
    constructor(dotnetRef, reportContainer, accessToken, filterTargets, embedUrl, embedReportId, embedPageId, embedVisualIds, embedSlicerIds) {
        this.dotnetRef = dotnetRef;
        this.reportContainer = reportContainer;
        this.accessToken = accessToken;
        this.filterTargets = filterTargets;
        this.embedUrl = embedUrl;
        this.embedReportId = embedReportId;
        this.embedPageId = embedPageId;
        this.embedVisualIds = embedVisualIds ?? [];
        this.embedSlicerIds = embedSlicerIds ?? [];
        this.filterStudyIds = [];

        // Get study filter from local storage
        this.loadFilterData();

        // Embed report
        this.report = powerbi.embed(reportContainer, this.getConfig());

        // Add event handlers
        // Update component state on load
        this.report.on('loaded', function() {
            this.onLoad();
        }.bind(this));

        window.powerBiEmbed = this; // TODO - remove temp debugging

        // Rerender on window resize, with debounce
        if (window.wrPbiResizeHandler) {
            window.removeEventListener("resize", window.wrPbiResizeHandler);
        }
        window.wrPbiResizeHandler = window.addEventListener("resize", debounce(function () {
            this.renderVisuals();
        }.bind(this), 500));
    }

    async renderVisuals() {
        console.log('rerender');
        this.loadFilterData();
        await this.report.updateSettings(this.getSettings());
        await this.report.updateFilters(models.FiltersOperations.Replace, this.getFilters())
    }

    loadFilterData() {
        let filterData = localStorage.getItem('selectedStudies');
        if (filterData) {
            let studies = JSON.parse(filterData);
            this.filterStudyIds = studies.map((s) => s.Study_Id);
        }
    }

    getFilters() {
        let filters = [];
        if (this.filterStudyIds && this.filterStudyIds.length > 0) {
            for (const target of this.filterTargets) {
                filters.push({
                    $schema: 'http://powerbi.com/product/schema#basic',
                    target: {
                        table: target.table,
                        column: target.column
                    },
                    operator: 'In',
                    filterType: models.FilterType.Basic,
                    values: this.filterStudyIds
                })
            }
        }
        console.log(filters);
        return filters;
    }

    getConfig() {
        return {
            type: 'report',
            tokenType: models.TokenType.Embed,
            accessToken: this.accessToken,
            embedUrl: this.embedUrl,
            id: this.embedReportId,
            permissions: models.Permissions.View,
            pageName: this.embedPageId,
            settings: this.getSettings(),
            filters: this.getFilters()
        }
    }

    onLoad() {
        //this.displayVisualInfo(); // TODO - remove temp debugging
        this.dotnetRef.invokeMethodAsync('OnLoad');
    }

    getLayoutParams() {
        // Calculate layout based on window width
        let reportWidth = this.reportContainer.offsetWidth;
        let columns = 1;
    
        if (reportWidth >= (LAYOUT.VISUAL_MIN_WIDTH * 2) + (LAYOUT.MARGIN * 3)) {
            columns = 2;
        } else if (reportWidth >= (LAYOUT.VISUAL_MIN_WIDTH * 3) + (LAYOUT.MARGIN * 4)) {
            columns = 3;
        }
    
        let visualsTotalWidth = reportWidth - (LAYOUT.MARGIN * (columns + 1));
        let visualWidth = visualsTotalWidth / columns;
        let slicerWidth = (visualWidth / 2) - (LAYOUT.MARGIN / 2);
        let visualHeight = visualWidth * LAYOUT.VISUAL_ASPECT_RATIO;
        //if (columns === 1) {
        //    visualHeight /= 2;
        //}
    
        let rows = Math.ceil(this.embedVisualIds.length / columns);
        let reportHeight = Math.max(0, (rows * visualHeight) + LAYOUT.SLICER_HEIGHT + (rows + 2) * LAYOUT.MARGIN);
        this.reportContainer.style.height = reportHeight + "px";
        return {
            reportWidth,
            reportHeight,
            columns,
            visualWidth,
            visualHeight,
            slicerWidth
        }
    }

    getSettings() {   
        var layoutParams = this.getLayoutParams();
    
        // Start building layout
        let x = LAYOUT.MARGIN;
        let y = LAYOUT.MARGIN;
    
        let visualsLayout = {};
        this.embedSlicerIds.forEach(function (slicerId) {
            visualsLayout[slicerId] = {
                x: x,
                y: y,
                width: layoutParams.slicerWidth,
                height: LAYOUT.SLICER_HEIGHT,
                displayState: {
                    mode: models.VisualContainerDisplayMode.Visible
                }
            };

            // Calculating (x,y) position for the next visual
            x += layoutParams.slicerWidth + LAYOUT.MARGIN;
    
            // Reset at end of row
            if (x + layoutParams.slicerWidth > layoutParams.reportWidth) {
                x = LAYOUT.MARGIN;
                y += LAYOUT.SLICER_HEIGHT + LAYOUT.MARGIN;
            }
        });

        x = LAYOUT.MARGIN;
        y += LAYOUT.MARGIN;

        this.embedVisualIds.forEach(function (visualId) {
            visualsLayout[visualId] = {
                x: x,
                y: y,
                width: layoutParams.visualWidth,
                height: layoutParams.visualHeight,
                displayState: {
                    mode: models.VisualContainerDisplayMode.Visible
                }
            };
    
            // Calculating (x,y) position for the next visual
            x += layoutParams.visualWidth + LAYOUT.MARGIN;
    
            // Reset at end of row
            if (x + layoutParams.visualWidth > layoutParams.reportWidth) {
                x = LAYOUT.MARGIN;
                y += layoutParams.visualHeight + LAYOUT.MARGIN;
            }
        });
    
        // Page default - hide any visuals not included
        let pagesLayout = {};
        pagesLayout[this.embedPageId] = {
            defaultLayout: {
                displayState: {
                    mode: models.VisualContainerDisplayMode.Hidden
                }
            },
            visualsLayout: visualsLayout
        };
    
        let settings = {
            background: models.BackgroundType.Transparent,
            layoutType: models.LayoutType.Custom,
            filterPaneEnabled: false,
            navContentPaneEnabled: false,
            customLayout: {
                pageSize: {
                    type: models.PageSizeType.Custom,
                    width: layoutParams.reportWidth,
                    height: layoutParams.reportHeight
                },
                displayOption: models.DisplayOption.FitToPage,
                pagesLayout: pagesLayout
            }
        };
        return settings
    }

    // Output page and visual ids to aid with manual data entry
    // TODO - remove when admin is implemented
    async displayVisualInfo() {
        const pages = await this.report.getPages()
        const reportFilters = await this.report.getFilters();
        console.log(reportFilters);
        pages.forEach(async (page) => {
            console.log(page);
            const visuals = await page.getVisuals();
            console.log(visuals);
            const filter = await page.getFilters();
            console.log(filter);
        });
    }
}

function debounce(callback, delay) {
    let timerId;
    return function (...args) {
        clearTimeout(timerId);
        timerId = setTimeout(() => {
            callback.apply(this, args);
        }, delay);
    };
}

export { initCustomLayoutReport, PowerBiEmbed }