<?php
require_once "../../database/db.php";
require_once "alertas.php";
require_once "validacion.php";

$tipo_doc = $_POST['tipo_doc'];
$documento = $_POST['documento'];
$usuarioWeb = $_POST['usuario'];
$passwordA = $_POST['passwordA'];
$passwordB = $_POST['passwordB'];

// Validar que las contraseñas coincidan 
if ($passwordA !== $passwordB) {
    mostrarAlerta('Las contraseñas no coinciden');
}

if (!Validacion::documentoValido($documento) || !Validacion::tipoDocumentoValido($tipo_doc)){
    
    mostrarAlerta('Documento inválidio');
}

// Verificar que el usuario exista en tabla usuarios
$sql = "SELECT usuario,
            password,
            nombre,
            apellido,
            email
        FROM usuarios
        WHERE documento = ?";
$stmt = $conexion -> prepare($sql);
$stmt->bind_param("s", $documento);
$stmt->execute();

$res = $stmt->get_result();
$usuarioDB = $res->fetch_assoc();

if (!$usuarioDB) {
    mostrarAlerta('El usuario no existe');
}

// Verificar que tenga tarjeta asociada 
$sql = "SELECT * FROM tarjetas WHERE dni_titular = ?";
$stmt = $conexion->prepare($sql);
$stmt->bind_param("s", $documento);
$stmt->execute();

$res = $stmt->get_result();

if ($res->num_rows == 0) {
    mostrarAlerta('No tenés tarjeta asociada');
}

// Verificar si tiene cuenta activa
if ($usuarioDB['usuario'] !== null || $usuarioDB['password'] !== null) {
    mostrarAlerta('La cuenta ya fue activada');
}

// Disponibilidad de usuario
$sql = "SELECT 1 FROM usuarios WHERE usuario = ?";
$stmt = $conexion->prepare($sql);
$stmt->bind_param("s", $usuarioWeb);
$stmt->execute();

if ($stmt->get_result()->num_rows > 0) {
    mostrarAlerta('Ese usuario ya está en uso');
}

// Activar cuenta
$sql = "UPDATE usuarios SET usuario = ?, password = ? WHERE documento = ?";
$stmt = $conexion->prepare($sql);
$stmt->bind_param("sss", $usuarioWeb, $passwordA, $documento);

if ($stmt->execute()) {
    mostrarAlerta('Cuenta activada correctamente. Ya podés iniciar sesión', 'ingreso.html', 'success');
} else {
    mostrarAlerta('Error al activar la cuenta');
}

$stmt->close();
$conexion->close();
?>