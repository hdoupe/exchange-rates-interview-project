# Exchange Rates App

This project contains the frontend application code for an exchange rates app.

It is built with [React](https://react.dev/) 19, [TypeScript](https://www.typescriptlang.org/) 5
and [Vite](https://vite.dev/).

## Getting started

### `npm install`

Install required packages. Requires Node.js 20.19+ or 22.12+.

### `npm run dev`

Runs the app in development mode.\
Open [http://localhost:3000](http://localhost:3000) to view it in the browser.

The page will hot-reload as you make edits.

(`npm start` is kept as an alias for the same thing.)

### `npm run build`

Type-checks the project and builds it for production to the `dist` folder.\
The build is minified and the filenames include content hashes.

Use `npm run preview` to serve the production build locally.

### `npm test`

Runs the test suite with [Vitest](https://vitest.dev/) in watch mode.\
Use `npx vitest run` for a single non-watching run.

### `npm run lint`

Lints the project with ESLint.

## Configuration

The API base URL is read from `VITE_BASE_URL` in [.env](.env). Vite only exposes
environment variables prefixed with `VITE_` to the client.
