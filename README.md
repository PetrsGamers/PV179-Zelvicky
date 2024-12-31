# PV179 - Beer cap collector

# Tým
+ [David Novák](https://gitlab.fi.muni.cz/xnovak20)
+ [Tadeáš Kozub](https://gitlab.fi.muni.cz/xkozub1)
+ [Petr Hýbl](https://gitlab.fi.muni.cz/xhybl)



## Overview
Tento projekt je databází pro sběratele pivních víček, která slouží k tomu, aby si sběratel mohl ukládat své exponáty do virtuálního alba a získal lepší přehled o své sbírce. Umožňuje nejen základní správu sbírky, ale také pokročilé funkce, které usnadní rozhodování při pořizování nových přírůstků.

### Přehled funkcionalit
+ Virtuální album sbírky: Ukládání a správa jednotlivých pivních víček v digitální podobě.
+ Statistiky sbírky: Přehled o počtu víček, rozdělení podle zemí nebo značek.
+ Porovnání sbírek: Možnost porovnávat sbírky s ostatními sběrateli na platformě.
+ Vyhledávání víček: Rychlé vyhledávání podle barvy víčka nebo textu na něm, aby se sběratel mohl snadno zorientovat, zda už konkrétní víčko vlastní.

### Role
+ Uživatel: Spravuje svou sbírku, zobrazuje statistiky, využívá vyhledávání a porovnává svou sbírku s ostatními.
+ Admin: Moderuje obsah, zajišťuje tak, aby nikdo nemohl zničit dataset

---

Tento projekt pomáhá sběratelům udržet přehled o jejich sbírce a zjednodušit rozšiřování sbírky.


## Getting started

### Setup DB
Prvně je potřeba mít nainstalovaný Docker.
V rootu projektu je soubor .env.example, který s default postgres kontejnerem stačí přejmenovat na `.env`
.env soubor se používá pro nastavení proměnných pro docker. Aplikace samotná používá `appsettings.json`.

Spuštění databáze
```
docker compose up -d
```

Vytvoření migrace, spuštění migrace
```
dotnet ef migrations add <nazev-migrace>
dotnet ef database update
```

### Setup projektu

Build a spuštění
```
dotnet build
dotnet run
```

Je potřeba po setupu potřeba vytvořit .env souboru z .env.example a doplnit do něj potřebné proměnné.


Používej formatter před každým commitem. Jinak neprojde CR.

```
 dotnet format ../CapEnjoyer.sln
```


## Technický pohled

![ERD diagram](erd.png)

![Use case diagram](use_case.png) 

## Aktualizace Milestone3

+ Admin může měnit hesla uživatelů
+ Přidána možnost hledat ve víčkách,lahvích a výrobcích
+ Vytvořeno celá WEB aplikace, ve které pro všechny entity je možné CRUD operace, nebo jenom zadávání požadavku na změny pro admina
+ Přidáno cachování
+ Přidáno logování pro API v MVC
+ Přidání možnost kupónů pro prémiové uživatele + svítící username
+ Přidáno stránkování pro víčka


# Použité technologie

+ C# 12.0
+ .NET 8.0
+ ASP.NET Core s REST API, MVC a Razor Pages
+ Entity Framework Core
+ PostgreSQL databáze
+ Docker - pro spuštění databáze
+ Bogus - seeding
+ Swagger - dokumentace API
+ Identity Manager - správa uživatelů
+ Mapster - mapování entit

# CI/CD

+ Gitlab CI/CD v `.gitlab-ci.yml`, který spouští build, linting a testy
+ Custom gitlab runner na MUNI FI Stratos (runner `ted`)
