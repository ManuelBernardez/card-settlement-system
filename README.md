# Sistema de Consulta de Liquidaciones "Mis Tarjetas"

> Trabajo Práctico Integrador - Programación III
> 
> Tecnicatura Universitaria en Programación | Universidad Tecnológica Nacional


## Descripción

Este sistema simula el circuito de administración y consulta de tarjetas de crédito mediante dos aplicaciones integradas:

- **Aplicación administrativa (C#)**: usada por los empleados de la entidad financiera para la gestión de clientes, tarjetas y liquidaciones. 
- **Portal web (PHP)**, donde los clientes pueden registrarse e iniciar sesión en el home banking (si ya tienen tarjeta),  para consultar información sobre sus liquidaciones.

**Base de Datos**: La aplicación utiliza una base de datos compartida por ambos sistemas. Tablas de usuarios, tarjetas y liquidaciones

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

## Reglas de negocio

**Validaciones**

- Documento obligatorio y tipo de documento válido.
- Existencia del cliente en el sistema.
- Credenciales de acceso al portal web.
- Banco emisor válido.
- Estado de la tarjeta antes de emitir liquidaciones.
- Formato del período de liquidación.
- Total a pagar válido.

**Prevención de duplicados**

- Números de tarjeta duplicados.
- Múltiples tarjetas para un mismo titular.
- Usuarios en la página web.

**Otros controles de seguridad**

- Manejo de sesiones de usuario en el portal web.
- Uso de consultas preparadas (Prepared Statements) en todas las operaciones SQL.


## Estructura del proyecto

```text
mis_tarjetas_tp/
├── database/
│   ├── mi_banco_db.sql
│   └── db.php
│
├── src/
│   ├── admin_consola/
│   │   ├── Program.cs
│   │   ├── ConexionBD.cs
│   │   └── Validacion.cs
│   │
│   ├── web_portal/
│   │   ├── ingreso.html
│   │   ├── registro.html
│   │   ├── ingreso.php
│   │   ├── altas.php
│   │   ├── resumen.php
│   │   ├── cerrar.php
│   │   ├── alertas.php
│   │   └── validacion.php
│   │
│   └── docs/
├── .gitignore
└── README.md
```


## Autor

**Alumno:** Manuel Bernardez

**Materia:** Programación III - Facultad Regional Haedo (UTN FRH)

**Año:** 2026
