(function ($, window, document, undefined) {
    var pluginName = "table2excel",

        defaults = {
            exclude: ".noExl",
            name: "Table2Excel"
        };

    // The actual plugin constructor
    function Plugin(element, options) {
        this.element = element;
        // jQuery has an extend method which merges the contents of two or
        // more objects, storing the result in the first object. The first object
        // is generally empty as we don't want to alter the default options for
        // future instances of the plugin
        //
        this.settings = $.extend({}, defaults, options);
        this._defaults = defaults;
        this._name = pluginName;
        this.init();
    }

    Plugin.prototype = {
        init: function () {
            var e = this;

            var utf8Heading = "<meta http-equiv=\"content-type\" content=\"application/vnd.ms-excel; charset=UTF-8\">";
            e.template = {
                head: "<html xmlns:o=\"urn:schemas-microsoft-com:office:office\" xmlns:x=\"urn:schemas-microsoft-com:office:excel\" xmlns=\"http://www.w3.org/TR/REC-html40\">" + utf8Heading + "<head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets>",
                sheet: {
                    head: "<x:ExcelWorksheet><x:Name>",
                    tail: "</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet>"
                },
                mid: "</x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head><body>",
                table: {
                    head: "<table>",
                    tail: "</table>"
                },
                foot: "</body></html>"
            };

            e.tableRows = [];

            // get contents of table except for exclude
            $(e.element).each(function (i, o) {
                var tempRows = "";
                $(o).find("tr").not(e.settings.exclude).each(function (i, o) {
                    tempRows += "<tr>" + $(o).html() + "</tr>";
                });
                e.tableRows.push(tempRows);
            });

            e.tableToExcel(e.tableRows, e.settings.name, e.settings.sheetName);
        },

        tableToExcel: function (table, name, sheetName) {
            var e = this, fullTemplate = "", i, link, a;

            e.uri = "data:application/vnd.ms-excel;base64,";
            e.base64 = function (s) {
                return window.btoa(unescape(encodeURIComponent(s)));
            };
            e.format = function (s, c) {
                return s.replace(/{(\w+)}/g, function (m, p) {
                    return c[p];
                });
            };

            sheetName = typeof sheetName === "undefined" ? "Sheet" : sheetName;

            e.ctx = {
                worksheet: name || "Worksheet",
                table: table,
                sheetName: sheetName,
            };

            fullTemplate = e.template.head;

            if ($.isArray(table)) {
                for (i in table) {
                    //fullTemplate += e.template.sheet.head + "{worksheet" + i + "}" + e.template.sheet.tail;
                    fullTemplate += e.template.sheet.head + sheetName + i + e.template.sheet.tail;
                }
            }

            fullTemplate += e.template.mid;

            if ($.isArray(table)) {
                for (i in table) {
                    fullTemplate += e.template.table.head + "{table" + i + "}" + e.template.table.tail;
                }
            }

            fullTemplate += e.template.foot;

            for (i in table) {
                e.ctx["table" + i] = table[i];
            }
            delete e.ctx.table;

            if (typeof msie !== "undefined" && msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./))      // If Internet Explorer
            {
                if (typeof Blob !== "undefined") {
                    //use blobs if we can
                    fullTemplate = [fullTemplate];
                    //convert to array
                    var blob1 = new Blob(fullTemplate, { type: "text/html" });
                    window.navigator.msSaveBlob(blob1, getFileName(e.settings));
                } else {
                    //otherwise use the iframe and save
                    //requires a blank iframe on page called txtArea1
                    txtArea1.document.open("text/html", "replace");
                    txtArea1.document.write(e.format(fullTemplate, e.ctx));
                    txtArea1.document.close();
                    txtArea1.focus();
                    sa = txtArea1.document.execCommand("SaveAs", true, getFileName(e.settings));
                }

            } else {
                link = e.uri + e.base64(e.format(fullTemplate, e.ctx));
                a = document.createElement("a");
                a.download = getFileName(e.settings);
                a.href = link;

                document.body.appendChild(a);

                a.click();

                document.body.removeChild(a);
            }

            return true;
        }
    };

    function getFileName(settings) {
        return (settings.filename ? settings.filename : "ReconsileReport") +
            (settings.fileext ? settings.fileext : ".xls");
    }

    $.fn[pluginName] = function (options) {
        var e = this;
        e.each(function () {
            if (!$.data(e, "plugin_" + pluginName)) {
                $.data(e, "plugin_" + pluginName, new Plugin(this, options));
            }
        });

        // chain jQuery functions
        return e;
    };

})(jQuery, window, document);
function exportClickCSV() {
    var titles = [];
    var data = [];

    $('#reconsileDatatable th').each(function () {
        titles.push($(this).text());
    });
    $('#reconsileDatatable td').each(function () {
        data.push($(this).text());
    });
    var CSVString = prepCSVRow(titles, titles.length, '');
    CSVString = prepCSVRow(data, titles.length, CSVString);
    var downloadLink = document.createElement("a");
    var blob = new Blob(["\ufeff", CSVString]);
    var url = URL.createObjectURL(blob);
    downloadLink.href = url;
    downloadLink.download = "ReconsileReport.csv";
    document.body.appendChild(downloadLink);
    downloadLink.click();
    document.body.removeChild(downloadLink);

    //var doc = new jsPDF('p', 'pt');
    //var elem = document.getElementById("reconsileDatatable");
    //var res = doc.autoTableHtmlToJson(elem);
    //doc.autoTable(res.columns, res.data);
    //doc.save("table.pdf");



    //var pdf = new jsPDF('p', 'pt');
    //// source can be HTML-formatted string, or a reference
    //// to an actual DOM element from which the text will be scraped.
    //source = $('#reconsileDatatable')[0];

    //// we support special element handlers. Register them with jQuery-style 
    //// ID selector for either ID or node name. ("#iAmID", "div", "span" etc.)
    //// There is no support for any other type of selectors 
    //// (class, of compound) at this time.
    //specialElementHandlers = {
    //    // element with id of "bypass" - jQuery style selector
    //    '#bypassme': function (element, renderer) {
    //        // true = "handled elsewhere, bypass text extraction"
    //        return true
    //    }
    //};
    //margins = {
    //    top: 80,
    //    bottom: 60,
    //    left: 40,
    //    width: 522
    //};
    //// all coords and widths are in jsPDF instance's declared units
    //// 'inches' in this case
    //pdf.fromHTML(
    //    source, // HTML string or DOM elem ref.
    //    margins.left, // x coord
    //    margins.top, { // y coord
    //        'width': margins.width, // max width of content on PDF
    //        'elementHandlers': specialElementHandlers
    //    },

    //    function (dispose) {
    //        // dispose: object with X, Y of the last line add to the PDF 
    //        //          this allow the insertion of new lines after html
    //        pdf.save('Test.pdf');
    //    }, margins);



    //var table = document.getElementById('reconsileDatatable');
    //var html = table.outerHTML;
    //window.open('data:application/vnd.ms-excel;base64,' + base64_encode(html));




}
function exportClickXLS() {

    $("#reconsileDatatable").table2excel();
    //var tab_text = '<html xmlns:x="urn:schemas-microsoft-com:office:excel">';
    //tab_text = tab_text + '<head><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet>';
    //tab_text = tab_text + '<x:Name>Test Sheet</x:Name>';
    //tab_text = tab_text + '<x:WorksheetOptions><x:Panes></x:Panes></x:WorksheetOptions></x:ExcelWorksheet>';
    //tab_text = tab_text + '</x:ExcelWorksheets></x:ExcelWorkbook></xml></head><body>';
    //tab_text = tab_text + "<table border='2px'><tr bgcolor='#87AFC6'>";
    //var textRange; var j = 0;
    //tab = document.getElementById('reconsileDatatable'); // id of table

    //for (j = 0; j < tab.rows.length; j++) {
    //    tab_text = tab_text + tab.rows[j].innerHTML + "</tr>";
    //    //tab_text=tab_text+"</tr>";
    //}

    //tab_text = tab_text + "</table>";
    //tab_text = tab_text.replace(/<A[^>]*>|<\/A>/g, "");//remove if u want links in your table
    //tab_text = tab_text.replace(/<img[^>]*>/gi, ""); // remove if u want images in your table
    //tab_text = tab_text.replace(/<input[^>]*>|<\/input>/gi, ""); // reomves input params

    //var ua = window.navigator.userAgent;
    //var msie = ua.indexOf("MSIE ");

    //if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./))      // If Internet Explorer
    //{
    //    txtArea1.document.open("txt/html", "replace");
    //    txtArea1.document.write(tab_text);
    //    txtArea1.document.close();
    //    txtArea1.focus();
    //    sa = txtArea1.document.execCommand("SaveAs", true, "Say Thanks to Sumit.xls");
    //}
    //else                 //other browser not tested on IE 11
    //    sa = window.open('data:application/vnd.ms-excel,' + encodeURIComponent(tab_text));

    //return (sa);
}
function base64_encode(data) {
    var b64 = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";
    var o1, o2, o3, h1, h2, h3, h4, bits, i = 0,
        ac = 0,
        enc = "",
        tmp_arr = [];

    if (!data) {
        return data;
    }

    do { // pack three octets into four hexets
        o1 = data.charCodeAt(i++);
        o2 = data.charCodeAt(i++);
        o3 = data.charCodeAt(i++);

        bits = o1 << 16 | o2 << 8 | o3;

        h1 = bits >> 18 & 0x3f;
        h2 = bits >> 12 & 0x3f;
        h3 = bits >> 6 & 0x3f;
        h4 = bits & 0x3f;

        // use hexets to index into b64, and append result to encoded string
        tmp_arr[ac++] = b64.charAt(h1) + b64.charAt(h2) + b64.charAt(h3) + b64.charAt(h4);
    } while (i < data.length);

    enc = tmp_arr.join('');

    var r = data.length % 3;

    return (r ? enc.slice(0, r - 3) : enc) + '==='.slice(r || 3);

}

function prepCSVRow(arr, columnCount, initial) {
    var row = ''; // this will hold data
    var delimeter = ','; // data slice separator, in excel it's `;`, in usual CSv it's `,`
    var newLine = '\r\n'; // newline separator for CSV row

    /*
     * Convert [1,2,3,4] into [[1,2], [3,4]] while count is 2
     * @param _arr {Array} - the actual array to split
     * @param _count {Number} - the amount to split
     * return {Array} - splitted array
     */
    function splitArray(_arr, _count) {
        var splitted = [];
        var result = [];
        _arr.forEach(function (item, idx) {
            if ((idx + 1) % _count === 0) {
                splitted.push(item);
                result.push(splitted);
                splitted = [];
            } else {
                splitted.push(item);
            }
        });
        return result;
    }
    var plainArr = splitArray(arr, columnCount);
    // don't know how to explain this
    // you just have to like follow the code
    // and you understand, it's pretty simple
    // it converts `['a', 'b', 'c']` to `a,b,c` string
    plainArr.forEach(function (arrItem) {
        arrItem.forEach(function (item, idx) {
            row += item + ((idx + 1) === arrItem.length ? '' : delimeter);
        });
        row += newLine;
    });
    return initial + row;
}