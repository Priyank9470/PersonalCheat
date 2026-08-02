# React Project Setup & Common Errors Cheatsheet

A comprehensive, step-by-step interview preparation guide covering how to create React projects using modern tools, run and build them, customize `package.json` commands, keep the initial codebase clean, and debug common setup and build errors.

---

## 1. Prerequisites Verification

Before creating a React project, ensure Node.js and npm are installed on your machine.

```bash
# Check Node.js version (Recommended: Node 18+ or 20+)
node -v

# Check npm version (Recommended: npm 9+)
npm -v
```

---

## 2. How to Create a React Project

### Method 1: Using Vite (Modern Industry Standard - Recommended)
Vite is the modern, extremely fast build tool for React single-page applications.

```bash
# 1. Create a new React project using Vite
npm create vite@latest {ProjectName}

# For TypeScript version:
# npm create vite@latest my-react-app -- --template react-ts

# 2. Navigate into the project directory
cd my-react-app

# 3. Install required node dependencies
npm install

# 4. Start the development server
npm run dev
```

---

### Method 2: Using Next.js (For Full-Stack / SSR React Framework)
Next.js is the official recommended framework by the React team for production apps requiring Server-Side Rendering (SSR) or Static Site Generation (SSG).

```bash
# 1. Create Next.js project
npx create-next-app@latest my-next-app

# 2. Follow interactive prompts (App Router, Tailwind, TypeScript, etc.)
cd my-next-app

# 3. Start development server
npm run dev
```

---

### Method 3: Using Create React App (CRA - Legacy Context)
> **Interview Note:** `create-react-app` is now officially deprecated by the React team. Vite or Next.js are preferred in modern workflows.

```bash
# Legacy command (slow build times, unmaintained)
npx create-react-app my-app
cd my-app
npm start
```

---

## 3. Creating a Clean Project Setup (No Extra Components)

When initializing a new React project for standard practice or interview setups, keep the project clean and minimal **without adding any custom components** (like Buttons, Headers, or Footers).

### Clean Vite Project Structure

```text
my-react-app/
├── node_modules/
├── public/
│   └── vite.svg
├── src/
│   ├── App.css
│   ├── App.jsx
│   ├── index.css
│   └── main.jsx
├── .gitignore
├── index.html
├── package.json
└── vite.config.js
```

### 1. Minimal `src/App.jsx`
Remove demo state, counter logic, and extra boilerplate SVG imports:

```jsx
import './App.css';

function App() {
  return (
    <div>
      <h1>React App initialized successfully</h1>
    </div>
  );
}

export default App;
```

### 2. Minimal `src/main.jsx`
Keep standard entry point logic intact:

```jsx
import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import App from './App.jsx';
import './index.css';

createRoot(document.getElementById('root')).render(
  <StrictMode>
    <App />
  </StrictMode>
);
```

---

## 4. How to Run, Build, and Preview the Project

| Action | Command (Vite) | Command (CRA) | Command (Next.js) | Description |
| :--- | :--- | :--- | :--- | :--- |
| **Start Dev Server** | `npm run dev` | `npm start` | `npm run dev` | Starts local dev server with Hot Module Replacement (HMR). |
| **Production Build** | `npm run build` | `npm run build` | `npm run build` | Bundles and optimizes code into static files (`dist` or `build`). |
| **Preview Build** | `npm run preview` | `npx serve -s build` | `npm start` | Runs a local server to test the production build before deployment. |

---

## 5. How to Update & Customize Commands in `package.json`

The `scripts` block in `package.json` maps short CLI aliases to execution commands.

### Standard `package.json` Scripts (Vite Default)

```json
"scripts": {
  "dev": "vite",
  "build": "vite build",
  "lint": "eslint .",
  "preview": "vite preview"
}
```

---

### Customization Scenarios & Solutions

