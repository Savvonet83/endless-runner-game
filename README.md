# C# Console Endless Runner — Demo

Piccolo demo in C# (.NET 7) — un "endless runner" testuale: controlli la macchina (`A`) con i tasti `A`/`D` o le frecce sinistra/destra, evita il traffico (`X`) e raccogli la benzina (`F`).

Caratteristiche:
- Contatore di distanza
- Sistema di carburante (consumo nel tempo, raccogliendo `F` recuperi fuel)
- Collisione con traffico riduce fuel

Esecuzione:

```bash
cd /workspaces/endless-runner-workspace
dotnet run
```

Nota:
- Serve .NET 7+ installato. Su Linux, installa il runtime/SDK ufficiale da https://dotnet.microsoft.com.
- Il gioco è testuale in console per portabilità rapida.

Vuoi che lo converta in versione grafica (MonoGame / Unity / Blazor)? Dimmi quale preferisci e procedo.
