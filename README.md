# MarketPlace
Asp.net Core проект маркетплейса с микросервисной архитектурой.

Стэк:
ASP.NET Core 8, PostgreSQL, RabbitMQ, Redis, REST и gRPC, JWT Auth.
Проект был разработан с DDD и Clean architecture, полностью упакован в docker-compose.
- Для отказоустойчивости используется паттерн Outbox.
- Оркестрация осуществляется паттреном SAGA.

Проект Marketplace является веб-приложением магазина. Данный проект позволяет просматривать каталог товара,
создавать заказ, оформлять заказ в выбранное пользователем место (пункт выдачи) и осуществлять покупку.

Веб приложение содержит авторизацию, у пользователя есть имя и баланс кошелька.

Ниже приведены экраны приложения:

Экран авторизации.

![img_auth.png](images/img_auth.png)

Экран каталога товаров и корзины.

![img_1.png](images/img_1.png)

Экран оформления заказа.

![img_2.png](images/img_2.png)

Экран осуществленных покупок.

![img_3.png](images/img_3.png)


This repo contains docker-compose file, so if you have installed docker in your computer:
1) run "docker-compose up -d"
2) go to "http://localhost:5173/"
3) login with user "patochin@gmail.com" and password "12345"
4) enjoy it !!!
