# PROY_HANOI
Sitio que guarda el proyecto de torres de hanoi (Curso UDEMY)

Juego de las Torres de Hanói en consola (C# / .NET 10) con solución automática y modo de juego paso a paso (SCRUM-5).

## Estructura

| Proyecto | Responsabilidad |
|---|---|
| `src/Hanoi.Core` | Lógica pura: estado de las torres, validación de movimientos, condición de victoria y solución recursiva. Sin dependencias de la consola. |
| `src/Hanoi.ConsoleApp` | Presentación: menú, lectura/validación de entradas y dibujo de las torres. |
| `tests/Hanoi.Core.Tests` | Pruebas unitarias (xUnit) de la lógica. |

## Uso

```
dotnet run --project src/Hanoi.ConsoleApp
dotnet test
```

En el juego paso a paso se indica la torre de origen y la de destino con las letras `A`, `B` o `C`; con `0` o `Q` se vuelve al menú.
