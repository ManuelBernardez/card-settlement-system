<?php
session_start();
require_once "config/db.php";

if (!isset($_SESSION['documento'])) {
    header("Location: ingreso.html");
    exit();
}

$documento = $_SESSION['documento'];

// datos usuario + tarjeta
$sql = "
SELECT u.nombre, u.apellido, u.documento,
       t.num_cuenta, t.numero_tarjeta, t.banco_emisor, t.saldo
FROM usuarios u
JOIN tarjetas t ON u.documento = t.dni_titular
WHERE u.documento = ?
";

$stmt = $conexion->prepare($sql);
$stmt->bind_param("s", $documento);
$stmt->execute();
$user = $stmt->get_result()->fetch_assoc();

if (!$user) {
    die("No se encontró el usuario.");
}

// liquidaciones
$sql2 = "
SELECT periodo, fecha_vencimiento, total_a_pagar, pago_minimo
FROM liquidaciones
WHERE num_cuenta = ?
ORDER BY periodo DESC
";

$stmt2 = $conexion->prepare($sql2);
$stmt2->bind_param("i", $user['num_cuenta']);
$stmt2->execute();
$liq = $stmt2->get_result();
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

        <div class="bg-white p-4 rounded shadow mb-4">
            <h2 class="font-bold text-lg">
                Hola, <?= htmlspecialchars($user['nombre']) ?> <?= htmlspecialchars($user['apellido']) ?>
                
            </h2>
            <p>DNI: <?= $user['documento'] ?></p>
        </div>

        <div class="bg-white p-4 rounded shadow mb-4">
            <h3 class="font-bold mb-2">Tu tarjeta</h3>
            <p>Número: <?= $user['numero_tarjeta'] ?></p>
            <p>Banco: <?= $user['banco_emisor'] ?></p>
            <p>Saldo: $<?= $user['saldo'] ?></p>
        </div>

        <div class="bg-white p-4 rounded shadow">
            <h3 class="font-bold mb-3">Liquidaciones</h3>

            <table class="w-full text-sm">
                <tr class="border-b">
                    <th>Periodo</th>
                    <th>Vencimiento</th>
                    <th>Total</th>
                    <th>Mínimo</th>
                </tr>

                <?php while($row = $liq->fetch_assoc()) { ?>
                <tr class="border-b">
                    <td><?= $row['periodo'] ?></td>
                    <td><?= $row['fecha_vencimiento'] ?></td>
                    <td>$<?= $row['total_a_pagar'] ?></td>
                    <td>$<?= $row['pago_minimo'] ?></td>
                </tr>
                <?php } ?>
            </table>
        </div>

        <div class="text-center mt-4">
            <a href="logout.php" class="text-red-600">Cerrar sesión</a>
        </div>

    </div>

</body>
</html>