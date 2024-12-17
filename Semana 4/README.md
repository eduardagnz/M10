# Criando Métricas no .NET
Este tutorial demonstra como criar e monitorar métricas personalizadas em aplicativos .NET usando as APIs `System.Diagnostics.Metrics`. A instrumentação de métricas permite rastrear valores importantes para entender melhor o desempenho e comportamento de seus aplicativos.

## Pré-requisitos
- **SDK do .NET Core 6 ou superior**.
- **Pacote NuGet System.Diagnostics.DiagnosticSource** (incluso por padrão no .NET 8+).

Para esse caso, estou utilizando a versão 7.0


## Criar uma Métrica Personalizada

1. **Criar um novo aplicativo console:**
   ```bash
   dotnet new console
   dotnet add package System.Diagnostics.DiagnosticSource
   ```

### Evidências da aplicação: 

2. **Atualizar o arquivo `Program.cs`:**
   ```csharp
   using System;
   using System.Diagnostics.Metrics;
   using System.Threading;

   class Program
   {
       static Meter s_meter = new Meter("HatCo.Store");
       static Counter<int> s_hatsSold = s_meter.CreateCounter<int>("hatco.store.hats_sold");

       static void Main(string[] args)
       {
           Console.WriteLine("Pressione qualquer tecla para sair");
           while (!Console.KeyAvailable)
           {
               // Simula a venda de 4 chapéus por segundo
               Thread.Sleep(1000);
               s_hatsSold.Add(4);
           }
       }
   }
   ```

3. **Executar o aplicativo:**
   ```bash
   dotnet run
   ```

---
![alt text](<../Semana 4/imgs/image2.png>)

Resultado: 
![alt text](<../Semana 4/imgs/image.png>)

## Monitorar a Nova Métrica

1. **Instalar a ferramenta `dotnet-counters`:**
   ```bash
   dotnet tool update -g dotnet-counters
   ```

2. **Monitorar o contador criado:**
   ```bash
   dotnet-counters monitor -n metric-demo.exe --counters HatCo.Store
   ```
   Exemplo de saída:
   ```
   [HatCo.Store]
   hatco.store.hats_sold (Count / 1 sec)                          4
   ```

---

## Boas Práticas

- Use nomes únicos para `Meter` e instrumentos, seguindo convenções como as do **OpenTelemetry** (nomes hierárquicos pontilhados e letras minúsculas, como `contoso.ticket_queue.duration`).
- Evite usar variáveis estáticas em bibliotecas que dependem de **Injeção de Dependência (DI)**.
- As APIs de métricas são **thread-safe** e apresentam desempenho otimizado.

---

## Tipos de Instrumentos Suportados

- **Counter**: Mede valores acumulativos.
- **Histogram**: Mede distribuições de valores.
- **Gauge**: Mede o estado atual de uma variável.

## COnclusão