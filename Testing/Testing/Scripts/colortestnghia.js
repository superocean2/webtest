function colorTestLevelColorDiff(level) {
    if (level < 44) {
        var col = [75, 60, 45, 30, 20, 18, 16, 15, 14, 13, 12, 11, 11, 11, 10, 10, 9, 9, 8, 8, 7, 7, 7, 7, 6, 6, 6, 6, 5, 5, 5, 5, 4, 4, 4, 4, 3, 3, 3, 3, 2, 2, 2, 2];
        return col[level];
    }
    return 1;
};
function colorTestLevelGrid(level) {
    if (level < 2) return 2;
    if (level < 4) return 3;
    if (level < 6) return 4;
    if (level < 10) return 5;
    if (level < 16) return 6;
    if (level < 20) return 7;
    if (level < 30) return 8;
    if (level < 40) return 9;
    if (level < 44) return 10;
    return 11;
};

var colorTestContainerId = 'game';
var colorTestLevel = 1;
var colorTestStartTime = 0;
var colorTestTimeleft = 15;

function ColorTestReset(container) {
    if (container) colorTestContainerId = container;
    colorTestLevel = 1;
    colorTestTimeleft = 15;
    colorTestStartTime = 0;
    colorTestRenderLevel(colorTestLevel);
    colorTestStartTime = 0;
    colorTestRefreshTime();
};
function colorTestUpdateScore() {
    var today = new Date();

    if (colorTestStartTime > 0) colorTestTimeleft = Math.round(15-(today.getTime()-colorTestStartTime)/1000);

    if (colorTestTimeleft < 0) colorTestTimeleft = 0;

    if (colorTestTimeleft > 0) {
        speedtestScoreUpdate((colorTestLevel-1).toFixed(0),colorTestTimeleft);
    } else {
        speedtestPublishResult((colorTestLevel-1).toFixed(0));
    }
};
function colorTestRefreshTime() {
    colorTestUpdateScore();
    if (colorTestTimeleft > 0) {
        setTimeout(colorTestRefreshTime, 100);
    }
}

// Returns a random integer between min (included) and max (excluded)
// Using Math.round() will give you a non-uniform distribution!
function getRandomInt(min, max) {
    return Math.floor(Math.random() * (max - min)) + min;
}

function colorTestRenderLevel(level) {
    if (colorTestTimeleft == 0) return;

    var container = $('#' + colorTestContainerId);
    container.empty();
    var grid = colorTestLevelGrid(level);
    var randomChooseRec = getRandomInt(0, grid*grid);
    for (var i = 0; i < grid * grid; i++) {

        if (i == randomChooseRec) {
            container.append('<div class="thechosenone">&nbsp;</div>');
        } else {
            container.append('<div class="missclick">&nbsp;</div>');
        }
    }
    var colordiff = colorTestLevelColorDiff(level);
    var r = Math.floor(Math.random() * (255 - colordiff));
    var g = Math.floor(Math.random() * (255 - colordiff));
    var b = Math.floor(Math.random() * (255 - colordiff));
    $('#' + colorTestContainerId + ' DIV').css({
        'width': (100 / grid).toString() + '%',
        'height': (100 / grid).toString() + '%',
        'backgroundColor': 'rgb(' + r.toString() + ',' + g.toString() + ',' + b.toString() + ')'
    });
    $('#' + colorTestContainerId + ' DIV.thechosenone').css({
        'backgroundColor': 'rgb(' + (r + colordiff).toString() + ',' + (g + colordiff).toString() + ',' + (b + colordiff).toString() + ')'
    });
    $('#' + colorTestContainerId + ' DIV.thechosenone').click(function () {
        colorTestLevel++;
        colorTestRenderLevel(colorTestLevel);
        var today = new Date();
        colorTestStartTime = today.getTime();

    });
    $('#' + colorTestContainerId + ' DIV.missclick').click(function () {
        if (colorTestStartTime>0) {
            speedtestPublishResult((colorTestLevel - 1).toFixed(0));
        }
    });
};