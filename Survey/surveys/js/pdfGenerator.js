$(function () {
    var form = $('.screenShotTempCanvas'),
        cache_width = form.width(),
        a4 = [500, 800]; var $dashbord = $("#dashboard-content");

        $('#toPdf').click(function () {
            $('body').scrollTop(10);
            $dashbord.find('svg').each(function () {
                var canvas, xml;
                $.each($(this).find('[style*=em]'), function (index, el) {
                    $(this).css('font-size', getStyle(el, 'font-size'));
                });
                canvas = document.createElement("canvas");
                canvas.className = "screenShotTempCanvas";
                xml = (new XMLSerializer()).serializeToString(this);
                xml = xml.replace(/xmlns=\"http:\/\/www\.w3\.org\/2000\/svg\"/, '');
                canvg(canvas, xml);
                $(canvas).insertAfter(this); $(this).attr('class', 'tempHide');
                $(this).hide();
            });
            createPDF();
            $.each(document.find('.screenShotTempCanvas'), function () { $(this).remove() });
            $.each(document.find('.tempHide'), function () {$(this).show().removeClass('tempHide')});
        });
    function startPrintProcess(canvasObj, fileName, callback) {
        var pdf = new jsPDF('p', 'pt', 'a4'),
        pdfConf = { pagesplit: false, background: '#d4d4d4' };
        document.body.appendChild(canvasObj);

        var imgWidth = 590;
        var pageHeight = 842;
        var imgHeight = canvasObj.height * imgWidth / canvasObj.width;
        var heightLeft = imgHeight;
        var position = 10;

        var img = canvasObj.toDataURL("image/png");

        pdf.addImage(img, 'JPEG', 0, position, imgWidth, imgHeight);
        heightLeft -= pageHeight;
        while (heightLeft >= 0) {
            position = heightLeft - imgHeight;
            pdf.addPage();
            pdf.addImage(img, 'JPEG', 0, position, imgWidth, imgHeight);
            heightLeft -= pageHeight;
        }

        pdf.save('html-to-pdf.pdf');
        document.body.removeChild(canvasObj);

    }
    function createPDF() {
        html2canvas($dashbord.get(0), {
            onrendered: function (canvasObj) {
                startPrintProcess(canvasObj, 'Dashboard', function () { });
            }
        });
    }
});