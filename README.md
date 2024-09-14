# Job Board Simple Application



## Overview

The Job Board Simple Application is designed to connect job seekers with employers efficiently. It provides a platform for job seekers to search and apply for jobs while allowing employers to post job listings and find suitable candidates.



## Table of Contents



- [Features](#features)
- [Technologies Used](#technologies-used)
- [Installation](#installation)
- [Usage](#usage)
- [User Roles](#user-roles)
- [API Endpoints](#api-endpoints)
- [Contributing](#contributing)
- [License](#license)
- [Contact](#contact)



## Features



### Job Seekers
    -Search and apply for job listings.
    -View and update their profile.
    -Track application status.

### Employers

    -Post new job listings.
    =View and manage job applications.
    =Update their company profile.



### General Features
    -User authentication and role management.
    -Responsive design for various devices.
    -Error handling and logging.




## Technologies Used



- **Backend**: .NET Core
- **Frontend**: None/yet
- **Database**: SQL Server
- **Authentication**:
- **Hosting**: Azure
- **Version Control**: Git



## Installation



### Prerequisites



- .NET Core SDK
- SQL Server



### Backend Setup



1.  Clone the repository:
       `bash
   git clone https://github.com/username/job-board.git
   cd job-board/backend
   `

2.  Install dependencies:
       `bash
   dotnet restore
   `

3.  Update database connection string in `appsettings.json`.

4.  Install EF Core CLI tools: To manage mirgration. `dotnet tool install --global dotnet-ef`
        
5.  Database Migration: Creating and updating migration using these two commands.
    `dotnet ef migrations add InitialCreate
     dotnet ef database update
     `
 
6.  Run the application:
       `bash
   dotnet run
   `



## Usage



1.  Open your browser and navigate to `http://localhost:3000` for the frontend application.
2.  Use `http://localhost:5000` for API endpoints.



## User Roles



### Job Seekers



### Employers



## API Endpoints



### Authentication



### Job Seekers
- **GET** `api/jobseekers` : Gets a list of All jobseekers.
    - **Response**:
        ```
        [{
      "Id" : 1,
      "Email": "jobseeker@example1.com",
      "FirstName": "exam",
      "LastName": "ple1",
      "PhoneNumber": "123-1234",
      "Password": "exmaple1Password1",
      "Address": "123 Example1 Street"
       },
        {
       "Id" :2,
      "Email": "jobseeker@example2.com",
      "FirstName": "exam",
      "LastName": "ple2",
      "PhoneNumber": "123-1234",
      "Password": "exmaple2Password1",
      "Address": "123 Example2 Street"
       }
         ]

        ```
    
- **GET** `api/jobseekers/{id}` : Gets details of a specific jobseeker.
    - **Parameters** :      
       -`id` int : Unique Identifier of the jobseekers.
    - **Response**:
        ```
       {
      "Email": "jobseeker@example.com",
      "FirstName": "exam",
      "LastName": "ple",
      "PhoneNumber": "123-1234",
      "Password": "exmaplePassword1",
      "Address": "123 Example Street"
       }    
        ```

- **POST** `api/jobseekers` : Adds a new jobseeker.
    - **Request Body**:
        ```
        {
      "Email": "jobseeker@example.com",
      "FirstName": "exam",
      "LastName": "ple",
      "PhoneNumber": "123-1234",
      "Password": "exmaplePassword1",
      "Address": "123 Example Street"
       }
        ```
- **PUT** `api/jobseekers/{id}` : Updates an existing jobseeker.
    - **Parameters** :        
          -`id` int : Unique Identifier of the jobseekers.
    - **Request Body**:
        ```
        {
      "Email": "newjobseeker@example.com",
      "FirstName": "exam",
      "LastName": "ple",
      "PhoneNumber": "123-1234",
      "Password": "newexmaplePassword1",
      "Address": "123 NewExample Street"
       }
        ```
- **Delete** `api/jobseekers/{id}` : Deletes an existing jobseeker.
    - **Parameters** :        
          -`id` int : Unique Identifier of the jobseekers.



### Employers

- **GET** `api/employers` : Gets a list of All Employers.
    - **Response**:
        ```
        [{
      "Id" : 1
      "Email": "employer@example1.com",
      "FirstName": "exam",
      "LastName": "ple",
      "PhoneNumber": "123-1234",
      "Password": "exmaplePassword1",
      "Address": "123 Example1 Street",
      "CompanyName": "Example1 ltd."
       },
       {
      "Id" : 2
      "Email": "employer@example2.com",
      "FirstName": "exam",
      "LastName": "ple",
      "PhoneNumber": "123-1234",
      "Password": "exmaple2Password1",
      "Address": "123 Example2 Street",
      "CompanyName": "Example2 ltd."
       }
    
        ]

        ```
    
- **GET** `api/employers/{id}` : Gets details of a specific employer.
    - **Parameters** :      
       -`id` int : Unique Identifier of the employer.
    - **Response**:
        ```
        {
      "Id" : 1
      "Email": "employer@example1.com",
      "FirstName": "exam",
      "LastName": "ple",
      "PhoneNumber": "123-1234",
      "Password": "exmaplePassword1",
      "Address": "123 Example1 Street",
      "CompanyName": "Example1 ltd."
       }
        ```

- **POST** `api/employers` : Adds a new Employer.
    - **Request Body**:
        ```
        {
      "Email": "employer@example.com",
      "FirstName": "exam",
      "LastName": "ple",
      "PhoneNumber": "123-1234",
      "Password": "exmaplePassword1",
      "Address": "123 Example Street",
      "CompanyName": "Example ltd."
       }
        ```
- **PUT** `api/employers/{id}` : Updates an existing Employer.
    - **Parameters** :        
          -`id` int : Unique Identifier of the employer.
    - **Request Body**:
        ```
        {
      "Email": "newemployer@example.com",
      "FirstName": "exam",
      "LastName": "ple",
      "PhoneNumber": "123-1234",
      "Password": "newexmaplePassword1",
      "Address": "123 NewExample Street",
      "CompanyName": "NewExample ltd."
       }
        ```
- **Delete** `api/employers/{id}` : Deletes an existing Employer
    - **Parameters** :        
          -`id` int : Unique Identifier of the employer.
### Jobs

- **GET** `api/jobs`:Gets the list of all Job Postings.
    - **Response**:
        ```
        [{
        "title": "Exmaple Job",
        "description": "Example Exmaple Example",
        "requirements": "Example Requirments",
        "location": "Example",
        "salary": 1234,
        "status": 0,
        "employerID": 123,
        "applicationDeadline": "2024-09-30T23:59:59"
      },{
      "title": "Exmaple Job",
      "description": "Example Exmaple Example",
      "requirements": "Example Requirments",
      "location": "Example",
      "salary": 1234,
      "status": 0,
      "employerID": 123,
      "applicationDeadline": "2024-09-30T23:59:59"
      }
        ]
        ```
- **GET** `api/jobs/{id}` : Gets the details of a specific JobPosting.
    - **Parameters** :        
          -`id` int : Unique Identifier of the Job Posting.
    - **Response**:
        ```
        {
      "Id":1
      "title": "Exmaple Job",
      "description": "Example Exmaple Example",
      "requirements": "Example Requirments",
      "location": "Example",
      "salary": 1234,
      "status": 0,
      "employerID": 123,
      "applicationDeadline": "2024-09-30T23:59:59"
      }
        ```
- **POST** `api/jobs` : Adds a new JobPosting.
    - **Body Request**:
        ```
        {
      "title": "Exmaple Job",
      "description": "Example Exmaple Example",
      "requirements": "Example Requirments",
      "location": "Example",
      "salary": 1234,
      "status": 0,
      "employerID": 123,
      "applicationDeadline": "2024-09-30T23:59:59"
      }
        ```
- **PUT** `api/jobs/{id}` : Updates a specific JobPosting.
    - **Parameters** :        
          -`id` int : Unique Identifier of the Job Posting.
    - **Body Request**:
        ```
        {
      "title": "New Exmaple Job",
      "description": "New Example New Exmaple New Example",
      "requirements": "New Example Requirments",
      "location": "New Example",
      "salary": 1234,
      "status": 0,
      "employerID": 123,
      "applicationDeadline": "2024-09-30T23:59:59"
      }
        ```
- **DELETE** `api/jobs/{id}` : Deletes a specific JobPosting.
    - **Parameters** :        
          -`id` int : Unique Identifier of the Job Posting.

### Applications

- **GET** `api/applictaions`:Gets the list of all Applications.
    - **Response**:
        ```
        [{
      "id": 1,
      "name": "Example1",
      "phoneNumber": "123-1234",
      "email": "example1@example.com",
      "jobID": 123,
      "jobSeekerID": 123,
      "applicationCVPath": "/exa/mple/example1cv.pdf",
      "status": 0,
      "coverLetter": "Example1 Example1 Example1"
      },
      {
      "id": 2,
      "name": "Example2",
      "phoneNumber": "123-1234",
      "email": "example2@example.com",
      "jobID": 123,
      "jobSeekerID": 123,
      "applicationCVPath": "/exa/mple/example2cv.pdf",
      "status": 0,
      "coverLetter": "Example2 Example2 Example2"
      }
        ]
        ```
- **GET** `api/applictaions/{id}` : Gets the details of a specific Application.
    - **Parameters** :        
          -`id` int : Unique Identifier of the Applications.
    - **Response**:
        ```
            {
      "id": 1,
      "name": "Example",
      "phoneNumber": "123-1234",
      "email": "example@example.com",
      "jobID": 123,
      "jobSeekerID": 123,
      "applicationCVPath": "/exa/mple/examplecv.pdf",
      "status": 0,
      "coverLetter": "Example Example Example"
      }
        ```
- **POST** `api/applictaions` : Adds a new Application.
    - **Body Request**:
        ```
        
        {
        "id": 1,
        "name": "Example",
        "phoneNumber": "123-1234",
        "email": "example@example.com",
        "jobID": 123,
        "jobSeekerID": 123,
        "applicationCVPath": "/exa/mple/examplecv.pdf",
        "status": 0,
        "coverLetter": "Example Example Example",
        "date": "2024-08-14T12:34:56Z"
        }
        ```
- **PUT** `api/applictaions/{id}` : Updates a specific Application.
    - **Parameters** :        
          -`id` int : Unique Identifier of the Applications.
    - **Body Request**:
        ```
        {
      "name": "New Example",
      "phoneNumber": "123-1234",
      "email": "Newexample@example.com",
      "jobID": 123,
      "jobSeekerID": 123,
      "applicationCVPath": "/exa/mple/Newexamplecv.pdf",
      "status": 0,
      "coverLetter": "New Example New Example New Example"
      }
        ```
- **DELETE** `api/applictaions/{id}` : Deletes a specific Application.
    - **Parameters** :        
          -`id` int : Unique Identifier of the Application.

## Contributing

Contributions are welcome! Please follow these steps:



1.  Fork the repository.
2.  Create a new branch.
3.  Make your changes.
4.  Submit a pull request.



## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.



## Contact

For any questions or suggestions, please contact:



- Name:
- Email:
