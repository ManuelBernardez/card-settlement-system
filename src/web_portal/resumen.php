<?php
session_start();
require_once "../../database/db.php";
require_once "../alertas.php";

// Se requiere una sesión válida activa
if (!isset($_SESSION['documento']) || empty($_SESSION['documento'])) {
    header("Location: ingreso.html");
    exit();
}

$documento = $_SESSION['documento'];

function obtenerUsuario($conexion, $documento)
{
    $sql = "
        SELECT u.nombre, u.apellido, u.documento, u.email,
            t.num_cuenta, t.numero_tarjeta, t.banco_emisor, t.estado, t.saldo
        FROM usuarios u
        JOIN tarjetas t ON u.documento = t.dni_titular
        WHERE u.documento = ?
    ";

    $stmt = $conexion->prepare($sql);
    $stmt->bind_param("s", $documento);
    $stmt->execute();

    $user = $stmt->get_result()->fetch_assoc();

    $stmt->close();
    return $user;
}

function obtenerLiquidaciones($conexion, $numCuenta)
{
    $sql = "
        SELECT periodo, fecha_vencimiento, total_a_pagar, pago_minimo
        FROM liquidaciones
        WHERE num_cuenta = ?
        ORDER BY periodo DESC
    ";

    $stmt = $conexion->prepare($sql);
    $stmt->bind_param("i", $numCuenta);
    $stmt->execute();

    $result = $stmt->get_result();

    $stmt->close();
    return $result;
}

// Obtener datos del usuario
$user = obtenerUsuario($conexion, $documento);

if (!$user || !isset($user['num_cuenta'])) {
    echo "<script>alert('No se encontró el usuario o no tiene tarjeta asociada')
    ;window.location='ingreso.html';</script>";
    exit();
}

$liq = obtenerLiquidaciones($conexion, $user['num_cuenta']);
?>

<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <title>Resumen</title>
    <script src="https://cdn.tailwindcss.com"></script>
</head>

<body class="bg-gray-100">

<header class="bg-[#004691] text-white p-4 text-center">
    <h1>Mis Tarjetas</h1>
</header>

<div class="max-w-4xl mx-auto p-6">

    <!-- DATOS DEL USUARIO -->
    <div class="bg-white p-4 rounded shadow mb-4">
        <h2 class="font-bold text-lg">
            Hola, <?= htmlspecialchars($user['nombre']) ?>
            <?= htmlspecialchars($user['apellido']) ?>
        </h2>

        <p>DNI: <?= htmlspecialchars($user['documento']) ?></p>
        <p>Email: <?= htmlspecialchars($user['email']) ?></p>
    </div>

    <!-- TARJETA -->
    <div class="bg-white p-4 rounded shadow mb-4">
        <h3 class="font-bold mb-2">Tu tarjeta</h3>

        <p>Número: <?= htmlspecialchars($user['numero_tarjeta']) ?></p>
        <p>Estado: <?= htmlspecialchars($user['estado']) ?></p>
        <p>Banco: <?= htmlspecialchars($user['banco_emisor']) ?></p>
        <p>Saldo: $<?= htmlspecialchars($user['saldo']) ?></p>
    </div>

    <!-- LIQUIDACIONES -->
    <div class="bg-white p-4 rounded shadow">
        <h3 class="font-bold mb-3">Liquidaciones</h3>

        <table class="w-full text-sm">
            <tr class="border-b">
                <th>Periodo</th>
                <th>Vencimiento</th>
                <th>Total</th>
                <th>Mínimo</th>
            </tr>

            <?php if ($liq->num_rows > 0) { ?>

                <?php while ($row = $liq->fetch_assoc()) { ?>
                    <tr class="border-b">
                        <td><?= htmlspecialchars($row['periodo']) ?></td>
                        <td><?= htmlspecialchars($row['fecha_vencimiento']) ?></td>
                        <td>$<?= htmlspecialchars($row['total_a_pagar']) ?></td>
                        <td>$<?= htmlspecialchars($row['pago_minimo']) ?></td>
                    </tr>
                <?php } ?>

            <?php } else { ?>

                <tr>
                    <td colspan="4" class="text-center py-3">
                        No hay liquidaciones disponibles.
                    </td>
                </tr>

            <?php } ?>

        </table>
    </div>

    <!-- LOGOUT-->
    <div class="text-center mt-4">
        <a href="cerrar.php" class="text-red-600">Cerrar sesión</a>
    </div>

</div>

</body>
</html>

<?php
$conexion->close();
?>