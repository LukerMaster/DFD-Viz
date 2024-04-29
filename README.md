![DFDViz-icon](https://github.com/LukerMaster/DFD-Viz/assets/35941818/19fa79ac-f0e9-4e14-9d1e-3abbfdba7601)
# DFD-Viz
DFD-Viz is a solution for easily drawing multi-level DFD diagrams using DFD-script (very simple declarative language designed for non-tech people).
## DFD-Script fast start
Here is a simple snippet to draw a simple, multilevel DFD diagram:
```dfd
# Level 1
Process SomeProcA "Process A":
	# Level 2
	Process SomeProcB "Process B"
	Process SomeProcC "Process C"
	SomeProcB --> SomeProcC "Process pipeline"

Storage DbA "Database A"

IO SomeInputA "Input A"

SomeInputA --> DbA "Logging into DB"
```
will produce:

![diagram](https://github.com/LukerMaster/DFD-Viz/assets/35941818/0ad1f2d6-b5ae-4904-b860-468a7b09b226)
## Features
- Diagram creation purely from code
- Collapsing diagram levels with a single click
- Exporting diagrams to png files
## Program is currently in very early stages of production
Things may break and be incompatible across versions.
