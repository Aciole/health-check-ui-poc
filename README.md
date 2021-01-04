# Health Check Monitoramento de Aplicações

O Health Check Pattern é um padrão de projeto do tipo observability, para arquitetura de microsserviços;

## Referências

[Padrão de monitoramento do ponto de extremidade de integridade (Microsoft)](https://docs.microsoft.com/pt-br/azure/architecture/patterns/health-endpoint-monitoring)
> É uma boa prática e geralmente um requisito de negócios para monitorar serviços back-end e aplicativos Web e garantir que eles estejam disponíveis e funcionando corretamente.  No entanto, é mais difícil monitorar serviços em execução na nuvem do que monitorar serviços locais.  Por exemplo, você não possui o controle total do ambiente de hospedagem e os serviços geralmente dependem de outros serviços fornecidos por fornecedores de plataformas e outros.

[Implement health checks in ASP.NET Core services (eShopOnContainers)](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/monitor-app-health)
> O aplicativo eShopOnContainers é um aplicativo de referência de software livre para .NET Core e microsserviços criado para ser implantado usando contêineres do Docker.  O aplicativo consiste em vários subsistemas, incluindo vários front-ends de interface do usuário de e-Store (um aplicativo Web MVC, um SPA da Web e um aplicativo móvel nativo).  Ele também inclui os microsserviços e os contêineres de back-end de todas as operações do servidor necessárias.

[How to set up ASP.NET Core 2.2 Health Checks with BeatPulse's AspNetCore.Diagnostics.HealthChecks (Scott Hanselman)](https://www.hanselman.com/blog/how-to-set-up-aspnet-core-22-health-checks-with-beatpulses-aspnetcorediagnosticshealthchecks)
> Scott Hanselman is a former professor, former Chief Architect in finance, now speaker, consultant, and Microsoft employee.

## Serviços para ser monitorado
[Instale o packager](https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks#Health-Checks) correspondente as dependências do seu projeto, ou crie o seu;

 ex:
 - Sql Server;
 - Redis;
 -  RabbitMQ;

```csharp
public void ConfigureServices(IServiceCollection services)
{
	...
	services.AddHealthChecks()
		.AddSqlServer(Configuration["Data:ConnectionStrings:Sql"])
		.AddRedis(Configuration["Data:ConnectionStrings:Redis"]);
	...
}
```
#### Instaler o package **AspNetCore.HealthChecks.UI.Client** e disponibilize a rota que mostrará a saúde desse serviço:
```csharp
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
	  {
		  ...
		  app.UseEndpoints(endpoints =>
		  {
			  endpoints.MapControllers();
 
			  endpoints.MapHealthChecks("/hc", new HealthCheckOptions()
			  {
				  Predicate = _ => true,
				  ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
			  });
		  });
		  ....
	  }
```

## Monitor de aplicações

Instale o packager **AspNetCore.HealthChecks.UI** e uma base de dados para esse portal, esse exemplo vou instalar InMemory, portanto usarei o **AspNetCore.HealthChecks.UI.InMemory.Storage**, confira outro [aqui](https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks#UI-Storage-Providers), ele precisa de uma base de dados, pois ele registra o tempo de indisponibilidade de uma dependência.

#### Registre as rotas das aplicações que serão monitoradas:
```csharp
public void ConfigureServices(IServiceCollection services)
	   {
		   ...
		   services.AddHealthChecks();
		   services
			   .AddHealthChecksUI(
			   setup =>
			   {
				   setup.AddHealthCheckEndpoint("Microsservice.One", "https://localhost:44385/hc");
				   setup.AddHealthCheckEndpoint("Microsservice.Two", "https://localhost:44332/hc");
			   })
			   .AddInMemoryStorage();
		   ...
	   }

public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
	   {
		   ...
		   app.UseRouting()
				 .UseEndpoints(config =>
				 {
					 config.MapControllers();
					 config.MapHealthChecksUI();
				 });
 
		   app.UseHealthChecksUI(setup =>
		   {
			   setup.UIPath = "/hc-ui";
		   });
		   ...
	  }
```

### Resultado
O painel de monitoramente por padrão tem essa aparencia:


Caso queira customizar, seguir documentação a [aqui](https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks#ui-style-and-branding-customization).
