# WebCrawler Iprazos 2025

Desafio técnico desenvolvido em C# (.NET 9) com objetivo de criar um web crawler multithread para coleta de proxies no site https://proxyservers.pro

# Objetivo

O programa acessa automaticamente o site e extrai os seguintes dados de todas as páginas disponíveis:

IP Address

Port

Country

Protocol

Os resultados são salvos em JSON, e os metadados da execução são gravados em um banco SQLite local.

# Tecnologias Utilizadas

.NET 9 Console App

HtmlAgilityPack – parsing do HTML

Entity Framework Core (SQLite) – persistência de dados

System.Text.Json – geração do arquivo JSON

Parallel.ForEachAsync + MaxDegreeOfParallelism – controle de concorrência (3 threads simultâneas)

# Como Executar

Clonar o repositório

git clone https://github.com/MatheusCalazans/WebCrawlerIprazos
cd WebCrawlerIprazos

Restaurar dependências
dotnet restore

Executar o projeto
dotnet run

Durante a execução, o programa exibirá o progresso no console, processando até 3 páginas simultaneamente.

# Saída dos Arquivos

Durante a execução, são gerados automaticamente os seguintes arquivos:

HTMLs das páginas - bin/Debug/net9.0/Outputs/Html/	Cópias locais de cada página visitada

JSON final - bin/Debug/net9.0/Outputs/Json/proxies_YYYYMMDD_HHmmss.json	Resultado da extração

Banco SQLite - bin/Debug/net9.0/crawler.db	Registro das execuções (metadados)

💡 Os arquivos são gravados no diretório de execução padrão do .NET (bin\Debug\net9.0). 

# Metadados Armazenados

Cada execução do crawler é registrada no banco crawler.db, na tabela Execucoes, com os seguintes campos:

HoraInicio:	Data/hora em que o crawler iniciou
HoraFinal:	Data/hora de término
PaginasProcessadas:	Quantidade total de páginas visitadas
TotalRegistros:	Quantidade total de proxies extraídos
CaminhoJson:	Caminho do arquivo JSON gerado


# Observação Importante sobre o Campo "Porta"

O site proxyservers.pro
 carrega o campo Port dinamicamente via JavaScript.
Como este projeto utiliza HtmlAgilityPack (que faz parsing de HTML estático), o valor da porta pode vir vazio no resultado.


# Possíveis Melhorias Futuras

Suporte a Playwright para capturar o campo Port real.
Interface web simples para visualizar as execuções.
Testes unitários para validação de parsing.

# Matheus Peres Calazans
 mathcalazanss@gmail.com
 https://www.linkedin.com/in/calazansmatheus/
