/*! dygraphs v1.2 dygraphs.com | dygraphs.com/license */
"use strict";
var DygraphLayout = function (a) {
    this.dygraph_ = a;
    this.datasets = [];
    this.annotations = [];
    this.yAxes_ = null;
    this.xTicks_ = null;
    this.yTicks_ = null;
};
DygraphLayout.prototype.attr_ = function (a) { return this.dygraph_.attr_(a); };
DygraphLayout.prototype.addDataset = function (a, b) { this.datasets[a] = b; };
DygraphLayout.prototype.getPlotArea = function () { return this.computePlotArea_(); };
DygraphLayout.prototype.computePlotArea_ = function () {
    var a = { x: 0, y: 0 };
    if (this.attr_("drawYAxis")) {
        a.x = this.attr_("yAxisLabelWidth") + 2 * this.attr_("axisTickSize");
    }
    a.w = this.dygraph_.width_ - a.x - this.attr_("rightGap");
    a.h = this.dygraph_.height_;
    if (this.attr_("drawXAxis")) {
        if (this.attr_("xAxisHeight")) {
            a.h -= this.attr_("xAxisHeight");
        } else {
            a.h -= this.attr_("axisLabelFontSize") + 2 * this.attr_("axisTickSize");
        }
    }
    if (this.dygraph_.numAxes() == 2) {
        a.w -= (this.attr_("yAxisLabelWidth") + 2 * this.attr_("axisTickSize"));
    } else {
        if (this.dygraph_.numAxes() > 2) {
            this.dygraph_.error("Only two y-axes are supported at this time. (Trying to use " + this.dygraph_.numAxes() + ")");
        }
    }
    if (this.attr_("title")) {
        a.h -= this.attr_("titleHeight");
        a.y += this.attr_("titleHeight");
    }
    if (this.attr_("xlabel")) {
        a.h -= this.attr_("xLabelHeight");
    }
    if (this.attr_("ylabel")) {
    }
    if (this.attr_("y2label")) {
    }
    if (this.attr_("showRangeSelector")) {
        a.h -= this.attr_("rangeSelectorHeight") + 4;
    }
    return a;
};
DygraphLayout.prototype.setAnnotations = function (d) {
    this.annotations = [];
    var e = this.attr_("xValueParser") || function (a) { return a; };
    for (var c = 0; c < d.length; c++) {
        var b = {};
        if (!d[c].xval && !d[c].x) {
            this.dygraph_.error("Annotations must have an 'x' property");
            return;
        }
        if (d[c].icon && !(d[c].hasOwnProperty("width") && d[c].hasOwnProperty("height"))) {
            this.dygraph_.error("Must set width and height when setting annotation.icon property");
            return;
        }
        Dygraph.update(b, d[c]);
        if (!b.xval) {
            b.xval = e(b.x);
        }
        this.annotations.push(b);
    }
};
DygraphLayout.prototype.setXTicks = function (a) { this.xTicks_ = a; };
DygraphLayout.prototype.setYAxes = function (a) { this.yAxes_ = a; };
DygraphLayout.prototype.setDateWindow = function (a) { this.dateWindow_ = a; };
DygraphLayout.prototype.evaluate = function () {
    this._evaluateLimits();
    this._evaluateLineCharts();
    this._evaluateLineTicks();
    this._evaluateAnnotations();
};
DygraphLayout.prototype._evaluateLimits = function () {
    this.minxval = this.maxxval = null;
    if (this.dateWindow_) {
        this.minxval = this.dateWindow_[0];
        this.maxxval = this.dateWindow_[1];
    } else {
        for (var c in this.datasets) {
            if (!this.datasets.hasOwnProperty(c)) {
                continue;
            }
            var e = this.datasets[c];
            if (e.length > 1) {
                var b = e[0][0];
                if (!this.minxval || b < this.minxval) {
                    this.minxval = b;
                }
                var a = e[e.length - 1][0];
                if (!this.maxxval || a > this.maxxval) {
                    this.maxxval = a;
                }
            }
        }
    }
    this.xrange = this.maxxval - this.minxval;
    this.xscale = (this.xrange !== 0 ? 1 / this.xrange : 1);
    for (var d = 0; d < this.yAxes_.length; d++) {
        var f = this.yAxes_[d];
        f.minyval = f.computedValueRange[0];
        f.maxyval = f.computedValueRange[1];
        f.yrange = f.maxyval - f.minyval;
        f.yscale = (f.yrange !== 0 ? 1 / f.yrange : 1);
        if (f.g.attr_("logscale")) {
            f.ylogrange = Dygraph.log10(f.maxyval) - Dygraph.log10(f.minyval);
            f.ylogscale = (f.ylogrange !== 0 ? 1 / f.ylogrange : 1);
            if (!isFinite(f.ylogrange) || isNaN(f.ylogrange)) {
                f.g.error("axis " + d + " of graph at " + f.g + " can't be displayed in log scale for range [" + f.minyval + " - " + f.maxyval + "]");
            }
        }
    }
};
DygraphLayout._calcYNormal = function (a, b) {
    if (a.logscale) {
        return 1 - ((Dygraph.log10(b) - Dygraph.log10(a.minyval)) * a.ylogscale);
    } else {
        return 1 - ((b - a.minyval) * a.yscale);
    }
};
DygraphLayout.prototype._evaluateLineCharts = function () {
    this.points = [];
    this.setPointsLengths = [];
    for (var f in this.datasets) {
        if (!this.datasets.hasOwnProperty(f)) {
            continue;
        }
        var b = this.datasets[f];
        var a = this.dygraph_.axisPropertiesForSeries(f);
        var g = 0;
        for (var d = 0; d < b.length; d++) {
            var l = b[d];
            var c = parseFloat(l[0]);
            var h = parseFloat(l[1]);
            var k = (c - this.minxval) * this.xscale;
            var e = DygraphLayout._calcYNormal(a, h);
            var i = { x: k, y: e, xval: c, yval: h, name: f };
            this.points.push(i);
            g += 1;
        }
        this.setPointsLengths.push(g);
    }
};
DygraphLayout.prototype._evaluateLineTicks = function () {
    var d, c, b, f;
    this.xticks = [];
    for (d = 0; d < this.xTicks_.length; d++) {
        c = this.xTicks_[d];
        b = c.label;
        f = this.xscale * (c.v - this.minxval);
        if ((f >= 0) && (f <= 1)) {
            this.xticks.push([f, b]);
        }
    }
    this.yticks = [];
    for (d = 0; d < this.yAxes_.length; d++) {
        var e = this.yAxes_[d];
        for (var a = 0; a < e.ticks.length; a++) {
            c = e.ticks[a];
            b = c.label;
            f = this.dygraph_.toPercentYCoord(c.v, d);
            if ((f >= 0) && (f <= 1)) {
                this.yticks.push([d, f, b]);
            }
        }
    }
};
DygraphLayout.prototype.evaluateWithError = function () {
    this.evaluate();
    if (!(this.attr_("errorBars") || this.attr_("customBars"))) {
        return;
    }
    var g = 0;
    for (var k in this.datasets) {
        if (!this.datasets.hasOwnProperty(k)) {
            continue;
        }
        var f = 0;
        var e = this.datasets[k];
        var d = this.dygraph_.axisPropertiesForSeries(k);
        for (f = 0; f < e.length; f++, g++) {
            var n = e[f];
            var b = parseFloat(n[0]);
            var l = parseFloat(n[1]);
            if (b == this.points[g].xval && l == this.points[g].yval) {
                var h = parseFloat(n[2]);
                var c = parseFloat(n[3]);
                var m = l - h;
                var a = l + c;
                this.points[g].y_top = DygraphLayout._calcYNormal(d, m);
                this.points[g].y_bottom = DygraphLayout._calcYNormal(d, a);
            }
        }
    }
};
DygraphLayout.prototype._evaluateAnnotations = function () {
    var d;
    var f = {};
    for (d = 0; d < this.annotations.length; d++) {
        var b = this.annotations[d];
        f[b.xval + "," + b.series] = b;
    }
    this.annotated_points = [];
    if (!this.annotations || !this.annotations.length) {
        return;
    }
    for (d = 0; d < this.points.length; d++) {
        var e = this.points[d];
        var c = e.xval + "," + e.name;
        if (c in f) {
            e.annotation = f[c];
            this.annotated_points.push(e);
        }
    }
};
DygraphLayout.prototype.removeAllDatasets = function () {
    delete this.datasets;
    this.datasets = [];
};
DygraphLayout.prototype.unstackPointAtIndex = function (b) {
    var a = this.points[b];
    var d = {};
    for (var e in a) {
        d[e] = a[e];
    }
    if (!this.attr_("stackedGraph")) {
        return d;
    }
    for (var c = b + 1; c < this.points.length; c++) {
        if (this.points[c].xval == a.xval) {
            d.yval -= this.points[c].yval;
            break;
        }
    }
    return d;
};
"use strict";
var DygraphCanvasRenderer = function (d, c, b, e) {
    this.dygraph_ = d;
    this.layout = e;
    this.element = c;
    this.elementContext = b;
    this.container = this.element.parentNode;
    this.height = this.element.height;
    this.width = this.element.width;
    if (!this.isIE && !(DygraphCanvasRenderer.isSupported(this.element))) {
        throw "Canvas is not supported.";
    }
    this.xlabels = [];
    this.ylabels = [];
    this.annotations = [];
    this.chartLabels = {};
    this.area = e.getPlotArea();
    this.container.style.position = "relative";
    this.container.style.width = this.width + "px";
    if (this.dygraph_.isUsingExcanvas_) {
        this._createIEClipArea();
    } else {
        if (!Dygraph.isAndroid()) {
            var a = this.dygraph_.canvas_ctx_;
            a.beginPath();
            a.rect(this.area.x, this.area.y, this.area.w, this.area.h);
            a.clip();
            a = this.dygraph_.hidden_ctx_;
            a.beginPath();
            a.rect(this.area.x, this.area.y, this.area.w, this.area.h);
            a.clip();
        }
    }
};
DygraphCanvasRenderer.prototype.attr_ = function (a) { return this.dygraph_.attr_(a); };
DygraphCanvasRenderer.prototype.clear = function () {
    var c;
    if (this.isIE) {
        try {
            if (this.clearDelay) {
                this.clearDelay.cancel();
                this.clearDelay = null;
            }
            c = this.elementContext;
        } catch (f) {
            return;
        }
    }
    c = this.elementContext;
    c.clearRect(0, 0, this.width, this.height);

    function a(g) {
        for (var e = 0; e < g.length; e++) {
            var h = g[e];
            if (h.parentNode) {
                h.parentNode.removeChild(h);
            }
        }
    }

    a(this.xlabels);
    a(this.ylabels);
    a(this.annotations);
    for (var b in this.chartLabels) {
        if (!this.chartLabels.hasOwnProperty(b)) {
            continue;
        }
        var d = this.chartLabels[b];
        if (d.parentNode) {
            d.parentNode.removeChild(d);
        }
    }
    this.xlabels = [];
    this.ylabels = [];
    this.annotations = [];
    this.chartLabels = {};
};
DygraphCanvasRenderer.isSupported = function (f) {
    var b = null;
    try {
        if (typeof (f) == "undefined" || f === null) {
            b = document.createElement("canvas");
        } else {
            b = f;
        }
        b.getContext("2d");
    } catch (c) {
        var d = navigator.appVersion.match(/MSIE (\d\.\d)/);
        var a = (navigator.userAgent.toLowerCase().indexOf("opera") != -1);
        if ((!d) || (d[1] < 6) || (a)) {
            return false;
        }
        return true;
    }
    return true;
};
DygraphCanvasRenderer.prototype.setColors = function (a) { this.colorScheme_ = a; };
DygraphCanvasRenderer.prototype.render = function () {
    var b = this.elementContext;

    function c(h) {
        return Math.round(h) + 0.5;
    }

    function g(h) {
        return Math.round(h) - 0.5;
    }

    if (this.attr_("underlayCallback")) {
        this.attr_("underlayCallback")(b, this.area, this.dygraph_, this.dygraph_);
    }
    var a, f, d, e;
    if (this.attr_("drawYGrid")) {
        e = this.layout.yticks;
        b.save();
        b.strokeStyle = this.attr_("gridLineColor");
        b.lineWidth = this.attr_("gridLineWidth");
        for (d = 0; d < e.length; d++) {
            if (e[d][0] !== 0) {
                continue;
            }
            a = c(this.area.x);
            f = g(this.area.y + e[d][1] * this.area.h);
            b.beginPath();
            b.moveTo(a, f);
            b.lineTo(a + this.area.w, f);
            b.closePath();
            b.stroke();
        }
    }
    if (this.attr_("drawXGrid")) {
        e = this.layout.xticks;
        b.save();
        b.strokeStyle = this.attr_("gridLineColor");
        b.lineWidth = this.attr_("gridLineWidth");
        for (d = 0; d < e.length; d++) {
            a = c(this.area.x + e[d][0] * this.area.w);
            f = g(this.area.y + this.area.h);
            b.beginPath();
            b.moveTo(a, f);
            b.lineTo(a, this.area.y);
            b.closePath();
            b.stroke();
        }
    }
    this._renderLineChart();
    this._renderAxis();
    this._renderChartLabels();
    this._renderAnnotations();
};
DygraphCanvasRenderer.prototype._createIEClipArea = function () {
    var g = "dygraph-clip-div";
    var f = this.dygraph_.graphDiv;
    for (var e = f.childNodes.length - 1; e >= 0; e--) {
        if (f.childNodes[e].className == g) {
            f.removeChild(f.childNodes[e]);
        }
    }
    var c = document.bgColor;
    var d = this.dygraph_.graphDiv;
    while (d != document) {
        var a = d.currentStyle.backgroundColor;
        if (a && a != "transparent") {
            c = a;
            break;
        }
        d = d.parentNode;
    }

    function b(j) {
        if (j.w === 0 || j.h === 0) {
            return;
        }
        var i = document.createElement("div");
        i.className = g;
        i.style.backgroundColor = c;
        i.style.position = "absolute";
        i.style.left = j.x + "px";
        i.style.top = j.y + "px";
        i.style.width = j.w + "px";
        i.style.height = j.h + "px";
        f.appendChild(i);
    }

    var h = this.area;
    b({ x: 0, y: 0, w: h.x, h: this.height });
    b({ x: h.x, y: 0, w: this.width - h.x, h: h.y });
    b({ x: h.x + h.w, y: 0, w: this.width - h.x - h.w, h: this.height });
    b({ x: h.x, y: h.y + h.h, w: this.width - h.x, h: this.height - h.h - h.y });
};
DygraphCanvasRenderer.prototype._renderAxis = function () {
    if (!this.attr_("drawXAxis") && !this.attr_("drawYAxis")) {
        return;
    }

    function q(i) {
        return Math.round(i) + 0.5;
    }

    function p(i) {
        return Math.round(i) - 0.5;
    }

    var d = this.elementContext;
    var l, n, m, s, r;
    var a = { position: "absolute", fontSize: this.attr_("axisLabelFontSize") + "px", zIndex: 10, color: this.attr_("axisLabelColor"), width: this.attr_("axisLabelWidth") + "px", lineHeight: "normal", overflow: "hidden" };
    var g = function (i, v, w) {
        var x = document.createElement("div");
        for (var u in a) {
            if (a.hasOwnProperty(u)) {
                x.style[u] = a[u];
            }
        }
        var t = document.createElement("div");
        t.className = "dygraph-axis-label dygraph-axis-label-" + v + (w ? " dygraph-axis-label-" + w : "");
        t.innerHTML = i;
        x.appendChild(t);
        return x;
    };
    d.save();
    d.strokeStyle = this.attr_("axisLineColor");
    d.lineWidth = this.attr_("axisLineWidth");
    if (this.attr_("drawYAxis")) {
        if (this.layout.yticks && this.layout.yticks.length > 0) {
            var b = this.dygraph_.numAxes();
            for (r = 0; r < this.layout.yticks.length; r++) {
                s = this.layout.yticks[r];
                if (typeof (s) == "function") {
                    return;
                }
                n = this.area.x;
                var j = 1;
                var c = "y1";
                if (s[0] == 1) {
                    n = this.area.x + this.area.w;
                    j = -1;
                    c = "y2";
                }
                m = this.area.y + s[1] * this.area.h;
                l = g(s[2], "y", b == 2 ? c : null);
                var o = (m - this.attr_("axisLabelFontSize") / 2);
                if (o < 0) {
                    o = 0;
                }
                if (o + this.attr_("axisLabelFontSize") + 3 > this.height) {
                    l.style.bottom = "0px";
                } else {
                    l.style.top = o + "px";
                }
                if (s[0] === 0) {
                    l.style.left = (this.area.x - this.attr_("yAxisLabelWidth") - this.attr_("axisTickSize")) + "px";
                    l.style.textAlign = "right";
                } else {
                    if (s[0] == 1) {
                        l.style.left = (this.area.x + this.area.w + this.attr_("axisTickSize")) + "px";
                        l.style.textAlign = "left";
                    }
                }
                l.style.width = this.attr_("yAxisLabelWidth") + "px";
                this.container.appendChild(l);
                this.ylabels.push(l);
            }
            var h = this.ylabels[0];
            var e = this.attr_("axisLabelFontSize");
            var k = parseInt(h.style.top, 10) + e;
            if (k > this.height - e) {
                h.style.top = (parseInt(h.style.top, 10) - e / 2) + "px";
            }
        }
        d.beginPath();
        d.moveTo(q(this.area.x), p(this.area.y));
        d.lineTo(q(this.area.x), p(this.area.y + this.area.h));
        d.closePath();
        d.stroke();
        if (this.dygraph_.numAxes() == 2) {
            d.beginPath();
            d.moveTo(p(this.area.x + this.area.w), p(this.area.y));
            d.lineTo(p(this.area.x + this.area.w), p(this.area.y + this.area.h));
            d.closePath();
            d.stroke();
        }
    }
    if (this.attr_("drawXAxis")) {
        if (this.layout.xticks) {
            for (r = 0; r < this.layout.xticks.length; r++) {
                s = this.layout.xticks[r];
                n = this.area.x + s[0] * this.area.w;
                m = this.area.y + this.area.h;
                l = g(s[1], "x");
                l.style.textAlign = "center";
                l.style.top = (m + this.attr_("axisTickSize")) + "px";
                var f = (n - this.attr_("axisLabelWidth") / 2);
                if (f + this.attr_("axisLabelWidth") > this.width) {
                    f = this.width - this.attr_("xAxisLabelWidth");
                    l.style.textAlign = "right";
                }
                if (f < 0) {
                    f = 0;
                    l.style.textAlign = "left";
                }
                l.style.left = f + "px";
                l.style.width = this.attr_("xAxisLabelWidth") + "px";
                this.container.appendChild(l);
                this.xlabels.push(l);
            }
        }
        d.beginPath();
        d.moveTo(q(this.area.x), p(this.area.y + this.area.h));
        d.lineTo(q(this.area.x + this.area.w), p(this.area.y + this.area.h));
        d.closePath();
        d.stroke();
    }
    d.restore();
};
DygraphCanvasRenderer.prototype._renderChartLabels = function () {
    var d, a;
    if (this.attr_("title")) {
        d = document.createElement("div");
        d.style.position = "absolute";
        d.style.top = "0px";
        d.style.left = this.area.x + "px";
        d.style.width = this.area.w + "px";
        d.style.height = this.attr_("titleHeight") + "px";
        d.style.textAlign = "center";
        d.style.fontSize = (this.attr_("titleHeight") - 8) + "px";
        d.style.fontWeight = "bold";
        a = document.createElement("div");
        a.className = "dygraph-label dygraph-title";
        a.innerHTML = this.attr_("title");
        d.appendChild(a);
        this.container.appendChild(d);
        this.chartLabels.title = d;
    }
    if (this.attr_("xlabel")) {
        d = document.createElement("div");
        d.style.position = "absolute";
        d.style.bottom = 0;
        d.style.left = this.area.x + "px";
        d.style.width = this.area.w + "px";
        d.style.height = this.attr_("xLabelHeight") + "px";
        d.style.textAlign = "center";
        d.style.fontSize = (this.attr_("xLabelHeight") - 2) + "px";
        a = document.createElement("div");
        a.className = "dygraph-label dygraph-xlabel";
        a.innerHTML = this.attr_("xlabel");
        d.appendChild(a);
        this.container.appendChild(d);
        this.chartLabels.xlabel = d;
    }
    var c = this;

    function b(h, g, f) {
        var i = { left: 0, top: c.area.y, width: c.attr_("yLabelWidth"), height: c.area.h };
        d = document.createElement("div");
        d.style.position = "absolute";
        if (h == 1) {
            d.style.left = i.left;
        } else {
            d.style.right = i.left;
        }
        d.style.top = i.top + "px";
        d.style.width = i.width + "px";
        d.style.height = i.height + "px";
        d.style.fontSize = (c.attr_("yLabelWidth") - 2) + "px";
        var e = document.createElement("div");
        e.style.position = "absolute";
        e.style.width = i.height + "px";
        e.style.height = i.width + "px";
        e.style.top = (i.height / 2 - i.width / 2) + "px";
        e.style.left = (i.width / 2 - i.height / 2) + "px";
        e.style.textAlign = "center";
        var j = "rotate(" + (h == 1 ? "-" : "") + "90deg)";
        e.style.transform = j;
        e.style.WebkitTransform = j;
        e.style.MozTransform = j;
        e.style.OTransform = j;
        e.style.msTransform = j;
        if (typeof (document.documentMode) !== "undefined" && document.documentMode < 9) {
            e.style.filter = "progid:DXImageTransform.Microsoft.BasicImage(rotation=" + (h == 1 ? "3" : "1") + ")";
            e.style.left = "0px";
            e.style.top = "0px";
        }
        a = document.createElement("div");
        a.className = g;
        a.innerHTML = f;
        e.appendChild(a);
        d.appendChild(e);
        return d;
    }

    var d;
    if (this.attr_("ylabel")) {
        d = b(1, "dygraph-label dygraph-ylabel", this.attr_("ylabel"));
        this.container.appendChild(d);
        this.chartLabels.ylabel = d;
    }
    if (this.attr_("y2label") && this.dygraph_.numAxes() == 2) {
        d = b(2, "dygraph-label dygraph-y2label", this.attr_("y2label"));
        this.container.appendChild(d);
        this.chartLabels.y2label = d;
    }
};
DygraphCanvasRenderer.prototype._renderAnnotations = function () {
    var h = { position: "absolute", fontSize: this.attr_("axisLabelFontSize") + "px", zIndex: 10, overflow: "hidden" };
    var j = function (i, q, r, a) {
        return function (s) {
            var p = r.annotation;
            if (p.hasOwnProperty(i)) {
                p[i](p, r, a.dygraph_, s);
            } else {
                if (a.dygraph_.attr_(q)) {
                    a.dygraph_.attr_(q)(p, r, a.dygraph_, s);
                }
            }
        };
    };
    var m = this.layout.annotated_points;
    for (var g = 0; g < m.length; g++) {
        var e = m[g];
        if (e.canvasx < this.area.x || e.canvasx > this.area.x + this.area.w) {
            continue;
        }
        var k = e.annotation;
        var l = 6;
        if (k.hasOwnProperty("tickHeight")) {
            l = k.tickHeight;
        }
        var c = document.createElement("div");
        for (var b in h) {
            if (h.hasOwnProperty(b)) {
                c.style[b] = h[b];
            }
        }
        if (!k.hasOwnProperty("icon")) {
            c.className = "dygraphDefaultAnnotation";
        }
        if (k.hasOwnProperty("cssClass")) {
            c.className += " " + k.cssClass;
        }
        var d = k.hasOwnProperty("width") ? k.width : 16;
        var n = k.hasOwnProperty("height") ? k.height : 16;
        if (k.hasOwnProperty("icon")) {
            var f = document.createElement("img");
            f.src = k.icon;
            f.width = d;
            f.height = n;
            c.appendChild(f);
        } else {
            if (e.annotation.hasOwnProperty("shortText")) {
                c.appendChild(document.createTextNode(e.annotation.shortText));
            }
        }
        c.style.left = (e.canvasx - d / 2) + "px";
        if (k.attachAtBottom) {
            c.style.top = (this.area.h - n - l) + "px";
        } else {
            c.style.top = (e.canvasy - n - l) + "px";
        }
        c.style.width = d + "px";
        c.style.height = n + "px";
        c.title = e.annotation.text;
        c.style.color = this.colors[e.name];
        c.style.borderColor = this.colors[e.name];
        k.div = c;
        Dygraph.addEvent(c, "click", j("clickHandler", "annotationClickHandler", e, this));
        Dygraph.addEvent(c, "mouseover", j("mouseOverHandler", "annotationMouseOverHandler", e, this));
        Dygraph.addEvent(c, "mouseout", j("mouseOutHandler", "annotationMouseOutHandler", e, this));
        Dygraph.addEvent(c, "dblclick", j("dblClickHandler", "annotationDblClickHandler", e, this));
        this.container.appendChild(c);
        this.annotations.push(c);
        var o = this.elementContext;
        o.strokeStyle = this.colors[e.name];
        o.beginPath();
        if (!k.attachAtBottom) {
            o.moveTo(e.canvasx, e.canvasy);
            o.lineTo(e.canvasx, e.canvasy - 2 - l);
        } else {
            o.moveTo(e.canvasx, this.area.h);
            o.lineTo(e.canvasx, this.area.h - 2 - l);
        }
        o.closePath();
        o.stroke();
    }
};
DygraphCanvasRenderer.prototype._renderLineChart = function () {
    var G = function (i) { return (i === null || isNaN(i)); };
    var M = this.elementContext;
    var m = this.attr_("fillAlpha");
    var C = this.attr_("errorBars") || this.attr_("customBars");
    var k = this.attr_("fillGraph");
    var w = this.attr_("stackedGraph");
    var v = this.attr_("stepPlot");
    var A = this.layout.points;
    var z = A.length;
    var u, K, I, b, a, f, t, F, n, D, h, d, r;
    var E = [];
    for (var l in this.layout.datasets) {
        if (this.layout.datasets.hasOwnProperty(l)) {
            E.push(l);
        }
    }
    var x = E.length;
    this.colors = {};
    for (K = 0; K < x; K++) {
        this.colors[E[K]] = this.colorScheme_[K % this.colorScheme_.length];
    }
    for (K = z; K--; ) {
        u = A[K];
        u.canvasx = this.area.w * u.x + this.area.x;
        u.canvasy = this.area.h * u.y + this.area.y;
    }
    var q = M;
    if (C) {
        if (k) {
            this.dygraph_.warn("Can't use fillGraph option with error bars");
        }
        for (K = 0; K < x; K++) {
            F = E[K];
            r = this.dygraph_.axisPropertiesForSeries(F);
            t = this.colors[F];
            q.save();
            b = NaN;
            a = NaN;
            f = [-1, -1];
            d = r.yscale;
            h = new RGBColor(t);
            D = "rgba(" + h.r + "," + h.g + "," + h.b + "," + m + ")";
            q.fillStyle = D;
            q.beginPath();
            for (I = 0; I < z; I++) {
                u = A[I];
                if (u.name == F) {
                    if (!Dygraph.isOK(u.y)) {
                        b = NaN;
                        continue;
                    }
                    if (v) {
                        n = [u.y_bottom, u.y_top];
                        a = u.y;
                    } else {
                        n = [u.y_bottom, u.y_top];
                    }
                    n[0] = this.area.h * n[0] + this.area.y;
                    n[1] = this.area.h * n[1] + this.area.y;
                    if (!isNaN(b)) {
                        if (v) {
                            q.moveTo(b, n[0]);
                        } else {
                            q.moveTo(b, f[0]);
                        }
                        q.lineTo(u.canvasx, n[0]);
                        q.lineTo(u.canvasx, n[1]);
                        if (v) {
                            q.lineTo(b, n[1]);
                        } else {
                            q.lineTo(b, f[1]);
                        }
                        q.closePath();
                    }
                    f = n;
                    b = u.canvasx;
                }
            }
            q.fill();
        }
    } else {
        if (k) {
            var H = [];
            for (K = x - 1; K >= 0; K--) {
                F = E[K];
                t = this.colors[F];
                r = this.dygraph_.axisPropertiesForSeries(F);
                var e = 1 + r.minyval * r.yscale;
                if (e < 0) {
                    e = 0;
                } else {
                    if (e > 1) {
                        e = 1;
                    }
                }
                e = this.area.h * e + this.area.y;
                q.save();
                b = NaN;
                f = [-1, -1];
                d = r.yscale;
                h = new RGBColor(t);
                D = "rgba(" + h.r + "," + h.g + "," + h.b + "," + m + ")";
                q.fillStyle = D;
                q.beginPath();
                for (I = 0; I < z; I++) {
                    u = A[I];
                    if (u.name == F) {
                        if (!Dygraph.isOK(u.y)) {
                            b = NaN;
                            continue;
                        }
                        if (w) {
                            var g = H[u.canvasx];
                            if (g === undefined) {
                                g = e;
                            }
                            H[u.canvasx] = u.canvasy;
                            n = [u.canvasy, g];
                        } else {
                            n = [u.canvasy, e];
                        }
                        if (!isNaN(b)) {
                            q.moveTo(b, f[0]);
                            if (v) {
                                q.lineTo(u.canvasx, f[0]);
                            } else {
                                q.lineTo(u.canvasx, n[0]);
                            }
                            q.lineTo(u.canvasx, n[1]);
                            q.lineTo(b, f[1]);
                            q.closePath();
                        }
                        f = n;
                        b = u.canvasx;
                    }
                }
                q.fill();
            }
        }
    }
    var L = 0;
    var o = 0;
    var c = 0;
    for (K = 0; K < x; K += 1) {
        c = this.layout.setPointsLengths[K];
        o += c;
        F = E[K];
        t = this.colors[F];
        var y = this.dygraph_.attr_("strokeWidth", F);
        M.save();
        var B = this.dygraph_.attr_("pointSize", F);
        b = null;
        a = null;
        var p = this.dygraph_.attr_("drawPoints", F);
        var s = this.dygraph_.attr_("strokePattern", F);
        if (!Dygraph.isArrayLike(s)) {
            s = null;
        }
        for (I = L; I < o; I++) {
            u = A[I];
            if (G(u.canvasy)) {
                if (v && b !== null) {
                    q.beginPath();
                    q.strokeStyle = t;
                    q.lineWidth = this.attr_("strokeWidth");
                    this._dashedLine(q, b, a, u.canvasx, a, s);
                    q.stroke();
                }
                b = a = null;
            } else {
                var J = (!b && (I == A.length - 1 || G(A[I + 1].canvasy)));
                if (b === null) {
                    b = u.canvasx;
                    a = u.canvasy;
                } else {
                    if (Math.round(b) == Math.round(u.canvasx) && Math.round(a) == Math.round(u.canvasy)) {
                        continue;
                    }
                    if (y) {
                        q.beginPath();
                        q.strokeStyle = t;
                        q.lineWidth = y;
                        if (v) {
                            this._dashedLine(q, b, a, u.canvasx, a, s);
                        }
                        this._dashedLine(q, b, a, u.canvasx, u.canvasy, s);
                        b = u.canvasx;
                        a = u.canvasy;
                        q.stroke();
                    }
                }
                if (p || J) {
                    q.beginPath();
                    q.fillStyle = t;
                    q.arc(u.canvasx, u.canvasy, B, 0, 2 * Math.PI, false);
                    q.fill();
                }
            }
        }
        L = o;
    }
    M.restore();
};
DygraphCanvasRenderer.prototype._dashedLine = function (j, i, g, a, h, f) {
    var l, k, e, b, c, d;
    if (!f || f.length <= 1) {
        j.moveTo(i, g);
        j.lineTo(a, h);
        return;
    }
    if (!Dygraph.compareArrays(f, this._dashedLineToHistoryPattern)) {
        this._dashedLineToHistoryPattern = f;
        this._dashedLineToHistory = [0, 0];
    }
    j.save();
    l = (a - i);
    k = (h - g);
    e = Math.sqrt(l * l + k * k);
    b = Math.atan2(k, l);
    j.translate(i, g);
    j.moveTo(0, 0);
    j.rotate(b);
    c = this._dashedLineToHistory[0];
    i = 0;
    while (e > i) {
        d = f[c];
        if (this._dashedLineToHistory[1]) {
            i += this._dashedLineToHistory[1];
        } else {
            i += d;
        }
        if (i > e) {
            this._dashedLineToHistory = [c, i - e];
            i = e;
        } else {
            this._dashedLineToHistory = [(c + 1) % f.length, 0];
        }
        if (c % 2 === 0) {
            j.lineTo(i, 0);
        } else {
            j.moveTo(i, 0);
        }
        c = (c + 1) % f.length;
    }
    j.restore();
};
"use strict";
var Dygraph = function (c, b, a) {
    if (arguments.length > 0) {
        if (arguments.length == 4) {
            this.warn("Using deprecated four-argument dygraph constructor");
            this.__old_init__(c, b, arguments[2], arguments[3]);
        } else {
            this.__init__(c, b, a);
        }
    }
};
Dygraph.NAME = "Dygraph";
Dygraph.VERSION = "1.2";
Dygraph.__repr__ = function () { return "[" + this.NAME + " " + this.VERSION + "]"; };
Dygraph.toString = function () { return this.__repr__(); };
Dygraph.DEFAULT_ROLL_PERIOD = 1;
Dygraph.DEFAULT_WIDTH = 480;
Dygraph.DEFAULT_HEIGHT = 320;
Dygraph.ANIMATION_STEPS = 10;
Dygraph.ANIMATION_DURATION = 200;
Dygraph.numberValueFormatter = function (a, e, h, d) {
    var b = e("sigFigs");
    if (b !== null) {
        return Dygraph.floatFormat(a, b);
    }
    var f = e("digitsAfterDecimal");
    var c = e("maxNumberWidth");
    if (a !== 0 && (Math.abs(a) >= Math.pow(10, c) || Math.abs(a) < Math.pow(10, -f))) {
        return a.toExponential(f);
    } else {
        return "" + Dygraph.round_(a, f);
    }
};
Dygraph.numberAxisLabelFormatter = function (a, d, c, b) { return Dygraph.numberValueFormatter(a, c, b); };
Dygraph.dateString_ = function (e) {
    var i = Dygraph.zeropad;
    var h = new Date(e);
    var f = "" + h.getFullYear();
    var g = i(h.getMonth() + 1);
    var a = i(h.getDate());
    var c = "";
    var b = h.getHours() * 3600 + h.getMinutes() * 60 + h.getSeconds();
    if (b) {
        c = " " + Dygraph.hmsString_(e);
    }
    return f + "/" + g + "/" + a + c;
};
Dygraph.dateAxisFormatter = function (b, c) {
    if (c >= Dygraph.DECADAL) {
        return b.strftime("%Y");
    } else {
        if (c >= Dygraph.MONTHLY) {
            return b.strftime("%b %y");
        } else {
            var a = b.getHours() * 3600 + b.getMinutes() * 60 + b.getSeconds() + b.getMilliseconds();
            if (a === 0 || c >= Dygraph.DAILY) {
                return new Date(b.getTime() + 3600 * 1000).strftime("%d%b");
            } else {
                return Dygraph.hmsString_(b.getTime());
            }
        }
    }
};
Dygraph.DEFAULT_ATTRS = { highlightCircleSize: 3, labelsDivWidth: 250, labelsDivStyles: {}, labelsSeparateLines: false, labelsShowZeroValues: true, labelsKMB: false, labelsKMG2: false, showLabelsOnHighlight: true, digitsAfterDecimal: 2, maxNumberWidth: 6, sigFigs: null, strokeWidth: 1, axisTickSize: 3, axisLabelFontSize: 14, xAxisLabelWidth: 50, yAxisLabelWidth: 50, rightGap: 5, showRoller: false, xValueParser: Dygraph.dateParser, delimiter: ",", sigma: 2, errorBars: false, fractions: false, wilsonInterval: true, customBars: false, fillGraph: false, fillAlpha: 0.15, connectSeparatedPoints: false, stackedGraph: false, hideOverlayOnMouseOut: true, legend: "onmouseover", stepPlot: false, avoidMinZero: false, titleHeight: 28, xLabelHeight: 18, yLabelWidth: 18, drawXAxis: true, drawYAxis: true, axisLineColor: "black", axisLineWidth: 0.3, gridLineWidth: 0.3, axisLabelColor: "black", axisLabelFont: "Arial", axisLabelWidth: 50, drawYGrid: true, drawXGrid: true, gridLineColor: "rgb(128,128,128)", interactionModel: null, animatedZooms: false, showRangeSelector: false, rangeSelectorHeight: 40, rangeSelectorPlotStrokeColor: "#808FAB", rangeSelectorPlotFillColor: "#A7B1C4", axes: { x: { pixelsPerLabel: 60, axisLabelFormatter: Dygraph.dateAxisFormatter, valueFormatter: Dygraph.dateString_, ticker: null }, y: { pixelsPerLabel: 30, valueFormatter: Dygraph.numberValueFormatter, axisLabelFormatter: Dygraph.numberAxisLabelFormatter, ticker: null }, y2: { pixelsPerLabel: 30, valueFormatter: Dygraph.numberValueFormatter, axisLabelFormatter: Dygraph.numberAxisLabelFormatter, ticker: null}} };
Dygraph.HORIZONTAL = 1;
Dygraph.VERTICAL = 2;
Dygraph.addedAnnotationCSS = false;
Dygraph.prototype.__old_init__ = function (f, d, e, b) {
    if (e !== null) {
        var a = ["Date"];
        for (var c = 0; c < e.length; c++) {
            a.push(e[c]);
        }
        Dygraph.update(b, { labels: a });
    }
    this.__init__(f, d, b);
};
Dygraph.prototype.__init__ = function (d, c, b) {
    if (/MSIE/.test(navigator.userAgent) && !window.opera && typeof (G_vmlCanvasManager) != "undefined" && document.readyState != "complete") {
        var a = this;
        setTimeout(function () { a.__init__(d, c, b); }, 100);
        return;
    }
    if (b === null || b === undefined) {
        b = {};
    }
    b = Dygraph.mapLegacyOptions_(b);
    if (!d) {
        Dygraph.error("Constructing dygraph with a non-existent div!");
        return;
    }
    this.isUsingExcanvas_ = typeof (G_vmlCanvasManager) != "undefined";
    this.maindiv_ = d;
    this.file_ = c;
    this.rollPeriod_ = b.rollPeriod || Dygraph.DEFAULT_ROLL_PERIOD;
    this.previousVerticalX_ = -1;
    this.fractions_ = b.fractions || false;
    this.dateWindow_ = b.dateWindow || null;
    this.is_initial_draw_ = true;
    this.annotations_ = [];
    this.zoomed_x_ = false;
    this.zoomed_y_ = false;
    d.innerHTML = "";
    if (d.style.width === "" && b.width) {
        d.style.width = b.width + "px";
    }
    if (d.style.height === "" && b.height) {
        d.style.height = b.height + "px";
    }
    if (d.style.height === "" && d.clientHeight === 0) {
        d.style.height = Dygraph.DEFAULT_HEIGHT + "px";
        if (d.style.width === "") {
            d.style.width = Dygraph.DEFAULT_WIDTH + "px";
        }
    }
    this.width_ = d.clientWidth;
    this.height_ = d.clientHeight;
    if (b.stackedGraph) {
        b.fillGraph = true;
    }
    this.user_attrs_ = {};
    Dygraph.update(this.user_attrs_, b);
    this.attrs_ = {};
    Dygraph.updateDeep(this.attrs_, Dygraph.DEFAULT_ATTRS);
    this.boundaryIds_ = [];
    this.createInterface_();
    this.start_();
};
Dygraph.prototype.isZoomed = function (a) {
    if (a == null) {
        return this.zoomed_x_ || this.zoomed_y_;
    }
    if (a === "x") {
        return this.zoomed_x_;
    }
    if (a === "y") {
        return this.zoomed_y_;
    }
    throw "axis parameter is [" + a + "] must be null, 'x' or 'y'.";
};
Dygraph.prototype.toString = function () {
    var a = this.maindiv_;
    var b = (a && a.id) ? a.id : a;
    return "[Dygraph " + b + "]";
};
Dygraph.prototype.attr_ = function (b, a) {
    if (a && typeof (this.user_attrs_[a]) != "undefined" && this.user_attrs_[a] !== null && typeof (this.user_attrs_[a][b]) != "undefined") {
        return this.user_attrs_[a][b];
    } else {
        if (typeof (this.user_attrs_[b]) != "undefined") {
            return this.user_attrs_[b];
        } else {
            if (typeof (this.attrs_[b]) != "undefined") {
                return this.attrs_[b];
            } else {
                return null;
            }
        }
    }
};
Dygraph.prototype.optionsViewForAxis_ = function (b) {
    var a = this;
    return function (c) {
        var d = a.user_attrs_.axes;
        if (d && d[b] && d[b][c]) {
            return d[b][c];
        }
        if (typeof (a.user_attrs_[c]) != "undefined") {
            return a.user_attrs_[c];
        }
        d = a.attrs_.axes;
        if (d && d[b] && d[b][c]) {
            return d[b][c];
        }
        if (b == "y" && a.axes_[0].hasOwnProperty(c)) {
            return a.axes_[0][c];
        } else {
            if (b == "y2" && a.axes_[1].hasOwnProperty(c)) {
                return a.axes_[1][c];
            }
        }
        return a.attr_(c);
    };
};
Dygraph.prototype.rollPeriod = function () { return this.rollPeriod_; };
Dygraph.prototype.xAxisRange = function () { return this.dateWindow_ ? this.dateWindow_ : this.xAxisExtremes(); };
Dygraph.prototype.xAxisExtremes = function () {
    var b = this.rawData_[0][0];
    var a = this.rawData_[this.rawData_.length - 1][0];
    return [b, a];
};
Dygraph.prototype.yAxisRange = function (a) {
    if (typeof (a) == "undefined") {
        a = 0;
    }
    if (a < 0 || a >= this.axes_.length) {
        return null;
    }
    var b = this.axes_[a];
    return [b.computedValueRange[0], b.computedValueRange[1]];
};
Dygraph.prototype.yAxisRanges = function () {
    var a = [];
    for (var b = 0; b < this.axes_.length; b++) {
        a.push(this.yAxisRange(b));
    }
    return a;
};
Dygraph.prototype.toDomCoords = function (a, c, b) { return [this.toDomXCoord(a), this.toDomYCoord(c, b)]; };
Dygraph.prototype.toDomXCoord = function (b) {
    if (b === null) {
        return null;
    }
    var c = this.plotter_.area;
    var a = this.xAxisRange();
    return c.x + (b - a[0]) / (a[1] - a[0]) * c.w;
};
Dygraph.prototype.toDomYCoord = function (d, a) {
    var c = this.toPercentYCoord(d, a);
    if (c === null) {
        return null;
    }
    var b = this.plotter_.area;
    return b.y + c * b.h;
};
Dygraph.prototype.toDataCoords = function (a, c, b) { return [this.toDataXCoord(a), this.toDataYCoord(c, b)]; };
Dygraph.prototype.toDataXCoord = function (b) {
    if (b === null) {
        return null;
    }
    var c = this.plotter_.area;
    var a = this.xAxisRange();
    return a[0] + (b - c.x) / c.w * (a[1] - a[0]);
};
Dygraph.prototype.toDataYCoord = function (h, b) {
    if (h === null) {
        return null;
    }
    var c = this.plotter_.area;
    var g = this.yAxisRange(b);
    if (typeof (b) == "undefined") {
        b = 0;
    }
    if (!this.axes_[b].logscale) {
        return g[0] + (c.y + c.h - h) / c.h * (g[1] - g[0]);
    } else {
        var f = (h - c.y) / c.h;
        var a = Dygraph.log10(g[1]);
        var e = a - (f * (a - Dygraph.log10(g[0])));
        var d = Math.pow(Dygraph.LOG_SCALE, e);
        return d;
    }
};
Dygraph.prototype.toPercentYCoord = function (e, b) {
    if (e === null) {
        return null;
    }
    if (typeof (b) == "undefined") {
        b = 0;
    }
    var d = this.yAxisRange(b);
    var c;
    if (!this.axes_[b].logscale) {
        c = (d[1] - e) / (d[1] - d[0]);
    } else {
        var a = Dygraph.log10(d[1]);
        c = (a - Dygraph.log10(e)) / (a - Dygraph.log10(d[0]));
    }
    return c;
};
Dygraph.prototype.toPercentXCoord = function (b) {
    if (b === null) {
        return null;
    }
    var a = this.xAxisRange();
    return (b - a[0]) / (a[1] - a[0]);
};
Dygraph.prototype.numColumns = function () { return this.rawData_[0] ? this.rawData_[0].length : this.attr_("labels").length; };
Dygraph.prototype.numRows = function () { return this.rawData_.length; };
Dygraph.prototype.fullXRange_ = function () {
    if (this.numRows() > 0) {
        return [this.rawData_[0][0], this.rawData_[this.numRows() - 1][0]];
    } else {
        return [0, 1];
    }
};
Dygraph.prototype.getValue = function (b, a) {
    if (b < 0 || b > this.rawData_.length) {
        return null;
    }
    if (a < 0 || a > this.rawData_[b].length) {
        return null;
    }
    return this.rawData_[b][a];
};
Dygraph.prototype.createInterface_ = function () {
    var a = this.maindiv_;
    this.graphDiv = document.createElement("div");
    this.graphDiv.style.width = this.width_ + "px";
    this.graphDiv.style.height = this.height_ + "px";
    a.appendChild(this.graphDiv);
    this.canvas_ = Dygraph.createCanvas();
    this.canvas_.style.position = "absolute";
    this.canvas_.width = this.width_;
    this.canvas_.height = this.height_;
    this.canvas_.style.width = this.width_ + "px";
    this.canvas_.style.height = this.height_ + "px";
    this.canvas_ctx_ = Dygraph.getContext(this.canvas_);
    this.hidden_ = this.createPlotKitCanvas_(this.canvas_);
    this.hidden_ctx_ = Dygraph.getContext(this.hidden_);
    if (this.attr_("showRangeSelector")) {
        this.rangeSelector_ = new DygraphRangeSelector(this);
    }
    this.graphDiv.appendChild(this.hidden_);
    this.graphDiv.appendChild(this.canvas_);
    this.mouseEventElement_ = this.createMouseEventElement_();
    this.layout_ = new DygraphLayout(this);
    if (this.rangeSelector_) {
        this.rangeSelector_.addToGraph(this.graphDiv, this.layout_);
    }
    var b = this;
    Dygraph.addEvent(this.mouseEventElement_, "mousemove", function (c) { b.mouseMove_(c); });
    Dygraph.addEvent(this.mouseEventElement_, "mouseout", function (c) { b.mouseOut_(c); });
    this.createStatusMessage_();
    this.createDragInterface_();
    this.resizeHandler = function (c) { b.resize(); };
    Dygraph.addEvent(window, "resize", this.resizeHandler);
};
Dygraph.prototype.destroy = function () {
    var a = function (c) {
        while (c.hasChildNodes()) {
            a(c.firstChild);
            c.removeChild(c.firstChild);
        }
    };
    a(this.maindiv_);
    var b = function (c) {
        for (var d in c) {
            if (typeof (c[d]) === "object") {
                c[d] = null;
            }
        }
    };
    Dygraph.removeEvent(window, "resize", this.resizeHandler);
    this.resizeHandler = null;
    b(this.layout_);
    b(this.plotter_);
    b(this);
};
Dygraph.prototype.createPlotKitCanvas_ = function (a) {
    var b = Dygraph.createCanvas();
    b.style.position = "absolute";
    b.style.top = a.style.top;
    b.style.left = a.style.left;
    b.width = this.width_;
    b.height = this.height_;
    b.style.width = this.width_ + "px";
    b.style.height = this.height_ + "px";
    return b;
};
Dygraph.prototype.createMouseEventElement_ = function () {
    if (this.isUsingExcanvas_) {
        var a = document.createElement("div");
        a.style.position = "absolute";
        a.style.backgroundColor = "white";
        a.style.filter = "alpha(opacity=0)";
        a.style.width = this.width_ + "px";
        a.style.height = this.height_ + "px";
        this.graphDiv.appendChild(a);
        return a;
    } else {
        return this.canvas_;
    }
};
Dygraph.prototype.setColors_ = function () {
    var e = this.attr_("labels").length - 1;
    this.colors_ = [];
    var a = this.attr_("colors");
    var d;
    if (!a) {
        var c = this.attr_("colorSaturation") || 1;
        var b = this.attr_("colorValue") || 0.5;
        var j = Math.ceil(e / 2);
        for (d = 1; d <= e; d++) {
            if (!this.visibility()[d - 1]) {
                continue;
            }
            var g = d % 2 ? Math.ceil(d / 2) : (j + d / 2);
            var f = (1 * g / (1 + e));
            this.colors_.push(Dygraph.hsvToRGB(f, c, b));
        }
    } else {
        for (d = 0; d < e; d++) {
            if (!this.visibility()[d]) {
                continue;
            }
            var h = a[d % a.length];
            this.colors_.push(h);
        }
    }
    this.plotter_.setColors(this.colors_);
};
Dygraph.prototype.getColors = function () { return this.colors_; };
Dygraph.prototype.createStatusMessage_ = function () {
    var d = this.user_attrs_.labelsDiv;
    if (d && null !== d && (typeof (d) == "string" || d instanceof String)) {
        this.user_attrs_.labelsDiv = document.getElementById(d);
    }
    if (!this.attr_("labelsDiv")) {
        var a = this.attr_("labelsDivWidth");
        var c = { position: "absolute", fontSize: "14px", zIndex: 10, width: a + "px", top: "0px", left: (this.width_ - a - 2) + "px", background: "white", textAlign: "left", overflow: "hidden" };
        Dygraph.update(c, this.attr_("labelsDivStyles"));
        var e = document.createElement("div");
        e.className = "dygraph-legend";
        for (var b in c) {
            if (c.hasOwnProperty(b)) {
                e.style[b] = c[b];
            }
        }
        this.graphDiv.appendChild(e);
        this.attrs_.labelsDiv = e;
    }
};
Dygraph.prototype.positionLabelsDiv_ = function () {
    if (this.user_attrs_.hasOwnProperty("labelsDiv")) {
        return;
    }
    var a = this.plotter_.area;
    var b = this.attr_("labelsDiv");
    b.style.left = a.x + a.w - this.attr_("labelsDivWidth") - 1 + "px";
    b.style.top = a.y + "px";
};
Dygraph.prototype.createRollInterface_ = function () {
    if (!this.roller_) {
        this.roller_ = document.createElement("input");
        this.roller_.type = "text";
        this.roller_.style.display = "none";
        this.graphDiv.appendChild(this.roller_);
    }
    var e = this.attr_("showRoller") ? "block" : "none";
    var d = this.plotter_.area;
    var b = { position: "absolute", zIndex: 10, top: (d.y + d.h - 25) + "px", left: (d.x + 1) + "px", display: e };
    this.roller_.size = "2";
    this.roller_.value = this.rollPeriod_;
    for (var a in b) {
        if (b.hasOwnProperty(a)) {
            this.roller_.style[a] = b[a];
        }
    }
    var c = this;
    this.roller_.onchange = function () { c.adjustRoll(c.roller_.value); };
};
Dygraph.prototype.dragGetX_ = function (b, a) { return Dygraph.pageX(b) - a.px; };
Dygraph.prototype.dragGetY_ = function (b, a) { return Dygraph.pageY(b) - a.py; };
Dygraph.prototype.createDragInterface_ = function () {
    var c = {
        isZooming: false,
        isPanning: false,
        is2DPan: false,
        dragStartX: null,
        dragStartY: null,
        dragEndX: null,
        dragEndY: null,
        dragDirection: null,
        prevEndX: null,
        prevEndY: null,
        prevDragDirection: null,
        initialLeftmostDate: null,
        xUnitsPerPixel: null,
        dateRange: null,
        px: 0,
        py: 0,
        boundedDates: null,
        boundedValues: null,
        initializeMouseDown: function (i, h, f) {
            if (i.preventDefault) {
                i.preventDefault();
            } else {
                i.returnValue = false;
                i.cancelBubble = true;
            }
            f.px = Dygraph.findPosX(h.canvas_);
            f.py = Dygraph.findPosY(h.canvas_);
            f.dragStartX = h.dragGetX_(i, f);
            f.dragStartY = h.dragGetY_(i, f);
        }
    };
    var e = this.attr_("interactionModel");
    var b = this;
    var d = function (f) { return function (g) { f(g, b, c); }; };
    for (var a in e) {
        if (!e.hasOwnProperty(a)) {
            continue;
        }
        Dygraph.addEvent(this.mouseEventElement_, a, d(e[a]));
    }
    Dygraph.addEvent(document, "mouseup", function (g) {
        if (c.isZooming || c.isPanning) {
            c.isZooming = false;
            c.dragStartX = null;
            c.dragStartY = null;
        }
        if (c.isPanning) {
            c.isPanning = false;
            c.draggingDate = null;
            c.dateRange = null;
            for (var f = 0; f < b.axes_.length; f++) {
                delete b.axes_[f].draggingValue;
                delete b.axes_[f].dragValueRange;
            }
        }
    });
};
Dygraph.prototype.drawZoomRect_ = function (e, c, i, b, g, a, f, d) {
    var h = this.canvas_ctx_;
    if (a == Dygraph.HORIZONTAL) {
        h.clearRect(Math.min(c, f), this.layout_.getPlotArea().y, Math.abs(c - f), this.layout_.getPlotArea().h);
    } else {
        if (a == Dygraph.VERTICAL) {
            h.clearRect(this.layout_.getPlotArea().x, Math.min(b, d), this.layout_.getPlotArea().w, Math.abs(b - d));
        }
    }
    if (e == Dygraph.HORIZONTAL) {
        if (i && c) {
            h.fillStyle = "rgba(128,128,128,0.33)";
            h.fillRect(Math.min(c, i), this.layout_.getPlotArea().y, Math.abs(i - c), this.layout_.getPlotArea().h);
        }
    } else {
        if (e == Dygraph.VERTICAL) {
            if (g && b) {
                h.fillStyle = "rgba(128,128,128,0.33)";
                h.fillRect(this.layout_.getPlotArea().x, Math.min(b, g), this.layout_.getPlotArea().w, Math.abs(g - b));
            }
        }
    }
    if (this.isUsingExcanvas_) {
        this.currentZoomRectArgs_ = [e, c, i, b, g, 0, 0, 0];
    }
};
Dygraph.prototype.clearZoomRect_ = function () {
    this.currentZoomRectArgs_ = null;
    this.canvas_ctx_.clearRect(0, 0, this.canvas_.width, this.canvas_.height);
};
Dygraph.prototype.doZoomX_ = function (c, a) {
    this.currentZoomRectArgs_ = null;
    var b = this.toDataXCoord(c);
    var d = this.toDataXCoord(a);
    this.doZoomXDates_(b, d);
};
Dygraph.zoomAnimationFunction = function (c, b) {
    var a = 1.5;
    return (1 - Math.pow(a, -c)) / (1 - Math.pow(a, -b));
};
Dygraph.prototype.doZoomXDates_ = function (c, e) {
    var a = this.xAxisRange();
    var d = [c, e];
    this.zoomed_x_ = true;
    var b = this;
    this.doAnimatedZoom(a, d, null, null, function () {
        if (b.attr_("zoomCallback")) {
            b.attr_("zoomCallback")(c, e, b.yAxisRanges());
        }
    });
};
Dygraph.prototype.doZoomY_ = function (h, f) {
    this.currentZoomRectArgs_ = null;
    var c = this.yAxisRanges();
    var b = [];
    for (var e = 0; e < this.axes_.length; e++) {
        var d = this.toDataYCoord(h, e);
        var a = this.toDataYCoord(f, e);
        b.push([a, d]);
    }
    this.zoomed_y_ = true;
    var g = this;
    this.doAnimatedZoom(null, null, c, b, function () {
        if (g.attr_("zoomCallback")) {
            var i = g.xAxisRange();
            g.attr_("zoomCallback")(i[0], i[1], g.yAxisRanges());
        }
    });
};
Dygraph.prototype.doUnzoom_ = function () {
    var c = false, d = false, a = false;
    if (this.dateWindow_ !== null) {
        c = true;
        d = true;
    }
    for (var f = 0; f < this.axes_.length; f++) {
        if (this.axes_[f].valueWindow !== null) {
            c = true;
            a = true;
        }
    }
    this.clearSelection();
    if (c) {
        this.zoomed_x_ = false;
        this.zoomed_y_ = false;
        var e = this.rawData_[0][0];
        var b = this.rawData_[this.rawData_.length - 1][0];
        if (!this.attr_("animatedZooms")) {
            this.dateWindow_ = null;
            for (f = 0; f < this.axes_.length; f++) {
                if (this.axes_[f].valueWindow !== null) {
                    delete this.axes_[f].valueWindow;
                }
            }
            this.drawGraph_();
            if (this.attr_("zoomCallback")) {
                this.attr_("zoomCallback")(e, b, this.yAxisRanges());
            }
            return;
        }
        var k = null, l = null, j = null, g = null;
        if (d) {
            k = this.xAxisRange();
            l = [e, b];
        }
        if (a) {
            j = this.yAxisRanges();
            var m = this.gatherDatasets_(this.rolledSeries_, null);
            var n = m[1];
            this.computeYAxisRanges_(n);
            g = [];
            for (f = 0; f < this.axes_.length; f++) {
                g.push(this.axes_[f].extremeRange);
            }
        }
        var h = this;
        this.doAnimatedZoom(k, l, j, g, function () {
            h.dateWindow_ = null;
            for (var o = 0; o < h.axes_.length; o++) {
                if (h.axes_[o].valueWindow !== null) {
                    delete h.axes_[o].valueWindow;
                }
            }
            if (h.attr_("zoomCallback")) {
                h.attr_("zoomCallback")(e, b, h.yAxisRanges());
            }
        });
    }
};
Dygraph.prototype.doAnimatedZoom = function (a, e, b, c, m) {
    var i = this.attr_("animatedZooms") ? Dygraph.ANIMATION_STEPS : 1;
    var l = [];
    var k = [];
    var f, d;
    if (a !== null && e !== null) {
        for (f = 1; f <= i; f++) {
            d = Dygraph.zoomAnimationFunction(f, i);
            l[f - 1] = [a[0] * (1 - d) + d * e[0], a[1] * (1 - d) + d * e[1]];
        }
    }
    if (b !== null && c !== null) {
        for (f = 1; f <= i; f++) {
            d = Dygraph.zoomAnimationFunction(f, i);
            var n = [];
            for (var g = 0; g < this.axes_.length; g++) {
                n.push([b[g][0] * (1 - d) + d * c[g][0], b[g][1] * (1 - d) + d * c[g][1]]);
            }
            k[f - 1] = n;
        }
    }
    var h = this;
    Dygraph.repeatAndCleanup(function (p) {
        if (k.length) {
            for (var o = 0; o < h.axes_.length; o++) {
                var j = k[p][o];
                h.axes_[o].valueWindow = [j[0], j[1]];
            }
        }
        if (l.length) {
            h.dateWindow_ = l[p];
        }
        h.drawGraph_();
    }, i, Dygraph.ANIMATION_DURATION / i, m);
};
Dygraph.prototype.mouseMove_ = function (b) {
    var r = this.layout_.points;
    if (r === undefined) {
        return;
    }
    var a = Dygraph.pageX(b) - Dygraph.findPosX(this.mouseEventElement_);
    var j = -1;
    var f;
    var n = 1e+100;
    var o = -1;
    for (f = 0; f < r.length; f++) {
        var q = r[f];
        if (q === null) {
            continue;
        }
        var h = Math.abs(q.canvasx - a);
        if (h > n) {
            continue;
        }
        n = h;
        o = f;
    }
    if (o >= 0) {
        j = r[o].xval;
    }
    this.selPoints_ = [];
    var d = r.length;
    if (!this.attr_("stackedGraph")) {
        for (f = 0; f < d; f++) {
            if (r[f].xval == j) {
                this.selPoints_.push(r[f]);
            }
        }
    } else {
        var g = 0;
        for (f = d - 1; f >= 0; f--) {
            if (r[f].xval == j) {
                var c = {};
                for (var e in r[f]) {
                    c[e] = r[f][e];
                }
                c.yval -= g;
                g += c.yval;
                this.selPoints_.push(c);
            }
        }
        this.selPoints_.reverse();
    }
    if (this.attr_("highlightCallback")) {
        var m = this.lastx_;
        if (m !== null && j != m) {
            this.attr_("highlightCallback")(b, j, this.selPoints_, this.idxToRow_(o));
        }
    }
    this.lastx_ = j;
    this.updateSelection_();
};
Dygraph.prototype.idxToRow_ = function (a) {
    if (a < 0) {
        return -1;
    }
    var d = -1;
    for (var c = 0; c < this.boundaryIds_.length; c++) {
        if (this.boundaryIds_[c] !== undefined) {
            d = c;
            break;
        }
    }
    if (d < 0) {
        return -1;
    }
    for (var b in this.layout_.datasets) {
        if (a < this.layout_.datasets[b].length) {
            return this.boundaryIds_[d][0] + a;
        }
        a -= this.layout_.datasets[b].length;
    }
    return -1;
};
Dygraph.prototype.generateLegendDashHTML_ = function (o, d, n) {
    var h = "";
    var f, e, b, k;
    var c = 0, m = 0;
    var l = [];
    var g;
    var a = (/MSIE/.test(navigator.userAgent) && !window.opera);
    if (a) {
        return "&mdash;";
    }
    if (!o || o.length <= 1) {
        h = '<div style="display: inline-block; position: relative; bottom: .5ex; padding-left: 1em; height: 1px; border-bottom: 2px solid ' + d + ';"></div>';
    } else {
        for (f = 0; f <= o.length; f++) {
            c += o[f % o.length];
        }
        g = Math.floor(n / (c - o[0]));
        if (g > 1) {
            for (f = 0; f < o.length; f++) {
                l[f] = o[f] / n;
            }
            m = l.length;
        } else {
            g = 1;
            for (f = 0; f < o.length; f++) {
                l[f] = o[f] / c;
            }
            m = l.length + 1;
        }
        for (e = 0; e < g; e++) {
            for (f = 0; f < m; f += 2) {
                b = l[f % l.length];
                if (f < o.length) {
                    k = l[(f + 1) % l.length];
                } else {
                    k = 0;
                }
                h += '<div style="display: inline-block; position: relative; bottom: .5ex; margin-right: ' + k + "em; padding-left: " + b + "em; height: 1px; border-bottom: 2px solid " + d + ';"></div>';
            }
        }
    }
    return h;
};
Dygraph.prototype.generateLegendHTML_ = function (k, f, b) {
    var l, u, o, s, m, g;
    if (typeof (k) === "undefined") {
        if (this.attr_("legend") != "always") {
            return "";
        }
        u = this.attr_("labelsSeparateLines");
        var r = this.attr_("labels");
        l = "";
        for (o = 1; o < r.length; o++) {
            if (!this.visibility()[o - 1]) {
                continue;
            }
            s = this.plotter_.colors[r[o]];
            if (l !== "") {
                l += (u ? "<br/>" : " ");
            }
            g = this.attr_("strokePattern", r[o]);
            m = this.generateLegendDashHTML_(g, s, b);
            l += "<span style='font-weight: bold; color: " + s + ";'>" + m + " " + r[o] + "</span>";
        }
        return l;
    }
    var t = this.optionsViewForAxis_("x");
    var h = t("valueFormatter");
    l = h(k, t, this.attr_("labels")[0], this) + ":";
    var p = [];
    var d = this.numAxes();
    for (o = 0; o < d; o++) {
        p[o] = this.optionsViewForAxis_("y" + (o ? 1 + o : ""));
    }
    var e = this.attr_("labelsShowZeroValues");
    u = this.attr_("labelsSeparateLines");
    for (o = 0; o < this.selPoints_.length; o++) {
        var n = this.selPoints_[o];
        if (n.yval === 0 && !e) {
            continue;
        }
        if (!Dygraph.isOK(n.canvasy)) {
            continue;
        }
        if (u) {
            l += "<br/>";
        }
        var j = p[this.seriesToAxisMap_[n.name]];
        var q = j("valueFormatter");
        s = this.plotter_.colors[n.name];
        var a = q(n.yval, j, n.name, this);
        l += " <b><span style='color: " + s + ";'>" + n.name + "</span></b>:" + a;
    }
    return l;
};
Dygraph.prototype.setLegendHTML_ = function (b, e) {
    var c = this.attr_("labelsDiv");
    var f = document.createElement("span");
    f.setAttribute("style", "margin: 0; padding: 0 0 0 1em; border: 0;");
    c.appendChild(f);
    var a = f.offsetWidth;
    var d = this.generateLegendHTML_(b, e, a);
    if (c !== null) {
        c.innerHTML = d;
    } else {
        if (typeof (this.shown_legend_error_) == "undefined") {
            this.error("labelsDiv is set to something nonexistent; legend will not be shown.");
            this.shown_legend_error_ = true;
        }
    }
};
Dygraph.prototype.updateSelection_ = function () {
    var d;
    var h = this.canvas_ctx_;
    if (this.previousVerticalX_ >= 0) {
        var e = 0;
        var f = this.attr_("labels");
        for (d = 1; d < f.length; d++) {
            var b = this.attr_("highlightCircleSize", f[d]);
            if (b > e) {
                e = b;
            }
        }
        var g = this.previousVerticalX_;
        h.clearRect(g - e - 1, 0, 2 * e + 2, this.height_);
    }
    if (this.isUsingExcanvas_ && this.currentZoomRectArgs_) {
        Dygraph.prototype.drawZoomRect_.apply(this, this.currentZoomRectArgs_);
    }
    if (this.selPoints_.length > 0) {
        if (this.attr_("showLabelsOnHighlight")) {
            this.setLegendHTML_(this.lastx_, this.selPoints_);
        }
        var c = this.selPoints_[0].canvasx;
        h.save();
        for (d = 0; d < this.selPoints_.length; d++) {
            var j = this.selPoints_[d];
            if (!Dygraph.isOK(j.canvasy)) {
                continue;
            }
            var a = this.attr_("highlightCircleSize", j.name);
            h.beginPath();
            h.fillStyle = this.plotter_.colors[j.name];
            h.arc(c, j.canvasy, a, 0, 2 * Math.PI, false);
            h.fill();
        }
        h.restore();
        this.previousVerticalX_ = c;
    }
};
Dygraph.prototype.setSelection = function (c) {
    this.selPoints_ = [];
    var d = 0;
    if (c !== false) {
        c = c - this.boundaryIds_[0][0];
    }
    if (c !== false && c >= 0) {
        for (var b in this.layout_.datasets) {
            if (c < this.layout_.datasets[b].length) {
                var a = this.layout_.points[d + c];
                if (this.attr_("stackedGraph")) {
                    a = this.layout_.unstackPointAtIndex(d + c);
                }
                this.selPoints_.push(a);
            }
            d += this.layout_.datasets[b].length;
        }
    }
    if (this.selPoints_.length) {
        this.lastx_ = this.selPoints_[0].xval;
        this.updateSelection_();
    } else {
        this.clearSelection();
    }
};
Dygraph.prototype.mouseOut_ = function (a) {
    if (this.attr_("unhighlightCallback")) {
        this.attr_("unhighlightCallback")(a);
    }
    if (this.attr_("hideOverlayOnMouseOut")) {
        this.clearSelection();
    }
};
Dygraph.prototype.clearSelection = function () {
    this.canvas_ctx_.clearRect(0, 0, this.width_, this.height_);
    this.setLegendHTML_();
    this.selPoints_ = [];
    this.lastx_ = -1;
};
Dygraph.prototype.getSelection = function () {
    if (!this.selPoints_ || this.selPoints_.length < 1) {
        return -1;
    }
    for (var a = 0; a < this.layout_.points.length; a++) {
        if (this.layout_.points[a].x == this.selPoints_[0].x) {
            return a + this.boundaryIds_[0][0];
        }
    }
    return -1;
};
Dygraph.prototype.loadedEvent_ = function (a) {
    this.rawData_ = this.parseCSV_(a);
    this.predraw_();
};
Dygraph.prototype.addXTicks_ = function () {
    var a;
    if (this.dateWindow_) {
        a = [this.dateWindow_[0], this.dateWindow_[1]];
    } else {
        a = this.fullXRange_();
    }
    var c = this.optionsViewForAxis_("x");
    var b = c("ticker")(a[0], a[1], this.width_, c, this);
    this.layout_.setXTicks(b);
};
Dygraph.prototype.extremeValues_ = function (d) {
    var h = null, f = null, c, g;
    var b = this.attr_("errorBars") || this.attr_("customBars");
    if (b) {
        for (c = 0; c < d.length; c++) {
            g = d[c][1][0];
            if (!g) {
                continue;
            }
            var a = g - d[c][1][1];
            var e = g + d[c][1][2];
            if (a > g) {
                a = g;
            }
            if (e < g) {
                e = g;
            }
            if (f === null || e > f) {
                f = e;
            }
            if (h === null || a < h) {
                h = a;
            }
        }
    } else {
        for (c = 0; c < d.length; c++) {
            g = d[c][1];
            if (g === null || isNaN(g)) {
                continue;
            }
            if (f === null || g > f) {
                f = g;
            }
            if (h === null || g < h) {
                h = g;
            }
        }
    }
    return [h, f];
};
Dygraph.prototype.predraw_ = function () {
    var f = new Date();
    this.computeYAxes_();
    if (this.plotter_) {
        this.plotter_.clear();
    }
    this.plotter_ = new DygraphCanvasRenderer(this, this.hidden_, this.hidden_ctx_, this.layout_);
    this.createRollInterface_();
    this.positionLabelsDiv_();
    if (this.rangeSelector_) {
        this.rangeSelector_.renderStaticLayer();
    }
    this.rolledSeries_ = [null];
    for (var c = 1; c < this.numColumns(); c++) {
        var e = this.attr_("connectSeparatedPoints", c);
        var d = this.attr_("logscale", c);
        var b = this.extractSeries_(this.rawData_, c, d, e);
        b = this.rollingAverage(b, this.rollPeriod_);
        this.rolledSeries_.push(b);
    }
    this.drawGraph_();
    var a = new Date();
    this.drawingTimeMs_ = (a - f);
};
Dygraph.prototype.gatherDatasets_ = function (w, c) {
    var s = [];
    var b = [];
    var e = [];
    var a = {};
    var u, t, r;
    var m = w.length - 1;
    for (u = m; u >= 1; u--) {
        if (!this.visibility()[u - 1]) {
            continue;
        }
        var h = [];
        for (t = 0; t < w[u].length; t++) {
            h.push(w[u][t]);
        }
        var o = this.attr_("errorBars") || this.attr_("customBars");
        if (c) {
            var A = c[0];
            var f = c[1];
            var p = [];
            var d = null, z = null;
            for (r = 0; r < h.length; r++) {
                if (h[r][0] >= A && d === null) {
                    d = r;
                }
                if (h[r][0] <= f) {
                    z = r;
                }
            }
            if (d === null) {
                d = 0;
            }
            if (d > 0) {
                d--;
            }
            if (z === null) {
                z = h.length - 1;
            }
            if (z < h.length - 1) {
                z++;
            }
            s[u - 1] = [d, z];
            for (r = d; r <= z; r++) {
                p.push(h[r]);
            }
            h = p;
        } else {
            s[u - 1] = [0, h.length - 1];
        }
        var n = this.extremeValues_(h);
        if (o) {
            for (t = 0; t < h.length; t++) {
                h[t] = [h[t][0], h[t][1][0], h[t][1][1], h[t][1][2]];
            }
        } else {
            if (this.attr_("stackedGraph")) {
                var q = h.length;
                var y;
                for (t = 0; t < q; t++) {
                    var g = h[t][0];
                    if (b[g] === undefined) {
                        b[g] = 0;
                    }
                    y = h[t][1];
                    b[g] += y;
                    h[t] = [g, b[g]];
                    if (b[g] > n[1]) {
                        n[1] = b[g];
                    }
                    if (b[g] < n[0]) {
                        n[0] = b[g];
                    }
                }
            }
        }
        var v = this.attr_("labels")[u];
        a[v] = n;
        e[u] = h;
    }
    return [e, a, s];
};
Dygraph.prototype.drawGraph_ = function (j) {
    var a = new Date();
    if (typeof (j) === "undefined") {
        j = true;
    }
    var e = this.is_initial_draw_;
    this.is_initial_draw_ = false;
    this.layout_.removeAllDatasets();
    this.setColors_();
    this.attrs_.pointSize = 0.5 * this.attr_("highlightCircleSize");
    var g = this.gatherDatasets_(this.rolledSeries_, this.dateWindow_);
    var d = g[0];
    var h = g[1];
    this.boundaryIds_ = g[2];
    for (var f = 1; f < d.length; f++) {
        if (!this.visibility()[f - 1]) {
            continue;
        }
        this.layout_.addDataset(this.attr_("labels")[f], d[f]);
    }
    this.computeYAxisRanges_(h);
    this.layout_.setYAxes(this.axes_);
    this.addXTicks_();
    var b = this.zoomed_x_;
    this.layout_.setDateWindow(this.dateWindow_);
    this.zoomed_x_ = b;
    this.layout_.evaluateWithError();
    this.renderGraph_(e, false);
    if (this.attr_("timingName")) {
        var c = new Date();
        if (console) {
            console.log(this.attr_("timingName") + " - drawGraph: " + (c - a) + "ms");
        }
    }
};
Dygraph.prototype.renderGraph_ = function (a, b) {
    this.plotter_.clear();
    this.plotter_.render();
    this.canvas_.getContext("2d").clearRect(0, 0, this.canvas_.width, this.canvas_.height);
    this.setLegendHTML_();
    if (!a) {
        if (b) {
            if (typeof (this.selPoints_) !== "undefined" && this.selPoints_.length) {
                this.clearSelection();
            } else {
                this.clearSelection();
            }
        }
    }
    if (this.rangeSelector_) {
        this.rangeSelector_.renderInteractiveLayer();
    }
    if (this.attr_("drawCallback") !== null) {
        this.attr_("drawCallback")(this, a);
    }
};
Dygraph.prototype.computeYAxes_ = function () {
    var g, c, m, b, j, a, p;
    if (this.axes_ !== undefined && this.user_attrs_.hasOwnProperty("valueRange") === false) {
        c = [];
        for (j = 0; j < this.axes_.length; j++) {
            c.push(this.axes_[j].valueWindow);
        }
    }
    this.axes_ = [{ yAxisId: 0, g: this}];
    this.seriesToAxisMap_ = {};
    var h = this.attr_("labels");
    var f = {};
    for (g = 1; g < h.length; g++) {
        f[h[g]] = (g - 1);
    }
    var e = ["includeZero", "valueRange", "labelsKMB", "labelsKMG2", "pixelsPerYLabel", "yAxisLabelWidth", "axisLabelFontSize", "axisTickSize", "logscale"];
    for (g = 0; g < e.length; g++) {
        var d = e[g];
        p = this.attr_(d);
        if (p) {
            this.axes_[0][d] = p;
        }
    }
    for (m in f) {
        if (!f.hasOwnProperty(m)) {
            continue;
        }
        b = this.attr_("axis", m);
        if (b === null) {
            this.seriesToAxisMap_[m] = 0;
            continue;
        }
        if (typeof (b) == "object") {
            a = {};
            Dygraph.update(a, this.axes_[0]);
            Dygraph.update(a, { valueRange: null });
            var o = this.axes_.length;
            a.yAxisId = o;
            a.g = this;
            Dygraph.update(a, b);
            this.axes_.push(a);
            this.seriesToAxisMap_[m] = o;
        }
    }
    for (m in f) {
        if (!f.hasOwnProperty(m)) {
            continue;
        }
        b = this.attr_("axis", m);
        if (typeof (b) == "string") {
            if (!this.seriesToAxisMap_.hasOwnProperty(b)) {
                this.error("Series " + m + " wants to share a y-axis with series " + b + ", which does not define its own axis.");
                return null;
            }
            var n = this.seriesToAxisMap_[b];
            this.seriesToAxisMap_[m] = n;
        }
    }
    if (c !== undefined) {
        for (j = 0; j < c.length; j++) {
            this.axes_[j].valueWindow = c[j];
        }
    }
    for (b = 0; b < this.axes_.length; b++) {
        if (b === 0) {
            a = this.optionsViewForAxis_("y" + (b ? "2" : ""));
            p = a("valueRange");
            if (p) {
                this.axes_[b].valueRange = p;
            }
        } else {
            var l = this.user_attrs_.axes;
            if (l && l.y2) {
                p = l.y2.valueRange;
                if (p) {
                    this.axes_[b].valueRange = p;
                }
            }
        }
    }
};
Dygraph.prototype.numAxes = function () {
    var c = 0;
    for (var b in this.seriesToAxisMap_) {
        if (!this.seriesToAxisMap_.hasOwnProperty(b)) {
            continue;
        }
        var a = this.seriesToAxisMap_[b];
        if (a > c) {
            c = a;
        }
    }
    return 1 + c;
};
Dygraph.prototype.axisPropertiesForSeries = function (a) { return this.axes_[this.seriesToAxisMap_[a]]; };
Dygraph.prototype.computeYAxisRanges_ = function (a) {
    var g = [], h;
    for (h in this.seriesToAxisMap_) {
        if (!this.seriesToAxisMap_.hasOwnProperty(h)) {
            continue;
        }
        var p = this.seriesToAxisMap_[h];
        while (g.length <= p) {
            g.push([]);
        }
        g[p].push(h);
    }
    for (var u = 0; u < this.axes_.length; u++) {
        var b = this.axes_[u];
        if (!g[u]) {
            b.extremeRange = [0, 1];
        } else {
            h = g[u];
            var x = Infinity;
            var w = -Infinity;
            var o, m;
            for (var s = 0; s < h.length; s++) {
                if (!a.hasOwnProperty(h[s])) {
                    continue;
                }
                o = a[h[s]][0];
                if (o !== null) {
                    x = Math.min(o, x);
                }
                m = a[h[s]][1];
                if (m !== null) {
                    w = Math.max(m, w);
                }
            }
            if (b.includeZero && x > 0) {
                x = 0;
            }
            if (x == Infinity) {
                x = 0;
            }
            if (w == -Infinity) {
                w = 1;
            }
            var t = w - x;
            if (t === 0) {
                t = w;
            }
            var d, z;
            if (b.logscale) {
                d = w + 0.1 * t;
                z = x;
            } else {
                d = w + 0.1 * t;
                z = x - 0.1 * t;
                if (!this.attr_("avoidMinZero")) {
                    if (z < 0 && x >= 0) {
                        z = 0;
                    }
                    if (d > 0 && w <= 0) {
                        d = 0;
                    }
                }
                if (this.attr_("includeZero")) {
                    if (w < 0) {
                        d = 0;
                    }
                    if (x > 0) {
                        z = 0;
                    }
                }
            }
            b.extremeRange = [z, d];
        }
        if (b.valueWindow) {
            b.computedValueRange = [b.valueWindow[0], b.valueWindow[1]];
        } else {
            if (b.valueRange) {
                b.computedValueRange = [b.valueRange[0], b.valueRange[1]];
            } else {
                b.computedValueRange = b.extremeRange;
            }
        }
        var n = this.optionsViewForAxis_("y" + (u ? "2" : ""));
        var y = n("ticker");
        if (u === 0 || b.independentTicks) {
            b.ticks = y(b.computedValueRange[0], b.computedValueRange[1], this.height_, n, this);
        } else {
            var l = this.axes_[0];
            var e = l.ticks;
            var f = l.computedValueRange[1] - l.computedValueRange[0];
            var A = b.computedValueRange[1] - b.computedValueRange[0];
            var c = [];
            for (var r = 0; r < e.length; r++) {
                var q = (e[r].v - l.computedValueRange[0]) / f;
                var v = b.computedValueRange[0] + q * A;
                c.push(v);
            }
            b.ticks = y(b.computedValueRange[0], b.computedValueRange[1], this.height_, n, this, c);
        }
    }
};
Dygraph.prototype.extractSeries_ = function (h, e, g, f) {
    var d = [];
    for (var c = 0; c < h.length; c++) {
        var b = h[c][0];
        var a = h[c][e];
        if (g) {
            if (a <= 0) {
                a = null;
            }
            d.push([b, a]);
        } else {
            if (a !== null || !f) {
                d.push([b, a]);
            }
        }
    }
    return d;
};
Dygraph.prototype.rollingAverage = function (l, d) {
    if (l.length < 2) {
        return l;
    }
    d = Math.min(d, l.length);
    var b = [];
    var s = this.attr_("sigma");
    var E, o, w, v, m, c, D, x;
    if (this.fractions_) {
        var k = 0;
        var h = 0;
        var e = 100;
        for (w = 0; w < l.length; w++) {
            k += l[w][1][0];
            h += l[w][1][1];
            if (w - d >= 0) {
                k -= l[w - d][1][0];
                h -= l[w - d][1][1];
            }
            var A = l[w][0];
            var u = h ? k / h : 0;
            if (this.attr_("errorBars")) {
                if (this.attr_("wilsonInterval")) {
                    if (h) {
                        var r = u < 0 ? 0 : u, t = h;
                        var z = s * Math.sqrt(r * (1 - r) / t + s * s / (4 * t * t));
                        var a = 1 + s * s / h;
                        E = (r + s * s / (2 * h) - z) / a;
                        o = (r + s * s / (2 * h) + z) / a;
                        b[w] = [A, [r * e, (r - E) * e, (o - r) * e]];
                    } else {
                        b[w] = [A, [0, 0, 0]];
                    }
                } else {
                    x = h ? s * Math.sqrt(u * (1 - u) / h) : 1;
                    b[w] = [A, [e * u, e * x, e * x]];
                }
            } else {
                b[w] = [A, e * u];
            }
        }
    } else {
        if (this.attr_("customBars")) {
            E = 0;
            var B = 0;
            o = 0;
            var g = 0;
            for (w = 0; w < l.length; w++) {
                var C = l[w][1];
                m = C[1];
                b[w] = [l[w][0], [m, m - C[0], C[2] - m]];
                if (m !== null && !isNaN(m)) {
                    E += C[0];
                    B += m;
                    o += C[2];
                    g += 1;
                }
                if (w - d >= 0) {
                    var q = l[w - d];
                    if (q[1][1] !== null && !isNaN(q[1][1])) {
                        E -= q[1][0];
                        B -= q[1][1];
                        o -= q[1][2];
                        g -= 1;
                    }
                }
                if (g) {
                    b[w] = [l[w][0], [1 * B / g, 1 * (B - E) / g, 1 * (o - B) / g]];
                } else {
                    b[w] = [l[w][0], [null, null, null]];
                }
            }
        } else {
            if (!this.attr_("errorBars")) {
                if (d == 1) {
                    return l;
                }
                for (w = 0; w < l.length; w++) {
                    c = 0;
                    D = 0;
                    for (v = Math.max(0, w - d + 1); v < w + 1; v++) {
                        m = l[v][1];
                        if (m === null || isNaN(m)) {
                            continue;
                        }
                        D++;
                        c += l[v][1];
                    }
                    if (D) {
                        b[w] = [l[w][0], c / D];
                    } else {
                        b[w] = [l[w][0], null];
                    }
                }
            } else {
                for (w = 0; w < l.length; w++) {
                    c = 0;
                    var f = 0;
                    D = 0;
                    for (v = Math.max(0, w - d + 1); v < w + 1; v++) {
                        m = l[v][1][0];
                        if (m === null || isNaN(m)) {
                            continue;
                        }
                        D++;
                        c += l[v][1][0];
                        f += Math.pow(l[v][1][1], 2);
                    }
                    if (D) {
                        x = Math.sqrt(f) / D;
                        b[w] = [l[w][0], [c / D, s * x, s * x]];
                    } else {
                        b[w] = [l[w][0], [null, null, null]];
                    }
                }
            }
        }
    }
    return b;
};
Dygraph.prototype.detectTypeFromString_ = function (b) {
    var a = false;
    var c = b.indexOf("-");
    if ((c > 0 && (b[c - 1] != "e" && b[c - 1] != "E")) || b.indexOf("/") >= 0 || isNaN(parseFloat(b))) {
        a = true;
    } else {
        if (b.length == 8 && b > "19700101" && b < "20371231") {
            a = true;
        }
    }
    if (a) {
        this.attrs_.xValueParser = Dygraph.dateParser;
        this.attrs_.axes.x.valueFormatter = Dygraph.dateString_;
        this.attrs_.axes.x.ticker = Dygraph.dateTicker;
        this.attrs_.axes.x.axisLabelFormatter = Dygraph.dateAxisFormatter;
    } else {
        this.attrs_.xValueParser = function (d) { return parseFloat(d); };
        this.attrs_.axes.x.valueFormatter = function (d) { return d; };
        this.attrs_.axes.x.ticker = Dygraph.numericTicks;
        this.attrs_.axes.x.axisLabelFormatter = this.attrs_.axes.x.valueFormatter;
    }
};
Dygraph.prototype.parseFloat_ = function (a, c, b) {
    var e = parseFloat(a);
    if (!isNaN(e)) {
        return e;
    }
    if (/^ *$/.test(a)) {
        return null;
    }
    if (/^ *nan *$/i.test(a)) {
        return NaN;
    }
    var d = "Unable to parse '" + a + "' as a number";
    if (b !== null && c !== null) {
        d += " on line " + (1 + c) + " ('" + b + "') of CSV.";
    }
    this.error(d);
    return null;
};
Dygraph.prototype.parseCSV_ = function (s) {
    var r = [];
    var a = s.split("\n");
    var g, k;
    var p = this.attr_("delimiter");
    if (a[0].indexOf(p) == -1 && a[0].indexOf("\t") >= 0) {
        p = "\t";
    }
    var b = 0;
    if (!("labels" in this.user_attrs_)) {
        b = 1;
        this.attrs_.labels = a[0].split(p);
    }
    var o = 0;
    var m;
    var q = false;
    var c = this.attr_("labels").length;
    var f = false;
    for (var l = b; l < a.length; l++) {
        var e = a[l];
        o = l;
        if (e.length === 0) {
            continue;
        }
        if (e[0] == "#") {
            continue;
        }
        var d = e.split(p);
        if (d.length < 2) {
            continue;
        }
        var h = [];
        if (!q) {
            this.detectTypeFromString_(d[0]);
            m = this.attr_("xValueParser");
            q = true;
        }
        h[0] = m(d[0], this);
        if (this.fractions_) {
            for (k = 1; k < d.length; k++) {
                g = d[k].split("/");
                if (g.length != 2) {
                    this.error('Expected fractional "num/den" values in CSV data but found a value \'' + d[k] + "' on line " + (1 + l) + " ('" + e + "') which is not of this form.");
                    h[k] = [0, 0];
                } else {
                    h[k] = [this.parseFloat_(g[0], l, e), this.parseFloat_(g[1], l, e)];
                }
            }
        } else {
            if (this.attr_("errorBars")) {
                if (d.length % 2 != 1) {
                    this.error("Expected alternating (value, stdev.) pairs in CSV data but line " + (1 + l) + " has an odd number of values (" + (d.length - 1) + "): '" + e + "'");
                }
                for (k = 1; k < d.length; k += 2) {
                    h[(k + 1) / 2] = [this.parseFloat_(d[k], l, e), this.parseFloat_(d[k + 1], l, e)];
                }
            } else {
                if (this.attr_("customBars")) {
                    for (k = 1; k < d.length; k++) {
                        var t = d[k];
                        if (/^ *$/.test(t)) {
                            h[k] = [null, null, null];
                        } else {
                            g = t.split(";");
                            if (g.length == 3) {
                                h[k] = [this.parseFloat_(g[0], l, e), this.parseFloat_(g[1], l, e), this.parseFloat_(g[2], l, e)];
                            } else {
                                this.warn('When using customBars, values must be either blank or "low;center;high" tuples (got "' + t + '" on line ' + (1 + l));
                            }
                        }
                    }
                } else {
                    for (k = 1; k < d.length; k++) {
                        h[k] = this.parseFloat_(d[k], l, e);
                    }
                }
            }
        }
        if (r.length > 0 && h[0] < r[r.length - 1][0]) {
            f = true;
        }
        if (h.length != c) {
            this.error("Number of columns in line " + l + " (" + h.length + ") does not agree with number of labels (" + c + ") " + e);
        }
        if (l === 0 && this.attr_("labels")) {
            var n = true;
            for (k = 0; n && k < h.length; k++) {
                if (h[k]) {
                    n = false;
                }
            }
            if (n) {
                this.warn("The dygraphs 'labels' option is set, but the first row of CSV data ('" + e + "') appears to also contain labels. Will drop the CSV labels and use the option labels.");
                continue;
            }
        }
        r.push(h);
    }
    if (f) {
        this.warn("CSV is out of order; order it correctly to speed loading.");
        r.sort(function (j, i) { return j[0] - i[0]; });
    }
    return r;
};
Dygraph.prototype.parseArray_ = function (b) {
    if (b.length === 0) {
        this.error("Can't plot empty data set");
        return null;
    }
    if (b[0].length === 0) {
        this.error("Data set cannot contain an empty row");
        return null;
    }
    var a;
    if (this.attr_("labels") === null) {
        this.warn("Using default labels. Set labels explicitly via 'labels' in the options parameter");
        this.attrs_.labels = ["X"];
        for (a = 1; a < b[0].length; a++) {
            this.attrs_.labels.push("Y" + a);
        }
    }
    if (Dygraph.isDateLike(b[0][0])) {
        this.attrs_.axes.x.valueFormatter = Dygraph.dateString_;
        this.attrs_.axes.x.axisLabelFormatter = Dygraph.dateAxisFormatter;
        this.attrs_.axes.x.ticker = Dygraph.dateTicker;
        var c = Dygraph.clone(b);
        for (a = 0; a < b.length; a++) {
            if (c[a].length === 0) {
                this.error("Row " + (1 + a) + " of data is empty");
                return null;
            }
            if (c[a][0] === null || typeof (c[a][0].getTime) != "function" || isNaN(c[a][0].getTime())) {
                this.error("x value in row " + (1 + a) + " is not a Date");
                return null;
            }
            c[a][0] = c[a][0].getTime();
        }
        return c;
    } else {
        this.attrs_.axes.x.valueFormatter = function (d) { return d; };
        this.attrs_.axes.x.axisLabelFormatter = Dygraph.numberAxisLabelFormatter;
        this.attrs_.axes.x.ticker = Dygraph.numericTicks;
        return b;
    }
};
Dygraph.prototype.parseDataTable_ = function (w) {
    var d = function (i) {
        var j = String.fromCharCode(65 + i % 26);
        i = Math.floor(i / 26);
        while (i > 0) {
            j = String.fromCharCode(65 + (i - 1) % 26) + j.toLowerCase();
            i = Math.floor((i - 1) / 26);
        }
        return j;
    };
    var h = w.getNumberOfColumns();
    var g = w.getNumberOfRows();
    var f = w.getColumnType(0);
    if (f == "date" || f == "datetime") {
        this.attrs_.xValueParser = Dygraph.dateParser;
        this.attrs_.axes.x.valueFormatter = Dygraph.dateString_;
        this.attrs_.axes.x.ticker = Dygraph.dateTicker;
        this.attrs_.axes.x.axisLabelFormatter = Dygraph.dateAxisFormatter;
    } else {
        if (f == "number") {
            this.attrs_.xValueParser = function (i) { return parseFloat(i); };
            this.attrs_.axes.x.valueFormatter = function (i) { return i; };
            this.attrs_.axes.x.ticker = Dygraph.numericTicks;
            this.attrs_.axes.x.axisLabelFormatter = this.attrs_.axes.x.valueFormatter;
        } else {
            this.error("only 'date', 'datetime' and 'number' types are supported for column 1 of DataTable input (Got '" + f + "')");
            return null;
        }
    }
    var m = [];
    var t = {};
    var s = false;
    var q, o;
    for (q = 1; q < h; q++) {
        var b = w.getColumnType(q);
        if (b == "number") {
            m.push(q);
        } else {
            if (b == "string" && this.attr_("displayAnnotations")) {
                var r = m[m.length - 1];
                if (!t.hasOwnProperty(r)) {
                    t[r] = [q];
                } else {
                    t[r].push(q);
                }
                s = true;
            } else {
                this.error("Only 'number' is supported as a dependent type with Gviz. 'string' is only supported if displayAnnotations is true");
            }
        }
    }
    var u = [w.getColumnLabel(0)];
    for (q = 0; q < m.length; q++) {
        u.push(w.getColumnLabel(m[q]));
        if (this.attr_("errorBars")) {
            q += 1;
        }
    }
    this.attrs_.labels = u;
    h = u.length;
    var v = [];
    var l = false;
    var a = [];
    for (q = 0; q < g; q++) {
        var e = [];
        if (typeof (w.getValue(q, 0)) === "undefined" || w.getValue(q, 0) === null) {
            this.warn("Ignoring row " + q + " of DataTable because of undefined or null first column.");
            continue;
        }
        if (f == "date" || f == "datetime") {
            e.push(w.getValue(q, 0).getTime());
        } else {
            e.push(w.getValue(q, 0));
        }
        if (!this.attr_("errorBars")) {
            for (o = 0; o < m.length; o++) {
                var c = m[o];
                e.push(w.getValue(q, c));
                if (s && t.hasOwnProperty(c) && w.getValue(q, t[c][0]) !== null) {
                    var p = {};
                    p.series = w.getColumnLabel(c);
                    p.xval = e[0];
                    p.shortText = d(a.length);
                    p.text = "";
                    for (var n = 0; n < t[c].length; n++) {
                        if (n) {
                            p.text += "\n";
                        }
                        p.text += w.getValue(q, t[c][n]);
                    }
                    a.push(p);
                }
            }
            for (o = 0; o < e.length; o++) {
                if (!isFinite(e[o])) {
                    e[o] = null;
                }
            }
        } else {
            for (o = 0; o < h - 1; o++) {
                e.push([w.getValue(q, 1 + 2 * o), w.getValue(q, 2 + 2 * o)]);
            }
        }
        if (v.length > 0 && e[0] < v[v.length - 1][0]) {
            l = true;
        }
        v.push(e);
    }
    if (l) {
        this.warn("DataTable is out of order; order it correctly to speed loading.");
        v.sort(function (j, i) { return j[0] - i[0]; });
    }
    this.rawData_ = v;
    if (a.length > 0) {
        this.setAnnotations(a, true);
    }
};
Dygraph.prototype.start_ = function () {
    var c = this.file_;
    if (typeof c == "function") {
        c = c();
    }
    if (Dygraph.isArrayLike(c)) {
        this.rawData_ = this.parseArray_(c);
        this.predraw_();
    } else {
        if (typeof c == "object" && typeof c.getColumnRange == "function") {
            this.parseDataTable_(c);
            this.predraw_();
        } else {
            if (typeof c == "string") {
                if (c.indexOf("\n") >= 0) {
                    this.loadedEvent_(c);
                } else {
                    var b = new XMLHttpRequest();
                    var a = this;
                    b.onreadystatechange = function () {
                        if (b.readyState == 4) {
                            if (b.status === 200 || b.status === 0) {
                                a.loadedEvent_(b.responseText);
                            }
                        }
                    };
                    b.open("GET", c, true);
                    b.send(null);
                }
            } else {
                this.error("Unknown data format: " + (typeof c));
            }
        }
    }
};
Dygraph.prototype.updateOptions = function (e, b) {
    if (typeof (b) == "undefined") {
        b = false;
    }
    var d = e.file;
    var c = Dygraph.mapLegacyOptions_(e);
    if ("rollPeriod" in c) {
        this.rollPeriod_ = c.rollPeriod;
    }
    if ("dateWindow" in c) {
        this.dateWindow_ = c.dateWindow;
        if (!("isZoomedIgnoreProgrammaticZoom" in c)) {
            this.zoomed_x_ = (c.dateWindow !== null);
        }
    }
    if ("valueRange" in c && !("isZoomedIgnoreProgrammaticZoom" in c)) {
        this.zoomed_y_ = (c.valueRange !== null);
    }
    var a = Dygraph.isPixelChangingOptionList(this.attr_("labels"), c);
    Dygraph.updateDeep(this.user_attrs_, c);
    if (d) {
        this.file_ = d;
        if (!b) {
            this.start_();
        }
    } else {
        if (!b) {
            if (a) {
                this.predraw_();
            } else {
                this.renderGraph_(false, false);
            }
        }
    }
};
Dygraph.mapLegacyOptions_ = function (c) {
    var a = {};
    for (var b in c) {
        if (b == "file") {
            continue;
        }
        if (c.hasOwnProperty(b)) {
            a[b] = c[b];
        }
    }
    var e = function (g, f, h) {
        if (!a.axes) {
            a.axes = {};
        }
        if (!a.axes[g]) {
            a.axes[g] = {};
        }
        a.axes[g][f] = h;
    };
    var d = function (f, g, h) {
        if (typeof (c[f]) != "undefined") {
            e(g, h, c[f]);
            delete a[f];
        }
    };
    d("xValueFormatter", "x", "valueFormatter");
    d("pixelsPerXLabel", "x", "pixelsPerLabel");
    d("xAxisLabelFormatter", "x", "axisLabelFormatter");
    d("xTicker", "x", "ticker");
    d("yValueFormatter", "y", "valueFormatter");
    d("pixelsPerYLabel", "y", "pixelsPerLabel");
    d("yAxisLabelFormatter", "y", "axisLabelFormatter");
    d("yTicker", "y", "ticker");
    return a;
};
Dygraph.prototype.resize = function (d, b) {
    if (this.resize_lock) {
        return;
    }
    this.resize_lock = true;
    if ((d === null) != (b === null)) {
        this.warn("Dygraph.resize() should be called with zero parameters or two non-NULL parameters. Pretending it was zero.");
        d = b = null;
    }
    var a = this.width_;
    var c = this.height_;
    if (d) {
        this.maindiv_.style.width = d + "px";
        this.maindiv_.style.height = b + "px";
        this.width_ = d;
        this.height_ = b;
    } else {
        this.width_ = this.maindiv_.clientWidth;
        this.height_ = this.maindiv_.clientHeight;
    }
    if (a != this.width_ || c != this.height_) {
        this.maindiv_.innerHTML = "";
        this.roller_ = null;
        this.attrs_.labelsDiv = null;
        this.createInterface_();
        if (this.annotations_.length) {
            this.layout_.setAnnotations(this.annotations_);
        }
        this.predraw_();
    }
    this.resize_lock = false;
};
Dygraph.prototype.adjustRoll = function (a) {
    this.rollPeriod_ = a;
    this.predraw_();
};
Dygraph.prototype.visibility = function () {
    if (!this.attr_("visibility")) {
        this.attrs_.visibility = [];
    }
    while (this.attr_("visibility").length < this.numColumns() - 1) {
        this.attrs_.visibility.push(true);
    }
    return this.attr_("visibility");
};
Dygraph.prototype.setVisibility = function (b, c) {
    var a = this.visibility();
    if (b < 0 || b >= a.length) {
        this.warn("invalid series number in setVisibility: " + b);
    } else {
        a[b] = c;
        this.predraw_();
    }
};
Dygraph.prototype.size = function () { return { width: this.width_, height: this.height_ }; };
Dygraph.prototype.setAnnotations = function (b, a) {
    Dygraph.addAnnotationRule();
    this.annotations_ = b;
    this.layout_.setAnnotations(this.annotations_);
    if (!a) {
        this.predraw_();
    }
};
Dygraph.prototype.annotations = function () { return this.annotations_; };
Dygraph.prototype.indexFromSetName = function (a) {
    var c = this.attr_("labels");
    for (var b = 0; b < c.length; b++) {
        if (c[b] == a) {
            return b;
        }
    }
    return null;
};
Dygraph.addAnnotationRule = function () {
    if (Dygraph.addedAnnotationCSS) {
        return;
    }
    var f = "border: 1px solid black; background-color: white; text-align: center;";
    var e = document.createElement("style");
    e.type = "text/css";
    document.getElementsByTagName("head")[0].appendChild(e);
    for (var b = 0; b < document.styleSheets.length; b++) {
        if (document.styleSheets[b].disabled) {
            continue;
        }
        var d = document.styleSheets[b];
        try {
            if (d.insertRule) {
                var a = d.cssRules ? d.cssRules.length : 0;
                d.insertRule(".dygraphDefaultAnnotation { " + f + " }", a);
            } else {
                if (d.addRule) {
                    d.addRule(".dygraphDefaultAnnotation", f);
                }
            }
            Dygraph.addedAnnotationCSS = true;
            return;
        } catch (c) {
        }
    }
    this.warn("Unable to add default annotation CSS rule; display may be off.");
};
var DateGraph = Dygraph;
"use strict";
Dygraph.LOG_SCALE = 10;
Dygraph.LN_TEN = Math.log(Dygraph.LOG_SCALE);
Dygraph.log10 = function (a) { return Math.log(a) / Dygraph.LN_TEN; };
Dygraph.DEBUG = 1;
Dygraph.INFO = 2;
Dygraph.WARNING = 3;
Dygraph.ERROR = 3;
Dygraph.LOG_STACK_TRACES = false;
Dygraph.DOTTED_LINE = [2, 2];
Dygraph.DASHED_LINE = [7, 3];
Dygraph.DOT_DASH_LINE = [7, 2, 2, 2];
Dygraph.log = function (b, d) {
    var a;
    if (typeof (printStackTrace) != "undefined") {
        a = printStackTrace({ guess: false });
        while (a[0].indexOf("stacktrace") != -1) {
            a.splice(0, 1);
        }
        a.splice(0, 2);
        for (var c = 0; c < a.length; c++) {
            a[c] = a[c].replace(/\([^)]*\/(.*)\)/, "@$1").replace(/\@.*\/([^\/]*)/, "@$1").replace("[object Object].", "");
        }
        var e = a.splice(0, 1)[0];
        d += " (" + e.replace(/^.*@ ?/, "") + ")";
    }
    if (typeof (console) != "undefined") {
        switch (b) {
            case Dygraph.DEBUG:
                console.debug("dygraphs: " + d);
                break;
            case Dygraph.INFO:
                console.info("dygraphs: " + d);
                break;
            case Dygraph.WARNING:
                console.warn("dygraphs: " + d);
                break;
            case Dygraph.ERROR:
                console.error("dygraphs: " + d);
                break;
        }
    }
    if (Dygraph.LOG_STACK_TRACES) {
        console.log(a.join("\n"));
    }
};
Dygraph.info = function (a) { Dygraph.log(Dygraph.INFO, a); };
Dygraph.prototype.info = Dygraph.info;
Dygraph.warn = function (a) { Dygraph.log(Dygraph.WARNING, a); };
Dygraph.prototype.warn = Dygraph.warn;
Dygraph.error = function (a) { Dygraph.log(Dygraph.ERROR, a); };
Dygraph.prototype.error = Dygraph.error;
Dygraph.getContext = function (a) { return a.getContext("2d"); };
Dygraph.addEvent = function addEvent(c, b, a) {
    if (c.addEventListener) {
        c.addEventListener(b, a, false);
    } else {
        c[b + a] = function () { a(window.event); };
        c.attachEvent("on" + b, c[b + a]);
    }
};
Dygraph.removeEvent = function addEvent(c, b, a) {
    if (c.removeEventListener) {
        c.removeEventListener(b, a, false);
    } else {
        c.detachEvent("on" + b, c[b + a]);
        c[b + a] = null;
    }
};
Dygraph.cancelEvent = function (a) {
    a = a ? a : window.event;
    if (a.stopPropagation) {
        a.stopPropagation();
    }
    if (a.preventDefault) {
        a.preventDefault();
    }
    a.cancelBubble = true;
    a.cancel = true;
    a.returnValue = false;
    return false;
};
Dygraph.hsvToRGB = function (h, g, k) {
    var c;
    var d;
    var l;
    if (g === 0) {
        c = k;
        d = k;
        l = k;
    } else {
        var e = Math.floor(h * 6);
        var j = (h * 6) - e;
        var b = k * (1 - g);
        var a = k * (1 - (g * j));
        var m = k * (1 - (g * (1 - j)));
        switch (e) {
            case 1:
                c = a;
                d = k;
                l = b;
                break;
            case 2:
                c = b;
                d = k;
                l = m;
                break;
            case 3:
                c = b;
                d = a;
                l = k;
                break;
            case 4:
                c = m;
                d = b;
                l = k;
                break;
            case 5:
                c = k;
                d = b;
                l = a;
                break;
            case 6:
            case 0:
                c = k;
                d = m;
                l = b;
                break;
        }
    }
    c = Math.floor(255 * c + 0.5);
    d = Math.floor(255 * d + 0.5);
    l = Math.floor(255 * l + 0.5);
    return "rgb(" + c + "," + d + "," + l + ")";
};
Dygraph.findPosX = function (b) {
    var c = 0;
    if (b.offsetParent) {
        var a = b;
        while (1) {
            c += a.offsetLeft;
            if (!a.offsetParent) {
                break;
            }
            a = a.offsetParent;
        }
    } else {
        if (b.x) {
            c += b.x;
        }
    }
    while (b && b != document.body) {
        c -= b.scrollLeft;
        b = b.parentNode;
    }
    return c;
};
Dygraph.findPosY = function (c) {
    var b = 0;
    if (c.offsetParent) {
        var a = c;
        while (1) {
            b += a.offsetTop;
            if (!a.offsetParent) {
                break;
            }
            a = a.offsetParent;
        }
    } else {
        if (c.y) {
            b += c.y;
        }
    }
    while (c && c != document.body) {
        b -= c.scrollTop;
        c = c.parentNode;
    }
    return b;
};
Dygraph.pageX = function (c) {
    if (c.pageX) {
        return (!c.pageX || c.pageX < 0) ? 0 : c.pageX;
    } else {
        var d = document;
        var a = document.body;
        return c.clientX + (d.scrollLeft || a.scrollLeft) - (d.clientLeft || 0);
    }
};
Dygraph.pageY = function (c) {
    if (c.pageY) {
        return (!c.pageY || c.pageY < 0) ? 0 : c.pageY;
    } else {
        var d = document;
        var a = document.body;
        return c.clientY + (d.scrollTop || a.scrollTop) - (d.clientTop || 0);
    }
};
Dygraph.isOK = function (a) { return a && !isNaN(a); };
Dygraph.floatFormat = function (a, b) {
    var c = Math.min(Math.max(1, b || 2), 21);
    return (Math.abs(a) < 0.001 && a !== 0) ? a.toExponential(c - 1) : a.toPrecision(c);
};
Dygraph.zeropad = function (a) {
    if (a < 10) {
        return "0" + a;
    } else {
        return "" + a;
    }
};
Dygraph.hmsString_ = function (a) {
    var c = Dygraph.zeropad;
    var b = new Date(a);
    if (b.getSeconds()) {
        return c(b.getHours()) + ":" + c(b.getMinutes()) + ":" + c(b.getSeconds());
    } else {
        return c(b.getHours()) + ":" + c(b.getMinutes());
    }
};
Dygraph.round_ = function (c, b) {
    var a = Math.pow(10, b);
    return Math.round(c * a) / a;
};
Dygraph.binarySearch = function (a, d, i, e, b) {
    if (e === null || e === undefined || b === null || b === undefined) {
        e = 0;
        b = d.length - 1;
    }
    if (e > b) {
        return -1;
    }
    if (i === null || i === undefined) {
        i = 0;
    }
    var h = function (j) { return j >= 0 && j < d.length; };
    var g = parseInt((e + b) / 2, 10);
    var c = d[g];
    if (c == a) {
        return g;
    }
    var f;
    if (c > a) {
        if (i > 0) {
            f = g - 1;
            if (h(f) && d[f] < a) {
                return g;
            }
        }
        return Dygraph.binarySearch(a, d, i, e, g - 1);
    }
    if (c < a) {
        if (i < 0) {
            f = g + 1;
            if (h(f) && d[f] > a) {
                return g;
            }
        }
        return Dygraph.binarySearch(a, d, i, g + 1, b);
    }
};
Dygraph.dateParser = function (a) {
    var b;
    var c;
    c = Dygraph.dateStrToMillis(a);
    if (c && !isNaN(c)) {
        return c;
    }
    if (a.search("-") != -1) {
        b = a.replace("-", "/", "g");
        while (b.search("-") != -1) {
            b = b.replace("-", "/");
        }
        c = Dygraph.dateStrToMillis(b);
    } else {
        if (a.length == 8) {
            b = a.substr(0, 4) + "/" + a.substr(4, 2) + "/" + a.substr(6, 2);
            c = Dygraph.dateStrToMillis(b);
        } else {
            c = Dygraph.dateStrToMillis(a);
        }
    }
    if (!c || isNaN(c)) {
        Dygraph.error("Couldn't parse " + a + " as a date");
    }
    return c;
};
Dygraph.dateStrToMillis = function (a) { return new Date(a).getTime(); };
Dygraph.update = function (b, c) {
    if (typeof (c) != "undefined" && c !== null) {
        for (var a in c) {
            if (c.hasOwnProperty(a)) {
                b[a] = c[a];
            }
        }
    }
    return b;
};
Dygraph.updateDeep = function (b, d) {

    function c(e) {
        return (typeof Node === "object" ? e instanceof Node : typeof e === "object" && typeof e.nodeType === "number" && typeof e.nodeName === "string");
    }

    if (typeof (d) != "undefined" && d !== null) {
        for (var a in d) {
            if (d.hasOwnProperty(a)) {
                if (d[a] === null) {
                    b[a] = null;
                } else {
                    if (Dygraph.isArrayLike(d[a])) {
                        b[a] = d[a].slice();
                    } else {
                        if (c(d[a])) {
                            b[a] = d[a];
                        } else {
                            if (typeof (d[a]) == "object") {
                                if (typeof (b[a]) != "object") {
                                    b[a] = {};
                                }
                                Dygraph.updateDeep(b[a], d[a]);
                            } else {
                                b[a] = d[a];
                            }
                        }
                    }
                }
            }
        }
    }
    return b;
};
Dygraph.isArrayLike = function (b) {
    var a = typeof (b);
    if ((a != "object" && !(a == "function" && typeof (b.item) == "function")) || b === null || typeof (b.length) != "number" || b.nodeType === 3) {
        return false;
    }
    return true;
};
Dygraph.isDateLike = function (a) {
    if (typeof (a) != "object" || a === null || typeof (a.getTime) != "function") {
        return false;
    }
    return true;
};
Dygraph.clone = function (c) {
    var b = [];
    for (var a = 0; a < c.length; a++) {
        if (Dygraph.isArrayLike(c[a])) {
            b.push(Dygraph.clone(c[a]));
        } else {
            b.push(c[a]);
        }
    }
    return b;
};
Dygraph.createCanvas = function () {
    var a = document.createElement("canvas");
    var b = (/MSIE/.test(navigator.userAgent) && !window.opera);
    if (b && (typeof (G_vmlCanvasManager) != "undefined")) {
        a = G_vmlCanvasManager.initElement(a);
    }
    return a;
};
Dygraph.isAndroid = function () { return (/Android/).test(navigator.userAgent); };
Dygraph.repeatAndCleanup = function (b, g, f, c) {
    var e = 0;
    var d = new Date().getTime();
    b(e);
    if (g == 1) {
        c();
        return;
    }
    (function a() {
        if (e >= g) {
            return;
        }
        var h = d + (1 + e) * f;
        setTimeout(function () {
            e++;
            b(e);
            if (e >= g - 1) {
                c();
            } else {
                a();
            }
        }, h - new Date().getTime());
    })();
};
Dygraph.isPixelChangingOptionList = function (h, e) {
    var d = { annotationClickHandler: true, annotationDblClickHandler: true, annotationMouseOutHandler: true, annotationMouseOverHandler: true, axisLabelColor: true, axisLineColor: true, axisLineWidth: true, clickCallback: true, digitsAfterDecimal: true, drawCallback: true, drawPoints: true, drawXGrid: true, drawYGrid: true, fillAlpha: true, gridLineColor: true, gridLineWidth: true, hideOverlayOnMouseOut: true, highlightCallback: true, highlightCircleSize: true, interactionModel: true, isZoomedIgnoreProgrammaticZoom: true, labelsDiv: true, labelsDivStyles: true, labelsDivWidth: true, labelsKMB: true, labelsKMG2: true, labelsSeparateLines: true, labelsShowZeroValues: true, legend: true, maxNumberWidth: true, panEdgeFraction: true, pixelsPerYLabel: true, pointClickCallback: true, pointSize: true, rangeSelectorPlotFillColor: true, rangeSelectorPlotStrokeColor: true, showLabelsOnHighlight: true, showRoller: true, sigFigs: true, strokeWidth: true, underlayCallback: true, unhighlightCallback: true, xAxisLabelFormatter: true, xTicker: true, xValueFormatter: true, yAxisLabelFormatter: true, yValueFormatter: true, zoomCallback: true };
    var a = false;
    var b = {};
    if (h) {
        for (var f = 1; f < h.length; f++) {
            b[h[f]] = true;
        }
    }
    for (var g in e) {
        if (a) {
            break;
        }
        if (e.hasOwnProperty(g)) {
            if (b[g]) {
                for (var c in e[g]) {
                    if (a) {
                        break;
                    }
                    if (e[g].hasOwnProperty(c) && !d[c]) {
                        a = true;
                    }
                }
            } else {
                if (!d[g]) {
                    a = true;
                }
            }
        }
    }
    return a;
};
Dygraph.compareArrays = function (c, b) {
    if (!Dygraph.isArrayLike(c) || !Dygraph.isArrayLike(b)) {
        return false;
    }
    if (c.length !== b.length) {
        return false;
    }
    for (var a = 0; a < c.length; a++) {
        if (c[a] !== b[a]) {
            return false;
        }
    }
    return true;
};
"use strict";
Dygraph.GVizChart = function (a) { this.container = a; };
Dygraph.GVizChart.prototype.draw = function (b, a) {
    this.container.innerHTML = "";
    if (typeof (this.date_graph) != "undefined") {
        this.date_graph.destroy();
    }
    this.date_graph = new Dygraph(this.container, b, a);
};
Dygraph.GVizChart.prototype.setSelection = function (b) {
    var a = false;
    if (b.length) {
        a = b[0].row;
    }
    this.date_graph.setSelection(a);
};
Dygraph.GVizChart.prototype.getSelection = function () {
    var d = [];
    var e = this.date_graph.getSelection();
    if (e < 0) {
        return d;
    }
    var b = 1;
    var c = this.date_graph.layout_.datasets;
    for (var a in c) {
        if (!c.hasOwnProperty(a)) {
            continue;
        }
        d.push({ row: e, column: b });
        b++;
    }
    return d;
};
"use strict";
Dygraph.Interaction = {};
Dygraph.Interaction.startPan = function (n, s, c) {
    var q, b;
    c.isPanning = true;
    var j = s.xAxisRange();
    c.dateRange = j[1] - j[0];
    c.initialLeftmostDate = j[0];
    c.xUnitsPerPixel = c.dateRange / (s.plotter_.area.w - 1);
    if (s.attr_("panEdgeFraction")) {
        var v = s.width_ * s.attr_("panEdgeFraction");
        var d = s.xAxisExtremes();
        var h = s.toDomXCoord(d[0]) - v;
        var k = s.toDomXCoord(d[1]) + v;
        var t = s.toDataXCoord(h);
        var u = s.toDataXCoord(k);
        c.boundedDates = [t, u];
        var f = [];
        var a = s.height_ * s.attr_("panEdgeFraction");
        for (q = 0; q < s.axes_.length; q++) {
            b = s.axes_[q];
            var o = b.extremeRange;
            var p = s.toDomYCoord(o[0], q) + a;
            var r = s.toDomYCoord(o[1], q) - a;
            var m = s.toDataYCoord(p);
            var e = s.toDataYCoord(r);
            f[q] = [m, e];
        }
        c.boundedValues = f;
    }
    c.is2DPan = false;
    for (q = 0; q < s.axes_.length; q++) {
        b = s.axes_[q];
        var l = s.yAxisRange(q);
        if (b.logscale) {
            b.initialTopValue = Dygraph.log10(l[1]);
            b.dragValueRange = Dygraph.log10(l[1]) - Dygraph.log10(l[0]);
        } else {
            b.initialTopValue = l[1];
            b.dragValueRange = l[1] - l[0];
        }
        b.unitsPerPixel = b.dragValueRange / (s.plotter_.area.h - 1);
        if (b.valueWindow || b.valueRange) {
            c.is2DPan = true;
        }
    }
};
Dygraph.Interaction.movePan = function (b, k, c) {
    c.dragEndX = k.dragGetX_(b, c);
    c.dragEndY = k.dragGetY_(b, c);
    var h = c.initialLeftmostDate - (c.dragEndX - c.dragStartX) * c.xUnitsPerPixel;
    if (c.boundedDates) {
        h = Math.max(h, c.boundedDates[0]);
    }
    var a = h + c.dateRange;
    if (c.boundedDates) {
        if (a > c.boundedDates[1]) {
            h = h - (a - c.boundedDates[1]);
            a = h + c.dateRange;
        }
    }
    k.dateWindow_ = [h, a];
    if (c.is2DPan) {
        for (var j = 0; j < k.axes_.length; j++) {
            var e = k.axes_[j];
            var d = c.dragEndY - c.dragStartY;
            var n = d * e.unitsPerPixel;
            var f = c.boundedValues ? c.boundedValues[j] : null;
            var l = e.initialTopValue + n;
            if (f) {
                l = Math.min(l, f[1]);
            }
            var m = l - e.dragValueRange;
            if (f) {
                if (m < f[0]) {
                    l = l - (m - f[0]);
                    m = l - e.dragValueRange;
                }
            }
            if (e.logscale) {
                e.valueWindow = [Math.pow(Dygraph.LOG_SCALE, m), Math.pow(Dygraph.LOG_SCALE, l)];
            } else {
                e.valueWindow = [m, l];
            }
        }
    }
    k.drawGraph_(false);
};
Dygraph.Interaction.endPan = function (c, b, a) {
    a.dragEndX = b.dragGetX_(c, a);
    a.dragEndY = b.dragGetY_(c, a);
    var e = Math.abs(a.dragEndX - a.dragStartX);
    var d = Math.abs(a.dragEndY - a.dragStartY);
    if (e < 2 && d < 2 && b.lastx_ !== undefined && b.lastx_ != -1) {
        Dygraph.Interaction.treatMouseOpAsClick(b, c, a);
    }
    a.isPanning = false;
    a.is2DPan = false;
    a.initialLeftmostDate = null;
    a.dateRange = null;
    a.valueRange = null;
    a.boundedDates = null;
    a.boundedValues = null;
};
Dygraph.Interaction.startZoom = function (c, b, a) { a.isZooming = true; };
Dygraph.Interaction.moveZoom = function (c, b, a) {
    a.dragEndX = b.dragGetX_(c, a);
    a.dragEndY = b.dragGetY_(c, a);
    var e = Math.abs(a.dragStartX - a.dragEndX);
    var d = Math.abs(a.dragStartY - a.dragEndY);
    a.dragDirection = (e < d / 2) ? Dygraph.VERTICAL : Dygraph.HORIZONTAL;
    b.drawZoomRect_(a.dragDirection, a.dragStartX, a.dragEndX, a.dragStartY, a.dragEndY, a.prevDragDirection, a.prevEndX, a.prevEndY);
    a.prevEndX = a.dragEndX;
    a.prevEndY = a.dragEndY;
    a.prevDragDirection = a.dragDirection;
};
Dygraph.Interaction.treatMouseOpAsClick = function (f, b, d) {
    var k = f.attr_("clickCallback");
    var n = f.attr_("pointClickCallback");
    var j = null;
    if (n) {
        var l = -1;
        var m = Number.MAX_VALUE;
        for (var e = 0; e < f.selPoints_.length; e++) {
            var c = f.selPoints_[e];
            var a = Math.pow(c.canvasx - d.dragEndX, 2) + Math.pow(c.canvasy - d.dragEndY, 2);
            if (!isNaN(a) && (l == -1 || a < m)) {
                m = a;
                l = e;
            }
        }
        var h = f.attr_("highlightCircleSize") + 2;
        if (m <= h * h) {
            j = f.selPoints_[l];
        }
    }
    if (j) {
        n(b, j);
    }
    if (k) {
        k(b, f.lastx_, f.selPoints_);
    }
};
Dygraph.Interaction.endZoom = function (c, b, a) {
    a.isZooming = false;
    a.dragEndX = b.dragGetX_(c, a);
    a.dragEndY = b.dragGetY_(c, a);
    var e = Math.abs(a.dragEndX - a.dragStartX);
    var d = Math.abs(a.dragEndY - a.dragStartY);
    if (e < 2 && d < 2 && b.lastx_ !== undefined && b.lastx_ != -1) {
        Dygraph.Interaction.treatMouseOpAsClick(b, c, a);
    }
    if (e >= 10 && a.dragDirection == Dygraph.HORIZONTAL) {
        b.doZoomX_(Math.min(a.dragStartX, a.dragEndX), Math.max(a.dragStartX, a.dragEndX));
    } else {
        if (d >= 10 && a.dragDirection == Dygraph.VERTICAL) {
            b.doZoomY_(Math.min(a.dragStartY, a.dragEndY), Math.max(a.dragStartY, a.dragEndY));
        } else {
            b.clearZoomRect_();
        }
    }
    a.dragStartX = null;
    a.dragStartY = null;
};
Dygraph.Interaction.defaultModel = {
    mousedown: function (c, b, a) {
        a.initializeMouseDown(c, b, a);
        if (c.altKey || c.shiftKey) {
            Dygraph.startPan(c, b, a);
        } else {
            Dygraph.startZoom(c, b, a);
        }
    },
    mousemove: function (c, b, a) {
        if (a.isZooming) {
            Dygraph.moveZoom(c, b, a);
        } else {
            if (a.isPanning) {
                Dygraph.movePan(c, b, a);
            }
        }
    },
    mouseup: function (c, b, a) {
        if (a.isZooming) {
            Dygraph.endZoom(c, b, a);
        } else {
            if (a.isPanning) {
                Dygraph.endPan(c, b, a);
            }
        }
    },
    mouseout: function (c, b, a) {
        if (a.isZooming) {
            a.dragEndX = null;
            a.dragEndY = null;
        }
    },
    dblclick: function (c, b, a) {
        if (c.altKey || c.shiftKey) {
            return;
        }
        b.doUnzoom_();
    }
};
Dygraph.DEFAULT_ATTRS.interactionModel = Dygraph.Interaction.defaultModel;
Dygraph.defaultInteractionModel = Dygraph.Interaction.defaultModel;
Dygraph.endZoom = Dygraph.Interaction.endZoom;
Dygraph.moveZoom = Dygraph.Interaction.moveZoom;
Dygraph.startZoom = Dygraph.Interaction.startZoom;
Dygraph.endPan = Dygraph.Interaction.endPan;
Dygraph.movePan = Dygraph.Interaction.movePan;
Dygraph.startPan = Dygraph.Interaction.startPan;
Dygraph.Interaction.nonInteractiveModel_ = {
    mousedown: function (c, b, a) { a.initializeMouseDown(c, b, a); },
    mouseup: function (c, b, a) {
        a.dragEndX = b.dragGetX_(c, a);
        a.dragEndY = b.dragGetY_(c, a);
        var e = Math.abs(a.dragEndX - a.dragStartX);
        var d = Math.abs(a.dragEndY - a.dragStartY);
        if (e < 2 && d < 2 && b.lastx_ !== undefined && b.lastx_ != -1) {
            Dygraph.Interaction.treatMouseOpAsClick(b, c, a);
        }
    }
};
Dygraph.Interaction.dragIsPanInteractionModel = {
    mousedown: function (c, b, a) {
        a.initializeMouseDown(c, b, a);
        Dygraph.startPan(c, b, a);
    },
    mousemove: function (c, b, a) {
        if (a.isPanning) {
            Dygraph.movePan(c, b, a);
        }
    },
    mouseup: function (c, b, a) {
        if (a.isPanning) {
            Dygraph.endPan(c, b, a);
        }
    }
};
"use strict";
var DygraphRangeSelector = function (a) {
    this.isIE_ = /MSIE/.test(navigator.userAgent) && !window.opera;
    this.isUsingExcanvas_ = a.isUsingExcanvas_;
    this.dygraph_ = a;
    this.createCanvases_();
    if (this.isUsingExcanvas_) {
        this.createIEPanOverlay_();
    }
    this.createZoomHandles_();
    this.initInteraction_();
};
DygraphRangeSelector.prototype.addToGraph = function (a, b) {
    this.layout_ = b;
    this.resize_();
    a.appendChild(this.bgcanvas_);
    a.appendChild(this.fgcanvas_);
    a.appendChild(this.leftZoomHandle_);
    a.appendChild(this.rightZoomHandle_);
};
DygraphRangeSelector.prototype.renderStaticLayer = function () {
    this.resize_();
    this.drawStaticLayer_();
};
DygraphRangeSelector.prototype.renderInteractiveLayer = function () {
    if (this.isChangingRange_) {
        return;
    }
    this.placeZoomHandles_();
    this.drawInteractiveLayer_();
};
DygraphRangeSelector.prototype.resize_ = function () {

    function c(d, e) {
        d.style.top = e.y + "px";
        d.style.left = e.x + "px";
        d.width = e.w;
        d.height = e.h;
        d.style.width = d.width + "px";
        d.style.height = d.height + "px";
    }

    var b = this.layout_.getPlotArea();
    var a = this.attr_("axisLabelFontSize") + 2 * this.attr_("axisTickSize");
    this.canvasRect_ = { x: b.x, y: b.y + b.h + a + 4, w: b.w, h: this.attr_("rangeSelectorHeight") };
    c(this.bgcanvas_, this.canvasRect_);
    c(this.fgcanvas_, this.canvasRect_);
};
DygraphRangeSelector.prototype.attr_ = function (a) { return this.dygraph_.attr_(a); };
DygraphRangeSelector.prototype.createCanvases_ = function () {
    this.bgcanvas_ = Dygraph.createCanvas();
    this.bgcanvas_.className = "dygraph-rangesel-bgcanvas";
    this.bgcanvas_.style.position = "absolute";
    this.bgcanvas_.style.zIndex = 9;
    this.bgcanvas_ctx_ = Dygraph.getContext(this.bgcanvas_);
    this.fgcanvas_ = Dygraph.createCanvas();
    this.fgcanvas_.className = "dygraph-rangesel-fgcanvas";
    this.fgcanvas_.style.position = "absolute";
    this.fgcanvas_.style.zIndex = 9;
    this.fgcanvas_.style.cursor = "default";
    this.fgcanvas_ctx_ = Dygraph.getContext(this.fgcanvas_);
};
DygraphRangeSelector.prototype.createIEPanOverlay_ = function () {
    this.iePanOverlay_ = document.createElement("div");
    this.iePanOverlay_.style.position = "absolute";
    this.iePanOverlay_.style.backgroundColor = "white";
    this.iePanOverlay_.style.filter = "alpha(opacity=0)";
    this.iePanOverlay_.style.display = "none";
    this.iePanOverlay_.style.cursor = "move";
    this.fgcanvas_.appendChild(this.iePanOverlay_);
};
DygraphRangeSelector.prototype.createZoomHandles_ = function () {
    var a = new Image();
    a.className = "dygraph-rangesel-zoomhandle";
    a.style.position = "absolute";
    a.style.zIndex = 10;
    a.style.visibility = "hidden";
    a.style.cursor = "col-resize";
    if (/MSIE 7/.test(navigator.userAgent)) {
        a.width = 7;
        a.height = 14;
        a.style.backgroundColor = "white";
        a.style.border = "1px solid #333333";
    } else {
        a.width = 9;
        a.height = 16;
        a.src = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAkAAAAQCAYAAADESFVDAAAAAXNSR0IArs4c6QAAAAZiS0dEANAAzwDP4Z7KegAAAAlwSFlzAAAOxAAADsQBlSsOGwAAAAd0SU1FB9sHGw0cMqdt1UwAAAAZdEVYdENvbW1lbnQAQ3JlYXRlZCB3aXRoIEdJTVBXgQ4XAAAAaElEQVQoz+3SsRFAQBCF4Z9WJM8KCDVwownl6YXsTmCUsyKGkZzcl7zkz3YLkypgAnreFmDEpHkIwVOMfpdi9CEEN2nGpFdwD03yEqDtOgCaun7sqSTDH32I1pQA2Pb9sZecAxc5r3IAb21d6878xsAAAAAASUVORK5CYII=";
    }
    this.leftZoomHandle_ = a;
    this.rightZoomHandle_ = a.cloneNode(false);
};
DygraphRangeSelector.prototype.initInteraction_ = function () {
    var i = this;
    var f = this.isIE_ ? document : window;
    var k = 0;
    var p = null;
    var n = false;
    var c = false;
    var j, d, m, g, q, e, r, o, l, b, h;
    j = function (w) {
        var v = i.dygraph_.xAxisExtremes();
        var t = (v[1] - v[0]) / i.canvasRect_.w;
        var u = v[0] + (w.leftHandlePos - i.canvasRect_.x) * t;
        var s = v[0] + (w.rightHandlePos - i.canvasRect_.x) * t;
        return [u, s];
    };
    d = function (s) {
        Dygraph.cancelEvent(s);
        n = true;
        k = s.screenX;
        p = s.target ? s.target : s.srcElement;
        Dygraph.addEvent(f, "mousemove", m);
        Dygraph.addEvent(f, "mouseup", g);
        i.fgcanvas_.style.cursor = "col-resize";
    };
    m = function (w) {
        if (!n) {
            return;
        }
        var t = w.screenX - k;
        if (Math.abs(t) < 4) {
            return;
        }
        k = w.screenX;
        var v = i.getZoomHandleStatus_();
        var s;
        if (p == i.leftZoomHandle_) {
            s = v.leftHandlePos + t;
            s = Math.min(s, v.rightHandlePos - p.width - 3);
            s = Math.max(s, i.canvasRect_.x);
        } else {
            s = v.rightHandlePos + t;
            s = Math.min(s, i.canvasRect_.x + i.canvasRect_.w);
            s = Math.max(s, v.leftHandlePos + p.width + 3);
        }
        var u = p.width / 2;
        p.style.left = (s - u) + "px";
        i.drawInteractiveLayer_();
        if (!i.isUsingExcanvas_) {
            q();
        }
    };
    g = function (s) {
        if (!n) {
            return;
        }
        n = false;
        Dygraph.removeEvent(f, "mousemove", m);
        Dygraph.removeEvent(f, "mouseup", g);
        i.fgcanvas_.style.cursor = "default";
        if (i.isUsingExcanvas_) {
            q();
        }
    };
    q = function () {
        try {
            var t = i.getZoomHandleStatus_();
            i.isChangingRange_ = true;
            if (!t.isZoomed) {
                i.dygraph_.doUnzoom_();
            } else {
                var s = j(t);
                i.dygraph_.doZoomXDates_(s[0], s[1]);
            }
        } finally {
            i.isChangingRange_ = false;
        }
    };
    e = function (u) {
        if (i.isUsingExcanvas_) {
            return u.srcElement == i.iePanOverlay_;
        } else {
            var s;
            if (u.offsetX != undefined) {
                s = i.canvasRect_.x + u.offsetX;
            } else {
                s = u.clientX;
            }
            var t = i.getZoomHandleStatus_();
            return (s > t.leftHandlePos && s < t.rightHandlePos);
        }
    };
    r = function (s) {
        if (!c && e(s) && i.getZoomHandleStatus_().isZoomed) {
            Dygraph.cancelEvent(s);
            c = true;
            k = s.screenX;
            Dygraph.addEvent(f, "mousemove", o);
            Dygraph.addEvent(f, "mouseup", l);
        }
    };
    o = function (w) {
        if (!c) {
            return;
        }
        Dygraph.cancelEvent(w);
        var t = w.screenX - k;
        if (Math.abs(t) < 4) {
            return;
        }
        k = w.screenX;
        var v = i.getZoomHandleStatus_();
        var y = v.leftHandlePos;
        var s = v.rightHandlePos;
        var x = s - y;
        if (y + t <= i.canvasRect_.x) {
            y = i.canvasRect_.x;
            s = y + x;
        } else {
            if (s + t >= i.canvasRect_.x + i.canvasRect_.w) {
                s = i.canvasRect_.x + i.canvasRect_.w;
                y = s - x;
            } else {
                y += t;
                s += t;
            }
        }
        var u = i.leftZoomHandle_.width / 2;
        i.leftZoomHandle_.style.left = (y - u) + "px";
        i.rightZoomHandle_.style.left = (s - u) + "px";
        i.drawInteractiveLayer_();
        if (!i.isUsingExcanvas_) {
            b();
        }
    };
    l = function (s) {
        if (!c) {
            return;
        }
        c = false;
        Dygraph.removeEvent(f, "mousemove", o);
        Dygraph.removeEvent(f, "mouseup", l);
        if (i.isUsingExcanvas_) {
            b();
        }
    };
    b = function () {
        try {
            i.isChangingRange_ = true;
            i.dygraph_.dateWindow_ = j(i.getZoomHandleStatus_());
            i.dygraph_.drawGraph_(false);
        } finally {
            i.isChangingRange_ = false;
        }
    };
    h = function (s) {
        if (n || c) {
            return;
        }
        var t = e(s) ? "move" : "default";
        if (t != i.fgcanvas_.style.cursor) {
            i.fgcanvas_.style.cursor = t;
        }
    };
    this.dygraph_.attrs_.interactionModel = Dygraph.Interaction.dragIsPanInteractionModel;
    this.dygraph_.attrs_.panEdgeFraction = 0.0001;
    var a = window.opera ? "mousedown" : "dragstart";
    Dygraph.addEvent(this.leftZoomHandle_, a, d);
    Dygraph.addEvent(this.rightZoomHandle_, a, d);
    if (this.isUsingExcanvas_) {
        Dygraph.addEvent(this.iePanOverlay_, "mousedown", r);
    } else {
        Dygraph.addEvent(this.fgcanvas_, "mousedown", r);
        Dygraph.addEvent(this.fgcanvas_, "mousemove", h);
    }
};
DygraphRangeSelector.prototype.drawStaticLayer_ = function () {
    var a = this.bgcanvas_ctx_;
    a.clearRect(0, 0, this.canvasRect_.w, this.canvasRect_.h);
    try {
        this.drawMiniPlot_();
    } catch (b) {
        Dygraph.warn(b);
    }
    var c = 0.5;
    this.bgcanvas_ctx_.lineWidth = 1;
    a.strokeStyle = "gray";
    a.beginPath();
    a.moveTo(c, c);
    a.lineTo(c, this.canvasRect_.h - c);
    a.lineTo(this.canvasRect_.w - c, this.canvasRect_.h - c);
    a.lineTo(this.canvasRect_.w - c, c);
    a.stroke();
};
DygraphRangeSelector.prototype.drawMiniPlot_ = function () {
    var p = this.attr_("rangeSelectorPlotFillColor");
    var l = this.attr_("rangeSelectorPlotStrokeColor");
    if (!p && !l) {
        return;
    }
    var m = this.computeCombinedSeriesAndLimits_();
    var e = m.yMax - m.yMin;
    var r = this.bgcanvas_ctx_;
    var f = 0.5;
    var j = this.dygraph_.xAxisExtremes();
    var b = Math.max(j[1] - j[0], 1e-30);
    var q = (this.canvasRect_.w - f) / b;
    var o = (this.canvasRect_.h - f) / e;
    var d = this.canvasRect_.w - f;
    var h = this.canvasRect_.h - f;
    r.beginPath();
    r.moveTo(f, h);
    for (var g = 0; g < m.data.length; g++) {
        var a = m.data[g];
        var n = (a[0] - j[0]) * q;
        var k = h - (a[1] - m.yMin) * o;
        if (isFinite(n) && isFinite(k)) {
            r.lineTo(n, k);
        }
    }
    r.lineTo(d, h);
    r.closePath();
    if (p) {
        var c = this.bgcanvas_ctx_.createLinearGradient(0, 0, 0, h);
        c.addColorStop(0, "white");
        c.addColorStop(1, p);
        this.bgcanvas_ctx_.fillStyle = c;
        r.fill();
    }
    if (l) {
        this.bgcanvas_ctx_.strokeStyle = l;
        this.bgcanvas_ctx_.lineWidth = 1.5;
        r.stroke();
    }
};
DygraphRangeSelector.prototype.computeCombinedSeriesAndLimits_ = function () {
    var u = this.dygraph_.rawData_;
    var t = this.attr_("logscale");
    var p = [];
    var c;
    var g;
    var f, m;
    var l;
    var s, r, q;
    for (s = 0; s < u.length; s++) {
        if (u[s].length > 1 && u[s][1] != null) {
            l = typeof u[s][1] != "number";
            if (l) {
                c = [];
                g = [];
                for (q = 0; q < u[s][1].length; q++) {
                    c.push(0);
                    g.push(0);
                }
            }
            break;
        }
    }
    for (s = 0; s < u.length; s++) {
        var h = u[s];
        var d = h[0];
        if (l) {
            for (q = 0; q < c.length; q++) {
                c[q] = g[q] = 0;
            }
        } else {
            c = g = 0;
        }
        for (r = 1; r < h.length; r++) {
            if (this.dygraph_.visibility()[r - 1]) {
                if (l) {
                    for (q = 0; q < c.length; q++) {
                        m = h[r][q];
                        if (m === null || isNaN(m)) {
                            continue;
                        }
                        c[q] += m;
                        g[q]++;
                    }
                } else {
                    m = h[r];
                    if (m === null || isNaN(m)) {
                        continue;
                    }
                    c += m;
                    g++;
                }
            }
        }
        if (l) {
            for (q = 0; q < c.length; q++) {
                c[q] /= g[q];
            }
            f = c.slice(0);
        } else {
            f = c / g;
        }
        p.push([d, f]);
    }
    p = this.dygraph_.rollingAverage(p, this.dygraph_.rollPeriod_);
    if (typeof p[0][1] != "number") {
        for (s = 0; s < p.length; s++) {
            f = p[s][1];
            p[s][1] = f[0];
        }
    }
    var a = Number.MAX_VALUE;
    var b = -Number.MAX_VALUE;
    for (s = 0; s < p.length; s++) {
        f = p[s][1];
        if (f !== null && isFinite(f) && (!t || f > 0)) {
            a = Math.min(a, f);
            b = Math.max(b, f);
        }
    }
    var n = 0.25;
    if (t) {
        b = Dygraph.log10(b);
        b += b * n;
        a = Dygraph.log10(a);
        for (s = 0; s < p.length; s++) {
            p[s][1] = Dygraph.log10(p[s][1]);
        }
    } else {
        var e;
        var o = b - a;
        if (o <= Number.MIN_VALUE) {
            e = b * n;
        } else {
            e = o * n;
        }
        b += e;
        a -= e;
    }
    return { data: p, yMin: a, yMax: b };
};
DygraphRangeSelector.prototype.placeZoomHandles_ = function () {
    var g = this.dygraph_.xAxisExtremes();
    var a = this.dygraph_.xAxisRange();
    var b = g[1] - g[0];
    var i = Math.max(0, (a[0] - g[0]) / b);
    var e = Math.max(0, (g[1] - a[1]) / b);
    var h = this.canvasRect_.x + this.canvasRect_.w * i;
    var d = this.canvasRect_.x + this.canvasRect_.w * (1 - e);
    var c = Math.max(this.canvasRect_.y, this.canvasRect_.y + (this.canvasRect_.h - this.leftZoomHandle_.height) / 2);
    var f = this.leftZoomHandle_.width / 2;
    this.leftZoomHandle_.style.left = (h - f) + "px";
    this.leftZoomHandle_.style.top = c + "px";
    this.rightZoomHandle_.style.left = (d - f) + "px";
    this.rightZoomHandle_.style.top = this.leftZoomHandle_.style.top;
    this.leftZoomHandle_.style.visibility = "visible";
    this.rightZoomHandle_.style.visibility = "visible";
};
DygraphRangeSelector.prototype.drawInteractiveLayer_ = function () {
    var b = this.fgcanvas_ctx_;
    b.clearRect(0, 0, this.canvasRect_.w, this.canvasRect_.h);
    var e = 1;
    var d = this.canvasRect_.w - e;
    var a = this.canvasRect_.h - e;
    var g = this.getZoomHandleStatus_();
    b.strokeStyle = "black";
    if (!g.isZoomed) {
        b.beginPath();
        b.moveTo(e, e);
        b.lineTo(e, a);
        b.lineTo(d, a);
        b.lineTo(d, e);
        b.stroke();
        if (this.iePanOverlay_) {
            this.iePanOverlay_.style.display = "none";
        }
    } else {
        var f = Math.max(e, g.leftHandlePos - this.canvasRect_.x);
        var c = Math.min(d, g.rightHandlePos - this.canvasRect_.x);
        b.fillStyle = "rgba(240, 240, 240, 0.6)";
        b.fillRect(0, 0, f, this.canvasRect_.h);
        b.fillRect(c, 0, this.canvasRect_.w - c, this.canvasRect_.h);
        b.beginPath();
        b.moveTo(e, e);
        b.lineTo(f, e);
        b.lineTo(f, a);
        b.lineTo(c, a);
        b.lineTo(c, e);
        b.lineTo(d, e);
        b.stroke();
        if (this.isUsingExcanvas_) {
            this.iePanOverlay_.style.width = (c - f) + "px";
            this.iePanOverlay_.style.left = f + "px";
            this.iePanOverlay_.style.height = a + "px";
            this.iePanOverlay_.style.display = "inline";
        }
    }
};
DygraphRangeSelector.prototype.getZoomHandleStatus_ = function () {
    var b = this.leftZoomHandle_.width / 2;
    var c = parseInt(this.leftZoomHandle_.style.left, 10) + b;
    var a = parseInt(this.rightZoomHandle_.style.left, 10) + b;
    return { leftHandlePos: c, rightHandlePos: a, isZoomed: (c - 1 > this.canvasRect_.x || a + 1 < this.canvasRect_.x + this.canvasRect_.w) };
};
"use strict";
Dygraph.numericTicks = function (I, H, w, r, d, s) {
    var C = r("pixelsPerLabel");
    var J = [];
    var F, D, v, A;
    if (s) {
        for (F = 0; F < s.length; F++) {
            J.push({ v: s[F] });
        }
    } else {
        if (r("logscale")) {
            A = Math.floor(w / C);
            var o = Dygraph.binarySearch(I, Dygraph.PREFERRED_LOG_TICK_VALUES, 1);
            var K = Dygraph.binarySearch(H, Dygraph.PREFERRED_LOG_TICK_VALUES, -1);
            if (o == -1) {
                o = 0;
            }
            if (K == -1) {
                K = Dygraph.PREFERRED_LOG_TICK_VALUES.length - 1;
            }
            var u = null;
            if (K - o >= A / 4) {
                for (var t = K; t >= o; t--) {
                    var p = Dygraph.PREFERRED_LOG_TICK_VALUES[t];
                    var m = Math.log(p / I) / Math.log(H / I) * w;
                    var G = { v: p };
                    if (u === null) {
                        u = { tickValue: p, pixel_coord: m };
                    } else {
                        if (Math.abs(m - u.pixel_coord) >= C) {
                            u = { tickValue: p, pixel_coord: m };
                        } else {
                            G.label = "";
                        }
                    }
                    J.push(G);
                }
                J.reverse();
            }
        }
        if (J.length === 0) {
            var h = r("labelsKMG2");
            var q;
            if (h) {
                q = [1, 2, 4, 8];
            } else {
                q = [1, 2, 5];
            }
            var L, z, c;
            for (F = -10; F < 50; F++) {
                var g;
                if (h) {
                    g = Math.pow(16, F);
                } else {
                    g = Math.pow(10, F);
                }
                var f = 0;
                for (D = 0; D < q.length; D++) {
                    L = g * q[D];
                    z = Math.floor(I / L) * L;
                    c = Math.ceil(H / L) * L;
                    A = Math.abs(c - z) / L;
                    f = w / A;
                    if (f > C) {
                        break;
                    }
                }
                if (f > C) {
                    break;
                }
            }
            if (z > c) {
                L *= -1;
            }
            for (F = 0; F < A; F++) {
                v = z + F * L;
                J.push({ v: v });
            }
        }
    }
    var B;
    var y = [];
    if (r("labelsKMB")) {
        B = 1000;
        y = ["K", "M", "B", "T"];
    }
    if (r("labelsKMG2")) {
        if (B) {
            Dygraph.warn("Setting both labelsKMB and labelsKMG2. Pick one!");
        }
        B = 1024;
        y = ["k", "M", "G", "T"];
    }
    var E = r("axisLabelFormatter");
    for (F = 0; F < J.length; F++) {
        if (J[F].label !== undefined) {
            continue;
        }
        v = J[F].v;
        var e = Math.abs(v);
        var l = E(v, 0, r, d);
        if (y.length > 0) {
            var x = B * B * B * B;
            for (D = 3; D >= 0; D--, x /= B) {
                if (e >= x) {
                    l = Dygraph.round_(v / x, r("digitsAfterDecimal")) + y[D];
                    break;
                }
            }
        }
        J[F].label = l;
    }
    return J;
};
Dygraph.dateTicker = function (e, c, i, g, f, h) {
    var d = Dygraph.pickDateTickGranularity(e, c, i, g);
    if (d >= 0) {
        return Dygraph.getDateAxis(e, c, d, g, f);
    } else {
        return [];
    }
};
Dygraph.SECONDLY = 0;
Dygraph.TWO_SECONDLY = 1;
Dygraph.FIVE_SECONDLY = 2;
Dygraph.TEN_SECONDLY = 3;
Dygraph.THIRTY_SECONDLY = 4;
Dygraph.MINUTELY = 5;
Dygraph.TWO_MINUTELY = 6;
Dygraph.FIVE_MINUTELY = 7;
Dygraph.TEN_MINUTELY = 8;
Dygraph.THIRTY_MINUTELY = 9;
Dygraph.HOURLY = 10;
Dygraph.TWO_HOURLY = 11;
Dygraph.SIX_HOURLY = 12;
Dygraph.DAILY = 13;
Dygraph.WEEKLY = 14;
Dygraph.MONTHLY = 15;
Dygraph.QUARTERLY = 16;
Dygraph.BIANNUAL = 17;
Dygraph.ANNUAL = 18;
Dygraph.DECADAL = 19;
Dygraph.CENTENNIAL = 20;
Dygraph.NUM_GRANULARITIES = 21;
Dygraph.SHORT_SPACINGS = [];
Dygraph.SHORT_SPACINGS[Dygraph.SECONDLY] = 1000 * 1;
Dygraph.SHORT_SPACINGS[Dygraph.TWO_SECONDLY] = 1000 * 2;
Dygraph.SHORT_SPACINGS[Dygraph.FIVE_SECONDLY] = 1000 * 5;
Dygraph.SHORT_SPACINGS[Dygraph.TEN_SECONDLY] = 1000 * 10;
Dygraph.SHORT_SPACINGS[Dygraph.THIRTY_SECONDLY] = 1000 * 30;
Dygraph.SHORT_SPACINGS[Dygraph.MINUTELY] = 1000 * 60;
Dygraph.SHORT_SPACINGS[Dygraph.TWO_MINUTELY] = 1000 * 60 * 2;
Dygraph.SHORT_SPACINGS[Dygraph.FIVE_MINUTELY] = 1000 * 60 * 5;
Dygraph.SHORT_SPACINGS[Dygraph.TEN_MINUTELY] = 1000 * 60 * 10;
Dygraph.SHORT_SPACINGS[Dygraph.THIRTY_MINUTELY] = 1000 * 60 * 30;
Dygraph.SHORT_SPACINGS[Dygraph.HOURLY] = 1000 * 3600;
Dygraph.SHORT_SPACINGS[Dygraph.TWO_HOURLY] = 1000 * 3600 * 2;
Dygraph.SHORT_SPACINGS[Dygraph.SIX_HOURLY] = 1000 * 3600 * 6;
Dygraph.SHORT_SPACINGS[Dygraph.DAILY] = 1000 * 86400;
Dygraph.SHORT_SPACINGS[Dygraph.WEEKLY] = 1000 * 604800;
Dygraph.PREFERRED_LOG_TICK_VALUES = function () {
    var c = [];
    for (var b = -39; b <= 39; b++) {
        var a = Math.pow(10, b);
        for (var d = 1; d <= 9; d++) {
            var e = a * d;
            c.push(e);
        }
    }
    return c;
} ();
Dygraph.pickDateTickGranularity = function (d, c, j, h) {
    var g = h("pixelsPerLabel");
    for (var f = 0; f < Dygraph.NUM_GRANULARITIES; f++) {
        var e = Dygraph.numDateTicks(d, c, f);
        if (j / e >= g) {
            return f;
        }
    }
    return -1;
};
Dygraph.numDateTicks = function (e, b, g) {
    if (g < Dygraph.MONTHLY) {
        var h = Dygraph.SHORT_SPACINGS[g];
        return Math.floor(0.5 + 1 * (b - e) / h);
    } else {
        var f = 1;
        var d = 12;
        if (g == Dygraph.QUARTERLY) {
            d = 3;
        }
        if (g == Dygraph.BIANNUAL) {
            d = 2;
        }
        if (g == Dygraph.ANNUAL) {
            d = 1;
        }
        if (g == Dygraph.DECADAL) {
            d = 1;
            f = 10;
        }
        if (g == Dygraph.CENTENNIAL) {
            d = 1;
            f = 100;
        }
        var c = 365.2524 * 24 * 3600 * 1000;
        var a = 1 * (b - e) / c;
        return Math.floor(0.5 + 1 * a * d / f);
    }
};
Dygraph.getDateAxis = function (n, h, a, l, w) {
    var u = l("axisLabelFormatter");
    var z = [];
    var k;
    if (a < Dygraph.MONTHLY) {
        var c = Dygraph.SHORT_SPACINGS[a];
        var v = c / 1000;
        var y = new Date(n);
        var f;
        if (v <= 60) {
            f = y.getSeconds();
            y.setSeconds(f - f % v);
        } else {
            y.setSeconds(0);
            v /= 60;
            if (v <= 60) {
                f = y.getMinutes();
                y.setMinutes(f - f % v);
            } else {
                y.setMinutes(0);
                v /= 60;
                if (v <= 24) {
                    f = y.getHours();
                    y.setHours(f - f % v);
                } else {
                    y.setHours(0);
                    v /= 24;
                    if (v == 7) {
                        y.setDate(y.getDate() - y.getDay());
                    }
                }
            }
        }
        n = y.getTime();
        for (k = n; k <= h; k += c) {
            z.push({ v: k, label: u(new Date(k), a, l, w) });
        }
    } else {
        var e;
        var o = 1;
        if (a == Dygraph.MONTHLY) {
            e = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11];
        } else {
            if (a == Dygraph.QUARTERLY) {
                e = [0, 3, 6, 9];
            } else {
                if (a == Dygraph.BIANNUAL) {
                    e = [0, 6];
                } else {
                    if (a == Dygraph.ANNUAL) {
                        e = [0];
                    } else {
                        if (a == Dygraph.DECADAL) {
                            e = [0];
                            o = 10;
                        } else {
                            if (a == Dygraph.CENTENNIAL) {
                                e = [0];
                                o = 100;
                            } else {
                                Dygraph.warn("Span of dates is too long");
                            }
                        }
                    }
                }
            }
        }
        var s = new Date(n).getFullYear();
        var p = new Date(h).getFullYear();
        var b = Dygraph.zeropad;
        for (var r = s; r <= p; r++) {
            if (r % o !== 0) {
                continue;
            }
            for (var q = 0; q < e.length; q++) {
                var m = r + "/" + b(1 + e[q]) + "/01";
                k = Dygraph.dateStrToMillis(m);
                if (k < n || k > h) {
                    continue;
                }
                z.push({ v: k, label: u(new Date(k), a, l, w) });
            }
        }
    }
    return z;
};
Dygraph.DEFAULT_ATTRS.axes.x.ticker = Dygraph.dateTicker;
Dygraph.DEFAULT_ATTRS.axes.y.ticker = Dygraph.numericTicks;
Dygraph.DEFAULT_ATTRS.axes.y2.ticker = Dygraph.numericTicks;
"use strict";

