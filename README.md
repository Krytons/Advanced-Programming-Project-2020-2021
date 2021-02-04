# Advanced Programming Languages : Name TBD Project
### Un progetto di:
- Bartolomeo Caruso (1000008726)
- Gabriele Costanzo (1000014221)
- Giuseppe Fallica (1000013477)

---

## 0. Prefazione
I tre membri del team, con l'intento di dimostrare ai professori del corso di aver acquisito le conoscenze richieste dei singoli PL e di aver sviluppato capacità decisionali relative alla scelta di un dato PL rispetto ad un altro sulla base di punti di forza e debolezze, domini applicativi, paradigmi supportati, etc.

Concordano sulla seguente struttura come la più ottimale per raggiungere il suddetto obiettivo:
- **CORE (C++ e/o C#):**

    Una solida *foundation* per il progetto software, capace di gestire in maniera solida, sicura ed efficiente diversi task.

    In particolare:
    - C++ per gestire librerie di calcoli matematici efficienti, per delineare la struttura Object-Oriented dei dati, per gestire I/O file ed eccezioni
    - C# in sostituzione di C++, o in alternativa adoperato come framework *on-top-of* C++, potendo fornire un'astrazione a più alto livello dei dati e delle funzioni, e gestire una GUI del software

- **BACKEND e API (Python o R):**

    Un server costantemente aperto in parallelo al processo principale, simulazione di un eventuale server reale separato dal client oppure componente integrato nel client che si occupa di gestire la comunicazione col server.

    Fornisce un'API di comunicazione con il CORE del client, e si occupa di processare dati ottenuti da API di terze parti, per strutturarli in accordo con il modello Object-Oriented stabilito nel CORE e/o per effettuare qualche leggero processamento dei dati (i.e. data mining, o interrogazione rispetto ad una I.A. / rete neurale addestrata offline o dal CORE).

- **SERVIZI AGGIUNTIVI (R, Go):**

    Un set di servizi paralleli al CORE e richiamati on-demand, o come eseguibili offline (i.e. un service che genera un grafico o un'immagine sulla base di un dataset), o come server interrogabili online (i.e. una rete neurale o un sistema di profilazione degli utenti).

Una volta concordata la suddetta struttura a tre macro-componenti, le seguenti proposte sono state prese in considerazione:

---

1. **Sistema di tracciamento dei prezzi di Amazon**

    Un software il cui scopo è quello di permettere ad un utente utilizzatore di cercare un prodotto dalla piattaforma Amazon.com e ottenere informazioni relative all'andamento dei prezzi di tale prodotto: tali informazioni verranno restituite all'utente mediante l'utilizzo di grafici con precisione al singolo giorno.
    
    Tale software permetterà all'utilizzatore di registrarsi al servizio, al fine di poter tracciare un prodotto e ricevere una notifica non appena il prezzo sarà al di sotto di una soglia decisa dall'utente stesso.
    
    **STRUTTURA:**
    - CORE:
        - **C#:** 
        
            User Interface grafica, pulsanti, struttura orientata agli oggetti di base
        - **C++:** 
        
            Librerie per la creazione e manipolazione di grafici dinamici
    - BACKEND:
        - **Python:**
            
            Gestione della comunicazione con Amazon Free API per ottenere tutte le informazioni relative ad un generico prodotto 
            
            Gestione della comunicazione con un database ai fini di conservare informazioni relative all'andamento dei prezzi dei vari prodotti e per gestire le informazioni degli utenti iscritti al servizio.
            
            Gestione generale della logica di backend mediante l'utilizzo di un framework come DJango
    - SERVIZI EXTRA:
        - **R:**
            
            Profilazione degli utenti iscritti al servizio mediante un'analisi dei dati relativi ai prodotti ricercati e/o tracciati, ai fini di poter consigliare all'utente dei prodotti in saldo conformi ai gusti personali di quest'ultimo.    
            
---

2. **Sistema di image editing**

    Un software con limitate funzionalità di modifica delle immagini, quali (ad esempio): disegnare sull'immagine; modificare luminosità, contrasto e saturazione; applicare filtri predefiniti (bianco/nero, blur, rimozione green-screen, etc.).

    Le immagini, una volta modificate, potranno essere salvate in locale o caricate su siti esterni tramite API di terze parti.

    **STRUTTURA:**
    - CORE:
        - **C#:** 
        
            User Interface grafica, pulsanti, struttura orientata agli oggetti di base
        - **C++:** 
        
            librerie di editing di immagini a basso livello (i.e. OpenCV), linkate al framework soprastante in C#
    - BACKEND:
        - **Python:**
        
            gestisce la comunicazione con API di ricerca di immagini per fornire l'immagine da modificare (alternativa al file in locale);
            
            gestisce la comunicazione con API di terze parti per caricare l'immagine modificata online;
            
            *extra:* gestisce un database locale degli utenti per caricare i propri lavori in forma di galleria.
    - SERVIZI EXTRA:
        - **R:**

            poiché i filtri di immagini sono trasformazioni matriciali, si potrebbe adoperare una sottoroutine scritta in R per ottimizzare filtri più complessi, o per gestire una libreria di filtri custom definiti dall'utente

---

3. **Solitario**

    Alcuni giochi famosi e già più volte implementati su social e siti web vari sono a singolo giocatore e basati su regole matematiche e matriciali ben definite.

    Si prenda, a titolo d'esempio, il Sudoku. Qualunque gioco con una struttura simile (cubo di Rubik, solitario, oppure giochi come Tris o Forza Quattro con avversario I.A.) può essere considerato in alternativa.

    Il sistema potrebbe occuparsi di preparare una tabella di Sudoku grazie ad un ragionamento del CORE (generato randomicamente, con diversi livelli di difficoltà) oppure scaricandola da un'API esterna.

    Il sistema stamperà a schermo la griglia, permettendo al giocatore di riempirla e cercare di risolverla.
    Il CORE dovrà occuparsi anche di calcolare la *"legalità"* di ogni mossa, ed eventuali mosse consigliate.

    Il BACKEND si occuperà di tenere una classifica degli utenti. Il punteggio può basarsi su diversi fattori: numero di puzzle risolti, o - per il singolo puzzle - numero di errori commessi o tempo impiegato per risolverlo.

    **STRUTTURA:**
    - CORE:
        - **C#:** 
        
            User Interface grafica, pulsanti, struttura orientata agli oggetti di base.

            Può prevedere l'inserimento dei numeri nella griglia da tastiera o con tastierino GUI.

            Può prevedere un timer aggiornato ogni secondo, e un sistema di *"hints"* sbloccati in automatico ogni X secondi.
        - **C++:** 
        
            il "cervello" del gioco, data la griglia deve saper calcolare se si sta per commettere una mossa "illegale", ed eventuali mosse "legali" disponibili per il suggetimento.
    - BACKEND:
        - **Python:**
        
            gestisce una leaderboard degli utenti che hanno giocato.

            permette di scaricare altri Sudoku, o tramite un'API di terze parti, o creati dai giocatori stessi con un tool apposito
    - SERVIZI EXTRA:
        - **R:**

            laddove la logica sia troppo complessa da implementare in C++, potrebbe fornire un servizio per la generazione randomizzata di una griglia di date dimensioni e caratteristiche

---

Infine, il progetto #? è stato scelto per i seguenti motivi:
- motivo 1
- motivo 2
- motivo 3

---

## 1. Introduzione
