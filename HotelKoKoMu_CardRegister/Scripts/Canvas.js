document.addEventListener('DOMContentLoaded', domloaded, false);
function domloaded() {

    var canvas = document.getElementById('csvRegister'),
        ctx = canvas.getContext('2d'),
        moveflg = 0,
        Xpoint,
        Ypoint;

    //初期値（サイズ、色、アルファ値）の決定
    var defSize = 7,
        defColor = "#555";

    // キャンバスを白色に塗る
    ctx.fillStyle = 'rgb(255,255,255)';

    // Event handler to resize the canvas when the document view is changed
    window.addEventListener('resize', resizeCanvas, false);

    // PC対応
    canvas.addEventListener('mousedown', startPoint, false);
    canvas.addEventListener('mousemove', movePoint, false);
    canvas.addEventListener('mouseup', endPoint, false);

    // スマホ対応
    canvas.addEventListener('touchstart', startPoint, false);
    canvas.addEventListener('touchmove', movePoint, false);
    canvas.addEventListener('touchend', endPoint, false);

    function startPoint(e) {
        e.preventDefault();
        ctx.beginPath();
        Xpoint = e.layerX;
        Ypoint = e.layerY;
        ctx.moveTo(Xpoint, Ypoint);
    }

    function resizeCanvas() {
        var modalWidth = $(".modal-dialog").css('max-width');
        canvas.width = parseInt(modalWidth.replace('px', '')) - 35;
    }

    resizeCanvas();

    function movePoint(e) {
        if (e.buttons === 1 || e.witch === 1 || e.type == 'touchmove') {
            Xpoint = e.layerX;
            Ypoint = e.layerY;
            moveflg = 1;
            ctx.lineTo(Xpoint, Ypoint);
            ctx.lineCap = "round";
            ctx.lineWidth = defSize * 2;
            ctx.strokeStyle = defColor;
            ctx.stroke();
        }
    }

    function endPoint(e) {
        if (moveflg === 0) {
            ctx.lineTo(Xpoint - 1, Ypoint - 1);
            ctx.lineCap = "round";
            ctx.lineWidth = defSize * 2;
            ctx.strokeStyle = defColor;
            ctx.stroke();
        }
        moveflg = 0;
    }

    function ClearData() {
        ctx.clearRect(0, 0, ctx.canvas.clientWidth, ctx.canvas.clientHeight);
        ctx.fillStyle = 'rgb(255,255,255)';       
    }
}
