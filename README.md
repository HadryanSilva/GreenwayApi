### Para rodar o projeto, siga os passos abaixo:

- Após extrair os arquivos do projeto, execute o comando abaixo na pasta raiz do projeto:
    ```bash 
    docker-compose up -d
  ```
  
- Após a execução do comando anterior, acesse o diretório onde se encontra o **código fonte** **_"/GreenwayApi/GreenwayApi"_**, execute o comando abaixo para executar as migrations de criação das tabelas no banco de dados:
    ```bash 
    dotnet ef database update
    ``` 
  
- Na mesma pasta irei deixar o arquivo "**_insomnia-trabalho-fiap.json_**" com as rotas da aplicação, basta importar o arquivo no insomnia e utilizar as rotas para testar a aplicação.


- Verifique nas rotas os ids utilizados nas requisições para garantir que os dados estão sendo passados corretamente para evitar erros.


- Para conectar no banco de dados localmente basta cadastrar a conexão no DBeaver com as seguintes informações:
    - Host: localhost
    - Port: 5432
    - Database: greenway_db
    - User: postgres
    - Password: postgres