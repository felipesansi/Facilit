
    function mostrarSenha() {
        var senhaInput = document.getElementById('Senha_Usuario');
    if (senhaInput.type === "password") {
        senhaInput.type = "text";
        } else {
        senhaInput.type = "password";
        }
    }

