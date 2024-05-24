<H1>Projeto TCC - Unifaat 2024</H1>

##Sistema Facilit

## Visão Geral
O Sistema Facilit é uma aplicação que gerencia a interação com produtos e clientes, permite a captura de fotos e a geração de relatórios em PDF, além de gerenciar a exibição e consulta de notas fiscais.

## Controladores Principais

### WebcanController
O `WebcanController` é responsável pelas seguintes funcionalidades:

1. **Verificar_produtos**
   - Consulta produtos no banco de dados e carrega uma lista de produtos.
   - Caso não haja produtos no banco, busca produtos da API TinyERP.

2. **Verificar_clientes**
   - Consulta clientes no banco de dados e carrega uma lista de clientes.
   - Caso não haja clientes no banco, busca clientes da API TinyERP.

3. **Registro**
   - Verifica se o usuário está logado e chama os métodos de verificação de produtos e clientes.
   - Exibe a página de registro.

4. **SalvarFoto**
   - Recebe dados da imagem, produto selecionado e cliente selecionado.
   - Salva a imagem no disco e registra os dados no banco de dados.

5. **Gerador_pdf**
   - Gera um relatório em PDF com informações de fotos tiradas nos últimos 10 dias.
   - Inclui uma tabela com dados dos usuários, produtos e clientes relacionados às fotos.

### Notas_tinyController
O `Notas_tinyController` é responsável pelas seguintes funcionalidades:

1. **Notas**
   - Verifica se o usuário está logado.
   - Redireciona para a página de login se necessário, ou exibe a página de notas fiscais.

2. **ObterNotas**
   - Recebe informações da nota fiscal.
   - Consulta a nota fiscal na API TinyERP e redireciona o usuário para o link da nota.
   - Exibe uma página de erro se a consulta falhar.

## Autenticação
Ambos os controladores asseguram que as funcionalidades estão disponíveis apenas para usuários autenticados, garantindo a segurança e integridade dos dados.

## Integração com API TinyERP
A aplicação se integra com a API TinyERP para buscar dados relevantes de produtos e clientes, garantindo que as informações estejam sempre atualizadas e sincronizadas.

## Requisitos
- .NET Framework
- MySQL
- API TinyERP

## Configuração
1. Configure a conexão com o banco de dados MySQL.
2. Adicione as credenciais da API TinyERP.
3. Compile e execute a aplicação.

## Uso
- Acesse a aplicação e faça login.
- Utilize as funcionalidades de registro de produtos, clientes e captura de fotos.
- Gere relatórios em PDF com base nos dados capturados.
- Consulte notas fiscais através da integração com a API TinyERP.

## Licença
Este projeto é licenciado sob os termos da licença MIT.