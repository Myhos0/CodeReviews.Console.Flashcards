# Flashcards.
My first intermediate console project on C# Academy.  
A console application for managing flashcards and study sessions, respectively.

# Requeriments.  

- [x] This is an application where the users will create Stacks of Flashcards.
- [x] You'll need two different tables for stacks and flashcards. The tables should be linked by a foreign key.
- [x] Stacks should have a unique name.
- [x] Every flashcard needs to be part of a stack. If a stack is deleted, the same should happen with the flashcard.
- [x] You should use DTOs to show the flashcards to the user without the Id of the stack it belongs to.
- [x] When showing a stack to the user, the flashcard Ids should always start with 1 without gaps between them.
If you have 10 cards and number 5 is deleted, the table should show Ids from 1 to 9.
- [x] After creating the flashcards functionalities, create a "Study Session" area, where the users will study the stacks.
 All study sessions should be stored, with date and score.
- [x] The study and stack tables should be linked. If a stack is deleted, it's study sessions should be deleted.
- [x] The project should contain a call to the study table so the users can see all their study sessions.
This table receives insert calls upon each study session, but there shouldn't be update and delete calls to it.

# C# Academy Challenge.
If you want to expand on this project, here’s an idea. Try to create a report system where you can see the number of sessions per month per stack.
And another one with the average score per month per stack. This is not an easy challenge if you’re just getting started with databases,
but it will teach you all the power of SQL and the possibilities it gives you to ask interesting questions from your tables.

# Features
- The Program uses a SQL Server local database to create and manage the data needed for its operation.
- In the main directory, there is a file named DataBaseFlashcards.sql that you can use to create and populate the database with data for your tests.
- Dapper was used to query the database.
- When displaying the IDs of the various data entries in the tables, they will be displayed consecutively without any gaps between them.

- **Spectre Console UI**
  - The application uses a Spectre Console UI where we can navigate using the keyboard.

<p align="center">
  <img width="224" height="153" alt="imagen" src="https://github.com/user-attachments/assets/f004dbde-811e-4bb6-aadd-fcda406d188c" />
</p>

- The first menu shows four options:
    - Stack
    - StudySession
    - ReportSystem
    - Exit (Close the program)

 ## Stack
 - The stack menu shows us a basic CRUD for managing each of them.
   
 <p align="center">
   <img width="361" height="218" alt="imagen" src="https://github.com/user-attachments/assets/80ee1141-4db9-49d6-958a-d2a52c91af88" />
 </p>

 - We also have an additional option where you can select a stack to add the corresponding flashcards to it.

<p align="center">
  <img width="724" height="218" alt="imagen" src="https://github.com/user-attachments/assets/0b35cca7-3193-4f5f-8571-f667a1d10e07" />
</p>

- ## Flashcard
  - The stack menu shows us a basic CRUD for managing for each of the flashcards in the respective stack.

<p align="center">
  <img width="1473" height="216" alt="imagen" src="https://github.com/user-attachments/assets/dc66e642-df1f-4362-8612-20a27fbe626a" />
</p>

## StudySession
- To access the Studysession menu, we first need to select the stack we want to study.

<p align="center">
  <img width="737" height="197" alt="imagen" src="https://github.com/user-attachments/assets/293194d4-432a-484a-a807-03b712a1aba8" />
</p>

- Once we log in, we see a menu with the following options.
  - StartSession
  - ViewAllSessions
  - ViewCurrentStackSessions
  - Exit
    
<p align="center">
  <img width="1476" height="175" alt="imagen" src="https://github.com/user-attachments/assets/145bf355-ef5f-42a7-a204-07e057515d6f" />
</p>

### StartSession
-  Once we start the session, the questions will be displayed one after another,
and we’ll have to answer them. At the end, it will show us the total score for the study session.

<p align="center">
  <img width="493" height="287" alt="imagen" src="https://github.com/user-attachments/assets/f014c605-77b0-4d8e-8585-a46927865080" />
</p>

### ViewAllSessions
- In this menu, you can see all the sessions in the stacks.

<p align="center">
  <img width="1125" height="631" alt="imagen" src="https://github.com/user-attachments/assets/be4f2021-47fe-4475-ba3e-43a2d78d2da3" />
</p>

## ViewCurrentStackSessions
- In this menu, you can see the sessions for the selected stack.

<p align="center">
  <img width="1140" height="401" alt="imagen" src="https://github.com/user-attachments/assets/fb4980ae-d5b0-404b-8a98-75e3f39a3f95" />
</p>

## ReportSystem
- In the following menu, there are two options that allow you to check various details regarding the study sessions.
  - SessionPerMonth.
  - AverageScorePerMonth
  - Exit
    
<p align="center">
  <img width="268" height="134" alt="imagen" src="https://github.com/user-attachments/assets/03c2e157-b8ac-42ec-b0b1-c9e101aec7c8" />
</p>

- The SessionPerMonth menu allows us to view study sessions for a specific year.

<p align="center">
  <img width="1888" height="192" alt="imagen" src="https://github.com/user-attachments/assets/53a0ea97-6b34-472c-b091-5253ea4f275e" />
</p>

- The AverageScorePerMonth menu allows us to view the average study points for a specific year.

<p align="center">
  <img width="1894" height="181" alt="imagen" src="https://github.com/user-attachments/assets/dad5a55d-3327-44dc-9248-96186710c417" />
</p>

# Challenges
- One of the challenges I faced was writing the necessary queries, which required me to use DTOs to retrieve the data and display it correctly in the console.
- Use a pivot table in SQL Server to run queries for the reporting system.
- Organize the classes into different directories to keep the project more organized.

# What I have learned
- How to run queries in SQL Server and use its tools.
- Use a pivot table to perform various queries.
- Use DTOs to retrieve data from different tables and combine them into a single table to display on the console.
- Organize the classes into different directories.

# Areas to improve
- Learn more about the queries that can be run in SQL Server.
- Make better use of DTOs to retrieve information from the database.
- Make better use of the tools provided by SpectreConsole to improve the console interface.

# Resourced Used 
- SQL Server documentation: https://learn.microsoft.com/es-mx/sql/sql-server/?view=sql-server-ver17
- Specter Console Documentation: https://spectreconsole.net/
- Learn Dapper: https://www.learndapper.com/
- PIVOT - Understanding the Basics in SQL by TeraData : https://www.youtube.com/watch?v=bNetxDl40pM&t=166s&pp=0gcJCdkKAYcqIYzv
- Microsoft documentation for IEnumerable: https://learn.microsoft.com/es-es/dotnet/api/system.collections.ienumerable?view=net-8.0
- Blog About DTO: https://reactiveprogramming.io/blog/es/patrones-arquitectonicos/dto
- Microsoft documentation for List: https://learn.microsoft.com/es-es/dotnet/api/system.collections.generic.list-1?view=net-8.0 
