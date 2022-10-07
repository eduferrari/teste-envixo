# Desafio Envixo 
## Aplicação Web ASP .Net Core, faz cálculo em memória de juros compostos. O projeto possui duas API's:
O objetivo deste teste é verificar as suas habilidades práticas como desenvolvedor .net na construção de APIs REST.
Não é necessário desenvolver as interfaces em HTML/CSS/JS, você deve focar mais em desenvolver as APIs que serão consumidas pelo frontend em React.js.
Você deve construir o projeto todo usando o framework web Spring (https://spring.io/) ou outro framework de desenvolvimento web similar para construção de APIs.
Para banco de dados você deve usar o Postgres e a estrutura das tabelas, nomes, índices fica ao seu critério, será um ponto avaliado no teste também a organização e estruturação do banco de dados que você criará.
Estamos com urgência na contratação, por isso pedimos a gentileza de entregar o teste o quanto antes. Quanto mais rápido você for com qualidade melhor, no cenário real do dia a dia, temos que fazer o nosso melhor com o menor prazo possível.
Segue abaixo as telas do frontend, através dessas telas você deve desenvolver as APIs necessárias para o frontend funcionar conforme descrito abaixo.
O objetivo principal do teste é criar um CRUD de produtos de e-commerce, com informações básicas sobre os produtos cadastrados. Lembre-se: concentre-se somente na construção das APIs.

### Instruções para rodar o projeto:
- Clonar o projeto para o seu pc;
- Abrir o projeto clicando no arquivo "api/api.sln"
- Rodar o projeto usando o "Start Debugging (F5)" no Visual Studio;

### Instruções para a acesso a documentação Swagger:
- Com o projeto rodando "instruções acima" acessar os endpoints usando o seguinte link `https://localhost:44385/swagger/ui/index`;

### Gerar token:
- No Postman execurar o endpoit `https://localhost:44385/api/Token`:
- Será necessário informar um email e uma senha: `eduardo.ferrari@outlook.com > 12345`;
~~~Script
>var form = new FormData();
>form.append("email", "eduardo.ferrari@outlook.com");
>form.append("senha", "12345");
>
>var settings = {
>  "url": "https://localhost:44385/api/Token",
>  "method": "POST",
>  "timeout": 0,
>  "processData": false,
>  "mimeType": "multipart/form-data",
>  "contentType": false,
>  "data": form
>};

>$.ajax(settings).done(function (response) {
>  console.log(response);
>});
~~~

### Exemplo de como listar os produtos
~~~Script
>var form = new FormData();
>var settings = {
>  "url": "https://localhost:44385/api/Produto",
>  "method": "GET",
>  "timeout": 0,
>  "headers": {
>    "Authorization": "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVc2VySW5mbyI6IntcclxuICBcIklkXCI6IDEsXHJcbiAgXCJFbWFpbFwiOiBcImVkdWFyZG8uZmVycmFyaUBvdXRsb29rLmNvbVwiLFxyXG4gIFwiU2VuaGFcIjogXCI4MjdDQ0IwRUVBOEE3MDZDNEMzNEExNjg5MUY4NEU3QlwiLFxyXG4gIFwiTW9kdWxvc1wiOiBbXHJcbiAgICBcIlByb2R1dG9cIixcclxuICAgIFwiQ2F0ZWdvcmlhXCJcclxuICBdLFxyXG4gIFwiVmFsaWRhZGVcIjogXCIyMDIyLTEwLTA4VDAwOjAwOjAwXCJcclxufSIsIm5iZiI6MTY2NTE1OTk0NiwiZXhwIjoxNjY1MjQ2MzQ2LCJpYXQiOjE2NjUxNTk5NDYsImlzcyI6Imh0dHBzOi8vbG9jYWxob3N0OjQ0Mzg1IiwiYXVkIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzODUifQ.iJLm9v2yE-3V7RONwDWRoPQ43lD2sAPkvs77JNIDgdk"
>  },
>  "processData": false,
>  "mimeType": "multipart/form-data",
>  "contentType": false,
>  "data": form
>};
>
>$.ajax(settings).done(function (response) {
>  console.log(response);
>});
~~~
   
