# 💳 Sistema de Consulta de Liquidaciones "Mis Tarjetas"

> Trabajo Práctico Integrador - Programación III
> Tecnicatura Universitaria en Programación
> Universidad Tecnológica Nacional

---

## 📖 Descripción

Este sistema simula el circuito básico de administración y consulta de tarjetas de crédito.

El proyecto está compuesto por dos aplicaciones independientes que comparten una base de datos MySQL:

* **Aplicación de consola desarrollada en C#**, usada por los empleados de la entidad financiera para administrar clientes, tarjetas y liquidaciones.
* **Portal web desarrollado en PHP**, donde los clientes pueden activar su cuenta, iniciar sesión y consultar sus liquidaciones.

---

Este proyecto tiene como objetivo aplicar conceptos de:

* Programación Orientada a Objetos
* Acceso a Bases de Datos
* Integración entre plataformas
* Arquitectura Cliente-Servidor
* Manejo de sesiones en PHP

---

## 🛠 Tecnologías

| Tecnología      | Uso                   |
| --------------- | --------------------- |
| C#              | Aplicación de consola |
| PHP             | Portal Web            |
| MySQL           | Base de datos         |
| HTML5           | Interfaces            |
| Tailwind CSS    | Diseño web            |
| Git             | Control de versiones  |

---


## Estructura del proyecto

```text
mis-tarjetas/

├── /docs              
├── /database           
├── /src
│   ├── /admin-console  # Proyecto C#
│   │   ├── Config.cs
│   │   └── Program.cs  
│   └── /web-portal     # Proyecto PHP
│       ├── ingreso.html
│       ├── registro.html
├── .gitignore
└── README.md
```

---

## ⚙️ Funcionalidades

### Aplicación de Consola (C#)

* Registrar clientes.
* Emitir tarjetas.
* Consultar información.
* Dar de baja tarjetas.
* Generar liquidaciones.

### Portal Web (PHP)

* Activar usuario.
* Iniciar sesión.
* Consultar última liquidación.
* Consultar historial.
* Cerrar sesión.

---

## 🗄 Base de Datos

La aplicación utiliza una única base de datos compartida por ambos sistemas.

Tablas principales:

* usuarios
* tarjetas
* liquidaciones

---


## Autor

**Alumno:** Manuel Bernardez

**Materia:** Programación III - Facultad Regional Haedo (UTN FRH)

**Año:** 2026
