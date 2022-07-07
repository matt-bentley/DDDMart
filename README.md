# DDDMart

This is a small sample application for a fake eCommerce company (**DDDMart**) which uses ***Domain Driven Design*** techniques for modelling the domain.

The application focuses on the core Domain layer so does not have any APIs. There are a lot of different opinionated ways to host domain models through an API and I specifically wanted to focus on modelling the domain and how Domain and Integration events are handled.

The **DDDMart** console application hosts a number of background services which are used to simulate how an **Order** is generated and paid using the different ***bounded contexts***.

## Sub-Domains

The domain is made up of 3 sub-domains:
- Catalogue (Core) - **Product** catalogue
- Ordering (Core) - Used for creating a **Basket** and creating an **Order** of different Products
- Payments (Supporting) - Used for generating and tracking **Invoices** and storing customer **Payment Methods**

Each sub-domain is made up for 1 or more bounded-contexts. The application has been layer to allow each sub-domain to be hosted as a separate **microservice**.