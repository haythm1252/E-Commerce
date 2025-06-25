# 🛍️ E-Commerce Web App

This is a full-featured e-commerce web application built with **ASP.NET MVC**, designed primarily for grocery and general product sales. It supports both **Admin** and **Customer** roles and includes a dynamic home page, shopping cart, real payment integration, and an admin dashboard.

---

## 🚀 Features

### 👤 User Side
- Register / Login / Logout
- Dynamic home page:
  - Top Categories
  - Best Sellers
  - Category Sections
- Product search and filtering
- Product details with related items
- Shopping cart stored via cookies (GUID-based)
- Checkout flow with:
  - Order summary
  - Paymob payment gateway integration (test mode)
- Profile page with:
  - View & edit personal information
  - View past orders and order details
  - Reset password securely

---

### 🛠 Admin Side
- Admin dashboard with real-time stats:
  - Today's sales & total
  - Last 30 days stats
- Full management system:
  - Categories (mark as top / section)
  - Products (mark as best seller)
  - Users (view data & orders)
  - Orders (verify via Paymob API)
- AJAX-based:
  - Pagination
  - Search and filtering
  - SweetAlert confirmation on deletion

---

## 🧰 Technologies Used

- ASP.NET MVC (with layered Clean-ish Architecture)
- Entity Framework Core
- Identity (User management & auth)
- Paymob Payment Integration (Test Mode)
- AJAX (jQuery + Partial Views)
- Bootstrap 5 + FontAwesome
- SweetAlert2 for confirmation dialogs

---

## 📂 Architecture

- Clean Architecture-inspired layering:
  - Domain
  - Infrastructure
  - Application / Service Layer
  - Web (MVC)
- Repository Pattern + Unit of Work
- Generic Repositories + Specific Repos

---

## 💻 Running the Project Locally

1. Clone the repository:
   ```bash
   git clone https://github.com/your-username/your-repo-name.git
