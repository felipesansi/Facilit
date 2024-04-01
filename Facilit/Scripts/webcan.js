document.addEventListener("DOMContentLoaded", function () {
    const video = document.querySelector('video');
    const canvas = document.querySelector('canvas');
    const botao = document.getElementById('btn_salvar');
    const proporcao = 0.75; 

    navigator.mediaDevices.getUserMedia({ video: true })
        .then(stream => {
            video.srcObject = stream;
        })
        .catch(erro => {
            console.error('Erro ao abrir a câmera :', erro);
        });

    botao.addEventListener('click', function () {
        const context = canvas.getContext('2d');
        const videoWidth = video.videoWidth;
        const videoHeight = video.videoHeight;
        const capturaWidth = videoWidth;
        const capturaHeight = videoWidth * proporcao;

        canvas.width = capturaWidth;
        canvas.height = capturaHeight;
        const produtoSelecionado = document.getElementById('drop_produtos').value;
        const clienteSelecionado = document.getElementById('drop_clientes').value;
        context.drawImage(video, 0, 0, capturaWidth, capturaHeight);
        const dados_imagem = canvas.toDataURL('image/jpg').replace('data:image/jpeg;base64,', '').replace('data:image/png;base64,', '');

        $.ajax({
            type: "POST",
            url: "/Webcan/SalvarFoto",
            data: { dados_imagem: dados_imagem, produto_selecionado: produtoSelecionado, cliente_selecionado: clienteSelecionado },
            success: function (response) {
                if (response.sucesso) {
                    alert(response.mensagem);
                }
                else {
                    alert(response.mensagem);
                }
            },
        });
    });
});
