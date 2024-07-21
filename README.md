# Projeto PCC/TCC - Unifaat 2024

[![Imagem do Projeto](https://i.ibb.co/Xy2x0P8/Design-sem-nome-20240721-002756-0000.png)](https://ibb.co/ZHLz4rm)

## Visão Geral

O Sistema Facilit é uma aplicação robusta projetada para gerenciar a interação com produtos e clientes, permitir a captura de fotos e a geração de relatórios em PDF, além de gerenciar a exibição e consulta de notas fiscais. Este sistema oferece uma solução integrada para otimizar processos empresariais e garantir a eficiência operacional.

## Controladores Principais

### WebcamController

O `WebcamController` é responsável por diversas funcionalidades essenciais:

1. **Verificar Produtos**
   - Consulta produtos no banco de dados e carrega uma lista de produtos.
   - Se não houver produtos no banco, busca produtos na API TinyERP.

2. **Verificar Clientes**
   - Consulta clientes no banco de dados e carrega uma lista de clientes.
   - Se não houver clientes no banco, busca clientes na API TinyERP.

3. **Registro**
   - Verifica se o usuário está logado e chama os métodos de verificação de produtos e clientes.
   - Exibe a página de registro.

4. **Salvar Foto**
   - Recebe dados da imagem, produto selecionado e cliente selecionado.
   - Salva a imagem no disco e registra os dados no banco de dados.

5. **Gerador de PDF**
   - Gera um relatório em PDF com informações de fotos tiradas nos últimos 10 dias.
   - Inclui uma tabela com dados dos usuários, produtos e clientes relacionados às fotos.

### NotasTinyController

O `NotasTinyController` é responsável pelas funcionalidades relacionadas às notas fiscais:

1. **Notas**
   - Verifica se o usuário está logado.
   - Redireciona para a página de login se necessário, ou exibe a página de notas fiscais.

2. **Obter Notas**
   - Recebe informações da nota fiscal.
   - Consulta a nota fiscal na API TinyERP e redireciona o usuário para o link da nota.
   - Exibe uma página de erro se a consulta falhar.

## Autenticação

Ambos os controladores garantem que as funcionalidades estão disponíveis apenas para usuários autenticados, assegurando a segurança e integridade dos dados.

## Integração com API TinyERP

A aplicação se integra com a API TinyERP para buscar dados relevantes de produtos e clientes, garantindo que as informações estejam sempre atualizadas e sincronizadas.

## Requisitos

- .NET Framework
- MySQL
- API TinyERP

## Uso

- Acesse a aplicação e faça login.
- Utilize as funcionalidades de registro de produtos, clientes e captura de fotos.
- Gere relatórios em PDF com base nos dados capturados.
- Consulte notas fiscais através da integração com a API TinyERP.

## Contato

Para mais informações ou suporte, entre em contato com os desenvolvedores:

**Felipe F. dos S. Silva**  
[LinkedIn](https://www.linkedin.com/in/felipesansi)  
[GitHub](https://github.com/felipesansi)  

**Matheus Pinheiro**  
[LinkedIn](https://www.linkedin.com/in/math-pinheiro23)  
[GitHub](https://github.com/math-pinheiro23)