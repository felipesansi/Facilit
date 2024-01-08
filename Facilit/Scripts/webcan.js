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

document.addEventListener("DOMContentLoaded", function () {
    const video = document.querySelector('video');
    const canvas = document.querySelector('canvas');
    const botao = document.getElementById('btn_salvar');

    navigator.mediaDevices.getUserMedia({ video: true })
        .then(stream => {
            video.srcObject = stream;
        })
        .catch(erro => {
            console.error('Erro ao abrir a câmera :', erro);
        });

    botao.addEventListener('click', function () {
        const context = canvas.getContext('2d');
        context.drawImage(video, 0, 0, canvas.width, canvas.height);
        const dados_imagem = canvas.toDataURL('image/jpg').replace('data:image/jpeg;base64,', '').replace('data:image/png;base64,', '');


        $.ajax({
            type: "POST",
            url: "/Webcan/SalvarFoto",
            data: { dados_imagem: dados_imagem },
            success: function (response) {
                console.log(response);
            },
        });
    });
});
