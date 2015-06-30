function colorTestLevelColorDiff(level) {
    if (level <= 58) {
        var col = [105, 75, 60, 45, 30, 20, 18, 16, 15, 15, 15, 14, 14, 14, 13, 13, 13, 12, 12, 12, 11, 11, 11, 10, 10, 9, 9, 8, 8, 7, 7, 7, 7, 6, 6, 6, 6, 5, 5, 5, 5, 4, 4, 4, 4, 3, 3, 3, 3, 2, 2, 2, 2, 1, 1, 1, 1, 1];
        return col[level - 1];
    }
    return 1;
};
function colorTestLevelGrid(level) {
    if (level < 2) return 2;
    if (level < 4) return 3;
    if (level < 8) return 4;
    if (level < 13) return 5;
    if (level < 22) return 6;
    if (level < 32) return 7;
    if (level < 36) return 8;
    if (level < 40) return 9;
    if (level < 44) return 10;
    if (level < 48) return 11;
    return 12;
};

var colorTestContainerId = 'game';
var colorTestLevel = 1;
var colorTestPenalty = 0;
var colorTestTimePenalty = 0;
var colorTestPenaltySec = 3;
var colorTestCorrect = -1;
var colorTestStartTime = 0;
var colorTestTimeleft = 15;
var colorTestHighscore = 0;
function ColorTestReset(container) {
    if (container) colorTestContainerId = container;
    colorTestLevel = 1;
    colorTestPenalty = 0;
    colorTestTimePenalty = 0;
    colorTestTimeleft = 15;
    colorTestStartTime = 0;
    colorTestRenderLevel(colorTestLevel);
    colorTestStartTime = 0;
    colorTestRefreshTime();
};
function colorTestUpdateScore(startclock) {
    var today = new Date();
    if (startclock) {
        if (colorTestStartTime == 0) colorTestStartTime = today.getTime();
    }



    if (colorTestStartTime > 0) colorTestTimeleft = Math.round((15 - colorTestTimePenalty * colorTestPenaltySec - today.getTime() / 1000 + colorTestStartTime / 1000) * 100) / 100;

    if (colorTestTimeleft < 0) colorTestTimeleft = 0;
    if (colorTestLevel > colorTestHighscore) colorTestHighscore = colorTestLevel;
    if (colorTestTimeleft > 0) {
        if (colorTestTimeleft > 5) {
            speedtestScoreUpdate((colorTestLevel - 1).toFixed(0), (colorTestHighscore - 1).toFixed(0), colorTestTimeleft, colorTestPenalty.toFixed(0));
        } else {
            speedtestScoreUpdate((colorTestLevel - 1).toFixed(0), (colorTestHighscore - 1).toFixed(0), colorTestTimeleft, colorTestPenalty.toFixed(0));
        }
    } else {
        speedtestPublishResult((colorTestLevel - 1).toFixed(0), (colorTestHighscore - 1).toFixed(0), colorTestTimeleft, colorTestPenalty.toFixed(0));
    }
};
function colorTestRefreshTime() {
    colorTestUpdateScore(false);
    if (colorTestTimeleft > 0) {
        setTimeout(colorTestRefreshTime, 100);
    }
}
function colorTestRenderLevel(level) {
    if (colorTestTimeleft == 0) return;
    colorTestUpdateScore(true);

    var container = $('#' + colorTestContainerId);
    container.empty();
    var grid = colorTestLevelGrid(level);
    var correct;
    do {
        correct = Math.floor(Math.random() * grid * grid);
    } while (colorTestCorrect == correct);
    colorTestCorrect = correct;
    for (var i = 0; i < grid * grid; i++) {
        if (i == correct) {
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
        'float': 'left',
        'borderRadius': '10px',
        'cursor': 'pointer',
        'border': '2px solid #fff',
        'boxSizing': 'border-box',
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
        colorTestTimePenalty = 0;

    });
    $('#' + colorTestContainerId + ' DIV.missclick').click(function () {
        if (colorTestTimeleft < 15) colorTestPenalty++;
        if (colorTestTimeleft < 15) colorTestTimePenalty++;

    });
};