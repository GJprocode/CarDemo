# CarDemoApp

A demo web app built with **Angular** and **ASP.NET Core Web API** for user authentication and car data management.

## 🚀 Features

- **Login & Register**  
  - Validates email format  
  - Password requires minimum 6 characters including:  
    - 1 uppercase, 1 lowercase, 1 number, and 1 special character  
  - Authentication is handled via the backend API  

- **Data Grid**  
  - Displays 10 rows of car data with 5 columns  
  - Data fetched from the ASP.NET Core API  
  - Supports grouping, filtering, and Excel export  
  - Built with DevExpress Grid Component

## 🛠️ Tech Stack

- **Frontend:** Angular 20.0.1, DevExpress/Extreme 
- **Backend:** ASP.NET Core Web API (.NET 8) 
- **Database:** SQL Server  
- **Testing:** xUnit for .NET backend unit testing  

## 📦 Tools & Libraries

- **Mock Data:** [generatedata.com](https://generatedata.com)  
- **Excel Export:** SheetJS (`xlsx`)
- **Styling:** Tailwind

## 📌 Notes

- DevExpress components UI works, work around needed for functionality
- Angular modules used for routing, reactive forms, table features, and UI components  
