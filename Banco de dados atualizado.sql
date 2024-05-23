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
    alterado DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    token VARCHAR(255) NULL,
   token_expiracao DATETIME NULL
);
CREATE TABLE tb_fotos
 (
  id INT NOT NULL AUTO_INCREMENT,
  id_usuario INT NOT NULL,
  nome_produto VARCHAR(255) NULL,
  nome_cliente VARCHAR(255) NULL,
  data_tirada DATETIME NOT NULL,
  link_foto VARCHAR(255)  NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_usuario_idx` (`id_usuario` ASC) VISIBLE,
  CONSTRAINT `fk_usuario`
    FOREIGN KEY (`id_usuario`)
    REFERENCES `bd_facilit`.`tb_usuarios` (`id`)
   );
   create table tb_produtos(
     id int primary key auto_increment,
     codigo_tiny_produto int,
     descricao varchar(255),
     unidade varchar(255),
     data_atualizacao_produto datetime
   );
create table tb_clientes(
id int primary key auto_increment,
codigo_tiny_cliente int,
nome varchar(255),
data_atualizacao_cliente datetime
);