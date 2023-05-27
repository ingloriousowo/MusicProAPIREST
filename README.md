# Music Pro APIREST C#
Servicio web RESTful creado para la evaluación de Integración de plataforma, Duoc UC.
## Tecnologías usadas

- ASP.NET C#
- SWAGGER UI
- AZURE DATABASE SQL SERVER

---
## ORGANIZACIÓN DEL PROYECTO
1. __Models__: Clases creadas para almacenar los resultados de las consultas a la Base de datos y usarlos dentro del programa.
2. __Controllers__: Controla los ENDPOINTS de la API
3. __Services__: Toda la logica dentro del servicio, incluyendo las conexiones a la base de datos y sus consultas

Estoy incluye los archivos predeterminados del proyecto como el [Program.cs](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-6.0) y [Appsettings.json](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-6.0)

La base de datos cuenta con 4 tablas:
- Articulo
- Carro
- Articulo Carro (creada como una tabla intermedias)
- Tarjeta
  
---
## Objetivo del Servicio
Servir de comunicación con la base de datos, permitiendo administrar los articulos y los carros de la tienda __Music PRO__, incluyendo un validador de tarjetas de debito que verifica el saldo y la existencia de la misma.
