**Keuzes**

- Dto project - Afscheiding van data dat de eind gebruiker zal zien en wat er in de DB zit
- Database project - EF met een override op saveChanges om modified data altijd te vullen wanneer nodig
- Tests project - Alleen nog maar een test voor Automapper waarmee je check of alle mappings valid zijn
- Api project
  - MediatR - Afscheiding van Single responsibility code om de controller schoon te houden 
  - AutoMapper - Automapper om DB entities om te zetten naar DTO modellen
  - FluentValidation - Valideren van de gebruiker input via de API
  - Hosted service - Service die om de 24 uur wordt uitgevoerd om nieuwe MazeTv shows op te halen
    - Berekend start pagina
    - 11 seconden request pause na 20 calls (Rate limit) 



**Wat had ik nog willen doen**

- Had meer unit tests kunnen doen, mogelijk om de fluentvalidation te testen
- De hosted service iets opschonen en running state bijhouden zodat je dit kan controleren
- IsArchived zou weer weg kunnen na de start page methode in de hosted service