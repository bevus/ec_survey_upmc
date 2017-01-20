$(function () {

    function svgToCanvas(targetElem) {
        var nodesToRecover = [];
        var nodesToRemove = [];
        var svgElem = targetElem.find('svg');
        svgElem.each(function (index, node) {
            var parentNode = node.parentNode;
            var svg = parentNode.innerHTML;
            var canvas = document.createElement('canvas');
            canvg(canvas, svg);
            nodesToRecover.push({
                parent: parentNode,
                child: node
            });
            parentNode.removeChild(node);
            nodesToRemove.push({
                parent: parentNode,
                child: canvas
            });
            parentNode.appendChild(canvas);
        });

        html2canvas(targetElem, {
            onrendered: function (canvas) {
                var ctx = canvas.getContext('2d');
                ctx.webkitImageSmoothingEnabled = false;
                ctx.mozImageSmoothingEnabled = false;
                ctx.imageSmoothingEnabled = false;
            }
        });
    }

    $('#toPdf').click(function () {
    
        console.log("button click");
        svgToCanvas("#dashboard-content");
           
            createPDF();
      
          
    });

   

    // permiere version 
    // var form = $('.screenShotTempCanvas'), cache_width = form.width(), a4 = [500, 800];
        var form = $('.screenShotTempCanvas'), cache_width = form.width();

        function createPDF() {
            //canvg();
        getCanvas().then(function (canvas) {
            var img = canvas.toDataURL("image/png");
            //var doc = new jsPDF({ unit: 'px', format: 'a4' });
            var doc = new jsPDF();

            doc.addImage(img, 'JPEG', 2, 2);
            if (canvas.height > 300 ) {
                doc.addPage();
                doc.addImage(img, 0, -365);
                console.log("canvas.height" + canvas.height);
                console.log("canvas.width" + canvas.width);
            }
            doc.save('html-to-pdf.pdf');
           // form.width(cache_width);
        });
    }



    function getCanvas() {
       
        //form.width((a4[0] * 1.333333 - 80)).css('max-width', 'none');
        return html2canvas(form.get(0),{
    
            imageTimeout: 100,
            removeContainer: false
        });
    }

    function getelemCanvas(elem) {
        return html2canvas(elem.get(0));
    }

    //$('#toPdf').click(function () {


    //    //$('text').each(function () {
    //    //    var offset = $(this).offset();
    //    //    $(this).parents('[data-role="canvas"]').append('<p class="app-svg-text-replace" style="top: ' + offset.top + 'px; left: ' +
    //    //        parseInt(offset.left - parseInt((($(window).width() / 2) - 512))) + 'px;">' + $(this)[0].textContent + '</p>');
    //    //});

    //    console.log("button click");
    //    $('body').scrollTop(10);
    //    var svgElements = $("#dashboard-content").find('svg');
    //    alert("svg");
    //    //replace all svgs with a temp canvas
    //    svgElements.each(function () {
    //        var canvas, xml;
    //        // canvg doesn't cope very well with em font sizes so find the calculated size in pixels and replace it in the element.
    //        $.each($(this).find('[style*=em]'), function (index, el) {
    //            $(this).css('font-size', getStyle(el, 'font-size'));
    //        });
    //        canvas = document.createElement("canvas");
    //        canvas.className = "screenShotTempCanvas";
    //        //convert SVG into a XML string
    //        xml = (new XMLSerializer()).serializeToString(this);
    //        // Removing the name space as IE throws an error
    //        xml = xml.replace(/xmlns=\"http:\/\/www\.w3\.org\/2000\/svg\"/, '');
    //        console.log("xml" + xml);
    //        //draw the SVG onto a canvas
    //        //canvg(canvas, xml);
    //       // $(canvas).insertAfter(this);
    //        //hide the SVG element
    //        $(this).attr('class', 'tempHide');
    //        //$(this).hide();

    //    });
    //    createPDF();

    //   $("#container").find('.screenShotTempCanvas').remove();
    //   $("#container").find('.tempHide').show().removeClass('tempHide');
    //});


    
                   //$("#toPdf").on("click", function (e) {
                   //    $('body').scrollTop(0);
                   //    var doc = new jsPDF('p', 'mm', 'a4');
                   //    var width = doc.internal.pageSize.width;
                   //    var height = doc.internal.pageSize.height;
       
                   //    html2canvas($("#dashboard-content").get(0), {
                   //        onrendered: function (canvas) {
                   //            //document.body.appendChild(canvas);
                              
                   //            var imgData = canvas.toDataURL('image/png');
       
                   //            doc.addImage(imgData, 'JPEG', 0, 0, 210, 240);
                   //            doc.save('testpdf.pdf');
       
                   //        }
                   //    });
                   //    // var pdf = new jsPDF('p', 'pt', 'a4');
                   //    // pdf.addHTML($(".dashboard-container"), function () { pdf.save('testpdf.pdf'); });
                   //    console.log("Button pdf");
       
                   //});
                   
});