CREATE DATABASE IF NOT EXISTS bd_facilit;

USE bd_facilit;

CREATE TABLE IF NOT EXISTS tb_usuarios
(
    id INT AUTO_INCREMENT PRIMARY KEY,
    nome_completo VARCHAR(255),
    email VARCHAR(255),
    nome_usuario VARCHAR(255),
    senha_usuario VARCHAR(255),
    excluido BOOLEAN DEFAULT 0,
    criado DATETIME,
    alterado DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);
