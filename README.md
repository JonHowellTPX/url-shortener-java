# Snip – URL Shortener

A full-stack URL shortener built with **Java Spring Boot** (backend) and **React + TypeScript** (frontend), containerised with Docker.

---

## Architecture

```
frontend/      React + TypeScript (Vite)
backend/       Java Spring Boot Web API + SQLite
docker-compose.yml       Orchestrates both services
```

The frontend proxies API calls through Nginx in production, so there are no CORS concerns once containerised. In development, Vite's dev server proxies to the local API.

---

## Running locally

### Option A – Docker Compose (recommended)

**Prerequisites:** Docker Desktop or Docker Engine + Compose v2.

```bash
git clone <your-fork-url>
cd url-shortener

docker compose up --build
```

- Frontend: http://localhost:3000
- API:      http://localhost:8080

The SQLite database is persisted in the `db-data` Docker volume across restarts.

To stop and remove containers:

```bash
docker compose down
```

To also wipe the database volume:

```bash
docker compose down -v
```

---

### Option B – Running services separately (development)

**Prerequisites:** Java 17+, Maven, Node.js 20+.

**Backend**

```bash
cd backend
mvn clean package -DskipTests
java -jar target/url-shortener-0.0.1-SNAPSHOT.jar
# API listening on http://localhost:8080
```

**Frontend**

```bash
cd frontend
npm install
npm run dev
# Dev server on http://localhost:3000 (proxies to :8080)
```

---

## Running tests

**Backend**

```bash
cd backend
mvn test
```

**Frontend**

```bash
cd frontend
npm install
npm test
```

---

## API reference

The API exposes the same contract expected by the existing frontend.

| Method | Path | Description |
|--------|------|-------------|
| `POST` | `/shorten` | Create a shortened URL |
| `GET` | `/urls` | List all shortened URLs |
| `GET` | `/{alias}` | Redirect to the full URL |
| `DELETE` | `/{alias}` | Delete a shortened URL |

### POST /shorten

```json
// Request
{
  "fullUrl": "https://example.com/very/long/path",
  "customAlias": "my-alias"   // optional
}

// Response 201
{
  "alias": "my-alias",
  "fullUrl": "https://example.com/very/long/path",
  "shortUrl": "http://localhost:8080/my-alias"
}
```

Returns `400` if the URL is invalid, the alias is malformed, or the alias is already taken.

### GET /urls

```json
// Response 200
[
  {
    "alias": "my-alias",
    "fullUrl": "https://example.com/very/long/path",
    "shortUrl": "http://localhost:8080/my-alias"
  }
]
```

### GET /{alias}

Returns `302 Redirect` to the full URL, or `404` if the alias doesn't exist.

### DELETE /{alias}

Returns `204 No Content` on success, `404` if alias not found.

---

## Design decisions & assumptions

**Persistence** — SQLite via JDBC. The database file path is configurable via the `DATABASE_PATH` environment variable and is volume-mounted in Docker so data survives container restarts.

**Alias validation** — Aliases must be 2–64 characters, containing only letters, numbers, and hyphens. This mirrors typical URL shortener conventions and avoids characters that are awkward in URLs.

**Random alias generation** — 7-character Base62 strings, generated inside the service layer. Collision probability is negligible at realistic scales; duplicate alias attempts return a clean 400 response.

**Error handling** — Invalid input and domain errors are translated into JSON error responses like `{ "error": "..." }`.

**Testing** — The backend includes service-level tests and can be extended with integration tests. The frontend includes React component tests and service-level API tests.

**Frontend** — The hosted frontend uses Nginx to proxy `/shorten`, `/urls`, and alias requests to the API, so the same Docker setup works without CORS issues.
