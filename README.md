# Bowling Score Calculator

## Overview

The **Bowling Score Calculator** is a web application designed to calculate the scores of a bowling game based on the input frames. 

This application is built using ASP.NET Core for the backend API and Razor Pages for the frontend. 

The architecture follows the CQRS (Command Query Responsibility Segregation) pattern, ensuring a clear separation between command (write) and query (read) operations.

## Features

- **Score Calculation**: Calculates and displays the total score for a 10-frame bowling game.
- **User Registration and Authentication**: Secure registration and login with JWT-based authentication.
- **Responsive UI**: User-friendly interface for entering scores and viewing the calculated total.
- **Dynamic Form Handling**: Real-time score calculation as users input their rolls.
- **Error Handling and Logging**: Comprehensive error handling and logging for both the API and frontend.

## Technologies Used
- **Backend**: ASP.NET Core Web API, Entity Framework Core, MediatR, FluentValidation
- **Frontend**: Razor Pages, JavaScript
- **Authentication**: JWT (JSON Web Token)
- **Testing**: xUnit, Moq, FluentAssertions
- **Logging**: Microsoft.Extensions.Logging

## Architecture

### Backend (API)

The backend is built using ASP.NET Core and employs the following key components:

- **CQRS Pattern**: Separates the write operations (commands) from the read operations (queries) for better scalability and maintainability.
- **Entity Framework Core**: Used for data access and manipulation.
- **MediatR**: Handles the dispatching of commands and queries.
- **JWT Authentication**: Secures the API endpoints using JSON Web Tokens.
    - **Local Storage**: For simplicity, JWT tokens are stored in the browser's local storage. In practice this would be done in a more secure way.
- **FluentValidation**: Validates the input models.
- **Logging**: Microsoft.Extensions.Logging is used for logging application events, errors, and information. Logging is configured to output to the console and debug windows. Custom logging providers can be added as needed.
- **Error Handling**: 
    - **Global Error Handling Middleware**: Catches unhandled exceptions in the API and returns a standardized error response. Logs errors for debugging and auditing.

### Frontend (Razor Pages)

The frontend is built using Razor Pages and includes:

- **HTML and JavaScript**: Provides the structure and functionality of the UI.
- **Bootstrap**: Ensures a responsive and visually appealing design.
- **AJAX**: Used for asynchronous communication with the backend API.

## Getting Started

### Prerequisites

- .NET 8.0 SDK or later
- SQL Server or a compatible database
- Node.js and npm (for frontend development)

### Installation

1. **Clone the repository**:

    ```bash
    git clone https://github.com/thebriankays/BowlingScoreCalculator.git
    ```

2. **Navigate to the API project**:

    ```bash
    cd BowlingScoreCalculator/BowlingScoreCalculatorAPI
    ```

3. **Install dependencies**:

    ```bash
    dotnet restore
    ```

4. **Configure the database**:

     Edit the Connection String: Update the connection string in appsettings.json under the API project to point to your SQL Server instance.


    ```bash
    dotnet ef database update
    ```

5. **Run the API**:

    ```bash
    dotnet run
    ```

6. **Navigate to the frontend project**:

    ```bash
    cd ../BowlingScoreCalculatorFrontend
    ```

7. **Install frontend dependencies**:

    ```bash
    dotnet restore
    ```

8. **Run the frontend**:

    ```bash
    dotnet run
    ```
9. **Runnign Tests**:  

    ```bash
    cd BowlingScoreCalculatorAPI.Tests
    dotnet test
    ```

## Usage

1. **Open solution in Visual Studio 2022. Start debug session
2. **Register a new user** or **login** with existing credentials.
3. **Enter the scores** for each frame in the bowling form.
4. **View the total score** calculated by the application.
