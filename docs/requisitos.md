# Especificaciones del Proyecto

## Objetivo

Desarrollar un sistema de administración y consulta de tarjetas de crédito, compuesto por una aplicación de consola en C# y un portal web en PHP, ambos conectados a una base de datos MySQL.

## Alcance y actores
* **Sección Administrativa (C#):** Emisión de tarjetas, alta de clientes y generación de liquidaciones financieras.
- Empleado del banco: Usa la aplicación de consola para administrar clientes, tarjetas y liquidaciones.
    

* **Sección del Cliente (PHP):** Onboarding digital mediante validación de DNI, inicio de sesión y consulta histórica de resúmenes.

- Cliente: Usa el portal web para activar su cuenta y consultar sus liquidaciones.

---

## Reglas de negocio

- Cada cliente se identifica por DNI.
- Cada cliente posee una única tarjeta.
- Una tarjeta pertenece a un único banco emisor.
- Una tarjeta puede tener múltiples liquidaciones.
- La activación del usuario se realiza desde el portal web.

---

## Exclusiones (Simplificaciones del TP)

* No se requiere encriptación o hasheo de claves (almacenamiento en texto plano)[cite: 1].
* No se contempla el desglose de consumos detallados (se trabaja sobre totales financieros)[cite: 1].
* No se requiere infraestructura de despliegue avanzado (Docker/LEMP)[cite: 1].

---

## ⚙️ Requisitos funcionales

### RF01
Registrar clientes en la base de datos.

### RF02
Emitir tarjetas asociadas a un cliente.

### RF03
Consultar clientes y tarjetas.

### RF04
Dar de baja tarjetas.

### RF05
Generar liquidaciones mensuales.

### RF06
Validar existencia de cliente por DNI en el portal web.

### RF07
Permitir activación de cuenta desde la web.

### RF08
Permitir inicio de sesión.

### RF09
Consultar última liquidación.

### RF10
Consultar historial de liquidaciones.