#### Scenario A: Change Default Port Number
* **Vite**: Default is `5173`. To change it to `3000`:
  ```json
  "scripts": {
    "dev": "vite --port 3000"
  }
  ```
  *Alternatively, edit `vite.config.js`:*
  ```javascript
  import { defineConfig } from 'vite';
  import react from '@vitejs/plugin-react';

  export default defineConfig({
    plugins: [react()],
    server: {
      port: 3000,
      open: true // Auto-opens browser on server launch
    }
  });
  ```
* **Create React App (CRA)**:
  ```json
  "scripts": {
    "start": "PORT=3000 react-scripts start"
  }
  ```
  *(On Windows PowerShell, use `set PORT=3000 && react-scripts start` or `cross-env PORT=3000 react-scripts start`).*

* **Next.js**:
  ```json
  "scripts": {
    "dev": "next dev -p 3000"
  }
  ```

---

#### Scenario B: Auto-Open Browser on Startup
```json
"scripts": {
  "dev": "vite --open"
}
```

---

#### Scenario C: Expose Local Dev Server to Network (Mobile Testing)
To view the site from another device on the same local Wi-Fi:
```json
"scripts": {
  "dev": "vite --host"
}
```

---

#### Scenario D: Environment-Specific Builds (Staging vs Production)
```json
"scripts": {
  "dev": "vite",
  "build:staging": "vite build --mode staging",
  "build:prod": "vite build --mode production"
}
```

---

#### Scenario E: Adding Code Quality Scripts (Linting & Formatting)
```json
"scripts": {
  "dev": "vite",
  "build": "vite build",
  "lint": "eslint . --ext js,jsx --report-unused-disable-directives --max-warnings 0",
  "format": "prettier --write \"src/**/*.{js,jsx,css,json}\"",
  "typecheck": "tsc --noEmit"
}
```

---

## 6. Common Errors and Solutions

### Error 1: `npm ERR! code ERESOLVE` / Peer Dependency Conflict
* **Symptom:** Terminal throws dependency resolution errors during `npm install`.
* **Cause:** Installed packages require conflicting versions of React or shared sub-dependencies.
* **Solution:**
  ```bash
  # Bypass strict peer dependency checks
  npm install --legacy-peer-deps

  # Or force installation
  npm install --force
  ```

---

### Error 2: `Port 5173 is already in use` / `Port 3000 is already in use`
* **Symptom:** Server starts on a random port (e.g., 5174) or fails immediately.
* **Cause:** Another running process or abandoned terminal session is utilizing the port.
* **Solution:**
  1. Kill the process running on that port:
     * **Windows (PowerShell):**
       ```powershell
       Get-Process -Id (Get-NetTCPConnection -LocalPort 5173).OwningProcess | Stop-Process
       ```
     * **macOS / Linux:**
       ```bash
       npx kill-port 5173
       ```
  2. Or explicitly pass a different port in `package.json` (`vite --port 3001`).

---

### Error 3: `Module not found: Can't resolve '...'` / `Failed to resolve import`
* **Symptom:** Build/Dev server fails with missing module errors.
* **Causes:**
  1. `node_modules` not installed yet.
  2. Case-sensitivity mismatched in import paths (e.g., `./component` vs `./Component`).
  3. Missing `.jsx` extension in Vite imports.
* **Solution:**
  ```bash
  # Ensure all modules are installed
  npm install
  ```
  Check the exact relative path in your code:
  ```jsx
  // Correct relative path with exact casing
  import App from './App.jsx';
  ```

---

### Error 4: Environment Variables are `undefined`
* **Symptom:** `process.env.MY_KEY` returns `undefined`.
* **Cause:** Framework-specific variable prefix rules are missing.
* **Solution:**
  * **In Vite:** Variables MUST start with `VITE_` in `.env` file and be accessed via `import.meta.env`.
    ```env
    # .env file
    VITE_API_URL=https://api.example.com
    ```
    ```javascript
    // Accessing in code
    const apiUrl = import.meta.env.VITE_API_URL;
    ```
  * **In Create React App:** Must start with `REACT_APP_`:
    ```env
    REACT_APP_API_URL=https://api.example.com
    ```
    ```javascript
    const apiUrl = process.env.REACT_APP_API_URL;
    ```
  * **Important:** Restart the development server after modifying any `.env` file!

