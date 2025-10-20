SimplePOS API

API RESTful desarrollada en ASP.NET Core 8 que forma parte del sistema SimplePOS, con base de datos SQL Server, todo containerizado con Docker y Docker Compose.

Requisitos

Docker Desktop
 instalado y funcionando

Git
 para clonar el repositorio

Cómo ejecutar la aplicación
1. Clonar el repositorio
   git clone https://github.com/RepoAriel/SimplePOSapp.git
   cd SimplePOS.API
2. Construir y levantar los contenedores con Docker Compose
   docker-compose up --build
3. Acceder a la API

Documentación Swagger (interfaz web para probar los endpoints):

http://localhost:5000/swagger
https://localhost:5001/swagger
