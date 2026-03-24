# 🦠 CatchCells: AI-Driven Evasion Game

![Unity](https://img.shields.io/badge/Unity-2022.3%2B-black?style=for-the-badge&logo=unity)
![ML-Agents](https://img.shields.io/badge/ML--Agents-Release%2020-blue?style=for-the-badge)
![C#](https://img.shields.io/badge/C%23-Programming-green?style=for-the-badge&logo=c-sharp)

**CatchCells** es un proyecto interactivo desarrollado en Unity que integra **Machine Learning** a través del framework **Unity ML-Agents**. En este juego, el jugador tiene el objetivo de hacer clic y "atrapar" células antes de que se acabe el tiempo. Sin embargo, estas células no son estáticas ni tienen un comportamiento programado fijo (hardcoded); son **agentes inteligentes** entrenados con Aprendizaje por Refuerzo Proximal (PPO) que aprenden activamente a evadir el cursor del mouse y a camuflarse con el entorno.

---

## ✨ Características Principales

* **🤖 Inteligencia Artificial (ML-Agents):** Los NPCs (células) son agentes entrenados que evalúan el entorno en tiempo real. Aprenden a tomar decisiones de movimiento, cambiar su escala y adaptar su color basándose en un sistema de recompensas y penalizaciones.
* **🎨 Camuflaje Dinámico:** El escenario está dividido en cuadrantes de diferentes colores. La red neuronal de las células calcula el nivel de contraste respecto al fondo y premia el camuflaje efectivo, haciendo que las células coincidan visualmente con el terreno sobre el que se desplazan.
* **🔄 Modo Híbrido (Play/Training):** El juego incluye un `TrainingManager` integrado que permite alternar entre:
  * **Modo Play (Manual):** El jugador usa el mouse para atrapar a las células.
  * **Modo Training (Automático):** El mouse se mueve automáticamente mediante scripts para generar escenarios iterativos, acelerando el proceso de entrenamiento de la IA sin que un humano deba mover el mouse.
* **⚡ Rendimiento Optimizado:** El código base está rigurosamente perfilado. Reemplaza los cálculos de distancias por sus variantes matemáticas elevadas al cuadrado (`sqrMagnitude`), aplica Inyección de Dependencias para eliminar cuellos de botella de búsqueda (`GameObject.Find`), y cachea variables para evitar fugas de recolección de basura (GC Alloc) en las actualizaciones constantes de la interfaz de usuario (UI).

---

## 🏗️ Arquitectura y Funcionamiento del Agente

El núcleo del proyecto recae en el script `CellAgent.cs` que maneja las interacciones del modelo de Machine Learning pre-entrenado (`.onnx`).

### Observaciones (Inputs al Cerebro)
Para tomar decisiones lógicas, la célula observa 7 valores continuos extraídos del motor del juego por *Step*:
1. El color exacto del fondo sobre el que está posicionada (Canales R, G, B).
2. Su propia posición en el espacio 2D (X, Y).
3. La distancia subyacente hacia el puntero del Mouse.
4. El tiempo restante de la ronda limitadora (normalizado de 0 a 1).

### Acciones (Outputs desde el Cerebro)
Con base en sus observaciones y sin programación condicional previa, la red neuronal dicta *Discrete Actions* (Decisiones discretas):
1. **Color:** Escoge entre una docena de variaciones de paletas de color para intentar igualar el fondo y volverse invisible al jugador.
2. **Tamaño:** Ajusta su capa transformadora entre 6 niveles predefinidos (hacerse pequeña y huidiza).
*(El escape cinemático del mouse es delegado a las Físicas de Unity dentro de `CellBehaviour.cs` para mantener una respuesta instantánea y fluida, mientras la IA domina la toma de decisiones estéticas y de táctica global de evasión visual).*

### Recompensas (Rewards)
- **Premios (Positive Rewards):** Por cada Step de simulación vivo extra, sobrevivir la ronda entera sin recibir clics, alejarse de forma evasiva a más de x unidades del mouse y conseguir el camuflaje minimizando el contraste absoluto frente al objeto base del piso.
- **Castigos (Negative Penalties):** Ser alcanzada y clickeada por el humano, salirse fuera de la pantalla jugable (límites *OutOfBounds*), o no ser capaz de evadir el puntero en su cercanía.

---

## 🚀 Requisitos Técnicos e Instalación

Para ejecutar este repositorio, visualizar el proceso de la IA en tiempo real o continuar entrenando el modelo se requiere de:

1. **Unity Editor**: Versión recomendada `2022.3.x` LTS.
2. **Python Environment**: Una instalación de Python (3.8.x a 3.10.x) local o virtualizada vía Anaconda.
3. **ML-Agents Package**: El paquete de Unity (`com.unity.ml-agents` Release 20 / v0.28.0+) integrado al Core de Unity.

### Instalación del Proyecto Local

1. Clona el repositorio a tu máquina local:
   ```bash
   git clone https://github.com/TU-USUARIO/CatchCells-devHibrido.git
   ```
2. Ejecuta Unity Hub, pulsa sobre la opción **Open** (Añadir proyecto desde el disco) y navega hasta donde ubicaste la raíz de `CatchCells-devHibrido`.
3. Asegúrate de permitir a Unity resolver los paquetes ubicados en el `manifest.json`.
4. En el dock de Asset Manager, inicia visualizando la escena principal ubicada usualmente dentro de la carpeta `Assets/_Project/`.

---

## 🎮 Cómo Jugar y Manipular el Simulador

### Empleo General del Modo "Play"
1. Al colocar "Play" encima del Editor, de inmediato serás adentrado en la Ronda inicial.
2. Debes valerte de tu ratón para apuntar con precisión (o rapidez cazar) haciéndole click (o pulsación) directo sobre las "células" representadas por esferas.
3. Mientras tratas de tocarlas, ellas cambiarán de tamaño, efectuarán parpadeos en los valores Alfa (`SpriteRenderer`), y **tratarán de despistarte al hacerse del color exacto del cuadrante de fondo**.

### Entrenar a la IA localmente (Modo Setup "Training")
Si quieres continuar enseñándole a las redes neuronales a evadir mejor o a dominar un panorama completamente diferente de colores:
1. Inicializa tu entorno Python en la terminal raíz y asegúrate de tener el módulo CLI oficial activado (`pip install mlagents==0.28.0`).
2. Digita el siguiente comando de aprendizaje en consola (puedes variar el identificador `run-id` a voluntad):
   ```bash
   mlagents-learn Assets/_Project/ML/Configs/cellagent_config.yaml --run-id=CatchCells_V2 --force --time-scale=20
   ```
3. Acto siguiente, da **Play** en Unity para que el puente comunicacional (Python-C#) se empareje.
4. En el Canvas de Unity del juego, pulsa el botón en pantalla de **Training Mode** para que el Mouse comience una ruta de vuelo automático, permitiendo que la simulación juegue de un lado a otro aceleradamente durante horas.
5. Copia el archivo arrojado en `.onnx` hacia la carpeta de tu cerebro visual, dentro de su casilla como "Behavior Parameters".

---

## 👥 Arquitectura de Desarrollo (Autores)

Este proyecto fue planificado, diagramado y estructurado de manera ágil y controlando Sprints.

* **Ricardo (@ricardonp501):** Core programing lógico del Gameplay, diseño e infraestructura de Agentes de IA, definición de tensores de *Rewards & Observations* e interacciones de física optimizada (Gestión de rendimiento CPU / RAM).
* **Alison (@pizarroapazaalisonbelen):** Diseño Visual/UX del software, control paramétrico de la dificultad dinámica, integraciones del menú híbrido de entrenamiento y balance integral algorítmico contra el jugador.
