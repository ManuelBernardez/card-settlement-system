<?php
require_once "../../database/db.php";
require_once "../alertas.php";

$documento = $_POST['documento'];
$usuarioWeb = $_POST['usuario'];
$passwordA = $_POST['passwordA'];
$passwordB = $_POST['passwordB'];

// Validar que las contraseñas coincidan 
if ($passwordA !== $passwordB) {
    mostrarAlerta('Las contraseñas no coinciden');
}

// Verificar que el usuario exista en tabla usuarios
$sql = "SELECT usuario, password FROM usuarios WHERE documento = ?";
$stmt = $conexion -> prepare($sql);
$stmt->bind_param("s", $documento);
$stmt->execute();

$res = $stmt->get_result();
$usuarioDB = $res->fetch_assoc();

if (!$usuarioDB) {
    echo "<script>alert('El usuario no existe')
    ;window.location='ingreso.html';</script>";
    exit();
}

// Verificar que tenga tarjeta asociada 
$sql = "SELECT * FROM tarjetas WHERE dni_titular = ?";
$stmt = $conexion->prepare($sql);
$stmt->bind_param("s", $documento);
$stmt->execute();

$res = $stmt->get_result();

if ($res->num_rows == 0) {
    echo "<script>alert('No tenés tarjeta asociada')
    ;window.location='ingreso.html';</script>";
    exit();
}

// Verificar si tiene cuenta activa
if ($usuarioDB['usuario'] !== null || $usuarioDB['password'] !== null) {
    echo "<script>alert('La cuenta ya fue activada')
    ;window.location='ingreso.html';</script>";
    exit();
}

// Activar cuenta
$sql = "UPDATE usuarios SET usuario = ?, password = ? WHERE documento = ?";
$stmt = $conexion->prepare($sql);
$stmt->bind_param("sss", $usuarioWeb, $passwordA, $documento);

if ($stmt->execute()) {
    echo "Cuenta activada correctamente. Ya podés iniciar sesión.";
} else {
    echo "Error al activar la cuenta.";
}

$stmt->close();
$conexion->close();
?>