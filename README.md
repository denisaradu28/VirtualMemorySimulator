# Virtual Memory Simulator

## Overview

**Virtual Memory Simulator** este o aplicație desktop dezvoltată în **C# și WPF**, care simulează algoritmi de înlocuire a paginilor utilizați în sistemele de operare.

Aplicația permite rularea pas cu pas a simulării și compararea performanței algoritmilor:

- FIFO (First-In, First-Out)
- LRU (Least Recently Used)
- Optimal (OPT)

Scopul proiectului este evidențierea diferențelor de comportament și performanță dintre acești algoritmi folosind același șir de referință și același număr de cadre.


## Features

- Introducerea manuală a șirului de referință  
- Configurarea numărului de cadre  
- Selectarea algoritmului de înlocuire  
- Rulare pas cu pas sau rulare completă  
- Vizualizarea în timp real a stării cadrelor  
- Afișarea metricilor de performanță  
- Generarea de grafice comparative  
- Salvarea rezultatelor în fișiere CSV  

---

## Architecture

Proiectul este structurat modular, separând logica aplicației de interfața grafică.
```text
src/
│
├── VirtualMemorySimulation.Core/
│ ├── Algorithms/
│ ├── Models/
│ ├── Services/
│ └── Utilities/
│
└── VirtualMemorySimulation.UI/
├── MainWindow.xaml
├── ChartsWindow.xaml
└── App.xaml
```

### Core Layer
Conține logica aplicației:
- implementarea algoritmilor
- gestionarea simulării
- calculul statisticilor
- persistența datelor

### UI Layer
Conține interfața grafică WPF:
- afișarea cadrelor
- controlul simulării
- vizualizarea graficelor

## Implemented Algorithms

### FIFO (First-In, First-Out)

- Elimină pagina care a fost încărcată prima în memorie.
- Nu ține cont de recența sau frecvența accesărilor.
- Simplu, dar poate produce înlocuiri ineficiente.


### LRU (Least Recently Used)

- Elimină pagina care nu a mai fost utilizată de cel mai mult timp.
- Se bazează pe principiul localității temporale.
- Oferă performanță mai bună decât FIFO în majoritatea cazurilor.


### Optimal (OPT)

- Elimină pagina care va fi utilizată cel mai târziu în viitor.
- Oferă numărul minim teoretic de page fault-uri.
- Nu poate fi implementat într-un sistem real, fiind utilizat ca referință de performanță.


## Performance Metrics

Aplicația calculează următoarele metrici:

- **Total Page Faults**
- **Fault Rate (%)**
- **Average Memory Access Time (AMAT)**
- **Memory Usage (%)**

AMAT este influențat direct de rata de page fault-uri și reflectă impactul algoritmului asupra performanței sistemului.


## Data Persistence

Rezultatele simulărilor sunt salvate automat în fișiere CSV în directorul aplicației:
```text
VirtualMemorySimulation.UI/bin/Debug/
```


Fiecare rulare este salvată separat și conține:

- Pasul simulării
- Pagina accesată
- Indicator de page fault (1/0)
- Starea cadrelor de memorie

Acest mecanism permite analiza ulterioară și compararea rezultatelor între algoritmi.

## Tested Scenarios

Proiectul include testarea următoarelor cazuri:

- Best-case pentru fiecare algoritm
- Worst-case pentru fiecare algoritm
- Șiruri fără repetiții
- Configurații cu număr redus de cadre
- Compararea directă a algoritmilor pe același input


## How to Run

1. Clonați repository-ul:
```text
git clone https://github.com/denisaradu28/VirtualMemorySimulator.git
```
2. Deschideți fișierul `.sln` în Visual Studio.
3. Selectați `VirtualMemorySimulation.UI` ca Startup Project.
4. Rulați aplicația.

## Technologies Used

- C#
- .NET
- WPF
- OxyPlot (pentru grafice)
- CSV pentru persistența datelor

## Educational Purpose

Proiectul are scop didactic și permite:

- Înțelegerea vizuală a algoritmilor de paginare
- Observarea diferențelor dintre strategii de înlocuire
- Analiza impactului page fault-urilor asupra performanței

## Future Improvements

- Implementarea algoritmului Clock (Second Chance)
- Import automat al șirurilor din fișier
- Comparare simultană într-un singur ecran
- Export PDF al rezultatelor
- Adăugarea testelor unitare
