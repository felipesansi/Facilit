//var video = document.querySelector('video');

//navigator.mediaDevices.getUserMedia({ video: true })
//    .then(stream => {
//        video.srcObject = stream;
//        video.play();
//    })
//    .catch(error => {
//        console.log(error);
//    })

//document.querySelector('button').addEventListener('click', () => {
//    var canvas = document.querySelector('canvas');
//    canvas.height = video.videoHeight;
//    canvas.width = video.videoWidth;
//    var context = canvas.getContext('2d');
//    context.drawImage(video, 0, 0);
//    var link = document.createElement('a');
//    link.download = 'foto.png';
//    link.href = canvas.toDataURL();
//    link.textContent = 'Clique para baixar a imagem';
//    document.body.appendChild(link);
//});


//$(document).ready(function () {
//    var video = document.querySelector('video');
//    var canvas = document.querySelector('canvas');
//    var context = canvas.getContext('2d');
//    var button = document.querySelector('#button');

//    navigator.mediaDevices.getUserMedia({ video: true })
//        .then(function (stream) {
//            video.srcObject = stream;
//            video.play();
//        });

//    button.addEventListener('click', function () {
//        context.drawImage(video, 0, 0, canvas.width, canvas.height);
//        var dataUrl = canvas.toDataURL('image/png');

//        // Aqui você pode enviar 'dataUrl' para o servidor para salvar a imagem
//        $.ajax({
//            url: 'C:\Users\Felipe\source\repos\Facilit\Facilit\fotos',  // Substitua por seu URL
//            type: 'POST',
//            data: { image: dataUrl },
//            success: function (response) {
//                console.log('Imagem salva com sucesso');
//            },
//            error: function (error) {
//                console.log('Erro ao salvar a imagem', error);
//            }
//        });
//    });
//});



    
 
