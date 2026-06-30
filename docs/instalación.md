## Instalación

### 1. Clonar el repositorio

```bash
git clone https://github.com/USUARIO/REPOSITORIO.git
```

### 2. Importar la base de datos

Abrir phpMyAdmin e importar:

```
mi_banco_db.sql
```

---

### 3. Configurar la conexión PHP

Editar:

```
src/web_portal/database/db.php
```

con las credenciales locales.

---

### 4. Configurar la conexión C#

Editar:

```
src/admin_consola/ConexionBD.cs
```

con las mismas credenciales de MySQL.

---

### 5. Ejecutar el portal web

Copiar el proyecto dentro de:

```
C:\xampp\htdocs\
```

Iniciar Apache y MySQL desde XAMPP.

Abrir:

```
http://localhost/mis_tarjetas_tp/src/web_portal/ingreso.html
```

---

### 6. Ejecutar la consola administrativa

Desde:

```
src/admin_consola
```

ejecutar:

```bash
dotnet restore
dotnet run
```

---

## Dependencias

Agregar el paquete MySQL para .NET:

```bash
dotnet add package MySql.Data
```

---