# LokalnieEU

LokalnieEU is an ASP.NET Core application that provides APIs for user registration, authentication, and management.

The frontend of the application is built with Svelte and it is located at `\www\svelte-app`.

## Table of Contents
1. [Installation](#installation)
2. [Usage](#usage)
3. [Frontend](#frontend)
4. [APIs](#apis)
5. [License](#license)

## Installation
**Clone the repository**: Clone the LokalnieEU repository to your local machine using the following command in your terminal:

```bash
git clone https://github.com/UnFriend-PL/LokalnieEu.git
```
Set up the database: Set up your database and update the connection string in the appsettings.json file.

Run the application: Navigate to the root directory of the project in your terminal and run the following command:
dotnet run
## Frontend
The frontend of the application is built with Svelte. To run the frontend application, navigate to the \www\svelte-app directory and run the following commands:
Install the dependencies:
```bash
npm install
```
Start the application:
```bash
npm run dev
```
The application will be available at http://localhost:8080.

## APIs
This section should provide a detailed description of the available APIs. It should describe the endpoints, request methods, request parameters, and expected responses. Here is a brief overview based on the provided code:

### Register
Endpoint: POST /api/Users/Register
Description: Registers a new user.

### Login
Endpoint: POST /api/Users/Login
Description: Authenticates a user and returns a token.

### UpdateUser
Endpoint: PUT /api/Users/UpdateUser
Description: Updates a user's details.

### UpdateUserPassword
Endpoint: PUT /api/Users/UpdateUserPassword
Description: Updates a user's password.

## License
This project is intended for educational purposes only. Any commercial use is strictly prohibited without the express written permission from the author.

If you would like to use this project for commercial purposes, please contact the author to discuss licensing options. Unauthorized commercial use is strictly prohibited and any violation of this policy may be subject to legal action.

Please note that this license does not permit you to claim this project as your own, remove or alter any copyright, trademark, or other proprietary rights notice in the project.

By using this project, you agree to the terms of this license.