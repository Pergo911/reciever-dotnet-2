### Automata "szinkronban tart" egy Youtube lejátszási listát egy helyi mappával

---

#### Telepítés

- `config.json` legyen .exe-vel egy mappában. Struktúrája legyen a következő:

```json
{
  "PLAYLIST_ID": "string", // használt lejátszási lista ID-je (URL utsó része)
  "OUTPUT_DIR": "string", // letöltési mappa elérési útja (pl. D:\\output)
  "CLIENT_ID": "string", // client_id kulcs
  "CLIENT_SECRET": "string", // client_secret kulcs
  "REDIRECT_URI": "string" // Átirányítás URL (legtöbb esetben http://localhost:3000)
}
```

- Szükséges továbbá >=.NET8.0 telepítve a gépen.

---

#### Futtatás

```
$ dotnet run
```
