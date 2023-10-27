create database  bd_facilit;
use bd_facilit;
create table tb_usuarios
(
id int auto_increment primary key,
nome_completo varchar(255),
email varchar(255),
nome_usuario varchar(255),
senha_usuario varchar(255),
excluido boolean
);
