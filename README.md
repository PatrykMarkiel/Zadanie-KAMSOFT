# Zadanie KAMSOFT – Content Parser API

API obsługuje parsowanie danych w formatach `CSV` oraz `INTERNAL_JSON` przesłanych jako Base64.

## Uruchomienie

Wymagany jest .NET SDK.

```bash
dotnet run
```

Swagger:

```text
{localhost:}/swagger/index.html
```

Endpoint:

```text
POST /api/v1/parse-content
```

Pole `content` musi zawierać dane zakodowane w Base64.

## Test CSV

```json
{
  "formatType": "CSV",
  "content": "TmFtZSxWYWx1ZQpTd29yZCwxMDAKU2hpZWxkLDE1MApIZWxtZXQsNzUKQXJtb3IsMjUwCkJvdywxMjA="
}
```
## Test INTERNAL_JSON

```json
{
  "formatType": "INTERNAL_JSON",
  "content": "WwogIHsKICAgICJOYW1lIjogIlN3b3JkIiwKICAgICJWYWx1ZSI6IDEwMAogIH0sCiAgewogICAgIk5hbWUiOiAiU2hpZWxkIiwKICAgICJWYWx1ZSI6IDE1MAogIH0sCiAgewogICAgIk5hbWUiOiAiSGVsbWV0IiwKICAgICJWYWx1ZSI6IDc1CiAgfSwKICB7CiAgICAiTmFtZSI6ICJBcm1vciIsCiAgICAiVmFsdWUiOiAyNTAKICB9LAogIHsKICAgICJOYW1lIjogIkJvdyIsCiAgICAiVmFsdWUiOiAxMjAKICB9Cl0="
}
```
Oczekiwana odpowiedź:

```json
{
  "status": "success",
  "count": 5,
  "data": [
    {
      "Name": "Sword",
      "Value": 100
    },
    {
      "Name": "Shield",
      "Value": 150
    },
    {
      "Name": "Helmet",
      "Value": 75
    },
    {
      "Name": "Armor",
      "Value": 250
    },
    {
      "Name": "Bow",
      "Value": 120
    }
  ]
}
```

Aplikacja zwraca `400 Bad Request` dla niepoprawnego `formatType`, pustego `content`, nieprawidłowego Base64 lub niepoprawnego JSON.
