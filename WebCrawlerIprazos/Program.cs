using System.Text.Json;
using WebCrawlerIprazos.Data;
using WebCrawlerIprazos.Models;
using WebCrawlerIprazos.Services;

Console.WriteLine("Iniciando web crawler...");

var inicio = DateTime.Now;


Directory.CreateDirectory(Path.Combine("Outputs", "Html"));
Directory.CreateDirectory(Path.Combine("Outputs", "Json"));

var crawler = new CrawlerService();
var registros = new List<Proxy>();


var urlBase = "https://proxyservers.pro/proxy/list/order/updated/order_dir/desc";


int totalPaginas = await crawler.ObterNumeroTotalPaginasAsync(urlBase);
Console.WriteLine($"Total de páginas detectadas: {totalPaginas}");

var opcoes = new ParallelOptions { MaxDegreeOfParallelism = 3 };


await Parallel.ForEachAsync(
    Enumerable.Range(1, totalPaginas),
    opcoes,
    async (pagina, token) =>
    {
        var url = $"{urlBase}/page/{pagina}";
        var proxies = await crawler.ObterProxiesAsync(url, pagina);

        
        lock (registros)
        {
            registros.AddRange(proxies);
        }

        Console.WriteLine($"Página {pagina} processada ({proxies.Count} proxies)");
    });

var final = DateTime.Now;


registros = registros
    .OrderBy(p => p.PaginaOrigem)
    .ToList();


var caminhoJson = Path.Combine("Outputs", "Json", $"proxies_{DateTime.Now:yyyyMMdd_HHmmss}.json");


await File.WriteAllTextAsync(
    caminhoJson,
    JsonSerializer.Serialize(registros, new JsonSerializerOptions { WriteIndented = true })
);


using var db = new AppDbContext();
db.Database.EnsureCreated();

db.Execucoes.Add(new Execucao
{
    HoraInicio = inicio,
    HoraFinal = final,
    PaginasProcessadas = totalPaginas,
    TotalRegistros = registros.Count,
    CaminhoJson = caminhoJson
});

await db.SaveChangesAsync();


Console.WriteLine($"\nCrawler finalizado com sucesso!");
Console.WriteLine($"Duração total: {(final - inicio).TotalSeconds:F1}s");
Console.WriteLine($"Páginas processadas: {totalPaginas}");
Console.WriteLine($"Total de proxies: {registros.Count}");
Console.WriteLine($"JSON salvo em: {caminhoJson}");
Console.WriteLine($"Banco: crawler.db");