function RGBColor(f) {
    this.ok = false;
    if (f.charAt(0) == "#") {
        f = f.substr(1, 6);
    }
    f = f.replace(/ /g, "");
    f = f.toLowerCase();
    var b = { aliceblue: "f0f8ff", antiquewhite: "faebd7", aqua: "00ffff", aquamarine: "7fffd4", azure: "f0ffff", beige: "f5f5dc", bisque: "ffe4c4", black: "000000", blanchedalmond: "ffebcd", blue: "0000ff", blueviolet: "8a2be2", brown: "a52a2a", burlywood: "deb887", cadetblue: "5f9ea0", chartreuse: "7fff00", chocolate: "d2691e", coral: "ff7f50", cornflowerblue: "6495ed", cornsilk: "fff8dc", crimson: "dc143c", cyan: "00ffff", darkblue: "00008b", darkcyan: "008b8b", darkgoldenrod: "b8860b", darkgray: "a9a9a9", darkgreen: "006400", darkkhaki: "bdb76b", darkmagenta: "8b008b", darkolivegreen: "556b2f", darkorange: "ff8c00", darkorchid: "9932cc", darkred: "8b0000", darksalmon: "e9967a", darkseagreen: "8fbc8f", darkslateblue: "483d8b", darkslategray: "2f4f4f", darkturquoise: "00ced1", darkviolet: "9400d3", deeppink: "ff1493", deepskyblue: "00bfff", dimgray: "696969", dodgerblue: "1e90ff", feldspar: "d19275", firebrick: "b22222", floralwhite: "fffaf0", forestgreen: "228b22", fuchsia: "ff00ff", gainsboro: "dcdcdc", ghostwhite: "f8f8ff", gold: "ffd700", goldenrod: "daa520", gray: "808080", green: "008000", greenyellow: "adff2f", honeydew: "f0fff0", hotpink: "ff69b4", indianred: "cd5c5c", indigo: "4b0082", ivory: "fffff0", khaki: "f0e68c", lavender: "e6e6fa", lavenderblush: "fff0f5", lawngreen: "7cfc00", lemonchiffon: "fffacd", lightblue: "add8e6", lightcoral: "f08080", lightcyan: "e0ffff", lightgoldenrodyellow: "fafad2", lightgrey: "d3d3d3", lightgreen: "90ee90", lightpink: "ffb6c1", lightsalmon: "ffa07a", lightseagreen: "20b2aa", lightskyblue: "87cefa", lightslateblue: "8470ff", lightslategray: "778899", lightsteelblue: "b0c4de", lightyellow: "ffffe0", lime: "00ff00", limegreen: "32cd32", linen: "faf0e6", magenta: "ff00ff", maroon: "800000", mediumaquamarine: "66cdaa", mediumblue: "0000cd", mediumorchid: "ba55d3", mediumpurple: "9370d8", mediumseagreen: "3cb371", mediumslateblue: "7b68ee", mediumspringgreen: "00fa9a", mediumturquoise: "48d1cc", mediumvioletred: "c71585", midnightblue: "191970", mintcream: "f5fffa", mistyrose: "ffe4e1", moccasin: "ffe4b5", navajowhite: "ffdead", navy: "000080", oldlace: "fdf5e6", olive: "808000", olivedrab: "6b8e23", orange: "ffa500", orangered: "ff4500", orchid: "da70d6", palegoldenrod: "eee8aa", palegreen: "98fb98", paleturquoise: "afeeee", palevioletred: "d87093", papayawhip: "ffefd5", peachpuff: "ffdab9", peru: "cd853f", pink: "ffc0cb", plum: "dda0dd", powderblue: "b0e0e6", purple: "800080", red: "ff0000", rosybrown: "bc8f8f", royalblue: "4169e1", saddlebrown: "8b4513", salmon: "fa8072", sandybrown: "f4a460", seagreen: "2e8b57", seashell: "fff5ee", sienna: "a0522d", silver: "c0c0c0", skyblue: "87ceeb", slateblue: "6a5acd", slategray: "708090", snow: "fffafa", springgreen: "00ff7f", steelblue: "4682b4", tan: "d2b48c", teal: "008080", thistle: "d8bfd8", tomato: "ff6347", turquoise: "40e0d0", violet: "ee82ee", violetred: "d02090", wheat: "f5deb3", white: "ffffff", whitesmoke: "f5f5f5", yellow: "ffff00", yellowgreen: "9acd32" };
    for (var g in b) {
        if (f == g) {
            f = b[g];
        }
    }
    var e = [{ re: /^rgb\((\d{1,3}),\s*(\d{1,3}),\s*(\d{1,3})\)$/, example: ["rgb(123, 234, 45)", "rgb(255,234,245)"], process: function (i) { return [parseInt(i[1]), parseInt(i[2]), parseInt(i[3])]; } }, { re: /^(\w{2})(\w{2})(\w{2})$/, example: ["#00ff00", "336699"], process: function (i) { return [parseInt(i[1], 16), parseInt(i[2], 16), parseInt(i[3], 16)]; } }, { re: /^(\w{1})(\w{1})(\w{1})$/, example: ["#fb0", "f0f"], process: function (i) { return [parseInt(i[1] + i[1], 16), parseInt(i[2] + i[2], 16), parseInt(i[3] + i[3], 16)]; } }];
    for (var c = 0; c < e.length; c++) {
        var j = e[c].re;
        var a = e[c].process;
        var h = j.exec(f);
        if (h) {
            var d = a(h);
            this.r = d[0];
            this.g = d[1];
            this.b = d[2];
            this.ok = true;
        }
    }
    this.r = (this.r < 0 || isNaN(this.r)) ? 0 : ((this.r > 255) ? 255 : this.r);
    this.g = (this.g < 0 || isNaN(this.g)) ? 0 : ((this.g > 255) ? 255 : this.g);
    this.b = (this.b < 0 || isNaN(this.b)) ? 0 : ((this.b > 255) ? 255 : this.b);
    this.toRGB = function () { return "rgb(" + this.r + ", " + this.g + ", " + this.b + ")"; };
    this.toHex = function () {
        var l = this.r.toString(16);
        var k = this.g.toString(16);
        var i = this.b.toString(16);
        if (l.length == 1) {
            l = "0" + l;
        }
        if (k.length == 1) {
            k = "0" + k;
        }
        if (i.length == 1) {
            i = "0" + i;
        }
        return "#" + l + k + i;
    };
}

