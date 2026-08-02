# SampleApiCall - React Router & Mock API Call Demo

This project demonstrates how to build a clean React + TypeScript application with **React Router v7** and execute **GET**, **POST**, **PUT**, and **DELETE** HTTP calls using both **Native Fetch API** and **Axios** (with Interceptors).

---

## Clean Architecture & Router Setup

- **`App.tsx`**: Kept completely minimal & clean (zero UI logic, only wraps `AppRoutes` in `BrowserRouter`).
- **Routes Overview**:
  - `/` or `/posts` -> **Listing Screen (Default)**: Displays post list with view, edit, and delete actions.
  - `/posts/new` -> **Add Post Screen**: Form submitting via `POST` API call.
  - `/posts/edit/:id` -> **Edit Post Screen**: Pre-filled form submitting via `PUT` API call.
  - `/posts/:id/:titleSlug` -> **Post Details Screen**: Dynamic URL incorporating the slugified post title.

---

## Quick Start

```bash
# 1. Install dependencies (axios & react-router-dom)
npm install

# 2. Start development server
npm run dev

# 3. Build for production
npm run build
```

---

## Comprehensive Guide

Refer to [`../sample-api-call-guide.md`](../sample-api-call-guide.md) for full step-by-step interview preparation documentation.
