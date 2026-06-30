# Sistema de Consulta de Liquidaciones "Mis Tarjetas"

> Trabajo Práctico Integrador - Programación III
> 
> Tecnicatura Universitaria en Programación | Universidad Tecnológica Nacional


## Descripción

Este sistema simula el circuito de administración y consulta de tarjetas de crédito mediante dos aplicaciones integradas:

- **Aplicación administrativa (C#)**: usada por los empleados de la entidad financiera para la gestión de clientes, tarjetas y liquidaciones. 
- **Portal web (PHP)**, donde los clientes pueden registrarse e iniciar sesión en el home banking (si ya tienen tarjeta),  para consultar información sobre sus liquidaciones.

**Base de Datos**: La aplicación utiliza una base de datos compartida por ambos sistemas.

Tablas:
    * usuarios
    * tarjetas
    * liquidaciones

Este proyecto tiene como objetivo aplicar conceptos de:

* Programación Orientada a Objetos
* Acceso a Bases de Datos
* Integración entre plataformas
* Arquitectura Cliente-Servidor
* Manejo de sesiones en PHP

---

## Funcionalidades

### Aplicación de Consola (C#)

- Registrar clientes.
- Emitir tarjetas.
- Consultar de clientes y tarjetas emitidas.
- Dar de baja tarjetas.
- Generar liquidaciones.

### Portal Web (PHP)

- Activar cuenta web del usuario.
- Iniciar sesión.
- Consultar datos personales e historial de liquidaciones.
- Cerrar sesión.

## Validaciones

- Documento obligatorio.
- Verificación de existencia de tarjeta para activar la cuenta web.
- Prevención de múltiples tarjetas para un mismo titular.
- Prevención de números de tarjeta duplicados.
- Validación del formato del período de liquidación.
- Validación del total a pagar.
- Verificación del estado de la tarjeta antes de emitir liquidaciones.
- Uso de consultas preparadas (`Prepared Statements`) para todas las operaciones SQL.

---

## Tecnologías

| Tecnología      | Uso                   |
| --------------- | --------------------- |
| C#              | Aplicación de consola |
| PHP             | Portal Web            |
| MySQL           | Base de datos         |
| HTML5           | Interfaces            |
| Tailwind CSS    | Diseño web            |
| Git             | Control de versiones  |



## Estructura del proyecto

```text
mis-tarjetas/
├── /docs     
├── /database
│   ├── mi_banco_db.sql     
│   └── db.php
│
├── /src
│   ├── /admin-console  # Aplicación C#
│   │   ├── Program.cs  
│   │   └── ConexionBD.cs
│   │
│   └── /web-portal     # Aplicación PHP
│       ├── registro.html
│       ├── ingreso.php
│       ├── resumen.php
│       ├── altas.php
│       ├── alertas.php
│       ├── registro.html
│       └── ingreso.html
├── .gitignore
└── README.md
```

---

## Requisitos

- PHP 8
- .NET SDK 8 o superior
- XAMPP (Apache + MySQL)
- MySQL Connector para .NET


## Ejecución

1. Importar `mi_banco_db.sql` en phpMyAdmin  
2. Copiar proyecto en `htdocs`  
3. Iniciar XAMPP (Apache + MySQL)  
4. Abrir:

```
http://localhost/mis_tarjetas_tp/src/web_portal/ingreso.html
```

5. Ejecutar consola:

```bash
dotnet run
```

## Autor

**Alumno:** Manuel Bernardez

**Materia:** Programación III - Facultad Regional Haedo (UTN FRH)

**Año:** 2026
