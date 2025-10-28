# 📝 To-Do List API

A clean and modular To-Do List REST API built with C#, .NET Core, and Entity Framework Core.  
This project demonstrates backend development best practices including layered architecture, SOLID principles, and the Repository Pattern.

---

## 🚀 Overview

This project provides a RESTful API for managing a simple To-Do List.  
Users can create, read, update, and delete (CRUD) tasks through structured endpoints.  

It is designed with scalability, maintainability, and clean code principles in mind — making it a great example of professional backend architecture.

---

## 🧠 Architecture & Design

The application follows a three-layer architecture:

### 🔹 Layers Explained
- ToDoList.Core → Defines business logic, models, and repository interfaces.  
- ToDoList.EF → Implements repository interfaces using Entity Framework Core for database access.  
- ToDoList (API) → Exposes REST endpoints that call repository methods through dependency injection.

This structure promotes:
- Separation of concerns  
- Testability  
- Easier maintenance and scalability  

---

## 🧩 Repository Pattern (Implemented)

The project implements the Repository Pattern to abstract data access from the business logic.  
This makes the application loosely coupled and easier to maintain.

### Example:
`csharp
// Core Layer
public interface IToDoRepository
{
    Task<IEnumerable<TodoItem>> GetAllAsync();
    Task<TodoItem?> GetByIdAsync(int id);
    Task AddAsync(TodoItem item);
    Task UpdateAsync(TodoItem item);
    Task DeleteAsync(int id);
    Task SaveAsync();
}

// EF Layer
public class ToDoRepository : IToDoRepository
{
    private readonly AppDbContext _context;
    public ToDoRepository(AppDbContext context) => _context = context;

    public async Task<IEnumerable<TodoItem>> GetAllAsync() => await _context.TodoItems.ToListAsync();
    public async Task<TodoItem?> GetByIdAsync(int id) => await _context.TodoItems.FindAsync(id);
    public async Task AddAsync(TodoItem item) => await _context.TodoItems.AddAsync(item);
    public async Task UpdateAsync(TodoItem item) => _context.TodoItems.Update(item);
    public async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null) _context.TodoItems.Remove(entity);
    }
    public async Task SaveAsync() => await _context.SaveChangesAsync();
}

// API Layer
[ApiController]
[Route("api/[controller]")]
public class ToDoController : ControllerBase
{
    private readonly IToDoRepository _repository;
    public ToDoController(IToDoRepository repository) => _repository = repository;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _repository.GetAllAsync());
}

✅ Result: The Controller never interacts directly with the database — it communicates only with an interface, ensuring clean separation and flexibility.


---

🧰 Tech Stack

Category Technology

Language C#
Framework .NET Core / ASP.NET Core Web API
ORM Entity Framework Core
Database SQL Server (configurable)
Architecture Layered + Repository Pattern
Principles SOLID, OOP, Clean Code
Tools Swagger UI, Dependency Injection



---

⚙️ How It Works

1. The client sends an HTTP request (e.g., POST /api/todo).


2. The Controller receives the request and calls the appropriate Repository method.


3. The Repository interacts with the database through Entity Framework Core.


4. The API returns a JSON response to the client.




---

🧪 Example Endpoints

Method Endpoint Description

GET /api/todo Get all to-do items
GET /api/todo/{id} Get a single to-do
POST /api/todo Create a new to-do
PUT /api/todo/{id} Update an existing to-do
DELETE /api/todo/{id} Delete a to-do

---

🧭 Running the Project

1. Clone the repository
git clone https://github.com/mohamadAlmounir/To-Do-List.git
cd To-Do-List

2. Build and run
dotnet build
dotnet run --project ToDoList

3. Access the api
https://localhost:5001/swagger

---

📸 Preview

Swagger UI



Architecture Diagram




---

💡 Highlights

✅ Implements Repository Pattern and SOLID principles.

✅ Built with Entity Framework Core using Dependency Injection.

✅ Follows a clean, scalable, and testable architecture.

✅ Includes Swagger for interactive API documentation.

✅ Demonstrates strong understanding of RESTful API design.

✅ Excellent sample project for Junior Backend Developer portfolio.



---

🪪 License

This project is open-source under the MIT License.


---

📬 Author

Mohamad Almounir
Backend Developer – C# / .NET
GitHub: @mohamadAlmounir .