Date.ext = {};
Date.ext.util = {};
Date.ext.util.xPad = function (a, c, b) {
    if (typeof (b) == "undefined") {
        b = 10;
    }
    for (; parseInt(a, 10) < b && b > 1; b /= 10) {
        a = c.toString() + a;
    }
    return a.toString();
};
Date.prototype.locale = "en-GB";
if (document.getElementsByTagName("html") && document.getElementsByTagName("html")[0].lang) {
    Date.prototype.locale = document.getElementsByTagName("html")[0].lang;
}
Date.ext.locales = {};
Date.ext.locales.en = { a: ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"], A: ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"], b: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"], B: ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"], c: "%a %d %b %Y %T %Z", p: ["AM", "PM"], P: ["am", "pm"], x: "%d/%m/%y", X: "%T" };
Date.ext.locales["en-US"] = Date.ext.locales.en;
Date.ext.locales["en-US"].c = "%a %d %b %Y %r %Z";
Date.ext.locales["en-US"].x = "%D";
Date.ext.locales["en-US"].X = "%r";
Date.ext.locales["en-GB"] = Date.ext.locales.en;
Date.ext.locales["en-AU"] = Date.ext.locales["en-GB"];
Date.ext.formats = {
    a: function (a) { return Date.ext.locales[a.locale].a[a.getDay()]; },
    A: function (a) { return Date.ext.locales[a.locale].A[a.getDay()]; },
    b: function (a) { return Date.ext.locales[a.locale].b[a.getMonth()]; },
    B: function (a) { return Date.ext.locales[a.locale].B[a.getMonth()]; },
    c: "toLocaleString",
    C: function (a) { return Date.ext.util.xPad(parseInt(a.getFullYear() / 100, 10), 0); },
    d: ["getDate", "0"],
    e: ["getDate", " "],
    g: function (a) { return Date.ext.util.xPad(parseInt(Date.ext.util.G(a) / 100, 10), 0); },
    G: function (c) {
        var e = c.getFullYear();
        var b = parseInt(Date.ext.formats.V(c), 10);
        var a = parseInt(Date.ext.formats.W(c), 10);
        if (a > b) {
            e++;
        } else {
            if (a === 0 && b >= 52) {
                e--;
            }
        }
        return e;
    },
    H: ["getHours", "0"],
    I: function (b) {
        var a = b.getHours() % 12;
        return Date.ext.util.xPad(a === 0 ? 12 : a, 0);
    },
    j: function (c) {
        var a = c - new Date("" + c.getFullYear() + "/1/1 GMT");
        a += c.getTimezoneOffset() * 60000;
        var b = parseInt(a / 60000 / 60 / 24, 10) + 1;
        return Date.ext.util.xPad(b, 0, 100);
    },
    m: function (a) { return Date.ext.util.xPad(a.getMonth() + 1, 0); },
    M: ["getMinutes", "0"],
    p: function (a) { return Date.ext.locales[a.locale].p[a.getHours() >= 12 ? 1 : 0]; },
    P: function (a) { return Date.ext.locales[a.locale].P[a.getHours() >= 12 ? 1 : 0]; },
    S: ["getSeconds", "0"],
    u: function (a) {
        var b = a.getDay();
        return b === 0 ? 7 : b;
    },
    U: function (e) {
        var a = parseInt(Date.ext.formats.j(e), 10);
        var c = 6 - e.getDay();
        var b = parseInt((a + c) / 7, 10);
        return Date.ext.util.xPad(b, 0);
    },
    V: function (e) {
        var c = parseInt(Date.ext.formats.W(e), 10);
        var a = (new Date("" + e.getFullYear() + "/1/1")).getDay();
        var b = c + (a > 4 || a <= 1 ? 0 : 1);
        if (b == 53 && (new Date("" + e.getFullYear() + "/12/31")).getDay() < 4) {
            b = 1;
        } else {
            if (b === 0) {
                b = Date.ext.formats.V(new Date("" + (e.getFullYear() - 1) + "/12/31"));
            }
        }
        return Date.ext.util.xPad(b, 0);
    },
    w: "getDay",
    W: function (e) {
        var a = parseInt(Date.ext.formats.j(e), 10);
        var c = 7 - Date.ext.formats.u(e);
        var b = parseInt((a + c) / 7, 10);
        return Date.ext.util.xPad(b, 0, 10);
    },
    y: function (a) { return Date.ext.util.xPad(a.getFullYear() % 100, 0); },
    Y: "getFullYear",
    z: function (c) {
        var b = c.getTimezoneOffset();
        var a = Date.ext.util.xPad(parseInt(Math.abs(b / 60), 10), 0);
        var e = Date.ext.util.xPad(b % 60, 0);
        return (b > 0 ? "-" : "+") + a + e;
    },
    Z: function (a) { return a.toString().replace(/^.*\(([^)]+)\)$/, "$1"); },
    "%": function (a) { return "%"; }
};
Date.ext.aggregates = { c: "locale", D: "%m/%d/%y", h: "%b", n: "\n", r: "%I:%M:%S %p", R: "%H:%M", t: "\t", T: "%H:%M:%S", x: "locale", X: "locale" };
Date.ext.aggregates.z = Date.ext.formats.z(new Date());
Date.ext.aggregates.Z = Date.ext.formats.Z(new Date());
Date.ext.unsupported = {};
Date.prototype.strftime = function (a) {
    if (!(this.locale in Date.ext.locales)) {
        if (this.locale.replace(/-[a-zA-Z]+$/, "") in Date.ext.locales) {
            this.locale = this.locale.replace(/-[a-zA-Z]+$/, "");
        } else {
            this.locale = "en-GB";
        }
    }
    var c = this;
    while (a.match(/%[cDhnrRtTxXzZ]/)) {
        a = a.replace(/%([cDhnrRtTxXzZ])/g, function (e, d) {
            var g = Date.ext.aggregates[d];
            return (g == "locale" ? Date.ext.locales[c.locale][d] : g);
        });
    }
    var b = a.replace(/%([aAbBCdegGHIjmMpPSuUVwWyY%])/g, function (e, d) {
        var g = Date.ext.formats[d];
        if (typeof (g) == "string") {
            return c[g]();
        } else {
            if (typeof (g) == "function") {
                return g.call(c, c);
            } else {
                if (typeof (g) == "object" && typeof (g[0]) == "string") {
                    return Date.ext.util.xPad(c[g[0]](), g[1]);
                } else {
                    return d;
                }
            }
        }
    });
    c = null;
    return b;
};