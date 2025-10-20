# 💻 SimplePOS API

API RESTful desarrollada en **ASP.NET Core 8** que forma parte del sistema SimplePOS. Utiliza una base de datos **SQL Server** y está completamente **containerizada** con **Docker** y **Docker Compose**.

***

## ⚙️ Requisitos

Para poder ejecutar la aplicación, necesitas tener instalados:

* **Docker Desktop**: Instalado y funcionando correctamente.
* **Git**: Para clonar el repositorio.

***

## ▶️ Cómo ejecutar la aplicación

Sigue esta secuencia de comandos para clonar el repositorio, construir y levantar la API, y acceder a la documentación Swagger:

```bash
# 1. Clonar el repositorio y entrar al directorio de la API
git clone https://github.com/RepoAriel/SimplePOSapp.git
cd SimplePOS.API

# 2. Construir y levantar los contenedores con Docker Compose
# La bandera --build asegura que las imágenes se construyan antes de levantar los servicios.
docker-compose up --build

# 3. Acceder a la API (después de que los contenedores estén corriendo)
# Documentación Swagger (Interfaz web para probar los endpoints):
# - HTTP: http://localhost:5000/swagger
# - HTTPS: https://localhost:5001/swagger
