# 🛒 E-Commerce Web App

A full-featured e-commerce platform built with **ASP.NET MVC**, focused on grocery & general product sales. It supports both **Admin** and **Customer** roles, a dynamic homepage, shopping cart, order management, and real **Paymob** payment integration (test mode).

Linkedin post with video: https://www.linkedin.com/posts/haythm-ibrahim_id-like-to-share-my-latest-project-a-full-stack-activity-7343682564244606976-WYR2?utm_source=share&utm_medium=member_desktop&rcm=ACoAAC2G9-sBG89X5TLl0Oq44BvH73IRCqyUn8o
---

## 🚀 Features

### 👤 Customer Side
- Register / Login / Logout
- Dynamic Home Page (controlled by Admin):
  - Top Categories
  - Best Sellers
  - Category-based Product Sections
- Product Search (AJAX + Pagination)
- Product Details with Related Items
- Add to Cart (AJAX)
- Shopping Cart:
  - Quantity updates via AJAX
  - Cart stored with cookie & GUID (30 days)
  - Merges with user account on login
- Checkout:
  - Order summary
  - Paymob Payment Integration
- Profile:
  - View & update details
  - Change password
  - View order history

### 🛠 Admin Side
- Dashboard with daily/monthly order stats
- Manage Categories & Products:
  - Mark categories for homepage (in top categories section /display it as sperated section on the home page)
  - Mark products as Best Sellers
- User Management:
  - List, search, and view users and their orders
- Orders:
  - Filter by status/date
  - Verify real payment status via Paymob API
  - One-click verification for all pending orders
- All tables:
  - AJAX pagination & search
  - SweetAlert2 delete confirmations

---

## 🧰 Technologies

- ASP.NET Core 9 MVC
- Entity Framework Core (Code-First)
- ASP.NET Identity
- Paymob API (Test Mode)
- Bootstrap 
- AJAX & Partial Views
- SweetAlert2

---

## 🧱 Architecture

Follows a **Clean Architecture-inspired** structure:

- Domain Layer  
- Application Layer  
- Infrastructure Layer  
- Web (MVC)

Includes:
- Generic & Custom Repositories
- Unit of Work Pattern
- Service Layer

> 🧪 Still evolving. A solid starting point for Clean Architecture.

---

## 📌 Notes

- 🔒 Paymob integration is **test mode only**, no real transactions.
- ✅ Order verification uses actual Paymob API calls (not mock).
- ⚙️ Admin fully controls homepage content (dynamic sections).
- 🧠 Cart uses GUID stored in cookie (30 days) and links to user on login.
- 🔍 Product search and all tables use AJAX (search + pagination).
- 🛠 The project uses real architectural patterns (UoW, Repos, Services).
