$(function () {
    function startPrintProcess(canvasObj, fileName, callback) {
        var pdf = new jsPDF('p', 'pt', 'a4'),
            pdfConf = {
                pagesplit: false,
                background: '#fff'
            };
        document.body.appendChild(canvasObj); //appendChild is required for html to add page in pdf
        pdf.addHTML(canvasObj, 0, 0, pdfConf, function () {
            document.body.removeChild(canvasObj);
            pdf.addPage();
            pdf.save(fileName + '.pdf');
            callback();
        });
    }

    $('#toPdf').click(() => {

        html2canvas(document.getElementById('dashboard-content'), {
            onrendered: function (canvasObj) {
                startPrintProcess(canvasObj, 'printedPDF', function () {
                    alert('PDF saved');
                });
                //save this object to the pdf
            }
        });
    })




});