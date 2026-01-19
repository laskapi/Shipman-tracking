# 📦 Shipman Tracking

A full‑stack shipment tracking system built with a **.NET 10 backend** and a **React + TypeScript + Vite frontend**.  
The project mirrors real logistics dashboards with clean architecture and backend‑driven UI.

---

## 🚀 Tech Stack

- **Backend:** ASP.NET Core 10 (`shipman.Server`)
- **Frontend:** React + TypeScript + Vite (`shipman.client`)
- **UI:** MUI 5
- **State / API:** Redux Toolkit + RTK Query
- **Database:** SQLite (development)

---

## 📁 Project Structure

```
Shipman-tracking/
│
├── shipman.client/      # React + TS + Vite frontend
├── shipman.Server/      # ASP.NET Core backend
├── shipman.sln          # Solution file
├── .gitignore
└── README.md
```

---

## ▶️ Running the Project

### Backend
```bash
cd shipman.Server
dotnet restore
dotnet run
```

### Frontend
```bash
cd shipman.client
npm install
npm run dev
```

---

## 📌 Features

- Shipment dashboard with sorting and filtering  
- Backend‑driven status system (single source of truth)  
- Clean architecture (Domain / Application / Api / Data)  
- Modern UI with MUI components  
- RTK Query for API communication  

---

## 📍 Roadmap

- Shipment timeline  
- Map integration  
- Authentication  
- Admin panel  
- Docker Compose setup  

---

