# transactional-outbox-sample

Este repo contiene una implementación sencilla del patrón Transactional Outbox con dotnet usando : MSSQL y RabbitMQ.

## Instrucciones

* Ejecute docker-compose para aprovisionar MSSQL y RabbitMQ
* Aplique las migraciones de la carpeta Migrations
* Ejecute la solucion y acceda a la interfaz swagger en http://localhost:5073/swagger y cree una orden (el payload del endpoint no requiere de nada, pero usted puede modificarlo para ajustarlo a su necesidad )
