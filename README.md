# Regions with API (Back-end)

## Summary
Create an API to expose data coming from CSV files.

## Description
You are provided with two CSV files - Regions.csv and Employees.csv.  Each row within Regions.csv defines a region. For each region the line provides the name of the region, the region ID and the parent region ID if any.  For example the following lines define the region Malta, its parent southern Europe, and its parent Europe.

Malta, 470, 39

Southern Europe, 39, 150

Europe, 150

Each row within Employees.csv, each line provides the region ID where that employee works followed by his name and surname.  For example the following lines define the employee Nick Cage who works in Malta and the employee Carmen De Niro who works in Europe.

470, Nick, Cage

150, Carmen, De Niro

## Functional Requirements
The application will work as follows:

- Parse the provided CSV files and use the data within them to seed a new database.
- Expose a RESTful Web API with the following endpoints: (a) An HTTP GET endpoint /region/{id}/employees which, given a region ID, retrieves all of the employees within that region. This endpoint should return any employee working in the defined region as well as any region that is a descendant of that region - e.g. if I search for employees in Europe I should get all the employees marked as working in Europe, one of its sub regions or any country within them. The function should output the first and last name of the employee together with all the regions he belongs to.  (b) An HTTP POST endpoint /employee which allows callers to submit new employee information to be persisted. (c) An HTTP POST endpoint /region which allows callers to submit new region information to be persisted.
- Build a very simple UI using either plain JavaScript, jQuery or any other client side framework of your choice to allow interaction with the HTTP GET endpoint only.

## Technical Requirements
The application will work as follows:

- Make use of one or more Web API controllers to handle all asynchronous requests.

- You may start off by storing the parsed CSV data in memory to quickly build the HTTP GET endpoint, and if there is time, replace it later with a SQL database. You may use whatever mechanism you prefer (e.g. ADO .NET, Entity Framework, Dapper, etc) to communicate with the database.

- All server side code should be asynchronous.

- You can use any boilerplate or starting projects of your choice.

- Do not worry about authentication for the scope of this task.

- Do not spend any time on styling.

- If time permits, structure your code as if you were building this for production.

- If time permits, include dependency injection as a mechanism for bootstrapping your service(s).
