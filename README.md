# 🏨 Hotel API

## 🖥 Sobre o projeto

Projeto desenvolvido durante a aceleração C# na Trybe, em Julho de 2024. Trata-se de uma aplicação do tipo WebAPI com a capacidade de realizar operações básicas de um sistema de booking de uma rede de hotéis controlando os hotéis em diversas cidades e os registros de reservas de pessoas clientes. A aplicação possui rotas de autenticação e autorização e realiza conexão com uma api externa para auxiliar na busca de hotéis mais próximos de acordo com o endereço fornecido pelo usuário.

## 🛠 Tecnologias e Libs utilizadas

As seguintes ferramentas foram utilizadas na construção do projeto:

- [ASP.NET Core](https://dotnet.microsoft.com/en-us/apps/aspnet)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/)
- [Entity Framework Core](https://learn.microsoft.com/pt-br/ef/core/)

## ↗️ Endpoints

Obs: Caso opte por fazer o download do projeto, você poderá observar os endpoints com mais detalhe
no link: `https://localhost:5001/swagger/index.html`

### Login

```html
  /login
```

- Utiliza o método POST, Realiza o login de usuário existente no banco de dados.
- Caso a requisição seja feita corretamente, retorna um objeto com um token de autorização, com o status http `200`. O token será necessário em outras requisições.
- Caso email e/ou senha estejam incorretos, retorna uma mensagem de erro, com o status hhtp `401`.
- O corpo da requisição deve obedecer o seguinte formato:

```json
  {
    "email": "string",
    "password": "string"
  }
```

### City

```html
/city
```

- Utilizando o método GET
  - retorna uma lista com as cidades presentes no banco de dados, com o status http `200`em caso de sucesso.

- Utilizando o método POST
  - Adiciona uma nova cidade ao banco de dados.
  - Caso o corpo da requisição esteja inválido, retorna uma mensagem de erro com o status http `400`.
  - Em caso de sucesso, retorna um objeto com os dados da cidade registrada e com o status http `201`.
  - O corpo da requisição deve obedecer o seguinte formato:

  ```json
    {
    "name": "string",
    "state": "string"
    }
  ```

- Utilizando o método PUT
  - Atualiza as informações de uma cidade presente no banco de dados.
  - Em caso de sucesso, retorna um objeto com os dados da cidade atualizado.
  - Caso o corpo da requisição esteja inválido, retorna uma mensagem de erro com o status http `400`.
  - Caso a aplicação não encontre uma cidade com o id especificado no corpo da requisição, retorna uma mensagem de erro com o status http `404`

### Hotel

```html
/hotel
```

- Utilizando o método GET
  - Responsável por listar os hotéis presentes no banco de dados, com um status http `200`.

- Utilizando o método POST
  - Adiciona um novo hotel ao banco de dados. Autorização de 'Admin' necessária para a operação.
  - Em caso de sucesso, retorna o status http `201` e um objeto com os dados do hotel adicionado ao banco de dados.
  - Caso o corpo da requisição esteja inválido, retorna uma mensagem de erro com o status http `400`.
  - Caso o usuário não possua a autorização necessária ou caso a mesma esteja inválida, retorna uma mensagem de erro com o status http `401`.
  - Se as entidades referenciadas não forem encontradas, retorna uma mensagem de erro com o status http `404`.
  - O corpo da requisição deve obedecer o seguinte formato:

  ```json
    {
      "name": "string",
      "address": "string",
      "cityId": 0
    }
  ```

### Room

```html
  /room/:HotelId
```

- Utiliza o método GET, em caso de sucesso retorna um status http `200`, junto de uma lista de quartos do hotel especificado pelo id na url.
- Caso não encontre o hotel especificado no banco de dados, retorna uma mensagem de erro com um status http `404`.

```html
  /room
```

- Utiliza o método POST, Adiciona um novo quarto ao banco de dados. Autorização de 'Admin' necessária para a operação.
- Em caso de sucesso, retorna um objeto com os dados do quarto inserido, junto do status http `201`.
- Caso o corpo da requisição esteja inválido, retorna uma mensagem de erro com o status http `400`.
- Caso o usuário não possua a autorização necessária ou caso a mesma esteja inválida, retorna uma mensagem de erro com o status http `401`.
- Se as entidades referenciadas não forem encontradas, retorna uma mensagem de erro com o status http `404`.
- O corpo da requisição deve obedecer o seguinte formato:

```json
  {
    "name": "string",
    "capacity": 0,
    "image": "string",
    "hotelId": 0
  }
```

```html
  /room/:RoomId
```

- Utiliza o método DELETE. Remove um quarto do banco de dados. Autorização de 'Admin' necessária para a operação.
- Caso a operação seja bem sucedida, retorna um status `204`.
- Caso o usuário não possua a autorização necessária ou caso a mesma esteja inválida, retorna uma mensagem de erro com o status http `401`.
- Se não for encontrado um quarto com o id especificado, retorna uma mensagem de erro com um status http `404`.

### User

```html
  /user
```

- Utilizando o método GET
  - Retorna uma lista com todos os usuários registrados no banco de dados. Autorização de 'Admin' necessária para a operação.
  - Em caso de sucesso, retorna um status http `200` com uma lista de usuários presentes no banco de dados.
  - Se o usuário não possuir a autorização necessária ou caso a mesma esteja inválida, retorna uma mensagem de erro com um status http `401`.

- Utilizando o método POST
  - Adiciona um usuário ao banco de dados. Por padrão todo usuário é registrado com userType 'client'
  - Em caso de sucesso, retorna um status http `201` com as informações do usuário criado.
  - se o email a ser registrado já existe no banco de dados, retorna uma mensagem de erro com status http `409`.
  - O corpo da requisição deve obedecer o seguinte formato:
  
  ```json
  {
    "name": "string",
    "email": "string",
    "password": "string"
  }
  ```

### Booking

```html
/booking/:BookingId
```

- Utiliza o método GET, endpoint responsável por listar uma única reserva. O usuário que quiser acessar a reserva deve ser o mesmo que a criou.
- Em caso de sucesso, retorna um status http `200` junto de um objeto com os dados da reserva.
- Se o usuário não estiver autenticado ou se as informações da reserva forem acessadas por um usuário que não a criou, retorna uma mensagem de erro com um status http `401`.

```html
/booking
```

- Utiliza o método POST, Adiciona uma nova reserva ao banco de dados. Autorização de 'Client' necessária para a operação.
- Em caso de sucesso, retorna um status http `201` e um objeto com os dados da reserva criada.
- Caso o corpo da requisição esteja inválido, retorna uma mensagem de erro com o status http `400`.
- Se o usuário não possuir a autorização necessária ou caso a mesma esteja inválida, retorna uma mensagem de erro com o status http `401`.
- Se o quarto referenciado no corpo da requisição não for encontrado, retorna uma mensagem de erro com o status http `404`.
- Se o número de hospedes for maior que a capacidade do quarto, retorna uma mensagem de erro com o status http `409`.
- O corpo da requisição deve obedecer o seguinte formato:

```json
  {
    "checkIn": "2024-07-22T23:14:00.035Z",
    "checkOut": "2024-07-22T23:14:00.035Z",
    "guestQuant": 0,
    "roomId": 0
  }
```

### Geo

```html
  /geo/status
```

- Utiliza o método GET, Retorna o status da api externa.
- Em caso de sucesso, a requisição retorna o status http `200` e um objeto com o status da api externa.
- Caso não seja obtida resposta, retorna uma mensagem de erro com o status http `401`.

```html
  /geo/address
```

- Utiliza o método GET, Retorna uma lista de hotéis ordenados por distância de um endereço (ordem crescente de distância).
- Em caso de sucesso, a resposta contém a lista de hotéis e o status http `200`.
- O corpo da requisição deve obedecer o seguinte formato:

```json
  {
    "Address":"string",
    "City":"string",
    "State":"string"
  }
```

## 👾Autor

 <a href="https://github.com/Gui-lfm">
 <img style="border-radius: 50%;" src="https://avatars.githubusercontent.com/u/72154970?v=4" width="100px;" alt=""/>
 <br />
 <sub><b>Guilherme Lucena</b></sub></a>

### ✉contato

<div>
  <a href="mailto:guilherme.lucena17@gmail.com" target="_blank"><img src="https://img.shields.io/badge/Gmail-D14836?style=for-the-badge&logo=gmail&logoColor=white" target="_blank"/></a>
  <a href="https://www.linkedin.com/in/gui-lucena/" target="_blank"><img src="https://img.shields.io/badge/LinkedIn-0077B5?style=for-the-badge&logo=linkedin&logoColor=white" target="_blank"/></a>
</div>
