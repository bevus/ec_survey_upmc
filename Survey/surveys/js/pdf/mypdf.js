$(function () {
    $('#toPdf').click(() => {
        console.log("onclick");
        window.CanvasRenderingContext2D
  
        html2canvas(document.getElementById('dashboard-content'), {
            taintTest:true,allowTaint: true,
            onrendered: function (canvas) {
                var context = canvas.getContext('2d');
                var imgData = context.toDataURL("image/png");
                Console.log(imgData);
                //595pt x 842pt
                var imgWidth = 590;
                var pageHeight = 842;
                var imgHeight = canvas.height * imgWidth / canvas.width;
                var heightLeft = imgHeight;
                console.log("imgH " + imgHeight+"canevas"+canvas.height);
                var doc = new jsPDF('p', 'pt');
                var position = 10;
                console.log("...........," + heightLeft);
                doc.addImage(imgData, 'png', 0, position, imgWidth, imgHeight);
                heightLeft -= pageHeight;
                console.log("...........," + heightLeft);
                while (heightLeft >= 0) {

                    position = heightLeft - imgHeight;
                    doc.addPage();
                    doc.addImage(imgData, 'PNG', 0, position, imgWidth, imgHeight);
                    heightLeft -= pageHeight;
                    console.log("....."+heightLeft);
                }

                doc.save("Dashboard.pdf");
 
            }
        });
    })
    function resize_canvas(canvas) {
       // canvas = document.getElementById("canvas");
        if (canvas.width < window.innerWidth) {
            canvas.width = window.innerWidth;
        }

        if (canvas.height < window.innerHeight) {
            canvas.height = window.innerHeight;
        }
    }

});