---

### Error 5: CORS (Cross-Origin Resource Sharing) Error in Dev API Calls
* **Symptom:** Browser console blocks `fetch` or `axios` requests with: `Access to XMLHttpRequest at ... has been blocked by CORS policy`.
* **Cause:** The frontend (`http://localhost:5173`) and backend (`http://localhost:5000`) run on different origins.
* **Solution:** Configure dev proxy in `vite.config.js`:
  ```javascript
  import { defineConfig } from 'vite';
  import react from '@vitejs/plugin-react';

  export default defineConfig({
    plugins: [react()],
    server: {
      proxy: {
        '/api': {
          target: 'http://localhost:5000', // Backend server URL
          changeOrigin: true,
          secure: false
        }
      }
    }
  });
  ```

---

### Error 6: `vite: command not found` / `react-scripts: command not found`
* **Symptom:** Running `npm run dev` or `npm start` throws `command not found`.
* **Cause:** `node_modules` directory is missing or incomplete (e.g., cloned project without running `npm install`).
* **Solution:**
  ```bash
  # Delete lock files and reinstall
  rm -rf node_modules package-lock.json
  npm install
  ```

---

### Error 7: Blank Page (White Screen) after Production Build / Deployment
* **Symptom:** `npm run build` succeeds, but opening index.html or deploying to subpath renders a blank page.
* **Cause:** Relative assets pathing issue in built files.
* **Solution:**
  * **Vite (`vite.config.js`):**
    ```javascript
    export default defineConfig({
      base: './', // Use relative path for assets
      plugins: [react()]
    });
    ```
  * **CRA (`package.json`):**
    ```json
    "homepage": "."
    ```

---

### Error 8: Uncaught ReferenceError: `process is not defined` in Vite
* **Symptom:** App crashes in browser when third-party package attempts to read `process.env`.
* **Cause:** Vite uses ES Modules (`import.meta.env`) instead of Node's `process.env`.
* **Solution:** Define `process.env` in `vite.config.js`:
  ```javascript
  export default defineConfig({
    define: {
      'process.env': {}
    },
    plugins: [react()]
  });
  ```

---

### Error 9: `ERR_OSSL_EVP_UNSUPPORTED` (Node.js 17+ OpenSSL issue in legacy CRA)
* **Symptom:** `npm start` in older React projects fails on Node 17+ with OpenSSL error.
* **Cause:** Legacy Webpack crypto algorithm incompatible with Node.js modern OpenSSL 3.0 implementation.
* **Solution:**
  Set the legacy provider flag in `package.json`:
  ```json
  "scripts": {
    "start": "NODE_OPTIONS=--openssl-legacy-provider react-scripts start"
  }
  ```
  *(Or upgrade project from CRA to Vite).*

---

### Error 10: Infinite Re-render Loop (`Too many re-renders`)
* **Symptom:** Browser tab freezes or console shows: `Uncaught Error: Too many re-renders. React limits the number of renders to prevent an infinite loop.`
* **Cause:** Invoking a state setter function immediately during render instead of passing a callback function.
* **Solution:**
  ```jsx
  // ❌ INCORRECT: Executes handleClick immediately during render
  <button onClick={setCount(count + 1)}>Click Me</button>

  // ✅ CORRECT: Pass an arrow function callback
  <button onClick={() => setCount(count + 1)}>Click Me</button>
  ```

---

## 7. Summary Checklist for React Interview Practical Round

1. **Create:** `npm create vite@latest project-name -- --template react`
2. **Install:** `cd project-name && npm install`
3. **Clean:** Strip out demo state, logo imports, and unused styles from `App.jsx`. Do not build extra components unless asked.
4. **Configure Scripts:** Adjust `package.json` scripts (`vite --port 3000 --open`) if required.
5. **Run & Build:** Use `npm run dev` to test locally and `npm run build` to build for production.
