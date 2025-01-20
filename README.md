# Cat Facts

Welcome to the **Cat Facts** project! This application is designed to provide cool facts about cats while leveraging modern development practices.

## Features

- **Interactive API**: Offers endpoints for fetching cat facts.
- **Scheduled Jobs**: Automatically fetches and updates cat facts periodically using Hangfire.
- **User Engagement**: Tracks likes, dislikes, and fact occurrence counts.
- **Responsive Design**: Accessible from both desktop and mobile devices.

## Technologies Used

### Backend

- **.NET 8**: Backend framework for APIs and business logic.
- **Entity Framework Core**: ORM for database interactions.
- **Hangfire**: Task scheduler for background jobs.
- **Serilog**: More structured logging messages.
- **Sentry**: Centralized error tracking.

### Frontend

- **Vue.js**: Modern JavaScript framework for user interface.
- **Vuetify**: Material Design components.
- **Pinia**: State management for Vue.
- **Axios**: HTTP requests handling.
- **io-ts**: Runtime type validation for TypeScript.

### Database

- **SQL Server**: Relational database for storing cat facts and related metadata.

## Getting Started

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/)
- [SQL Server](https://www.microsoft.com/sql-server/)

### Installation

1. Clone the repository:

   ```bash
   git clone https://github.com/M4dCh34t3r/cat-facts.git
   cd cat-facts
   ```

2. Setup the database:

   - Ensure SQL Server is running.
   - Update the connection string in both `appsettings.json` files in the backend hosts.

3. Install backend dependencies:

   ```bash
   cd server-side
   dotnet restore
   ```

4. Apply database migrations:

   ```bash
   cd server-side
   dotnet ef database update --project Infrastructure --startup-project WebAPIHost
   ```

5. Install frontend dependencies:
   ```bash
   cd client-side
   npm install
   ```

### Running the Application

#### Backend

Fetch some data from the external API:

```bash
cd server-side
dotnet run --project WorkerServiceHost
```

Start the backend server using the https profile (required for secure communication with the client-side proxy):

```bash
cd server-side
dotnet run --project WebAPIHost --launch-profile https
```

#### Frontend

Start the frontend development server:

```bash
npm run dev
```

Access the application at `http://localhost:5173/#/`.

## API Endpoints

### Cat Facts

- **GET** `/api/fact` - Retrieve all (paginated) cat facts.
- **POST** `/api/fact/{id}/dislike` - Dislike an existing cat fact.
- **POST** `/api/fact/{id}/like` - Like an existing cat fact.

## Scheduled Jobs

The project uses Hangfire to manage recurring tasks, such as:

- Fetching new cat facts from external APIs

Jobs are configured to run hourly... unless you are in development mode, in this case, the job runs minutely

## License

This project is licensed under the MIT License. See the `LICENSE` file for details.
