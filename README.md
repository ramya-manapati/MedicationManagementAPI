# Medication Management API 

A secure and scalable ASP.NET Core API for managing patient medications.

## Features ✅
- User authentication with JWT
- Add, view, update, and delete medications
- Filter medications by date and description
- Secure access (users can only access their data)
- Swagger documentation for easy API testing

## Tech Stack 🛠️
- ASP.NET Core
- Entity Framework Core
- SQL Server
- JWT Authentication
- Swagger (OpenAPI)

## Installation ⚙️
1. **Clone the repo**  
   ```sh
   git clone https://github.com/your-username/medication-api.git
   cd medication-api

2. Configure database (update appsettings.json):
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=MedicationDB;User Id=YOUR_USER;Password=YOUR_PASSWORD;"
}
3. Run migrations
dotnet ef migrations add InitialCreate
dotnet ef database update

4. Run the project

## API Endpoints 📝

User Authentication

POST /api/auth/register → Register a new user
POST /api/auth/login → Login and get a JWT token

Medication Management

POST /api/medications → Add a new medication
GET /api/medications → Get all medications
PUT /api/medications/{id} → Update a medication
DELETE /api/medications/{id} → Delete a medication

## How to Test in Swagger 🛠️

Step - 1 : Run the API (dotnet run)
Step - 2 : Open Swagger UI
Step - 3 : Click "Authorize" and enter your JWT token
Step - 4 : Test API endpoints!

## Contributing 🤝

Fork the repository
Create a new branch (feature-branch)
Commit changes (git commit -m "Added new feature")
Push to GitHub (git push origin feature-branch)
Open a Pull Request 


