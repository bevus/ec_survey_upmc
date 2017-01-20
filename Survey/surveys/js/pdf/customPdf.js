//$(function () {
//    $('#toPdf').click(function () {
//        html2canvas($("#dashboard-content").get(0), {
//            onrendered: function (canvas) {
//                document.body.appendChild(canvas);
//            }
//        });
//    })
//});
$(function () {

    $('#toPdf').click(function () {
        console.log("button click");
        var doc = new jsPDF('p', 'mm', 'a4');

        $('body').scrollTop(10);

        var svgElements = $("#questions").find('svg');
        console.log("nbsvg :" + svgElements.length);
        //replace all svgs with a temp canvas
        svgElements.each(function () {
            var pdf = new jsPDF('p', 'pt', 'a4');
            svgElementToPdf(svg, pdf, {
                scale: 72 / 96,
                removeInvalid: true
            }); pdf.output('datauri');

           // var canvas, xml;
           // // canvg doesn't cope very well with em font sizes so find the calculated size in pixels and replace it in the element.
           //$.each($(this).find('[style*=em]'), function (index, el) {
           //     $(this).css('font-size', getStyle(el, 'font-size'));
           // });

           // //canvas = document.createElement("canvas");
           // //canvas.className = "screenShotTempCanvas";
           // //convert SVG into a XML string
           // xml = (new XMLSerializer()).serializeToString(this);

           // // Removing the name space as IE throws an error
           // xml = xml.replace(/xmlns=\"http:\/\/www\.w3\.org\/2000\/svg\"/, '');
     

            //draw the SVG onto a canvas
            //canvg(canvas, xml);
            //$(canvas).insertAfter(this);
            //hide the SVG element
            //$(this).attr('class', 'tempHide');
            //$(this).hide();
         
        });
        createPDF();
      
        $("#container").find('.screenShotTempCanvas').remove();
        $("#container").find('.tempHide').show().removeClass('tempHide');
    });



    // permiere version 
    var form = $('.screenShotTempCanvas'), cache_width = form.width(), a4 = [500, 800];
    function createPDF() {
        getCanvas().then(function (canvas) {
            var img = canvas.toDataURL("image/png");
            var doc = new jsPDF({ unit: 'px', format: 'a4' });
            //var doc = new jsPDF();

            doc.addImage(img, 'JPEG', 2, 2);
            if (canvas.height > 365) {
                doc.addPage();
                doc.addImage(img, 0, -370);
            }
            doc.save('html-to-pdf.pdf');
        });
    }

    // create canvas object
    function getCanvas() {
        return html2canvas($('.screenShotTempCanvas').get(0), {
            //imageTimeout: 1000,
           // removeContainer: false
        });
    }

    //// I recommend to keep the svg visible as a preview 
    //var svg = $('#container > svg').get(0);
    //// you should set the format dynamically, write [width, height] instead of 'a4' 
    //var pdf = new jsPDF('p', 'pt', 'a4');
    //svgElementToPdf(svg, pdf, {
    //    scale: 72/96,
    //    removeInvalid: true 
    //}); pdf.output('datauri');
    //// use output() to get the jsPDF buffer 
    /*
                   $("#toPdf").on("click", function (e) {
                       $('body').scrollTop(0);
                       var doc = new jsPDF('p', 'mm', 'a4');
                       var width = doc.internal.pageSize.width;
                       var height = doc.internal.pageSize.height;
       
                       html2canvas($("#dashboard-content"), {
                           onrendered: function (canvas) {
                               //document.body.appendChild(canvas);
                              
                               var imgData = canvas.toDataURL('image/png');
       
                               doc.addImage(imgData, 'JPEG', 0, 0, 210, 240);
                               doc.save('testpdf.pdf');
       
                           }
                       });
                       // var pdf = new jsPDF('p', 'pt', 'a4');
                       // pdf.addHTML($(".dashboard-container"), function () { pdf.save('testpdf.pdf'); });
                       console.log("Button pdf");
       
                   });
                   */
});