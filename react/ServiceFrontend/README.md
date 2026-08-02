# ServiceFrontend - Service Management & Authentication System

A complete React + TypeScript application built with **Vite**, **Native Fetch API**, **JWT Token Authentication**, **Formik & Yup Validation**, and **React Router v7**.

---

## Features

- **JWT Authentication**: Full login flow with token storage in `localStorage` and route protection.
- **Service Management**: Full CRUD (Create, Read, Update, Delete) against C# .NET Web API (`http://localhost:5090/api`).
- **Dynamic Routing**:
  - `/service/edit/:id` -> Dynamic Edit URL
  - `/service/:serviceNumber` -> Dynamic Detail URL
- **Formik & Yup Validation**: Schema validation for Login and Service Add/Edit forms.
- **Environment Base Path**: Base URL configured via `.env` (`VITE_API_BASE_URL`).

---

## Quick Start

```bash
# 1. Install dependencies
npm install

# 2. Start development server
npm run dev

# 3. Build for production
npm run build
```

---

## Detailed Documentation

See the complete guide at [`../service-frontend-guide.md`](../service-frontend-guide.md) covering the project architecture, step-by-step interview creation workflow, API flow, and common error troubleshooting.
