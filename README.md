# 🎨 Shatbli - AI-Powered Interior Design Platform

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=flat&logo=dotnet)
![Angular](https://img.shields.io/badge/Angular-18-DD0031?style=flat&logo=angular)
![C#](https://img.shields.io/badge/C%23-12.0-239120?style=flat&logo=csharp)
![License](https://img.shields.io/badge/License-MIT-green.svg)

**Shatbli** is an intelligent interior design platform that leverages AI to help homeowners visualize ceramic tiles and paint colors in their actual rooms before making purchase decisions. Upload a room photo, select products, and get realistic AI-generated designs in seconds.

---

## 📋 Table of Contents

- [Features](#-features)
- [Technology Stack](#-technology-stack)
- [System Architecture](#-system-architecture)
- [Project Structure](#-project-structure)
- [Getting Started](#-getting-started)
- [API Documentation](#-api-documentation)
- [Database Schema](#-database-schema)
- [AI Processing Pipeline](#-ai-processing-pipeline)
- [Deployment](#-deployment)
- [Contributing](#-contributing)
- [License](#-license)

---

## ✨ Features

### 🎯 Core Capabilities

- **AI-Powered Design Generation**: Transform room photos with realistic ceramic tile and paint visualizations
- **Multiple Design Modes**:
  - Ceramic floor visualization using catalog products
  - Custom ceramic tile upload
  - Combined ceramic + paint designs
  - Paint-only wall transformations
- **Smart Product Catalog**: Browse extensive ceramic tile collections with category filtering
- **Subscription-Based Access**: Free, Basic, and Premium plans with varying limits
- **Real-Time Processing**: Background job system for efficient AI computations
- **Cloud Storage Integration**: Seamless Cloudinary integration for media management

### 👥 User Features

- **Secure Authentication**: JWT-based authentication with role management
- **Design Management**: Save, view, and delete generated designs
- **Subscription Control**: Track usage limits and upgrade plans
- **Instant Previews**: View generated designs immediately with auto-cleanup

### 🛠️ Admin Features

- **Product Management**: Bulk import ceramic tiles via Excel
- **User Administration**: Monitor subscriptions and usage statistics
- **Analytics Dashboard**: Track AI processing logs and system performance

---

## 🚀 Technology Stack

### Backend (.NET 8)

| Technology | Purpose |
|------------|---------|
| **ASP.NET Core 8** | RESTful API framework |
| **Entity Framework Core** | ORM for database operations |
| **MediatR** | CQRS pattern implementation |
| **FluentValidation** | Request validation |
| **JWT Bearer** | Authentication & authorization |
| **Hangfire** | Background job processing |
| **BCrypt.Net** | Password hashing |
| **Cloudinary SDK** | Cloud storage service |
| **Swagger/OpenAPI** | API documentation |

### Frontend (Angular)

| Technology | Purpose |
|------------|---------|
| **Angular 18+** | Progressive web framework |
| **TypeScript** | Type-safe JavaScript |
| **RxJS** | Reactive programming |
| **Angular Router** | Client-side routing |
| **HttpClient** | API communication |

### AI/ML Processing

| Technology | Purpose |
|------------|---------|
| **Python** | AI service runtime |
| **Computer Vision Libraries** | Image segmentation & processing |
| **Neural Networks** | Design generation models |
| **Flask/FastAPI** | AI microservice API *(inferred)* |

### Database

- **SQL Server**: Primary relational database
- **Entity Framework Migrations**: Schema version control

### DevOps & Tools

- **Git**: Version control
- **Visual Studio 2026**: Primary IDE
- **Swagger UI**: Interactive API testing

---

## 🏗️ System Architecture

### Clean Architecture Layers
┌─────────────────────────────────────────────────┐ │              Presentation Layer                 │ │        (Shatabli API Controllers)               │ ├─────────────────────────────────────────────────┤ │           Application Layer                     │ │     (Features, Commands, Queries, DTOs)         │ ├─────────────────────────────────────────────────┤ │              Domain Layer                       │ │    (Entities, Enums, Base Classes)              │ ├─────────────────────────────────────────────────┤ │          Infrastructure Layer                   │ │  (Database, Services, External APIs)            │ └─────────────────────────────────────────────────┘

### Microservices Communication
┌──────────────┐      HTTP/REST      ┌──────────────┐ │   Angular    │ ←─────────────────→ │   .NET API   │ │   Frontend   │   JSON Responses     │   Backend    │ └──────────────┘                      └──────┬───────┘ │ │ HTTP │ ┌──────▼───────┐ │   AI Python  │ │   Service    │ └──────────────┘

---

## 📁 Project Structure
Shatbli/ │ ├── Shatabli/                           # Main API Project │   ├── Controllers/                    # API Endpoints │   │   ├── DesignController.cs        # Design generation endpoints │   │   └── ProductController.cs       # Product management │   ├── Program.cs                      # Application entry point │   └── appsettings.json               # Configuration settings │ ├── Shatabli.Core.Domain/              # Domain Layer │   ├── Entities/                      # Business entities │   │   ├── User.cs                    # User entity │   │   ├── Design.cs                  # Design entity │   │   ├── Product.cs                 # Ceramic product entity │   │   ├── SubscriptionPlan.cs        # Subscription plans │   │   ├── UserSubscription.cs        # User subscriptions │   │   └── AIProcessingLog.cs         # AI tracking │   ├── Enums/                         # Domain enumerations │   │   ├── UserRole.cs                # Admin/Homeowner roles │   │   ├── PlanType.cs                # Free/Basic/Premium │   │   ├── DesignStatus.cs            # Pending/Completed/Failed │   │   └── ProductCategory.cs         # Ceramic categories │   └── Common/                        # Base classes │       └── BaseEntity.cs              # Audit fields │ ├── Shatabli.Core.Application/         # Application Layer │   ├── Features/                      # CQRS features │   │   ├── Designs/                   # Design operations │   │   │   ├── Commands/              # Create, Save, Delete │   │   │   └── Queries/               # Get designs │   │   ├── Products/                  # Product operations │   │   ├── Users/                     # Authentication │   │   └── Subscriptions/             # Plan management │   ├── Interfaces/                    # Service contracts │   │   ├── IGenerateRoomImageService.cs │   │   ├── IStorageService.cs │   │   ├── ITokenService.cs │   │   └── IDesignBackgroundJobService.cs │   └── DTOs/                          # Data transfer objects │ ├── Shatabli.Infrastructure/           # Infrastructure Layer │   ├── Context/                       # Database context │   │   └── ApplictionDbContext.cs     # EF Core DbContext │   ├── Data/                          # Seeders & migrations │   │   └── DatabaseSeeder.cs          # Initial data setup │   ├── Services/                      # External services │   │   ├── CloudinaryService.cs       # Image storage │   │   ├── TokenService.cs            # JWT generation │   │   ├── ClaimsService.cs           # User claims │   │   └── DesignBackgroundJobService.cs │   ├── Middleware/                    # Custom middleware │   │   └── GlobalExceptionHandlerMiddleware.cs │   └── Migrations/                    # EF Core migrations │ ├── Frontend/                          # Angular Application │   ├── src/ │   │   ├── app/                       # Application modules │   │   │   ├── components/            # UI components │   │   │   ├── services/              # HTTP services │   │   │   ├── guards/                # Route guards │   │   │   ├── interceptors/          # HTTP interceptors │   │   │   └── models/                # TypeScript interfaces │   │   ├── assets/                    # Static resources │   │   └── environments/              # Environment configs │   ├── angular.json                   # Angular configuration │   └── package.json                   # NPM dependencies │ └── AI-Service/                        # Python AI Microservice ├── models/                        # Trained ML models ├── app.py                         # Flask/FastAPI server ├── requirements.txt               # Python dependencies └── utils/                         # Image processing utilities

---

## 🎯 Getting Started

### Prerequisites

- **.NET 8 SDK** - [Download](https://dotnet.microsoft.com/download/dotnet/8.0)
- **Node.js 18+** & **npm** - [Download](https://nodejs.org/)
- **SQL Server** (LocalDB, Express, or full version)
- **Python 3.9+** - For AI service
- **Visual Studio 2022+** or **VS Code**
- **Git** - Version control

### Backend Setup

1. **Clone the repository**
git clone https://github.com/saeed-owais/Shatbli.git cd Shatbli 
2. **Configure Database Connection**
   
   Update `appsettings.json`:{ "ConnectionStrings": { "DefaultConnection": "Server=(localdb)\mssqllocaldb;Database=ShatabliDB;Trusted_Connection=true;TrustServerCertificate=true;" } }
   
3. **Configure JWT Settings** { "Jwt": { "SecretKey": "YourSecretKey_MinimumLength32Characters!", "Issuer": "ShatabliAPI", "Audience": "ShatabliClient", "ExpirationDays": 7 } }

4. **Configure Cloudinary** (Optional - for production)

Update`CloudinaryService.cs` or use configuration: { "Cloudinary": { "CloudName": "your_cloud_name", "ApiKey": "your_api_key", "ApiSecret": "your_api_secret" } }
5. **Configure Default Admin** { "DefaultAdmin": { "Email": "admin@shatabli.com", "Password": "Admin@123", "FullName": "System Administrator", "PhoneNumber": "+1234567890", "ForcePasswordUpdate": false } }

6. **Apply Database Migrations**
cd Shatabli dotnet ef database update

*Database seeder will automatically create:*
- Subscription plans (Free, Basic, Premium)
- Admin user with Premium subscription

7. **Run the API**
dotnet run --project Shatabli

API will be available at: `https://localhost:5001`
Swagger UI: `https://localhost:5001/swagger`

### Frontend Setup

1. **Navigate to Frontend directory**

cd Frontend
2. **Install dependencies**
npm install
3. **Configure API URL**
Update `src/environments/environment.ts` and `environment.prod.ts`:
export const environment = { production: false, apiUrl: 'https://localhost:5001/api' };
4. **Run the Angular application**
npm start
The frontend will be available at: `http://localhost:4200`
### AI Service Setup
1. **Navigate to AI-Service directory**
 cd AI-Service
2. **Create virtual environment**
python -m venv venv
source venv/bin/activate  # On Windows: venv\Scripts\activate
3. **Install dependencies**
pip install -r requirements.txt
4. **Run the AI service**
python app.py
AI service will be available at: `http://localhost:5000`
## 🤖 AI Processing Pipeline

### Workflow

1. **Request Initiation**
   - User uploads room photo + selects ceramic/paint
   - Frontend sends multipart form data to .NET API
   - Backend validates subscription limits

2. **Pre-Processing**
   - Images converted to byte arrays
   - Temporary files created in `/wwwroot/temp-images`
   - Background job scheduled for cleanup (5 minutes)

3. **AI Service Call**
   - .NET backend sends HTTP request to Python AI service
   - Payload includes room image + ceramic image + design type
   - AI service performs:
     - Image segmentation (floor/wall detection)
     - Perspective analysis
     - Ceramic pattern application with realistic lighting/shadows
     - Paint color overlay with proper blending

4. **Post-Processing**
   - AI returns generated image bytes
   - Backend saves to temp storage
   - Response sent to frontend with image path
   - User can preview and choose to save

5. **Permanent Storage**
   - User clicks "Save Design"
   - Image uploaded to Cloudinary
   - Database record updated with permanent URL
   - Temp file deleted immediately

6. **Auto-Cleanup**
   - Hangfire job runs after 5 minutes
   - Deletes unsaved designs from database
   - Removes temp files from local storage

---

## 🔒 Security Features

- **JWT Authentication**: Stateless token-based auth with 7-day expiration
- **Role-Based Authorization**: Admin and Homeowner roles with distinct permissions
- **Password Hashing**: BCrypt with salt for secure password storage
- **Global Exception Handling**: Standardized error responses with proper HTTP status codes
- **Input Validation**: FluentValidation for all incoming requests
- **CORS Configuration**: Configurable cross-origin policies
- **HTTPS Enforcement**: Redirect HTTP to HTTPS in production

---

## 📊 Subscription Plans

| Feature | Free | Basic | Premium |
|---------|------|-------|---------|
| **Price** | $0/month | $30/month | $100/month |
| **Images/Month** | 10 | 200 | Unlimited |
| **Images/Day** | 3 | 20 | Unlimited |
| **Watermark** | ✅ Yes | ❌ No | ❌ No |
| **Priority Processing** | ❌ No | ❌ No | ✅ Yes |
| **Custom Uploads** | ✅ Yes | ✅ Yes | ✅ Yes |
| **Design History** | ✅ Yes | ✅ Yes | ✅ Yes |

---

## 🤝 Contributing

Contributions are welcome! Please follow these guidelines:

1. **Fork the repository**
2. **Create a feature branch** (`git checkout -b feature/AmazingFeature`)
3. **Commit your changes** (`git commit -m 'Add some AmazingFeature'`)
4. **Push to the branch** (`git push origin feature/AmazingFeature`)
5. **Open a Pull Request**

### Code Standards
- Follow Clean Architecture principles
- Use CQRS pattern for new features
- Write unit tests for business logic
- Document all public APIs
- Use meaningful variable names
- Add XML comments for complex methods

---

## 📝 License

This project is licensed under the **MIT License** - see the [LICENSE](LICENSE) file for details.

---

## 👥 Authors

- **Saeed Owais** - *Lead Developer* - [@saeed-owais](https://github.com/saeed-owais)

---

## 🙏 Acknowledgments

- Clean Architecture pattern by Robert C. Martin
- CQRS implementation with MediatR
- Cloudinary for image storage solutions
- Microsoft .NET team for excellent documentation
- Angular team for modern frontend framework

---

## 📞 Support

For questions or issues:
- **GitHub Issues**: [Create an issue](https://github.com/saeed-owais/Shatbli/issues)
- **Email**: support@shatbli.com
- **Documentation**: Check Swagger UI at `/swagger`

---

## 🗺️ Roadmap

### Phase 1 (Current)
- ✅ Core design generation features
- ✅ Subscription management
- ✅ User authentication
- ✅ Product catalog

### Phase 2 (Upcoming)
- [ ] 3D room modeling
- [ ] Mobile applications (iOS/Android)
- [ ] Social sharing features
- [ ] AI design recommendations
- [ ] E-commerce integration
- [ ] Multi-language support

### Phase 3 (Future)
- [ ] Virtual reality walkthroughs
- [ ] Interior designer collaboration tools
- [ ] Expanded product categories (furniture, lighting)
- [ ] Real-time collaboration
- [ ] Advanced analytics dashboard

---

<div align="center">

**Made with ❤️ using .NET 8, Angular, and AI**

⭐ Star this repository if you find it helpful!

</div>
