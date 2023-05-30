const LAYOUT = {
    MARGIN: 16,
    VISUAL_ASPECT_RATIO: 9 / 16,
    VISUAL_MIN_WIDTH: 600,
    SLICER_HEIGHT: 70
};

const models = window['powerbi-client'].models;

function initCustomLayoutReport(dotnetRef, reportContainer, accessToken, embedUrl, embedReportId, embedPageId, embedVisualIds) {
    let report = new PowerBiEmbed(dotnetRef, reportContainer, accessToken, embedUrl, embedReportId, embedPageId, embedVisualIds);
    return report;
}

function embedFullReport(reportContainer, accessToken, embedUrl, embedReportId) {
    var config = {
        type: 'report',
        tokenType: models.TokenType.Embed,
        accessToken: accessToken,
        embedUrl: embedUrl,
        id: embedReportId,
        permissions: models.Permissions.All,
        settings: {
            filterPaneEnabled: true,
            navContentPaneEnabled: true
        }
    };
    return powerbi.embed(reportContainer, config);
}

class PowerBiEmbed {
    constructor(dotnetRef, reportContainer, accessToken, embedUrl, embedReportId, embedPageId, embedVisualIds) {
        this.dotnetRef = dotnetRef;
        this.reportContainer = reportContainer;
        this.accessToken = accessToken;
        this.embedUrl = embedUrl;
        this.embedReportId = embedReportId;
        this.embedPageId = embedPageId;
        this.embedVisualIds = embedVisualIds;
        this.embedSlicerIds = [];

        this.report = powerbi.embed(reportContainer, this.getConfig());
        this.report.on('loaded', function() {
            console.log('Loaded');
            window.powerBiEmbed.onLoad();
        });
        window.powerBiEmbed = this;
        window.addEventListener("resize", this.renderVisuals.bind(this));
    }

    async renderSlicers() {
        let page = await this.report.getPageByName(this.embedPageId);
        let slicers = await page.getSlicers();
        this.embedSlicerIds = slicers.map((s) => s.name)
        this.renderVisuals();
    }

    async renderVisuals() {
        await this.report.updateSettings(this.getSettings());
    }

    getConfig() {
        return {
            type: 'report',
            tokenType: models.TokenType.Embed,
            accessToken: this.accessToken,
            embedUrl: this.embedUrl,
            id: this.embedReportId,
            permissions: models.Permissions.View,
            settings: this.getSettings()
        }
    }

    onLoad() {
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
        if (columns === 1) {
            visualHeight /= 2;
        }
    
        let rows = Math.ceil(this.embedVisualIds.length / columns);
        let reportHeight = Math.max(0, (rows * visualHeight) + LAYOUT.SLICER_HEIGHT + (rows + 2) * LAYOUT.MARGIN);
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
        y += LAYOUT.SLICER_HEIGHT + LAYOUT.MARGIN;

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
}

export { initCustomLayoutReport, embedFullReport, PowerBiEmbed }